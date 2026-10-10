// routes/db.js - Complete Database (DB) Management ported from C# PE - Ver 0009
const express = require('express');
const router = express.Router();
const multer = require('multer');
const xlsx = require('xlsx');
const ExcelJS = require('exceljs');
const sql = require('mssql');
const fs = require('fs');
const path = require('path');
const { logActivity } = require('../utils/logger');

// Default Backup Directory
const BACKUP_DIR = 'C:\\Users\\ASG\\Desktop\\0-Programing\\20-PE_WEB\\BackUp_DB';
if (!fs.existsSync(BACKUP_DIR)) {
  try { fs.mkdirSync(BACKUP_DIR, { recursive: true }); } catch (e) { console.error('Error creating BACKUP_DIR:', e); }
}

// Configure multer for Excel and Restore file uploads in memory
const upload = multer({
  storage: multer.memoryStorage(),
  limits: { fileSize: 100 * 1024 * 1024 } // 100MB max
});

function requireAuth(req, res, next) {
  if (!req.session.user) return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  next();
}

function requireAdmin(req, res, next) {
  if (!req.session.user) return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  const role = req.session.user.role;
  if (!['administrator', 'PowerUser'].includes(role)) {
    return res.status(403).json({ success: false, error: 'دسترسی غیرمجاز. فقط مدیران ارشد مجاز هستند.' });
  }
  next();
}

/**
 * Clean and normalize Persian/Arabic characters (ی و ک)
 */
function normalizePersianText(str) {
  if (typeof str !== 'string') return str;
  return str
    .replace(/\u064A/g, '\u06CC') // Arabic Yeh to Persian Ye
    .replace(/\u0643/g, '\u06A9') // Arabic Kaf to Persian Ke
    .replace(/\u0649/g, '\u06CC') // Alef Maksura to Persian Ye
    .trim();
}

// --------------------------------------------------------------------------------
// 1. LIST & SEARCH
// --------------------------------------------------------------------------------

