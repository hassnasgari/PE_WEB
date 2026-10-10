// routes/backup.js - Database Backup & Export Management for Enterprise System
const express = require('express');
const router = express.Router();
const fs = require('fs');
const path = require('path');
const sql = require('mssql');
const { logActivity } = require('../utils/logger');

const BACKUP_DIR = path.join(__dirname, '..', 'backups');
if (!fs.existsSync(BACKUP_DIR)) {
  fs.mkdirSync(BACKUP_DIR, { recursive: true });
}

function requireAuth(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  }
  next();
}

function requireAdmin(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  }
  const role = req.session.user.role;
  if (!['administrator', 'PowerUser'].includes(role)) {
    return res.status(403).json({ success: false, error: 'فقط مدیران ارشد اجازه پشتیبان‌گیری از دیتابیس‌ها را دارند' });
  }
  next();
}

// GET /api/pe/backups - لیست فایل‌های پشتیبان موجود
router.get('/backups', requireAdmin, (req, res) => {
  try {
    if (!fs.existsSync(BACKUP_DIR)) {
      return res.json({ success: true, data: [] });
    }

    const files = fs.readdirSync(BACKUP_DIR).filter(f => !f.startsWith('.'));
    const backupList = files.map(filename => {
      const filePath = path.join(BACKUP_DIR, filename);
      const stat = fs.statSync(filePath);
      return {
        filename,
        sizeBytes: stat.size,
        sizeFormatted: formatBytes(stat.size),
        createdAt: stat.mtime,
        createdDateStr: stat.mtime.toLocaleString('fa-IR')
      };
    }).sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

    res.json({ success: true, data: backupList });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

function formatBytes(bytes) {
  if (bytes === 0) return '0 بایت';
  const k = 1024;
  const sizes = ['بایت', 'کیلوبایت', 'مگابایت', 'گیگابایت'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

// POST /api/pe/backup-database - پشتیبان‌گیری از دیتابیس انتخابی
router.post('/backup-database', requireAdmin, async (req, res) => {
  const { dbName } = req.body;
  const validDbs = ['DT', 'DB', 'PE', 'TEL', 'SDM', 'ALL'];
  const targetDb = validDbs.includes(dbName) ? dbName : 'DT';

  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, message: 'عدم اتصال به سرور دیتابیس' });

    const timestamp = new Date().toISOString().replace(/[:.]/g, '-').slice(0, 19);
    const dbsToBackup = targetDb === 'ALL' ? ['DT', 'DB', 'PE', 'TEL', 'SDM'] : [targetDb];

    const backupResults = [];

    for (const db of dbsToBackup) {
      const backupFileName = `Backup_${db}_${timestamp}.json`;
      const backupFilePath = path.join(BACKUP_DIR, backupFileName);

      // Extract tables and data as structured backup snapshot
      let dbData = {};
      try {
        const tablesRes = await pool.request().query(`
          SELECT TABLE_NAME FROM [${db}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'
        `);

        for (const tbl of tablesRes.recordset) {
          const tableName = tbl.TABLE_NAME;
          const rowsRes = await pool.request().query(`SELECT TOP 50000 * FROM [${db}].[dbo].[${tableName}]`);
          dbData[tableName] = rowsRes.recordset;
        }

        fs.writeFileSync(backupFilePath, JSON.stringify(dbData, null, 2), 'utf8');
        const stat = fs.statSync(backupFilePath);

        backupResults.push({
          db,
          filename: backupFileName,
          sizeFormatted: formatBytes(stat.size),
          status: 'success'
        });
      } catch (dbErr) {
        console.warn(`Backup error for ${db}:`, dbErr.message);
        backupResults.push({ db, error: dbErr.message, status: 'failed' });
      }
    }

    // Log Activity to tblLog
    await logActivity(req, `تهیه پشتیبان کامل از پایگاه‌های داده (${dbsToBackup.join(', ')})`);

    res.json({
      success: true,
      message: `پشتیبان‌گیری از دیتابیس‌های (${dbsToBackup.join(', ')}) با موفقیت انجام شد.`,
      results: backupResults
    });
  } catch (err) {
    console.error('Backup creation error:', err);
    res.status(500).json({ success: false, message: 'خطا در فرآیند پشتیبان‌گیری: ' + err.message });
  }
});

// GET /api/pe/download-backup/:filename - دانلود فایل پشتیبان
router.get('/download-backup/:filename', requireAdmin, (req, res) => {
  const filename = req.params.filename;
  if (!filename || filename.includes('..') || filename.includes('/') || filename.includes('\\')) {
    return res.status(400).json({ error: 'نام فایل نامعتبر است' });
  }

  const filePath = path.join(BACKUP_DIR, filename);
  if (!fs.existsSync(filePath)) {
    return res.status(404).json({ error: 'فایل پشتیبان مورد نظر یافت نشد' });
  }

  res.download(filePath, filename);
});

module.exports = router;
