// routes/cr.js - Management, Excel Comparison & Merging for Network CRs (Change Requests)
const express = require('express');
const router = express.Router();
const multer = require('multer');
const xlsx = require('xlsx');
const sql = require('mssql');
const { logActivity } = require('../utils/logger');

// Configure multer memory storage for fast in-memory excel processing
const upload = multer({ storage: multer.memoryStorage() });

function requireAuth(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  }
  next();
}

/**
 * Remove columns with # or Column1 (Ported directly from C# delHashtagAndColumn1)
 */
function cleanColumns(rowObj) {
  const cleaned = {};
  for (const key of Object.keys(rowObj)) {
    if (key.includes('Column1') || key.includes('#')) {
      continue; // skip garbage column
    }
    cleaned[key.trim()] = rowObj[key];
  }
  return cleaned;
}

// GET /api/cr/list - دریافت لیست سوابق سی‌آرها
router.get('/list', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const { q, status, workflowType, isOvertime } = req.query;
    let whereClauses = ['1=1'];
    const request = pool.request();

    if (q && q.trim()) {
      request.input('searchQ', sql.NVarChar, `%${q.trim()}%`);
      whereClauses.push('([Ticket ID] LIKE @searchQ OR [Title] LIKE @searchQ OR [Dispatched by] LIKE @searchQ OR [Status] LIKE @searchQ)');
    }

    if (status && status !== 'all') {
      request.input('statusFilter', sql.NVarChar, status.trim());
      whereClauses.push('[Status] = @statusFilter');
    }

    if (workflowType && workflowType !== 'all') {
      request.input('wfFilter', sql.NVarChar, workflowType.trim());
      whereClauses.push('[Workflow Type] = @wfFilter');
    }

    if (isOvertime && isOvertime !== 'all') {
      request.input('otFilter', sql.NVarChar, isOvertime.trim());
      whereClauses.push('[Is overtime] = @otFilter');
    }

    const whereSql = whereClauses.join(' AND ');

    const result = await request.query(`
      SELECT 
        [id],
        [Ticket ID] AS ticketId,
        [Title] AS title,
        [Workflow Type] AS workflowType,
        [Status] AS status,
        [State] AS state,
        [Is overtime] AS isOvertime,
        [Dispatched by] AS dispatchedBy,
        [Dispatch Time] AS dispatchTime,
        [Last Modification Time] AS lastModTime
      FROM [DT].[dbo].[tblTotalCr]
      WHERE ${whereSql}
      ORDER BY [Last Modification Time] DESC
    `);

    // Get filter distinct lists
    const filterData = await pool.request().query(`
      SELECT DISTINCT [Status] FROM [DT].[dbo].[tblTotalCr] WHERE [Status] IS NOT NULL AND [Status] <> '';
      SELECT DISTINCT [Workflow Type] FROM [DT].[dbo].[tblTotalCr] WHERE [Workflow Type] IS NOT NULL AND [Workflow Type] <> '';
    `);

    res.json({
      success: true,
      data: result.recordset,
      total: result.recordset.length,
      availableStatuses: (filterData.recordsets[0] || []).map(r => r.Status),
      availableWorkflows: (filterData.recordsets[1] || []).map(r => r['Workflow Type'])
    });
  } catch (err) {
    console.error('CR List error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// POST /api/cr/upload - بارگذاری اکسل، مقایسه و ادغام با دیتابیس (Matching C# CompairAndInsertToDb)
router.post('/upload', requireAuth, upload.single('excelFile'), async (req, res) => {
  try {
    if (!req.file || !req.file.buffer) {
      return res.status(400).json({ success: false, message: 'فایل اکسل ارسال نشده است.' });
    }

    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, message: 'عدم اتصال به دیتابیس' });

    const sheetNameInput = (req.body.sheetName || 'Table 1').trim();

    // 1. Read Workbook
    const workbook = xlsx.read(req.file.buffer, { type: 'buffer' });
    let targetSheetName = sheetNameInput;

    if (!workbook.Sheets[targetSheetName]) {
      // Fallback to first sheet if named sheet does not exist
      targetSheetName = workbook.SheetNames[0];
    }

    const worksheet = workbook.Sheets[targetSheetName];
    if (!worksheet) {
      return res.status(400).json({ success: false, message: `شیت با نام "${sheetNameInput}" در فایل یافت نشد.` });
    }

    // 2. Parse Rows
    const rawRows = xlsx.utils.sheet_to_json(worksheet, { defval: '' });
    if (!rawRows || rawRows.length === 0) {
      return res.status(400).json({ success: false, message: 'فایل اکسل خالی است یا رکوردی در آن یافت نشد.' });
    }

    let insertCount = 0;
    let updateCount = 0;
    let skippedCount = 0;

    // Process each row matching C# MergeDataTables logic
    for (const rawRow of rawRows) {
      const row = cleanColumns(rawRow);
      
      const ticketId = (row['Ticket ID'] || row['ORDERID'] || row['شناسه تیکت'] || '').toString().trim();
      if (!ticketId) {
        skippedCount++;
        continue;
      }

      const title = (row['Title'] || row['TITLE'] || row['عنوان'] || '').toString().trim();
      const workflowType = (row['Workflow Type'] || row['WORKITEMNAME'] || row['نوع فرآیند'] || '').toString().trim();
      const status = (row['Status'] || row['PROCESSSTATUS'] || row['وضعیت'] || '').toString().trim();
      const state = (row['State'] || row['STATE'] || row['حالت'] || '').toString().trim();
      const isOvertime = (row['Is overtime'] || row['DOWNTIME'] || row['تاخیر'] || '').toString().trim();
      const dispatchedBy = (row['Dispatched by'] || row['CREATOR_NAME'] || row['IMPLEMENTOR'] || '').toString().trim();
      const dispatchTime = (row['Dispatch Time'] || row['CREATETIME'] || row['ACTION_START_TIME'] || '').toString().trim();
      const lastModTime = (row['Last Modification Time'] || row['FULLDATE'] || row['ACTION_END_DATE'] || '').toString().trim();

      // Check if ticketId exists in database
      const checkReq = pool.request();
      checkReq.input('ticketId', sql.NVarChar, ticketId);
      const existing = await checkReq.query(`SELECT [id] FROM [DT].[dbo].[tblTotalCr] WHERE [Ticket ID] = @ticketId`);

      if (existing.recordset.length > 0) {
        // UPDATE existing row
        const updateReq = pool.request();
        updateReq.input('ticketId', sql.NVarChar, ticketId);
        updateReq.input('title', sql.NVarChar, title);
        updateReq.input('workflowType', sql.NVarChar, workflowType);
        updateReq.input('status', sql.NVarChar, status);
        updateReq.input('state', sql.NVarChar, state);
        updateReq.input('isOvertime', sql.NVarChar, isOvertime);
        updateReq.input('dispatchedBy', sql.NVarChar, dispatchedBy);
        updateReq.input('dispatchTime', sql.NVarChar, dispatchTime);
        updateReq.input('lastModTime', sql.NVarChar, lastModTime);

        await updateReq.query(`
          UPDATE [DT].[dbo].[tblTotalCr]
          SET [Title] = @title,
              [Workflow Type] = @workflowType,
              [Status] = @status,
              [State] = @state,
              [Is overtime] = @isOvertime,
              [Dispatched by] = @dispatchedBy,
              [Dispatch Time] = @dispatchTime,
              [Last Modification Time] = @lastModTime,
              [UpdatedAt] = GETDATE()
          WHERE [Ticket ID] = @ticketId
        `);
        updateCount++;
      } else {
        // INSERT new row
        const insertReq = pool.request();
        insertReq.input('ticketId', sql.NVarChar, ticketId);
        insertReq.input('title', sql.NVarChar, title);
        insertReq.input('workflowType', sql.NVarChar, workflowType);
        insertReq.input('status', sql.NVarChar, status);
        insertReq.input('state', sql.NVarChar, state);
        insertReq.input('isOvertime', sql.NVarChar, isOvertime);
        insertReq.input('dispatchedBy', sql.NVarChar, dispatchedBy);
        insertReq.input('dispatchTime', sql.NVarChar, dispatchTime);
        insertReq.input('lastModTime', sql.NVarChar, lastModTime);

        await insertReq.query(`
          INSERT INTO [DT].[dbo].[tblTotalCr]
          ([Ticket ID], [Title], [Workflow Type], [Status], [State], [Is overtime], [Dispatched by], [Dispatch Time], [Last Modification Time])
          VALUES
          (@ticketId, @title, @workflowType, @status, @state, @isOvertime, @dispatchedBy, @dispatchTime, @lastModTime)
        `);
        insertCount++;
      }
    }

    // Log Activity to tblLog
    const origFileName = req.file.originalname || 'CR_Excel.xlsx';
    await logActivity(req, `بارگذاری و ادغام فایل اکسل سی‌آرها (${origFileName}): ${insertCount} تیکت جدید افزوده شد و ${updateCount} تیکت به‌روزرسانی گردید.`);

    res.json({
      success: true,
      message: `عملیات ادغام با موفقیت انجام شد: ${insertCount} رکورد جدید ثبت شد و ${updateCount} رکورد به‌روزرسانی گردید.`,
      stats: {
        totalProcessed: rawRows.length,
        inserted: insertCount,
        updated: updateCount,
        skipped: skippedCount,
        sheetNameUsed: targetSheetName
      }
    });
  } catch (err) {
    console.error('CR Upload error:', err);
    res.status(500).json({ success: false, message: 'خطا در پردازش فایل اکسل: ' + err.message });
  }
});

