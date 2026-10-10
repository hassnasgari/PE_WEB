const express = require('express');
const router = express.Router();

function requireAuth(req, res, next) {
  if (!req.session.user) return res.status(401).json({ error: 'لاگین نشده‌اید' });
  next();
}

// GET /api/req/list
router.get('/list', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const result = await pool.request().query(`
      SELECT * FROM [PE].[dbo].[tblRequestChange] ORDER BY reqNo DESC
    `);
    res.json({ success: true, data: result.recordset, count: result.recordset.length });
  } catch (err) {
    console.error('REQ list error:', err);
    res.status(500).json({ error: err.message });
  }
});

// POST /api/req/create (Create new change request)
router.post('/create', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const { requester, tel, requestText } = req.body;
    if (!tel || !tel.trim()) return res.status(400).json({ error: 'لطفاً شماره تماس را وارد کنید.' });
    if (!requestText || !requestText.trim()) return res.status(400).json({ error: 'لطفاً متن درخواست را وارد کنید.' });

    const username = req.session.user.username || 'کاربر';
    const fullName = req.session.user.fullName || username;

    // Get max reqNo + 1
    const maxRes = await pool.request().query(`
      SELECT ISNULL(MAX(reqNo), 0) + 1 AS nextReqNo FROM [PE].[dbo].[tblRequestChange]
    `);
    const nextReqNo = maxRes.recordset[0].nextReqNo;

    const now = new Date();
    const timeStr = now.getHours().toString().padStart(2, '0') + ':' + now.getMinutes().toString().padStart(2, '0');
    const reqDateTime = `${now.getFullYear()}/${(now.getMonth()+1).toString().padStart(2, '0')}/${now.getDate().toString().padStart(2, '0')}__${timeStr}`;

    await pool.request()
      .input('reqNo', nextReqNo)
      .input('user', username)
      .input('requester', requester || fullName)
      .input('tel', tel)
      .input('request', requestText)
      .input('reqDateTime', reqDateTime)
      .input('status', 'منتظر بررسی')
      .query(`
        INSERT INTO [PE].[dbo].[tblRequestChange] ([reqNo], [user], [requester], [tel], [request], [requestDateTime], [status], [comments])
        VALUES (@reqNo, @user, @requester, @tel, @request, @reqDateTime, @status, N'')
      `);

    // Signal DB Change for instant SSE notification
    await pool.request().query(`
      INSERT INTO [DT].[dbo].[tblDbChangeSignal] (tableName, actionType) 
      VALUES ('tblRequestChange', 'NEW_REQ');
    `);

    res.json({ success: true, message: 'درخواست شما با موفقیت ثبت گردید.' });
  } catch (err) {
    console.error('Create REQ error:', err);
    res.status(500).json({ error: err.message });
  }
});

// POST /api/req/update-status (Approve or Reject request with comments)
router.post('/update-status', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const { reqNo, status, comments } = req.body;
    if (!reqNo) return res.status(400).json({ error: 'شماره درخواست الزامی است.' });

    await pool.request()
      .input('reqNo', reqNo)
      .input('status', status || 'دیده‌شده و در حال بررسی')
      .input('comments', comments || '')
      .query(`
        UPDATE [PE].[dbo].[tblRequestChange]
        SET [status] = @status,
            [comments] = @comments
        WHERE [reqNo] = @reqNo
      `);

    // Signal DB Change for SSE notification
    await pool.request().query(`
      INSERT INTO [DT].[dbo].[tblDbChangeSignal] (tableName, actionType) 
      VALUES ('tblRequestChange', 'UPDATE_REQ');
    `);

    res.json({ success: true, message: 'وضعیت درخواست به‌روزرسانی شد.' });
  } catch (err) {
    console.error('Update REQ status error:', err);
    res.status(500).json({ error: err.message });
  }
});

module.exports = router;
