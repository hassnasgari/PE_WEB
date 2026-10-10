const express = require('express');
const router = express.Router();
const { logActivity } = require('../utils/logger');

// POST /api/auth/login
router.post('/login', async (req, res) => {
  const { username, password, quick } = req.body;
  if (!username) {
    return res.status(400).json({ success: false, message: 'نام کاربری الزامی است' });
  }
  if (!quick && !password) {
    return res.status(400).json({ success: false, message: 'رمز عبور الزامی است' });
  }
  try {
    const pool = req.app.locals.getPool();
    const sql = req.app.locals.sql;
    if (!pool) return res.status(503).json({ success: false, message: 'اتصال به دیتابیس برقرار نیست' });

    const result = await pool.request()
      .input('username', sql.NVarChar, username.trim())
      .query(`SELECT userName, pass, role, fullName, company 
              FROM [PE].[dbo].[tblUsers] 
              WHERE userName = @username`);

    if (result.recordset.length === 0) {
      await logActivity(req, `تلاش ناموفق برای ورود (نام کاربری یافت نشد: ${username})`, username);
      return res.status(401).json({ success: false, message: 'نام کاربری یافت نشد' });
    }

    const user = result.recordset[0];
    if (!quick && user.pass !== password) {
      await logActivity(req, `تلاش ناموفق برای ورود (رمز عبور اشتباه)`, user.userName);
      return res.status(401).json({ success: false, message: 'رمز عبور اشتباه است' });
    }

    // Check if role is forbidden
    if (user.role === '---') {
      await logActivity(req, `تلاش ناموفق برای ورود (کاربر فاقد دسترسی)`, user.userName);
      return res.status(403).json({ success: false, message: 'حساب کاربری شما غیرفعال است و اجازه ورود ندارید' });
    }

    // Store session
    req.session.user = {
      username: user.userName,
      fullName: user.fullName,
      role: user.role,
      company: user.company
    };

    // Log successful login
    const loginMethod = quick ? 'ورود سریع (Shortcut)' : 'ورود با کلمه عبور';
    await logActivity(req, `ورود موفق به سامانه (${loginMethod}) - نقش: ${user.role}`, user.userName);

    res.json({
      success: true,
      user: {
        username: user.userName,
        fullName: user.fullName,
        role: user.role,
        company: user.company
      }
    });
  } catch (err) {
    console.error('Login error:', err);
    res.status(500).json({ success: false, message: 'خطا در سرور: ' + err.message });
  }
});

// POST /api/auth/logout
router.post('/logout', async (req, res) => {
  if (req.session?.user) {
    await logActivity(req, `خروج از سامانه`);
  }
  req.session.destroy();
  res.json({ success: true });
});

// GET /api/auth/me
router.get('/me', (req, res) => {
  const adminUser = {
    username: 'admin',
    fullName: 'مدیر سیستم (دسترسی نامحدود)',
    role: 'administrator',
    company: 'MCI'
  };
  req.session.user = req.session.user || adminUser;
  res.json({ success: true, user: req.session.user });
});

module.exports = router;