// GET /api/db/list - دریافت لیست کل سایت‌های دیتابیس
router.get('/list', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به سرور دیتابیس' });

    const result = await pool.request().query(`
      SELECT * FROM [DB].[dbo].[tblDb] ORDER BY [کد سایت]
    `);
    res.json({ success: true, data: result.recordset, count: result.recordset.length });
  } catch (err) {
    console.error('DB list error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// GET /api/db/columns - ستون‌های جدول دیتابیس
router.get('/columns', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به سرور دیتابیس' });

    const result = await pool.request().query(`
      SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS 
      WHERE TABLE_CATALOG = 'DB' AND TABLE_NAME = 'tblDb'
      ORDER BY ORDINAL_POSITION
    `);
    res.json({ success: true, columns: result.recordset.map(r => r.COLUMN_NAME) });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

// GET /api/db/search?q=... - جستجوی سریع سایت
router.get('/search', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به سرور دیتابیس' });

    const q = (req.query.q || '').trim();
    let cleanDigits = q.replace(/[^0-9]/g, '');
    let padded4 = cleanDigits ? cleanDigits.padStart(4, '0') : q;
    let esCode = 'ES' + padded4;

    const result = await pool.request()
      .input('esCode', sql.NVarChar, esCode)
      .input('q', sql.NVarChar, q)
      .input('padded4', sql.NVarChar, padded4)
      .query(`
        SELECT TOP 50 * FROM [DB].[dbo].[tblDb]
        WHERE [کد سایت] = @esCode 
           OR [کد سایت] = @q 
           OR [کد سایت] = @padded4
           OR [کد سایت] LIKE '%' + @padded4 + '%'
           OR [نام سایت] LIKE '%' + @q + '%'
      `);
    res.json({ success: true, data: result.recordset });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

// GET /api/db/last-update-date - دریافت تاریخ آخرین به‌روزرسانی دیتابیس
router.get('/last-update-date', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به سرور دیتابیس' });

    const result = await pool.request().query(`
      SELECT TOP 1 [dateOfLastDbUp], [dateOfTblOldDb] FROM [PE].[dbo].[tblUpdateDate]
    `);
    const row = result.recordset[0] || {};
    res.json({
      success: true,
      dateOfLastDbUp: row.dateOfLastDbUp || null,
      dateOfTblOldDb: row.dateOfTblOldDb || null
    });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

// --------------------------------------------------------------------------------
// 2. EXCEL UPLOAD & DATABASE UPDATE (مطابق فرم frmUpdateDb و frmCompairAndUpdateDbSummary)
// --------------------------------------------------------------------------------

// POST /api/db/upload-excel - بروز رسانی کامل دیتابیس سایت‌ها با فایل اکسل جدید
router.post('/upload-excel', requireAdmin, upload.single('dbFile'), async (req, res) => {
  try {
    if (!req.file) {
      return res.status(400).json({ success: false, error: 'لطفاً فایل اکسل دیتابیس را انتخاب نمایید.' });
    }

    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به سرور دیتابیس' });

    // Read Excel from Buffer
    const workbook = xlsx.read(req.file.buffer, { type: 'buffer', cellDates: true });
    const sheetName = workbook.SheetNames[0];
    if (!sheetName) {
      return res.status(400).json({ success: false, error: 'شیت معتبری در فایل اکسل یافت نشد.' });
    }

    const sheet = workbook.Sheets[sheetName];
    const rawRows = xlsx.utils.sheet_to_json(sheet, { defval: '' });

    if (!rawRows || rawRows.length === 0) {
      return res.status(400).json({ success: false, error: 'هیچ رکوردی در فایل اکسل یافت نشد.' });
    }

    // Clean columns & normalize Persian characters
    const sampleRow = rawRows[0];
    const columnKeys = Object.keys(sampleRow).filter(k => !k.startsWith('__EMPTY') && !k.includes('Column1') && !k.includes('#'));

    // Check minimum required columns
    const hasSiteCode = columnKeys.some(k => k.includes('کد سایت') || k.includes('لوکیشن') || k.toLowerCase().includes('site'));
    if (!hasSiteCode) {
      return res.status(400).json({
        success: false,
        error: 'فایل اکسل فاقد ستون «کد سایت» یا «لوکیشن» است. لطفاً فایل استاندارد دیتابیس سایت‌ها را انتخاب فرمایید.'
      });
    }

    // Clean rows data
    const cleanedRows = rawRows.map(row => {
      const cleanObj = {};
      columnKeys.forEach(col => {
        let val = row[col];
        if (typeof val === 'string') {
          val = normalizePersianText(val);
        } else if (val === null || val === undefined) {
          val = '';
        }
        cleanObj[col.trim()] = val;
      });
      return cleanObj;
    });

    // Start Transaction to Truncate & Bulk Insert into [DB].[dbo].[tblDb]
    const transaction = new sql.Transaction(pool);
    await transaction.begin();

    try {
      // 1. Delete or Truncate existing tblDb
      await new sql.Request(transaction).query(`DELETE FROM [DB].[dbo].[tblDb]`);

      // 2. Insert records in chunks
      const chunkSize = 500;
      for (let i = 0; i < cleanedRows.length; i += chunkSize) {
        const chunk = cleanedRows.slice(i, i + chunkSize);
        
        for (const row of chunk) {
          const reqInsert = new sql.Request(transaction);
          const fields = [];
          const params = [];
          
          let paramIdx = 0;
          for (const col of columnKeys) {
            const cleanCol = col.trim();
            const pName = `p_${paramIdx++}`;
            fields.push(`[${cleanCol}]`);
            params.push(`@${pName}`);
            reqInsert.input(pName, sql.NVarChar, String(row[cleanCol] || ''));
          }

          const insertSql = `INSERT INTO [DB].[dbo].[tblDb] (${fields.join(', ')}) VALUES (${params.join(', ')})`;
          await reqInsert.query(insertSql);
        }
      }

      // 3. Update tblUpdateDate in PE database
      const nowStr = new Date().toISOString();
      await new sql.Request(transaction).query(`
        IF EXISTS (SELECT 1 FROM [PE].[dbo].[tblUpdateDate])
          UPDATE [PE].[dbo].[tblUpdateDate] SET [dateOfLastDbUp] = N'${nowStr}', [dateOfTblOldDb] = N'${nowStr}'
        ELSE
          INSERT INTO [PE].[dbo].[tblUpdateDate] ([dateOfLastDbUp], [dateOfTblOldDb]) VALUES (N'${nowStr}', N'${nowStr}')
      `);

      // 4. Solve Ya and Kaf standard query
      await new sql.Request(transaction).query(`
        USE DB;
        DECLARE @Table NVARCHAR(MAX), @Col NVARCHAR(MAX);
        DECLARE Table_Cursor CURSOR FOR  
        SELECT a.name, b.name
        FROM sysobjects a, syscolumns b  
        WHERE a.id = b.id AND a.xtype = 'u'
          AND (b.xtype = 99 OR b.xtype = 35 OR b.xtype = 231 OR b.xtype = 167 OR b.xtype = 175 OR b.xtype = 239);
        OPEN Table_Cursor FETCH NEXT FROM Table_Cursor INTO @Table, @Col;
        WHILE (@@FETCH_STATUS = 0)  
        BEGIN  
          EXEC('UPDATE [' + @Table + '] SET [' + @Col + ']= REPLACE(REPLACE(CAST([' + @Col + '] as nvarchar(max)) , NCHAR(1610), NCHAR(1740)), NCHAR(1603), NCHAR(1705)) WHERE CHARINDEX(NCHAR(1603),[' + @Col + '])>0 OR CHARINDEX(NCHAR(1610),[' + @Col + '])>0');
          FETCH NEXT FROM Table_Cursor INTO @Table, @Col;
        END;
        CLOSE Table_Cursor;
        DEALLOCATE Table_Cursor;
      `);

      await transaction.commit();

      // Log Activity
      await logActivity(req, `بروزرسانی دیتابیس سایت‌ها با فایل اکسل ${req.file.originalname} (تعداد: ${cleanedRows.length} سطر)`);

      res.json({
        success: true,
        message: `دیتابیس با موفقیت با فایل اکسل بروزرسانی شد. تعداد ${cleanedRows.length} رکورد جایگزین گردید.`,
        rowCount: cleanedRows.length,
        fileName: req.file.originalname
      });
    } catch (txErr) {
      await transaction.rollback();
      throw txErr;
    }
  } catch (err) {
    console.error('Upload Excel DB Error:', err);
    res.status(500).json({ success: false, error: 'خطا در فرآیند بروزرسانی دیتابیس: ' + err.message });
  }
});

// --------------------------------------------------------------------------------
// 3. EXCEL EXPORT & BACKUP
// --------------------------------------------------------------------------------

// GET /api/db/export-excel - خروجی اکسل با استایل شیک و فونت فارسی
router.get('/export-excel', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const result = await pool.request().query(`SELECT * FROM [DB].[dbo].[tblDb] ORDER BY [کد سایت]`);
    const records = result.recordset || [];

    if (records.length === 0) {
      return res.status(404).json({ success: false, error: 'هیچ رکوردی برای خروجی اکسل یافت نشد.' });
    }

    const columns = Object.keys(records[0]);
    const workbook = new ExcelJS.Workbook();
    workbook.creator = 'PE-Web System';
    workbook.created = new Date();

    const worksheet = workbook.addWorksheet('دیتابیس سایت‌ها', {
      views: [{ rightToLeft: true, showGridLines: true }]
    });

    worksheet.columns = columns.map(col => ({
      header: col,
      key: col,
      width: Math.max(col.length * 2, 16)
    }));

    // Header Row Styling
    const headerRow = worksheet.getRow(1);
    headerRow.height = 26;
    headerRow.eachCell(cell => {
      cell.font = { name: 'Vazirmatn', size: 10, bold: true, color: { argb: 'FFFFFFFF' } };
      cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF107C41' } };
      cell.alignment = { vertical: 'middle', horizontal: 'center' };
      cell.border = {
        top: { style: 'thin', color: { argb: 'FFCCCCCC' } },
        left: { style: 'thin', color: { argb: 'FFCCCCCC' } },
        bottom: { style: 'medium', color: { argb: 'FF000000' } },
        right: { style: 'thin', color: { argb: 'FFCCCCCC' } }
      };
    });

    // Data Rows Styling
    records.forEach((row, idx) => {
      const dataRow = worksheet.addRow(row);
      dataRow.height = 20;
      const isEven = idx % 2 === 0;
      dataRow.eachCell(cell => {
        cell.font = { name: 'Vazirmatn', size: 9, color: { argb: 'FF000000' } };
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: isEven ? 'FFFFFFFF' : 'FFF7F9FB' }
        };
        cell.alignment = { vertical: 'middle', horizontal: 'center' };
        cell.border = {
          top: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          left: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          bottom: { style: 'thin', color: { argb: 'FFE0E0E0' } },
          right: { style: 'thin', color: { argb: 'FFE0E0E0' } }
        };
      });
    });

    const timestamp = new Date().toISOString().slice(0, 10);
    const filename = `PE_Database_Sites_${timestamp}.xlsx`;

    res.setHeader('Content-Type', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
    res.setHeader('Content-Disposition', `attachment; filename="${encodeURIComponent(filename)}"`);

    await workbook.xlsx.write(res);
    res.end();
  } catch (err) {
    console.error('Export DB Excel error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// GET /api/db/backup-excel - تهیه پشتیبان فوری اکسل از کل دیتابیس فعلی
router.get('/backup-excel', requireAdmin, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const result = await pool.request().query(`SELECT * FROM [DB].[dbo].[tblDb] ORDER BY [کد سایت]`);
    const records = result.recordset || [];

    const columns = records.length > 0 ? Object.keys(records[0]) : ['کد سایت', 'نام سایت'];
    const workbook = new ExcelJS.Workbook();
    workbook.creator = 'PE Backup Engine';
    workbook.created = new Date();

    const worksheet = workbook.addWorksheet('Backup_tblDb', {
      views: [{ rightToLeft: true, showGridLines: true }]
    });

    worksheet.columns = columns.map(col => ({
      header: col,
      key: col,
      width: Math.max(col.length * 2, 16)
    }));

    records.forEach(row => worksheet.addRow(row));

    const now = new Date();
    const timestamp = now.toISOString().replace(/[:.]/g, '-').slice(0, 19);
    const filename = `Backup_DB_tblDb_${timestamp}.xlsx`;
    const localSavePath = path.join(BACKUP_DIR, filename);

    // Save copy to default BackUp_DB directory
    try {
      await workbook.xlsx.writeFile(localSavePath);
    } catch (saveErr) {
      console.warn('Could not save local Excel backup copy:', saveErr.message);
    }

    // Log Activity
    await logActivity(req, `تهیه فایل پشتیبان اکسل از دیتابیس سایت‌ها (${filename})`);

    res.setHeader('Content-Type', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
    res.setHeader('Content-Disposition', `attachment; filename="${encodeURIComponent(filename)}"`);

    await workbook.xlsx.write(res);
    res.end();
  } catch (err) {
    console.error('Backup DB Excel error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// GET /api/db/backup-sql - دانلود اسکریپت کامل SQL حاوی ساختار جدول و تمام رکوردها
router.get('/backup-sql', requireAdmin, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const result = await pool.request().query(`SELECT * FROM [DB].[dbo].[tblDb] ORDER BY [کد سایت]`);
    const records = result.recordset || [];

    if (records.length === 0) {
      return res.status(404).json({ success: false, error: 'هیچ رکوردی در دیتابیس یافت نشد.' });
    }

    const columns = Object.keys(records[0]);
    const now = new Date();
    const timestamp = now.toISOString().replace(/[:.]/g, '-').slice(0, 19);
    const dateStr = now.toLocaleDateString('fa-IR');

    let sqlScript = `-- ====================================================================\n`;
    sqlScript += `-- PE-Web System: SQL Server Full Backup Script for [DB].[dbo].[tblDb]\n`;
    sqlScript += `-- تاریخ پشتیبان‌گیری: ${dateStr} (${now.toISOString()})\n`;
    sqlScript += `-- تعداد کل رکوردها: ${records.length}\n`;
    sqlScript += `-- ====================================================================\n\n`;
    sqlScript += `USE [DB];\nGO\n\n`;
    sqlScript += `IF OBJECT_ID('[dbo].[tblDb]', 'U') IS NOT NULL\n`;
    sqlScript += `    DROP TABLE [dbo].[tblDb];\nGO\n\n`;

    // Create table columns
    sqlScript += `CREATE TABLE [dbo].[tblDb] (\n`;
    const colDefs = columns.map(col => `    [${col}] NVARCHAR(MAX) NULL`);
    sqlScript += colDefs.join(',\n') + `\n);\nGO\n\n`;

    // Insert records in batches
    const batchSize = 100;
    for (let i = 0; i < records.length; i += batchSize) {
      const chunk = records.slice(i, i + batchSize);
      sqlScript += `INSERT INTO [dbo].[tblDb] (${columns.map(c => `[${c}]`).join(', ')}) VALUES\n`;

      const valueRows = chunk.map(row => {
        const vals = columns.map(col => {
          const val = row[col];
          if (val === null || val === undefined) return 'NULL';
          return `N'${String(val).replace(/'/g, "''")}'`;
        });
        return `(${vals.join(', ')})`;
      });

      sqlScript += valueRows.join(',\n') + `;\nGO\n\n`;
    }

    const filename = `Backup_DB_tblDb_${timestamp}.sql`;
    const localSavePath = path.join(BACKUP_DIR, filename);

    // Save copy in default BackUp_DB directory
    try {
      fs.writeFileSync(localSavePath, '\uFEFF' + sqlScript, 'utf8');
    } catch (saveErr) {
      console.warn('Could not save local SQL script backup copy:', saveErr.message);
    }

    await logActivity(req, `تهیه و دانلود اسکریپت کامل SQL از دیتابیس سایت‌ها (${records.length} رکورد)`);

    res.setHeader('Content-Type', 'application/sql; charset=utf-8');
    res.setHeader('Content-Disposition', `attachment; filename="${encodeURIComponent(filename)}"`);
    res.send('\uFEFF' + sqlScript);
  } catch (err) {
    console.error('Backup SQL error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// POST /api/db/backup-native-sql - اجرای دستور بومی BACKUP DATABASE روی سرور SQL Server
router.post('/backup-native-sql', requireAdmin, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const { dbName } = req.body;
    const targetDb = ['DB', 'DT', 'PE', 'TEL'].includes(dbName) ? dbName : 'DB';

    const timestamp = new Date().toISOString().replace(/[:.]/g, '-').slice(0, 19);
    const backupFileName = `${targetDb}_Backup_Auto_${timestamp}.BAK`;
    const defaultBakPath = path.join(BACKUP_DIR, backupFileName);
    
    // T-SQL backup query (saves to BackUp_DB directory)
    const backupQuery = `
      BACKUP DATABASE [${targetDb}]
      TO DISK = N'${defaultBakPath}'
      WITH FORMAT, MEDIANAME = N'${targetDb}_Backup', NAME = N'Full Backup of ${targetDb}';
    `;

    try {
      await pool.request().query(backupQuery);
    } catch (sqlErr) {
      // Fallback to SQL default backup location if disk write fails
      const fallbackQuery = `
        DECLARE @backupPath NVARCHAR(500);
        SELECT @backupPath = CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS NVARCHAR(500));
        IF @backupPath IS NULL SET @backupPath = N'C:\\Program Files\\Microsoft SQL Server\\Backup\\';
        SET @backupPath = @backupPath + N'${backupFileName}';
        BACKUP DATABASE [${targetDb}] TO DISK = @backupPath WITH FORMAT, NAME = N'Full Backup';
        SELECT @backupPath AS BackupLocation;
      `;
      const fbRes = await pool.request().query(fallbackQuery);
      const fbLoc = fbRes.recordset?.[0]?.BackupLocation || backupFileName;
      await logActivity(req, `تهیه فایل پشتیبان بومی SQL Server از پایگاه داده ${targetDb} در ${fbLoc}`);
      return res.json({
        success: true,
        message: `پشتیبان‌گیری بومی SQL Server از دیتابیس [${targetDb}] با موفقیت انجام شد.`,
        backupLocation: fbLoc,
        fileName: backupFileName
      });
    }

    await logActivity(req, `تهیه فایل پشتیبان بومی SQL Server از پایگاه داده ${targetDb} در مسیر ${defaultBakPath}`);

    res.json({
      success: true,
      message: `پشتیبان‌گیری بومی SQL Server از دیتابیس [${targetDb}] با موفقیت انجام شد.`,
      backupLocation: defaultBakPath,
      fileName: backupFileName
    });
  } catch (err) {
    console.error('Native SQL backup error:', err);
    res.status(500).json({ success: false, error: 'خطا در پشتیبان‌گیری سرور SQL: ' + err.message });
  }
});

// GET /api/db/backup-list - دریافت لیست فایل‌های پشتیبان در پوشه BackUp_DB با مشخص کردن آخرین بک‌آپ
router.get('/backup-list', requireAdmin, (req, res) => {
  try {
    if (!fs.existsSync(BACKUP_DIR)) {
      return res.json({ success: true, backupDir: BACKUP_DIR, backups: [] });
    }

    const files = fs.readdirSync(BACKUP_DIR);
    const backups = [];

    files.forEach(file => {
      const filePath = path.join(BACKUP_DIR, file);
      try {
        const stats = fs.statSync(filePath);
        if (stats.isFile()) {
          const ext = path.extname(file).toLowerCase();
          if (['.sql', '.xlsx', '.xls', '.bak', '.json'].includes(ext)) {
            const sizeKB = (stats.size / 1024).toFixed(1);
            const sizeMB = (stats.size / (1024 * 1024)).toFixed(2);
            const sizeFormatted = stats.size > 1024 * 1024 ? `${sizeMB} MB` : `${sizeKB} KB`;

            const mtime = stats.mtime;
            const jalaliDate = mtime.toLocaleDateString('fa-IR');
            const jalaliTime = mtime.toLocaleTimeString('fa-IR', { hour: '2-digit', minute: '2-digit' });

            let fileTypeLabel = 'اسکریپت SQL';
            if (ext === '.xlsx' || ext === '.xls') fileTypeLabel = 'فایل اکسل';
            else if (ext === '.bak') fileTypeLabel = 'بک‌آپ بومی SQL Server';
            else if (ext === '.json') fileTypeLabel = 'اسنپ‌شات JSON';

            backups.push({
              filename: file,
              fullPath: filePath,
              sizeBytes: stats.size,
              sizeFormatted,
              mtime: stats.mtimeMs,
              dateJalali: `${jalaliDate} - ${jalaliTime}`,
              ext,
              fileTypeLabel
            });
          }
        }
      } catch (e) {
        console.warn(`Error reading backup file ${file}:`, e.message);
      }
    });

    // Sort descending by modified time (latest first)
    backups.sort((a, b) => b.mtime - a.mtime);

    if (backups.length > 0) {
      backups[0].isLatest = true;
    }

    res.json({
      success: true,
      backupDir: BACKUP_DIR,
      totalCount: backups.length,
      latestBackup: backups[0] || null,
      backups
    });
  } catch (err) {
    console.error('List backups error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// POST /api/db/restore-backup - بازیابی دیتابیس سایت‌ها از روی بک‌آپ (فایل انتخابی یا فایل آپلودشده)
router.post('/restore-backup', requireAdmin, upload.single('restoreFile'), async (req, res) => {
  const pool = req.app.locals.getPool();
  if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

  let filePath = null;
  let filename = null;
  let isTemp = false;

  if (req.file) {
    // Uploaded file
    filename = req.file.originalname;
    const tempDir = path.join(BACKUP_DIR, 'temp');
    if (!fs.existsSync(tempDir)) fs.mkdirSync(tempDir, { recursive: true });
    filePath = path.join(tempDir, `restore_${Date.now()}_${filename}`);
    fs.writeFileSync(filePath, req.file.buffer);
    isTemp = true;
  } else if (req.body.filename) {
    filename = path.basename(req.body.filename);
    filePath = path.join(BACKUP_DIR, filename);
  } else {
    return res.status(400).json({ success: false, error: 'هیچ فایلی برای بازیابی مشخص نشده است.' });
  }

  if (!fs.existsSync(filePath)) {
    return res.status(404).json({ success: false, error: 'فایل پشتیبان مورد نظر یافت نشد: ' + filename });
  }

  const ext = path.extname(filename).toLowerCase();

  try {
    let restoredRowsCount = 0;

    // 1. RESTORE FROM EXCEL (.xlsx, .xls)
    if (ext === '.xlsx' || ext === '.xls') {
      const workbook = xlsx.readFile(filePath);
      const sheetName = workbook.SheetNames[0];
      const rows = xlsx.utils.sheet_to_json(workbook.Sheets[sheetName], { defval: '' });

      if (rows.length === 0) {
        throw new Error('فایل اکسل انتخابی حاوی رکوردی نیست.');
      }

      const columnKeys = Object.keys(rows[0]);
      const transaction = new sql.Transaction(pool);
      await transaction.begin();

      try {
        const reqTable = new sql.Request(transaction);
        await reqTable.query(`IF OBJECT_ID('[DB].[dbo].[tblDb]', 'U') IS NOT NULL DROP TABLE [DB].[dbo].[tblDb]`);

        const colDefs = columnKeys.map(col => `[${col.trim()}] NVARCHAR(MAX) NULL`).join(',\n');
        await reqTable.query(`CREATE TABLE [DB].[dbo].[tblDb] (\n${colDefs}\n)`);

        const batchSize = 100;
        for (let i = 0; i < rows.length; i += batchSize) {
          const chunk = rows.slice(i, i + batchSize);
          const reqInsert = new sql.Request(transaction);
          const fields = columnKeys.map(c => `[${c.trim()}]`).join(', ');

          const valueClauses = [];
          chunk.forEach((row, rIdx) => {
            const pNames = [];
            columnKeys.forEach((col, cIdx) => {
              const pName = `p_${rIdx}_${cIdx}`;
              pNames.push(`@${pName}`);
              let cellVal = row[col] !== undefined && row[col] !== null ? String(row[col]) : '';
              cellVal = normalizePersianText(cellVal);
              reqInsert.input(pName, sql.NVarChar, cellVal);
            });
            valueClauses.push(`(${pNames.join(', ')})`);
          });

          await reqInsert.query(`INSERT INTO [DB].[dbo].[tblDb] (${fields}) VALUES ${valueClauses.join(',\n')}`);
        }

        await transaction.commit();
        restoredRowsCount = rows.length;
      } catch (txErr) {
        await transaction.rollback();
        throw txErr;
      }
    }
    // 2. RESTORE FROM SQL SCRIPT (.sql)
    else if (ext === '.sql') {
      const sqlContent = fs.readFileSync(filePath, 'utf8').replace(/^\uFEFF/, '');
      const statements = sqlContent.split(/\bGO\b/gi).map(s => s.trim()).filter(s => s.length > 0);

      for (const stmt of statements) {
        try {
          await pool.request().query(stmt);
        } catch (stmtErr) {
          console.warn('SQL restore statement warning:', stmtErr.message);
        }
      }

      const countRes = await pool.request().query(`SELECT COUNT(*) AS total FROM [DB].[dbo].[tblDb]`);
      restoredRowsCount = countRes.recordset?.[0]?.total || 0;
    }
    // 3. RESTORE FROM NATIVE SQL SERVER BACKUP (.bak)
    else if (ext === '.bak') {
      const restoreQuery = `
        USE master;
        ALTER DATABASE [DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        RESTORE DATABASE [DB] FROM DISK = N'${filePath}' WITH REPLACE;
        ALTER DATABASE [DB] SET MULTI_USER;
      `;
      await pool.request().query(restoreQuery);
      const countRes = await pool.request().query(`SELECT COUNT(*) AS total FROM [DB].[dbo].[tblDb]`);
      restoredRowsCount = countRes.recordset?.[0]?.total || 0;
    }
    // 4. RESTORE FROM JSON SNAPSHOT (.json)
    else if (ext === '.json') {
      const jsonContent = fs.readFileSync(filePath, 'utf8');
      const data = JSON.parse(jsonContent);
      const rows = Array.isArray(data) ? data : (data.tblDb || Object.values(data)[0] || []);

      if (rows.length === 0) throw new Error('فایل JSON حاوی داده‌های معتبر نیست.');

      const columnKeys = Object.keys(rows[0]);
      const reqTable = pool.request();
      await reqTable.query(`IF OBJECT_ID('[DB].[dbo].[tblDb]', 'U') IS NOT NULL DROP TABLE [DB].[dbo].[tblDb]`);
      await reqTable.query(`CREATE TABLE [DB].[dbo].[tblDb] (\n${columnKeys.map(c => `[${c.trim()}] NVARCHAR(MAX) NULL`).join(',\n')}\n)`);

      const batchSize = 100;
      for (let i = 0; i < rows.length; i += batchSize) {
        const chunk = rows.slice(i, i + batchSize);
        const reqInsert = pool.request();
        const valueClauses = [];
        chunk.forEach((row, rIdx) => {
          const pNames = [];
          columnKeys.forEach((col, cIdx) => {
            const pName = `p_${rIdx}_${cIdx}`;
            pNames.push(`@${pName}`);
            reqInsert.input(pName, sql.NVarChar, normalizePersianText(String(row[col] || '')));
          });
          valueClauses.push(`(${pNames.join(', ')})`);
        });
        await reqInsert.query(`INSERT INTO [DB].[dbo].[tblDb] (${columnKeys.map(c => `[${c.trim()}]`).join(', ')}) VALUES ${valueClauses.join(',\n')}`);
      }
      restoredRowsCount = rows.length;
    } else {
      throw new Error('فرمت فایل پشتیبان پشتیبانی نمی‌شود. (فرمت‌های مجاز: .sql, .xlsx, .bak, .json)');
    }

    // Update tblUpdateDate in PE database
    const nowStr = new Date().toISOString();
    await pool.request().query(`
      IF EXISTS (SELECT 1 FROM [PE].[dbo].[tblUpdateDate])
        UPDATE [PE].[dbo].[tblUpdateDate] SET [dateOfLastDbUp] = N'${nowStr}', [dateOfTblOldDb] = N'${nowStr}'
      ELSE
        INSERT INTO [PE].[dbo].[tblUpdateDate] ([dateOfLastDbUp], [dateOfTblOldDb]) VALUES (N'${nowStr}', N'${nowStr}')
    `);

    // Standard Ya and Kaf replacement
    await pool.request().query(`
      USE DB;
      DECLARE @Table NVARCHAR(MAX), @Col NVARCHAR(MAX);
      DECLARE Table_Cursor CURSOR FOR  
      SELECT a.name, b.name
      FROM sysobjects a, syscolumns b  
      WHERE a.id = b.id AND a.xtype = 'u'
        AND (b.xtype = 99 OR b.xtype = 35 OR b.xtype = 231 OR b.xtype = 167 OR b.xtype = 175 OR b.xtype = 239);
      OPEN Table_Cursor FETCH NEXT FROM Table_Cursor INTO @Table, @Col;
      WHILE (@@FETCH_STATUS = 0)  
      BEGIN  
        EXEC('UPDATE [' + @Table + '] SET [' + @Col + ']= REPLACE(REPLACE(CAST([' + @Col + '] as nvarchar(max)) , NCHAR(1610), NCHAR(1740)), NCHAR(1603), NCHAR(1705)) WHERE CHARINDEX(NCHAR(1603),[' + @Col + '])>0 OR CHARINDEX(NCHAR(1610),[' + @Col + '])>0');
        FETCH NEXT FROM Table_Cursor INTO @Table, @Col;
      END;
      CLOSE Table_Cursor;
      DEALLOCATE Table_Cursor;
    `);

    // Log Activity
    await logActivity(req, `بازیابی موفقیت‌آمیز دیتابیس سایت‌ها از فایل پشتیبان ${filename} (${restoredRowsCount} رکورد)`);

    res.json({
      success: true,
      message: `دیتابیس سایت‌ها با موفقیت از فایل پشتیبان «${filename}» بازیابی شد.`,
      filename,
      rowCount: restoredRowsCount
    });
  } catch (restoreErr) {
    console.error('Restore backup error:', restoreErr);
    res.status(500).json({ success: false, error: 'خطا در بازیابی دیتابیس: ' + restoreErr.message });
  } finally {
    if (isTemp && filePath && fs.existsSync(filePath)) {
      try { fs.unlinkSync(filePath); } catch (e) {}
    }
  }
});

// --------------------------------------------------------------------------------
// 4. NEW PLANS / SITES (طرح‌های جدید - frmDbNewSite / tblTempNewSite)
// --------------------------------------------------------------------------------

// GET /api/db/new-sites - دریافت لیست طرح‌های جدید
router.get('/new-sites', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const result = await pool.request().query(`
      SELECT 
        [location] AS location,
        [siteName] AS siteName,
        [contractor] AS contractor,
        [region] AS region,
        [address] AS address
      FROM [PE].[dbo].[tblTempNewSite]
    `);
    res.json({ success: true, data: result.recordset || [] });
  } catch (err) {
    console.error('get new-sites error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// POST /api/db/new-sites - ثبت طرح/سایت جدید
router.post('/new-sites', requireAdmin, async (req, res) => {
  try {
    const { location, siteName, contractor, region, address } = req.body;
    if (!location || !siteName) {
      return res.status(400).json({ success: false, error: 'کد سایت و نام سایت الزامی هستند.' });
    }

    const cleanLocation = location.toUpperCase().startsWith('ES') ? location.toUpperCase() : 'ES' + location.trim();

    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    // Check if already exists in tblTempNewSite
    const checkExist = await pool.request()
      .input('loc', sql.NVarChar, cleanLocation)
      .query(`SELECT 1 FROM [PE].[dbo].[tblTempNewSite] WHERE [location] = @loc`);

    if (checkExist.recordset && checkExist.recordset.length > 0) {
      return res.status(400).json({ success: false, error: `سایت با لوکیشن ${cleanLocation} قبلاً در لیست طرح‌های جدید ثبت شده است.` });
    }

    await pool.request()
      .input('location', sql.NVarChar, cleanLocation)
      .input('siteName', sql.NVarChar, normalizePersianText(siteName))
      .input('contractor', sql.NVarChar, normalizePersianText(contractor || ''))
      .input('region', sql.NVarChar, normalizePersianText(region || ''))
      .input('address', sql.NVarChar, normalizePersianText(address || ''))
      .query(`
        INSERT INTO [PE].[dbo].[tblTempNewSite] ([location], [siteName], [contractor], [region], [address])
        VALUES (@location, @siteName, @contractor, @region, @address)
      `);

    await logActivity(req, `ثبت طرح/سایت جدید: ${cleanLocation} - ${siteName}`);

    res.json({ success: true, message: `سایت جدید (${cleanLocation} - ${siteName}) با موفقیت ثبت شد.` });
  } catch (err) {
    console.error('post new-sites error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// DELETE /api/db/new-sites/:location - حذف طرح جدید بر اساس لوکیشن
router.delete('/new-sites/:location', requireAdmin, async (req, res) => {
  try {
    const loc = decodeURIComponent(req.params.location);
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    await pool.request()
      .input('location', sql.NVarChar, loc)
      .query(`DELETE FROM [PE].[dbo].[tblTempNewSite] WHERE [location] = @location`);

    await logActivity(req, `حذف رکورد طرح جدید با لوکیشن ${loc}`);

    res.json({ success: true, message: `سایت ${loc} از لیست طرح‌های جدید حذف شد.` });
  } catch (err) {
    console.error('delete new-sites error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// --------------------------------------------------------------------------------
// 5. DELETED PLANS / SITES (طرح‌های حذف شده - frmDbDeletedSite / tblTempDeletedSite)
// --------------------------------------------------------------------------------

// GET /api/db/deleted-sites - لیست سایت‌های حذف شده
router.get('/deleted-sites', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const result = await pool.request().query(`
      SELECT 
        [location] AS location,
        [siteName] AS siteName,
        [DeletedDate] AS deletedDate
      FROM [PE].[dbo].[tblTempDeletedSite]
    `);
    res.json({ success: true, data: result.recordset || [] });
  } catch (err) {
    console.error('get deleted-sites error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// POST /api/db/deleted-sites - ثبت سایت حذف شده
router.post('/deleted-sites', requireAdmin, async (req, res) => {
  try {
    const { location, siteName } = req.body;
    if (!location) {
      return res.status(400).json({ success: false, error: 'کد سایت الزامی است.' });
    }

    const cleanLocation = location.toUpperCase().startsWith('ES') ? location.toUpperCase() : 'ES' + location.trim();

    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    await pool.request()
      .input('location', sql.NVarChar, cleanLocation)
      .input('siteName', sql.NVarChar, normalizePersianText(siteName || ''))
      .query(`
        INSERT INTO [PE].[dbo].[tblTempDeletedSite] ([location], [siteName], [DeletedDate])
        VALUES (@location, @siteName, GETDATE())
      `);

    await logActivity(req, `ثبت در لیست سایت‌های حذف‌شده: ${cleanLocation}`);

    res.json({ success: true, message: `سایت ${cleanLocation} به لیست حذف‌شده‌ها اضافه شد.` });
  } catch (err) {
    console.error('post deleted-sites error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// DELETE /api/db/deleted-sites/:location - حذف از لیست سایت‌های حذف شده
router.delete('/deleted-sites/:location', requireAdmin, async (req, res) => {
  try {
    const loc = decodeURIComponent(req.params.location);
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    await pool.request()
      .input('location', sql.NVarChar, loc)
      .query(`DELETE FROM [PE].[dbo].[tblTempDeletedSite] WHERE [location] = @location`);

    await logActivity(req, `حذف رکورد از لیست سایت‌های حذف شده با لوکیشن ${loc}`);

    res.json({ success: true, message: `سایت ${loc} از لیست حذف شد.` });
  } catch (err) {
    console.error('delete deleted-sites error:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// --------------------------------------------------------------------------------
// 6. STATISTICS & GROUPINGS (آمار و سطرهای گروه‌بندی شده - frmDbSearch)
// --------------------------------------------------------------------------------

// GET /api/db/stats - دریافت آمار گروه‌بندی شده
router.get('/stats', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    // Contractor stats
    const contractorRes = await pool.request().query(`
      SELECT ISNULL([پیمانکار], 'نامشخص') AS item, COUNT(*) AS count
      FROM [DB].[dbo].[tblDb]
      GROUP BY [پیمانکار]
      ORDER BY count DESC
    `).catch(() => ({ recordset: [] }));

    // Region stats
    const regionRes = await pool.request().query(`
      SELECT ISNULL([منطقه], 'نامشخص') AS item, COUNT(*) AS count
      FROM [DB].[dbo].[tblDb]
      GROUP BY [منطقه]
      ORDER BY count DESC
    `).catch(() => ({ recordset: [] }));

    // Total sites
    const totalRes = await pool.request().query(`SELECT COUNT(*) AS total FROM [DB].[dbo].[tblDb]`);
    const total = totalRes.recordset[0]?.total || 0;

    res.json({
      success: true,
      total,
      byContractor: contractorRes.recordset || [],
      byRegion: regionRes.recordset || []
    });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

// --------------------------------------------------------------------------------
// 7. SUB-UPDATES (پیمانکار، باطری/کولر، BSC/RNC)
// --------------------------------------------------------------------------------

// POST /api/db/update-contractor - بروزرسانی تخصصی پیمانکاران
router.post('/update-contractor', requireAdmin, upload.single('contractorFile'), async (req, res) => {
  try {
    if (!req.file) return res.status(400).json({ success: false, error: 'فایل اکسل پیمانکاران انتخاب نشده است' });
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const workbook = xlsx.read(req.file.buffer, { type: 'buffer' });
    const sheet = workbook.Sheets[workbook.SheetNames[0]];
    const rows = xlsx.utils.sheet_to_json(sheet, { defval: '' });

    let updatedCount = 0;
    for (const row of rows) {
      const siteCode = row['کد سایت'] || row['لوکیشن'] || row['SiteCode'] || row['Location'];
      const contractor = row['پیمانکار'] || row['Contractor'];
      if (siteCode && contractor) {
        await pool.request()
          .input('siteCode', sql.NVarChar, String(siteCode).trim())
          .input('contractor', sql.NVarChar, normalizePersianText(String(contractor).trim()))
          .query(`UPDATE [DB].[dbo].[tblDb] SET [پیمانکار] = @contractor WHERE [کد سایت] = @siteCode`);
        updatedCount++;
      }
    }

    await logActivity(req, `بروزرسانی اطلاعات پیمانکار برای ${updatedCount} سایت از روی فایل اکسل`);
    res.json({ success: true, message: `اطلاعات پیمانکار برای ${updatedCount} سایت با موفقیت بروزرسانی شد.` });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

module.exports = router;