// DELETE /api/cr/all - پاک‌سازی تمام سوابق سی‌آرها (Matching C# deleteAllDataToolStripMenuItem)
router.delete('/all', requireAuth, async (req, res) => {
  // Only Admin or PowerUser
  const role = req.session.user?.role;
  if (!['administrator', 'PowerUser'].includes(role)) {
    return res.status(403).json({ success: false, message: 'شما دسترسی مجاز برای حذف داده‌های سی‌آر را ندارید.' });
  }

  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, message: 'عدم اتصال به دیتابیس' });

    const countRes = await pool.request().query('SELECT COUNT(*) AS total FROM [DT].[dbo].[tblTotalCr]');
    const totalDeleted = countRes.recordset[0]?.total || 0;

    await pool.request().query('TRUNCATE TABLE [DT].[dbo].[tblTotalCr]');

    // Log Activity
    await logActivity(req, `پاک‌سازی و حذف کامل سوابق جدول سی‌آرها (${totalDeleted} رکورد حذف شد)`);

    res.json({
      success: true,
      message: `تمام سوابق جدول سی‌آرها (${totalDeleted} رکورد) با موفقیت پاک‌سازی شد.`
    });
  } catch (err) {
    console.error('Delete all CR error:', err);
    res.status(500).json({ success: false, message: 'خطا در پاک‌سازی سوابق: ' + err.message });
  }
});

module.exports = router;
