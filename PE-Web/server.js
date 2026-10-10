require('dotenv').config();
const express = require('express');
const session = require('express-session');
const cors = require('cors');
const path = require('path');
const sql = require('mssql');

const app = express();
const PORT = process.env.PORT || 3000;

// ===== DB CONFIG =====
const dbConfig = {
  server: process.env.DB_SERVER,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  port: parseInt(process.env.DB_PORT) || 1433,
  options: {
    encrypt: false,
    trustServerCertificate: true,
    enableArithAbort: true,
    connectTimeout: 30000,
    requestTimeout: 60000,
  },
  pool: {
    max: 10,
    min: 0,
    idleTimeoutMillis: 30000
  }
};

// اتصال به SQL Server
let pool;
async function connectDB() {
  try {
    pool = await sql.connect(dbConfig);
    console.log(`[SQL Server] Connected successfully to: ${process.env.DB_SERVER}`);
    return pool;
  } catch (err) {
    console.error(`[SQL Server Error] Connection failed: ${err.message}`);
    // تلاش مجدد بعد از 5 ثانیه
    setTimeout(connectDB, 5000);
  }
}

// expose pool to routes
app.locals.getPool = () => pool;
app.locals.sql = sql;

// ===== MIDDLEWARES =====
app.use(cors());
app.use(express.json({ limit: '50mb' }));
app.use(express.urlencoded({ limit: '50mb', extended: true }));
app.use(express.static(path.join(__dirname, 'public')));

app.use(session({
  secret: process.env.SESSION_SECRET || 'pe-secret',
  resave: false,
  saveUninitialized: false,
  cookie: {
    secure: false,
    maxAge: 8 * 60 * 60 * 1000 // 8 ساعت
  }
}));

// ===== AUTO-LOGIN MIDDLEWARE (DIRECT UNLIMITED ACCESS) =====
app.use((req, res, next) => {
  if (!req.session.user) {
    req.session.user = {
      username: 'admin',
      fullName: 'مدیر سیستم',
      role: 'administrator',
      company: 'MCI'
    };
  }
  next();
});

// ===== ROUTES =====
app.use('/api/auth',   require('./routes/auth'));
app.use('/api/dt',     require('./routes/dt'));
app.use('/api/db',     require('./routes/db'));
app.use('/api/tel',    require('./routes/tel'));
app.use('/api/req',    require('./routes/req'));
app.use('/api/pe',     require('./routes/pe'));
app.use('/api/cr',     require('./routes/cr'));
app.use('/api/totaldt', require('./routes/totaldt'));
app.use('/api/backup', require('./routes/backup'));

// ===== HEALTH CHECK =====
app.get('/api/health', async (req, res) => {
  try {
    const p = app.locals.getPool();
    if (p) {
      await p.request().query('SELECT 1');
      res.json({ status: 'ok', db: 'connected', server: process.env.DB_SERVER });
    } else {
      res.json({ status: 'ok', db: 'connecting...', server: process.env.DB_SERVER });
    }
  } catch(e) {
    res.json({ status: 'ok', db: 'error: ' + e.message });
  }
});

// ===== LIVE UPDATES (SSE - Instant Trigger Based Notifications) =====
let lastMaxSignalId = null;

app.get('/api/live-updates', (req, res) => {
  res.setHeader('Content-Type', 'text/event-stream');
  res.setHeader('Cache-Control', 'no-cache');
  res.setHeader('Connection', 'keep-alive');
  res.flushHeaders();

  const intervalId = setInterval(async () => {
    const p = app.locals.getPool();
    if (!p) return;
    try {
      const result = await p.request().query(`
        SELECT ISNULL(MAX(id), 0) AS maxId FROM [DT].[dbo].[tblDbChangeSignal]
      `);
      const currentMaxId = result.recordset[0]?.maxId ?? 0;

      if (lastMaxSignalId !== null && currentMaxId > lastMaxSignalId) {
        res.write(`data: ${JSON.stringify({ type: 'db-changed' })}\n\n`);
      }

      lastMaxSignalId = currentMaxId;
    } catch (e) {
      console.error('SSE monitoring error:', e.message);
    }
  }, 1500);

  req.on('close', () => {
    clearInterval(intervalId);
  });
});


// ===== 404 & SPA FALLBACK =====
app.use((req, res, next) => {
  if (req.path.startsWith('/api')) {
    return res.status(404).json({ success: false, error: `مسیر ${req.method} ${req.path} در سرور یافت نشد.` });
  }
  if (req.method === 'GET') {
    return res.sendFile(path.join(__dirname, 'public', 'index.html'));
  }
  res.status(404).json({ success: false, error: 'آدرس مورد نظر یافت نشد' });
});

// ===== START =====
app.listen(PORT, () => {
  console.log(`[PE Server] Server is running at: http://localhost:${PORT}`);
  console.log(`[Database] Target Server: ${process.env.DB_SERVER}`);
  connectDB();
});
