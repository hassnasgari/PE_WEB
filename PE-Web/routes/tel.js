const express = require('express');
const router = express.Router();

function requireAuth(req, res, next) {
  if (!req.session.user) return res.status(401).json({ error: 'لاگین نشده‌اید' });
  next();
}

// GET /api/tel/list
router.get('/list', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const result = await pool.request().query(`
      SELECT telId AS [ردیف], Name AS [نام و نام خانوادگی], tel AS [شماره تلفن], WorkPlace AS [محل کار]
      FROM [TEL].[dbo].[tblTel]
      ORDER BY Name
    `);
    res.json({ success: true, data: result.recordset, count: result.recordset.length });
  } catch (err) {
    console.error('TEL list error:', err);
    res.status(500).json({ error: err.message });
  }
});

// POST /api/tel/add
router.post('/add', requireAuth, async (req, res) => {
  const { name, tel, workPlace } = req.body;
  const role = req.session.user.role;
  if (!['administrator', 'PowerUser', 'Support'].includes(role)) {
    return res.status(403).json({ error: 'دسترسی ندارید' });
  }
  try {
    const pool = req.app.locals.getPool();
    const sql = req.app.locals.sql;
    await pool.request()
      .input('name', sql.NVarChar, name)
      .input('tel', sql.NVarChar, tel)
      .input('wp', sql.NVarChar, workPlace || '')
      .query(`INSERT INTO [TEL].[dbo].[tblTel] (Name, tel, WorkPlace) VALUES (@name, @tel, @wp)`);
    res.json({ success: true });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// PUT /api/tel/:id
router.put('/:id', requireAuth, async (req, res) => {
  const role = req.session.user.role;
  if (!['administrator', 'PowerUser', 'Support'].includes(role)) {
    return res.status(403).json({ error: 'دسترسی ندارید' });
  }
  const { name, tel, workPlace } = req.body;
  try {
    const pool = req.app.locals.getPool();
    const sql = req.app.locals.sql;
    await pool.request()
      .input('id', sql.Int, parseInt(req.params.id))
      .input('name', sql.NVarChar, name)
      .input('tel', sql.NVarChar, tel)
      .input('wp', sql.NVarChar, workPlace || '')
      .query(`UPDATE [TEL].[dbo].[tblTel] SET Name=@name, tel=@tel, WorkPlace=@wp WHERE telId=@id`);
    res.json({ success: true });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// DELETE /api/tel/:id
router.delete('/:id', requireAuth, async (req, res) => {
  const role = req.session.user.role;
  if (!['administrator', 'PowerUser', 'Support'].includes(role)) {
    return res.status(403).json({ error: 'دسترسی ندارید' });
  }
  try {
    const pool = req.app.locals.getPool();
    const sql = req.app.locals.sql;
    await pool.request()
      .input('id', sql.Int, parseInt(req.params.id))
      .query(`DELETE FROM [TEL].[dbo].[tblTel] WHERE telId=@id`);
    res.json({ success: true });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

module.exports = router;
