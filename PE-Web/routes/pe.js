const express = require('express');
const router = express.Router();
const sql = require('mssql');
const { logActivity } = require('../utils/logger');

// Security Middleware: Require authentication
function requireAuth(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  }
  next();
}

// Security Middleware: Require Admin or PowerUser
function requireAdminOrPower(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  }
  const role = req.session.user.role;
  if (!['administrator', 'PowerUser'].includes(role)) {
    return res.status(403).json({ success: false, error: 'شما به بخش مدیریت سیستم دسترسی ندارید' });
  }
  next();
}

// Security Middleware: Strict Administrator Only
function requireStrictAdmin(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  }
  if (req.session.user.role !== 'administrator') {
    return res.status(403).json({ success: false, error: 'این عملیات فقط مختص مدیر ارشد سیستم (Administrator) است' });
  }
  next();
}

// ==========================================
// 1. AUDIT LOGS (سوابق و لاگ‌های سیستم)
// ==========================================

// GET /api/pe/logs
router.get('/logs', requireAdminOrPower, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم ارتباط با دیتابیس' });

    const { q, user, actionType, period, page = 1, limit = 100 } = req.query;
    const pageNum = Math.max(1, parseInt(page) || 1);
    const limitNum = Math.min(500, Math.max(10, parseInt(limit) || 100));
    const offset = (pageNum - 1) * limitNum;

    let whereClauses = ['1=1'];
    const request = pool.request();

    if (q && q.trim()) {
      request.input('searchQuery', sql.NVarChar, `%${q.trim()}%`);
      whereClauses.push('(l.userName LIKE @searchQuery OR u.fullName LIKE @searchQuery OR u.family LIKE @searchQuery OR (ISNULL(u.fullName,\'\') + \' \' + ISNULL(u.family,\'\')) LIKE @searchQuery OR l.[action] LIKE @searchQuery OR l.[date] LIKE @searchQuery OR l.[Ip] LIKE @searchQuery OR l.[computerName] LIKE @searchQuery)');
    }

    if (user && user.trim() && user !== 'all') {
      request.input('userFilter', sql.NVarChar, user.trim());
      whereClauses.push('l.userName = @userFilter');
    }

    if (actionType && actionType.trim() && actionType !== 'all') {
      request.input('actionFilter', sql.NVarChar, `%${actionType.trim()}%`);
      whereClauses.push('l.[action] LIKE @actionFilter');
    }

    // Date / Period Filter
    if (period === 'today') {
      const todayParts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
        year: 'numeric', month: '2-digit', day: '2-digit'
      }).formatToParts(new Date());
      const y = todayParts.find(p => p.type === 'year')?.value || '';
      const m = todayParts.find(p => p.type === 'month')?.value || '';
      const d = todayParts.find(p => p.type === 'day')?.value || '';
      const todayStr = `${y}/${m}/${d}`;
      request.input('todayStr', sql.NVarChar, `%${todayStr}%`);
      whereClauses.push('l.[date] LIKE @todayStr');
    }

    const whereSql = whereClauses.join(' AND ');

    // Total Count Query
    const countResult = await request.query(`
      SELECT COUNT(*) AS totalCount 
      FROM [PE].[dbo].[tblLog] l
      LEFT JOIN [PE].[dbo].[tblUsers] u ON LOWER(LTRIM(RTRIM(l.userName))) = LOWER(LTRIM(RTRIM(u.userName)))
      WHERE ${whereSql}
    `);
    const totalRecords = countResult.recordset[0]?.totalCount || 0;

    // Data Query (Top sorted by ID desc)
    request.input('offsetRows', sql.Int, offset);
    request.input('fetchRows', sql.Int, limitNum);

    const dataResult = await request.query(`
      SELECT 
        l.id, 
        l.userName, 
        ISNULL(u.fullName, N'') AS fullName, 
        ISNULL(u.family, N'') AS family, 
        COALESCE(
          NULLIF(LTRIM(RTRIM(u.fullName)), N''),
          NULLIF(LTRIM(RTRIM(ISNULL(u.fullName, N'') + N' ' + ISNULL(u.family, N''))), N''),
          l.userName
        ) AS userFullName,
        l.[action], 
        l.[date], 
        l.[time], 
        l.mac, 
        l.computerName, 
        l.SelectedNet, 
        l.Ip
      FROM [PE].[dbo].[tblLog] l
      LEFT JOIN [PE].[dbo].[tblUsers] u ON LOWER(LTRIM(RTRIM(l.userName))) = LOWER(LTRIM(RTRIM(u.userName)))
      WHERE ${whereSql}
      ORDER BY l.id DESC
      OFFSET @offsetRows ROWS FETCH NEXT @fetchRows ROWS ONLY
    `);

    // Get list of distinct users with full names for filter dropdown
    const usersResult = await pool.request().query(`
      SELECT DISTINCT 
        l.userName, 
        COALESCE(
          NULLIF(LTRIM(RTRIM(u.fullName)), N''),
          NULLIF(LTRIM(RTRIM(ISNULL(u.fullName, N'') + N' ' + ISNULL(u.family, N''))), N''),
          l.userName
        ) AS userFullName
      FROM [PE].[dbo].[tblLog] l
      LEFT JOIN [PE].[dbo].[tblUsers] u ON LOWER(LTRIM(RTRIM(l.userName))) = LOWER(LTRIM(RTRIM(u.userName)))
      WHERE l.userName IS NOT NULL AND LTRIM(RTRIM(l.userName)) <> ''
      ORDER BY l.userName
    `);

    res.json({
      success: true,
      data: dataResult.recordset,
      pagination: {
        page: pageNum,
        limit: limitNum,
        total: totalRecords,
        totalPages: Math.ceil(totalRecords / limitNum)
      },
      availableUsers: usersResult.recordset.map(u => ({
        userName: u.userName,
        userFullName: u.userFullName || ''
      }))
    });
  } catch (err) {
    console.error('Error fetching logs:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// ==========================================
// 2. USER MANAGEMENT (مدیریت کاربران و دسترسی‌ها)
// ==========================================

// GET /api/pe/users - دریافت لیست تمام کاربران با اطلاعات نقش
router.get('/users', requireAdminOrPower, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم ارتباط با دیتابیس' });

    const result = await pool.request().query(`
      SELECT 
        u.userName,
        u.fullName,
        u.family,
        u.company,
        u.role,
        r.Description AS roleDescription
      FROM [PE].[dbo].[tblUsers] u
      LEFT JOIN [PE].[dbo].[tblRol] r ON u.role = r.rol
      ORDER BY 
        CASE 
          WHEN u.role = 'administrator' THEN 1 
          WHEN u.role = 'PowerUser' THEN 2 
          ELSE 3 
        END,
        u.fullName ASC
    `);

    res.json({ success: true, data: result.recordset });
  } catch (err) {
    console.error('Error fetching users:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// GET /api/pe/roles - لیست نقش‌های موجود
router.get('/roles', requireAdminOrPower, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم ارتباط با دیتابیس' });

    const result = await pool.request().query(`
      SELECT id, rol, Description 
      FROM [PE].[dbo].[tblRol]
      ORDER BY id ASC
    `);

    res.json({ success: true, data: result.recordset });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

// POST /api/pe/users - افزودن کاربر جدید
router.post('/users', requireStrictAdmin, async (req, res) => {
  const { userName, password, fullName, family, company, role } = req.body;

  if (!userName || !userName.trim()) {
    return res.status(400).json({ success: false, message: 'نام کاربری الزامی است' });
  }
  if (!password || !password.trim()) {
    return res.status(400).json({ success: false, message: 'رمز عبور الزامی است' });
  }

  try {
    const pool = req.app.locals.getPool();
    const cleanUser = userName.trim();

    // Check duplicate
    const checkRes = await pool.request()
      .input('userName', sql.NVarChar(50), cleanUser)
      .query('SELECT userName FROM [PE].[dbo].[tblUsers] WHERE userName = @userName');

    if (checkRes.recordset.length > 0) {
      return res.status(400).json({ success: false, message: 'این نام کاربری قبلاً در سیستم ثبت شده است' });
    }

    await pool.request()
      .input('userName', sql.NVarChar(50), cleanUser)
      .input('pass', sql.NVarChar(50), password.trim())
      .input('fullName', sql.NVarChar(100), (fullName || '').trim())
      .input('family', sql.NVarChar(100), (family || '').trim())
      .input('company', sql.NVarChar(100), (company || 'مخابرات استان اصفهان').trim())
      .input('role', sql.NVarChar(50), (role || 'register').trim())
      .query(`
        INSERT INTO [PE].[dbo].[tblUsers] (userName, pass, fullName, family, company, role)
        VALUES (@userName, @pass, @fullName, @family, @company, @role)
      `);

    // Log Activity
    await logActivity(req, `افزودن کاربر جدید: ${cleanUser} با نقش ${role} (${fullName || ''})`);

    res.json({ success: true, message: 'کاربر با موفقیت افزوده شد' });
  } catch (err) {
    console.error('Create user error:', err);
    res.status(500).json({ success: false, message: 'خطا در ثبت کاربر: ' + err.message });
  }
});

// PUT /api/pe/users/:username - ویرایش کاربر و تغییر نقش یا رمز
router.put('/users/:username', requireStrictAdmin, async (req, res) => {
  const targetUsername = req.params.username;
  const { fullName, family, company, role, password } = req.body;

  if (!targetUsername) {
    return res.status(400).json({ success: false, message: 'نام کاربری نامعتبر است' });
  }

  try {
    const pool = req.app.locals.getPool();
    const request = pool.request()
      .input('userName', sql.NVarChar(50), targetUsername)
      .input('fullName', sql.NVarChar(100), (fullName || '').trim())
      .input('family', sql.NVarChar(100), (family || '').trim())
      .input('company', sql.NVarChar(100), (company || '').trim())
      .input('role', sql.NVarChar(50), (role || 'register').trim());

    let passUpdateSql = '';
    if (password && password.trim()) {
      request.input('pass', sql.NVarChar(50), password.trim());
      passUpdateSql = ', pass = @pass';
    }

    await request.query(`
      UPDATE [PE].[dbo].[tblUsers]
      SET fullName = @fullName,
          family = @family,
          company = @company,
          role = @role
          ${passUpdateSql}
      WHERE userName = @userName
    `);

    // Log Activity
    const passChangeNote = password && password.trim() ? ' و تغییر رمز عبور' : '';
    await logActivity(req, `ویرایش مشخصات کاربر: ${targetUsername} (نقش: ${role})${passChangeNote}`);

    res.json({ success: true, message: 'مشخصات کاربر با موفقیت بروزرسانی شد' });
  } catch (err) {
    console.error('Update user error:', err);
    res.status(500).json({ success: false, message: 'خطا در بروزرسانی کاربر: ' + err.message });
  }
});

// DELETE /api/pe/users/:username - حذف کاربر
router.delete('/users/:username', requireStrictAdmin, async (req, res) => {
  const targetUsername = req.params.username;

  if (!targetUsername) {
    return res.status(400).json({ success: false, message: 'نام کاربری نامعتبر است' });
  }

  // Prevent self-deletion
  if (req.session.user && req.session.user.username.toLowerCase() === targetUsername.toLowerCase()) {
    return res.status(400).json({ success: false, message: 'نمی‌توانید حساب کاربری جاری خود را حذف کنید' });
  }

  try {
    const pool = req.app.locals.getPool();
    await pool.request()
      .input('userName', sql.NVarChar(50), targetUsername)
      .query('DELETE FROM [PE].[dbo].[tblUsers] WHERE userName = @userName');

    // Log Activity
    await logActivity(req, `حذف کاربر از سیستم: ${targetUsername}`);

    res.json({ success: true, message: 'کاربر با موفقیت حذف شد' });
  } catch (err) {
    console.error('Delete user error:', err);
    res.status(500).json({ success: false, message: 'خطا در حذف کاربر: ' + err.message });
  }
});

// ==========================================
// 3. SYSTEM STATS & HEALTH (آمار کلی و تنظیمات)
// ==========================================

// GET /api/pe/system-stats
router.get('/system-stats', requireAdminOrPower, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'عدم اتصال به دیتابیس' });

    const [usersCount, logsCount, clientsCount, dtCount, dbCount, telCount] = await Promise.all([
      pool.request().query('SELECT COUNT(*) AS total FROM [PE].[dbo].[tblUsers]'),
      pool.request().query('SELECT COUNT(*) AS total FROM [PE].[dbo].[tblLog]'),
      pool.request().query('SELECT COUNT(*) AS total FROM [PE].[dbo].[tblClient]'),
      pool.request().query('SELECT COUNT(*) AS total FROM [DT].[dbo].[tblDownTimeShort]'),
      pool.request().query('SELECT COUNT(*) AS total FROM [DB].[dbo].[tblDb]'),
      pool.request().query('SELECT COUNT(*) AS total FROM [TEL].[dbo].[tblTel]')
    ]);

    res.json({
      success: true,
      stats: {
        totalUsers: usersCount.recordset[0]?.total || 0,
        totalLogs: logsCount.recordset[0]?.total || 0,
        totalClients: clientsCount.recordset[0]?.total || 0,
        totalDowntimes: dtCount.recordset[0]?.total || 0,
        totalSites: dbCount.recordset[0]?.total || 0,
        totalContacts: telCount.recordset[0]?.total || 0,
        dbServer: process.env.DB_SERVER || '172.16.3.116',
        serverPort: process.env.PORT || 3000
      }
    });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
});

// GET /api/pe/clients - لاگ کلاینت‌ها
router.get('/clients', requireAdminOrPower, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const result = await pool.request().query(`
      SELECT TOP 200 * FROM [PE].[dbo].[tblClient] ORDER BY id DESC
    `);
    res.json({ success: true, data: result.recordset });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET /api/pe/vpns
router.get('/vpns', requireAdminOrPower, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const result = await pool.request().query(`SELECT * FROM [PE].[dbo].[tblExsitVpn]`);
    res.json({ success: true, data: result.recordset });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

module.exports = router;
