// utils/logger.js - Enterprise Logging Service for PE System
const sql = require('mssql');

/**
 * Get formatted Shamsi Date & Persian Day of week
 * Example: 'یک شنبه  1405/05/25'
 */
function getShamsiLogDate() {
  try {
    const d = new Date();
    const parts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      weekday: 'long'
    }).formatToParts(d);

    const weekday = parts.find(p => p.type === 'weekday')?.value || '';
    const y = parts.find(p => p.type === 'year')?.value || '';
    const m = parts.find(p => p.type === 'month')?.value || '';
    const day = parts.find(p => p.type === 'day')?.value || '';

    return `${weekday}  ${y}/${m}/${day}`;
  } catch (e) {
    return '1405/05/25';
  }
}

/**
 * Get current time in H:m:s
 */
function getLogTime() {
  const d = new Date();
  return `${d.getHours()}:${d.getMinutes()}:${d.getSeconds()}`;
}

/**
 * Extract clean client IP
 */
function getClientIp(req) {
  if (!req) return '127.0.0.1';
  let ip = req.headers['x-forwarded-for'] || req.socket?.remoteAddress || req.ip || '127.0.0.1';
  if (ip.includes('::ffff:')) {
    ip = ip.replace('::ffff:', '');
  }
  if (ip === '::1') ip = '127.0.0.1';
  return ip;
}

/**
 * Log activity to [PE].[dbo].[tblLog]
 */
async function logActivity(req, action, customUser = null) {
  try {
    const pool = req?.app?.locals?.getPool ? req.app.locals.getPool() : null;
    if (!pool) return;

    const username = customUser || req?.session?.user?.username || 'Guest';
    const logDate = getShamsiLogDate();
    const logTime = getLogTime();
    const clientIp = getClientIp(req);
    const computerName = req?.headers['x-client-hostname'] || 'PE-WEB-CLIENT';
    const mac = 'WebSession';
    const selectedNet = ' * Net is MCI * ';

    const request = pool.request();
    request.input('userName', sql.NVarChar(50), String(username).substring(0, 50));
    request.input('action', sql.NVarChar(500), String(action).substring(0, 500));
    request.input('date', sql.NVarChar(50), logDate);
    request.input('time', sql.NVarChar(50), logTime);
    request.input('mac', sql.NVarChar(50), mac);
    request.input('computerName', sql.NVarChar(100), computerName);
    request.input('SelectedNet', sql.NVarChar(50), selectedNet);
    request.input('Ip', sql.NVarChar(50), clientIp);

    await request.query(`
      INSERT INTO [PE].[dbo].[tblLog] 
      (userName, [action], [date], [time], mac, computerName, SelectedNet, Ip)
      VALUES 
      (@userName, @action, @date, @time, @mac, @computerName, @SelectedNet, @Ip)
    `);
  } catch (err) {
    console.error('⚠️ Logging failed:', err.message);
  }
}

module.exports = {
  logActivity,
  getShamsiLogDate,
  getLogTime,
  getClientIp
};
