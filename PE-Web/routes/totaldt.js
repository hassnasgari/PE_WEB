const express = require('express');
const router = express.Router();
const fs = require('fs');
const path = require('path');
const sql = require('mssql');
const ExcelJS = require('exceljs');
const { logActivity } = require('../utils/logger');

// Middleware for authentication
function requireAuth(req, res, next) {
  if (!req.session.user) {
    return res.status(401).json({ success: false, error: 'احراز هویت انجام نشده است' });
  }
  next();
}

// Convert Gregorian Date to Persian Shamsi (YYYY/MM/DD)
function toShamsi(dateVal) {
  if (!dateVal) return '';
  try {
    const d = new Date(dateVal);
    if (isNaN(d.getTime())) return String(dateVal);
    const parts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit'
    }).formatToParts(d);
    const y = parts.find(p => p.type === 'year').value;
    const m = parts.find(p => p.type === 'month').value;
    const day = parts.find(p => p.type === 'day').value;
    return `${y}/${m}/${day}`;
  } catch (e) {
    return String(dateVal);
  }
}

// Convert Shamsi (1403/05/20 or 1403-05-20) to Gregorian Date Object
function parseShamsiToGregorian(shamsiStr) {
  if (!shamsiStr) return null;
  try {
    const parts = shamsiStr.replace(/-/g, '/').split('/').map(Number);
    if (parts.length !== 3 || isNaN(parts[0]) || isNaN(parts[1]) || isNaN(parts[2])) {
      // maybe it is already gregorian?
      const d = new Date(shamsiStr);
      return isNaN(d.getTime()) ? null : d;
    }
    const [jy, jm, jd] = parts;
    if (jy > 1800) {
      return new Date(jy, jm - 1, jd);
    }
    // Algorithm from jalaali.js
    let gy = jy <= 979 ? 621 : 1600;
    let jy2 = jy <= 979 ? jy : jy - 979;
    let days = (365 * jy2) + (Math.floor(jy2 / 33) * 8) + Math.floor(((jy2 % 33) + 3) / 4) + 78 + jd + ((jm < 7) ? (jm - 1) * 31 : ((jm - 7) * 30) + 186);
    gy += 400 * Math.floor(days / 146097);
    days %= 146097;
    if (days > 36524) {
      gy += 100 * Math.floor(--days / 36524);
      days %= 36524;
      if (days >= 365) days++;
    }
    gy += 4 * Math.floor(days / 1461);
    days %= 1461;
    if (days > 365) {
      gy += Math.floor((days - 1) / 365);
      days = (days - 1) % 365;
    }
    let gd = days + 1;
    const sal_a = [0, 31, ((gy % 4 === 0 && gy % 100 !== 0) || (gy % 400 === 0)) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    let gm;
    for (gm = 0; gm < 13 && gd > sal_a[gm]; gm++) gd -= sal_a[gm];
    return new Date(gy, gm - 1, gd);
  } catch (e) {
    return null;
  }
}

// Format date to SQL ISO string YYYY-MM-DD
function formatDateToSql(d, isEnd = false) {
  if (!d) return null;
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  if (isEnd) {
    return `${year}-${month}-${day} 23:59:59`;
  }
  return `${year}-${month}-${day} 00:00:00`;
}

// -------------------------------------------------------------
// 1. GET /api/totaldt/data - Main Explorer Grid
// -------------------------------------------------------------
router.get('/data', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    let { page = 1, pageSize = 50, search = '', startDate = '', endDate = '', preimpel = '', sortCol = 'CREATETIME', sortDir = 'DESC' } = req.query;
    page = parseInt(page) || 1;
    pageSize = parseInt(pageSize) || 50;
    if (pageSize > 500) pageSize = 500;
    const offset = (page - 1) * pageSize;

    const request = pool.request();
    let whereClauses = [];

    // Date filters
    if (startDate) {
      const gStart = parseShamsiToGregorian(startDate);
      if (gStart) {
        request.input('startDate', sql.DateTime, gStart);
        whereClauses.push(`TRY_CAST(T1.ACTION_START_DATE AS DATETIME) >= @startDate`);
      }
    }
    if (endDate) {
      const gEnd = parseShamsiToGregorian(endDate);
      if (gEnd) {
        gEnd.setHours(23, 59, 59, 999);
        request.input('endDate', sql.DateTime, gEnd);
        whereClauses.push(`TRY_CAST(T1.ACTION_START_DATE AS DATETIME) <= @endDate`);
      }
    }

    // Preimpel filter
    if (preimpel && preimpel.trim() !== '') {
      const preimpelList = preimpel.split('|||').map(p => p.trim()).filter(Boolean);
      if (preimpelList.length > 0) {
        const pNames = [];
        preimpelList.forEach((val, idx) => {
          const pName = `pre_${idx}`;
          request.input(pName, sql.NVarChar, val);
          pNames.push(`@${pName}`);
        });
        whereClauses.push(`T1.PREIMPEL_DESC IN (${pNames.join(',')})`);
      }
    }

    // Global Search
    if (search && search.trim() !== '') {
      request.input('searchParam', sql.NVarChar, `%${search.trim()}%`);
      whereClauses.push(`(
        T1.ORDERID LIKE @searchParam OR
        T1.PROCESSID LIKE @searchParam OR
        T1.TARGET_NODE LIKE @searchParam OR
        T1.TITLE LIKE @searchParam OR
        T1.GROUPNAME LIKE @searchParam OR
        T1.CREATOR_NAME LIKE @searchParam OR
        T1.EXECUTOR_NAME LIKE @searchParam OR
        T1.PREIMPEL_DESC LIKE @searchParam OR
        T1.IMPEL_DESC LIKE @searchParam OR
        T2.[نام سایت] LIKE @searchParam OR
        T2.[منطقه] LIKE @searchParam
      )`);
    }

    const whereStr = whereClauses.length > 0 ? `WHERE ${whereClauses.join(' AND ')}` : '';

    // Allowed sort columns
    const safeSortCols = ['CREATETIME', 'ACTION_START_DATE', 'ORDERID', 'PROCESSID', 'TARGET_NODE', 'GROUPNAME', 'REGION', 'DOWNTIME', 'TITLE'];
    const safeCol = safeSortCols.includes(sortCol.toUpperCase()) ? `T1.[${sortCol.toUpperCase()}]` : `T1.[CREATETIME]`;
    const safeDir = (sortDir.toUpperCase() === 'ASC') ? 'ASC' : 'DESC';

    // Count query
    const countSql = `
      SELECT COUNT(*) AS totalRecords
      FROM [SDM].[dbo].[TotalDownTime] AS T1 WITH (NOLOCK)
      LEFT JOIN [DB].[dbo].[tblDb] AS T2 WITH (NOLOCK) ON T1.TARGET_NODE = T2.[کد سایت]
      ${whereStr}
    `;
    const countResult = await request.query(countSql);
    const totalRecords = countResult.recordset[0]?.totalRecords || 0;

    // Data query
    const dataSql = `
      SELECT 
        T1.[ID], T1.[YEAR], T1.[QUARTER], T1.[WEEK], T1.[FULLDATE], T1.[DAYOFWEEKNAME], T1.[PWEEK_END_DATE],
        T1.[ORDERID], T1.[PROCESSID], T1.[CREATETIME],
        T1.[WORKITEMINDEX], T1.[PROCESSSTATUS], T1.[GROUPNAME], T1.[DEFAULTGROUP], T1.[IMPLEMENTOR],
        T1.[IMPLEMENTOR_GROUP], T1.[AFFECTED_NODE], T1.[PHYSICAL_ACCESS], T1.[DOWNTIME], T1.[REGION],
        T1.[CHANGEINTENTION], T1.[WORKITEMNAME], T1.[BUSINESSSTATUS], T1.[PERFORMMETHOD], T1.[APPROVED_TYPE],
        T1.[CRTYPE_NAME], T1.[DOMAIN], T1.[CHANGE_TYPE], T1.[PROVINCE], T1.[STATE], T1.[CREATOR_DEFAULT_GROUP],
        T1.[PHYSICALACCESSCODE], T1.[PHYSICALACCESS], T1.[OPEARTIONMODEAPPROVECODE1ST], T1.[OPEARTIONMODEAPPROVE1ST],
        T1.[OPEARTIONMODEAPPROVECODE2ST], T1.[OPEARTIONMODEAPPROVE2ST], T1.[OPEARTIONMODEAPPROVECODE3ST],
        T1.[OPEARTIONMODEAPPROVE3ST], T1.[OPEARTIONMODEAPPROVECODE4ST], T1.[OPEARTIONMODEAPPROVE4ST],
        T1.[OPEARTIONMODEAPPROVECODE5ST], T1.[OPEARTIONMODEAPPROVE5ST], T1.[OPEARTIONMODECODEPREIMPLEMENTOR],
        T1.[OPEARTIONMODEPREIMPLEMENTOR], T1.[OPEARTIONMODECODEIMPLEMENTOR], T1.[OPEARTIONMODEIMPLEMENTOR],
        T1.[POSTHEALTHCODE], T1.[POSTHEALTH], T1.[POSTHEATHDESC], T1.[CRVALIDATECODE], T1.[CRVALIDATE],
        T1.[CABAPPROVECODE], T1.[CABAPPROVE], T1.[ARASUPDATECODE], T1.[ARASUPDATE], T1.[ECABCODE], T1.[ECAB],
        T1.[ECABBOCODE], T1.[ECABBO], T1.[KPICHECKCODE], T1.[KPICHECK], T1.[TITLE], T1.[ACTION_START_DATE],
        T1.[ACTION_START_TIME], T1.[OUTAGE_START], T1.[OUTAGE_END], T1.[ACTION_END_DATE], T1.[ACTION_END_TIME],
        T1.[PROCESSOR], T1.[REQUESTTIMECAB], T1.[TARGET_NODE], T1.[RISK], T1.[IMPACTFTA], T1.[CAB_APPROVER_PERSON],
        T1.[CAB_FINISH_TIME], T1.[SUB_CHANGE], T1.[CAB_FINISH_GROUP], T1.[LOCATION], T1.[REASONFORCHANGE],
        T1.[VENDOR], T1.[IMPEL_DESC], T1.[CONFIRM_DESC], T1.[PREIMPEL_DESC], T1.[CREATOR_NAME], T1.[EXECUTOR_NAME],
        T1.[UA_CONFIRM], T2.[نام سایت], T2.[منطقه]
      FROM [SDM].[dbo].[TotalDownTime] AS T1 WITH (NOLOCK)
      LEFT JOIN [DB].[dbo].[tblDb] AS T2 WITH (NOLOCK) ON T1.TARGET_NODE = T2.[کد سایت]
      ${whereStr}
      ORDER BY ${safeCol} ${safeDir}
      OFFSET ${offset} ROWS FETCH NEXT ${pageSize} ROWS ONLY
    `;

    const dataResult = await request.query(dataSql);
    const rows = dataResult.recordset.map(row => {
      const createTimeRaw = row.CREATETIME;
      const actStartRaw = row.ACTION_START_DATE;
      const actEndRaw = row.ACTION_END_DATE;

      return {
        ...row,
        CREATETIME_MILADI: createTimeRaw ? String(createTimeRaw).substring(0, 10) : '',
        CREATETIME_SHAMSI: toShamsi(createTimeRaw),
        ACTION_START_DATE_MILADI: actStartRaw ? String(actStartRaw).substring(0, 10) : '',
        ACTION_START_DATE_SHAMSI: toShamsi(actStartRaw),
        ACTION_END_DATE_MILADI: actEndRaw ? String(actEndRaw).substring(0, 10) : '',
        ACTION_END_DATE_SHAMSI: toShamsi(actEndRaw)
      };
    });

    res.json({
      success: true,
      data: rows,
      total: totalRecords,
      page,
      pageSize,
      totalPages: Math.ceil(totalRecords / pageSize)
    });

  } catch (err) {
    console.error('Error fetching TotalDt data:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 2. GET /api/totaldt/preimpel-list - Distinct PREIMPEL_DESC
// -------------------------------------------------------------
router.get('/preimpel-list', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    const { startDate, endDate } = req.query;
    const request = pool.request();
    let whereClauses = ["PREIMPEL_DESC IS NOT NULL AND LTRIM(RTRIM(PREIMPEL_DESC)) != ''"];

    if (startDate) {
      const gStart = parseShamsiToGregorian(startDate);
      if (gStart) {
        request.input('startDate', sql.DateTime, gStart);
        whereClauses.push(`TRY_CAST(ACTION_START_DATE AS DATETIME) >= @startDate`);
      }
    }
    if (endDate) {
      const gEnd = parseShamsiToGregorian(endDate);
      if (gEnd) {
        gEnd.setHours(23, 59, 59, 999);
        request.input('endDate', sql.DateTime, gEnd);
        whereClauses.push(`TRY_CAST(ACTION_START_DATE AS DATETIME) <= @endDate`);
      }
    }

    const query = `
      SELECT PREIMPEL_DESC FROM (
        SELECT DISTINCT PREIMPEL_DESC,
          CASE WHEN LOWER(LTRIM(RTRIM(PREIMPEL_DESC))) = 'cancel by system' THEN 0 ELSE 1 END AS SortOrder
        FROM [SDM].[dbo].[TotalDownTime] WITH (NOLOCK)
        WHERE ${whereClauses.join(' AND ')}
      ) AS T
      ORDER BY SortOrder, PREIMPEL_DESC
    `;

    const result = await request.query(query);
    const list = result.recordset.map(r => r.PREIMPEL_DESC);
    res.json({ success: true, list });
  } catch (err) {
    console.error('Error fetching PREIMPEL list:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 3. POST /api/totaldt/filtered-data - Get Filtered Table
// -------------------------------------------------------------
router.post('/filtered-data', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    const { startDate, endDate, preimpelValues = [] } = req.body;
    if (!preimpelValues || preimpelValues.length === 0) {
      return res.json({ success: true, data: [], total: 0 });
    }

    const request = pool.request();
    let whereClauses = [];

    if (startDate) {
      const gStart = parseShamsiToGregorian(startDate);
      if (gStart) {
        request.input('startDate', sql.DateTime, gStart);
        whereClauses.push(`TRY_CAST(T1.ACTION_START_DATE AS DATETIME) >= @startDate`);
      }
    }
    if (endDate) {
      const gEnd = parseShamsiToGregorian(endDate);
      if (gEnd) {
        gEnd.setHours(23, 59, 59, 999);
        request.input('endDate', sql.DateTime, gEnd);
        whereClauses.push(`TRY_CAST(T1.ACTION_START_DATE AS DATETIME) <= @endDate`);
      }
    }

    const pNames = [];
    preimpelValues.forEach((val, idx) => {
      const pName = `p_${idx}`;
      request.input(pName, sql.NVarChar, val);
      pNames.push(`@${pName}`);
    });
    whereClauses.push(`T1.PREIMPEL_DESC IN (${pNames.join(',')})`);

    const query = `
      SELECT 
        T1.[YEAR], T1.[QUARTER], T1.[WEEK], T1.[FULLDATE], T1.[DAYOFWEEKNAME], T1.[PWEEK_END_DATE],
        T1.[ORDERID], T1.[PROCESSID], T1.[CREATETIME],
        T1.[WORKITEMINDEX], T1.[PROCESSSTATUS], T1.[GROUPNAME], T1.[DEFAULTGROUP], T1.[IMPLEMENTOR],
        T1.[IMPLEMENTOR_GROUP], T1.[AFFECTED_NODE], T1.[PHYSICAL_ACCESS], T1.[DOWNTIME], T1.[REGION],
        T1.[CHANGEINTENTION], T1.[WORKITEMNAME], T1.[BUSINESSSTATUS], T1.[PERFORMMETHOD], T1.[APPROVED_TYPE],
        T1.[CRTYPE_NAME], T1.[DOMAIN], T1.[CHANGE_TYPE], T1.[PROVINCE], T1.[STATE], T1.[CREATOR_DEFAULT_GROUP],
        T1.[PHYSICALACCESSCODE], T1.[PHYSICALACCESS], T1.[OPEARTIONMODEAPPROVECODE1ST], T1.[OPEARTIONMODEAPPROVE1ST],
        T1.[OPEARTIONMODEAPPROVECODE2ST], T1.[OPEARTIONMODEAPPROVE2ST], T1.[OPEARTIONMODEAPPROVECODE3ST],
        T1.[OPEARTIONMODEAPPROVE3ST], T1.[OPEARTIONMODEAPPROVECODE4ST], T1.[OPEARTIONMODEAPPROVE4ST],
        T1.[OPEARTIONMODEAPPROVECODE5ST], T1.[OPEARTIONMODEAPPROVE5ST], T1.[OPEARTIONMODECODEPREIMPLEMENTOR],
        T1.[OPEARTIONMODEPREIMPLEMENTOR], T1.[OPEARTIONMODECODEIMPLEMENTOR], T1.[OPEARTIONMODEIMPLEMENTOR],
        T1.[POSTHEALTHCODE], T1.[POSTHEALTH], T1.[POSTHEATHDESC], T1.[CRVALIDATECODE], T1.[CRVALIDATE],
        T1.[CABAPPROVECODE], T1.[CABAPPROVE], T1.[ARASUPDATECODE], T1.[ARASUPDATE], T1.[ECABCODE], T1.[ECAB],
        T1.[ECABBOCODE], T1.[ECABBO], T1.[KPICHECKCODE], T1.[KPICHECK], T1.[TITLE], T1.[ACTION_START_DATE],
        T1.[ACTION_START_TIME], T1.[OUTAGE_START], T1.[OUTAGE_END], T1.[ACTION_END_DATE], T1.[ACTION_END_TIME],
        T1.[PROCESSOR], T1.[REQUESTTIMECAB], T1.[TARGET_NODE], T1.[RISK], T1.[IMPACTFTA], T1.[CAB_APPROVER_PERSON],
        T1.[CAB_FINISH_TIME], T1.[SUB_CHANGE], T1.[CAB_FINISH_GROUP], T1.[LOCATION], T1.[REASONFORCHANGE],
        T1.[VENDOR], T1.[IMPEL_DESC], T1.[CONFIRM_DESC], T1.[PREIMPEL_DESC], T1.[CREATOR_NAME], T1.[EXECUTOR_NAME],
        T1.[UA_CONFIRM], T2.[نام سایت], T2.[منطقه]
      FROM [SDM].[dbo].[TotalDownTime] AS T1 WITH (NOLOCK)
      LEFT JOIN [DB].[dbo].[tblDb] AS T2 WITH (NOLOCK) ON T1.TARGET_NODE = T2.[کد سایت]
      WHERE ${whereClauses.join(' AND ')}
      ORDER BY TRY_CAST(T1.CREATETIME AS DATETIME) DESC
    `;

    const result = await request.query(query);
    const rows = result.recordset.map(row => {
      const createTimeRaw = row.CREATETIME;
      const actStartRaw = row.ACTION_START_DATE;
      const actEndRaw = row.ACTION_END_DATE;

      return {
        ...row,
        CREATETIME_MILADI: createTimeRaw ? String(createTimeRaw).substring(0, 10) : '',
        CREATETIME_SHAMSI: toShamsi(createTimeRaw),
        ACTION_START_DATE_MILADI: actStartRaw ? String(actStartRaw).substring(0, 10) : '',
        ACTION_START_DATE_SHAMSI: toShamsi(actStartRaw),
        ACTION_END_DATE_MILADI: actEndRaw ? String(actEndRaw).substring(0, 10) : '',
        ACTION_END_DATE_SHAMSI: toShamsi(actEndRaw)
      };
    });

    res.json({ success: true, data: rows, total: rows.length });
  } catch (err) {
    console.error('Error fetching filtered TotalDt data:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 4. POST /api/totaldt/executor-analysis - Staff Intelligent Matching
// -------------------------------------------------------------
router.post('/executor-analysis', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    const { startDate, endDate, preimpelValues = [] } = req.body;

    // 1. Fetch Staff Reference table
    const staffResult = await pool.request().query(`
      SELECT [NationalCode], [Name_Personnel], [Name_PhoneBook], [Family_Personnel], [Family_PhoneBook], [Mobile_Personnel]
      FROM [EMPLOYEES].[dbo].[staff1] WITH (NOLOCK)
    `);
    const staffList = staffResult.recordset || [];

    // 2. Fetch Data to analyze
    const request = pool.request();
    let whereClauses = [];

    if (startDate) {
      const gStart = parseShamsiToGregorian(startDate);
      if (gStart) {
        request.input('startDate', sql.DateTime, gStart);
        whereClauses.push(`TRY_CAST(ACTION_START_DATE AS DATETIME) >= @startDate`);
      }
    }
    if (endDate) {
      const gEnd = parseShamsiToGregorian(endDate);
      if (gEnd) {
        gEnd.setHours(23, 59, 59, 999);
        request.input('endDate', sql.DateTime, gEnd);
        whereClauses.push(`TRY_CAST(ACTION_START_DATE AS DATETIME) <= @endDate`);
      }
    }
    if (preimpelValues && preimpelValues.length > 0) {
      const pNames = [];
      preimpelValues.forEach((val, idx) => {
        const pName = `p_${idx}`;
        request.input(pName, sql.NVarChar, val);
        pNames.push(`@${pName}`);
      });
      whereClauses.push(`PREIMPEL_DESC IN (${pNames.join(',')})`);
    }

    const whereStr = whereClauses.length > 0 ? `WHERE ${whereClauses.join(' AND ')}` : '';
    const query = `
      SELECT [EXECUTOR_NAME], [CREATOR_NAME], [ORDERID], [TITLE], [GROUPNAME], [CREATETIME]
      FROM [SDM].[dbo].[TotalDownTime] WITH (NOLOCK)
      ${whereStr}
    `;

    const dataResult = await request.query(query);
    const rows = dataResult.recordset || [];

    const nationalCodeRegex = /\b\d{10}\b/;
    const mobileRegex = /(?:\+98|0098|0)?9\d{9}/g;
    const persianRegex = /[\u0600-\u06FF]/;

    // Matching Algorithm
    const countsMap = {};
    let totalCount = 0;

    for (const row of rows) {
      const executor = (row.EXECUTOR_NAME || '').trim();
      const creator = (row.CREATOR_NAME || '').trim();
      let calculatedName = 'نامشخص';

      // 1. National Code check
      let natMatch = executor.match(nationalCodeRegex) || creator.match(nationalCodeRegex);
      if (natMatch) {
        const code = natMatch[0];
        const staff = staffList.find(s => s.NationalCode && s.NationalCode.trim() === code);
        if (staff) {
          calculatedName = `${staff.Name_PhoneBook || ''} ${staff.Family_PhoneBook || ''} (مشابه‌سازی کدملی)`.trim();
        }
      }

      // 2. Cancel by system
      if (calculatedName === 'نامشخص') {
        if (executor.toLowerCase() === 'cancel by system' || creator.toLowerCase() === 'cancel by system') {
          calculatedName = 'cancel by system';
        }
      }

      // 3. Pure Persian Text
      if (calculatedName === 'نامشخص' && executor && persianRegex.test(executor) && !mobileRegex.test(executor)) {
        calculatedName = executor;
      }

      // 4. Mobile phone matching
      if (calculatedName === 'نامشخص') {
        const sourceText = executor || creator;
        const isFromCreator = !executor;

        const mobileMatches = sourceText.match(mobileRegex);
        if (mobileMatches && mobileMatches.length > 0) {
          const rawMob = mobileMatches[0];
          let shortMob = rawMob.length > 10 ? rawMob.substring(rawMob.length - 10) : rawMob;
          if (shortMob.startsWith('0')) shortMob = shortMob.substring(1);

          const staff = staffList.find(s => {
            const dbMob = (s.Mobile_Personnel || '').trim();
            if (!dbMob) return false;
            return dbMob.includes(shortMob) || shortMob.includes(dbMob);
          });

          if (staff) {
            calculatedName = `${staff.Name_PhoneBook || ''} ${staff.Family_PhoneBook || ''} (مشابه‌سازی تلفن)`.trim();
          } else {
            const cleanText = sourceText.replace(mobileRegex, '').replace(/-/g, '').trim();
            if (cleanText && cleanText.length > 2) {
              calculatedName = cleanText;
            }
          }
        }

        // 5. Smart Finglish tokens match
        if (calculatedName === 'نامشخص' && sourceText) {
          const cleanText = sourceText.replace(/[_.-]/g, ' ').toLowerCase();
          const tokens = cleanText.split(/\s+/).filter(Boolean);

          const staff = staffList.find(s => {
            const pName = (s.Name_Personnel || '').trim().toLowerCase();
            const pFamily = (s.Family_Personnel || '').trim().toLowerCase();
            const pPhone = (s.Name_PhoneBook || '').trim().toLowerCase();
            const pFamBook = (s.Family_PhoneBook || '').trim().toLowerCase();
            const fullDbName = `${pName} ${pFamily}`.trim();

            if (fullDbName && (fullDbName.includes(cleanText) || cleanText.includes(fullDbName))) {
              return true;
            }

            if (tokens.length >= 2) {
              const hasFirstName = tokens.some(t => t.length > 2 && (pName.includes(t) || pPhone.includes(t)));
              const hasLastName = tokens.some(t => t.length > 2 && (pFamily.includes(t) || pFamBook.includes(t)));
              return hasFirstName && hasLastName;
            } else if (tokens.length === 1) {
              const single = tokens[0];
              return single.length > 2 && (pName === single || pFamily === single || pPhone === single || pFamBook === single);
            }
            return false;
          });

          if (staff) {
            const marker = isFromCreator ? ' (مشابه‌سازی سازنده)' : ' (مشابه‌سازی فینگلیش)';
            calculatedName = `${staff.Name_PhoneBook || ''} ${staff.Family_PhoneBook || ''}${marker}`.trim();
          }
        }

        if (calculatedName === 'نامشخص' && sourceText) {
          calculatedName = sourceText.replace(/[_.-]/g, ' ').trim();
        }
      }

      if (!calculatedName || calculatedName === 'نامشخص') {
        calculatedName = 'کاربر ثبت نشده / نامشخص';
      }

      countsMap[calculatedName] = (countsMap[calculatedName] || 0) + 1;
      totalCount++;
    }

    // Sort: cancel by system always at the top, then count DESC
    const sortedList = Object.keys(countsMap).map(name => ({
      name,
      count: countsMap[name]
    })).sort((a, b) => {
      const aIsSys = a.name.toLowerCase() === 'cancel by system';
      const bIsSys = b.name.toLowerCase() === 'cancel by system';
      if (aIsSys && !bIsSys) return -1;
      if (!aIsSys && bIsSys) return 1;
      return b.count - a.count;
    });

    const maxCount = sortedList.length > 0 ? Math.max(...sortedList.map(x => x.count)) : 1;

    const resultTable = sortedList.map(item => {
      const pct = totalCount > 0 ? Number(((item.count * 100) / totalCount).toFixed(2)) : 0;
      let barVal = Math.round((item.count * 100) / maxCount);
      if (barVal === 0 && item.count > 0) barVal = 1;
      return {
        executor: item.name,
        count: item.count,
        percentage: pct,
        barPercent: barVal,
        isSystem: item.name.toLowerCase() === 'cancel by system'
      };
    });

    res.json({
      success: true,
      totalRecords: totalCount,
      uniqueExecutors: resultTable.length,
      data: resultTable
    });

  } catch (err) {
    console.error('Error in executor analysis:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 5. POST /api/totaldt/group-stats - Group Statistics & Classification
// -------------------------------------------------------------
function classifyGroupName(groupName) {
  if (!groupName) return 'سایر (Other)';
  const g = groupName.toUpperCase();
  if (g.includes('RAN') || g.includes('BSS') || g.includes('2G') || g.includes('3G') || g.includes('4G') || g.includes('5G') || g.includes('NODE')) return 'RAN / BSS';
  if (g.includes('TX') || g.includes('TRANSMISSION') || g.includes('FIBER') || g.includes('OPTICAL') || g.includes('MW')) return 'انتقال (TX / Transmission)';
  if (g.includes('CORE') || g.includes('CS') || g.includes('PS') || g.includes('EPC') || g.includes('IMS')) return 'شبکه اصلی (Core Network)';
  if (g.includes('IP') || g.includes('ROUTER') || g.includes('SWITCH') || g.includes('DATA')) return 'شبکه دیتا و IP';
  if (g.includes('FM') || g.includes('NOC') || g.includes('FAULT') || g.includes('MONITOR')) return 'مرکز عملیات و نگهداری (NOC / FM)';
  if (g.includes('POWER') || g.includes('BATTERY') || g.includes('RECTIFIER') || g.includes('DIESEL')) return 'تاسیسات و تغذیه نیرو (Power)';
  if (g.includes('CIVIL') || g.includes('TOWER') || g.includes('SITE') || g.includes('AC')) return 'پشتیبانی سایت و فیزیکی (Civil/AC)';
  if (g.includes('IT') || g.includes('SYS') || g.includes('DEV') || g.includes('SERVER')) return 'فناوری اطلاعات و سرور (IT/Systems)';
  return groupName.trim();
}

router.post('/group-stats', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    const { startDate, endDate, preimpelValues = [] } = req.body;
    const request = pool.request();
    let whereClauses = [];

    if (startDate) {
      const gStart = parseShamsiToGregorian(startDate);
      if (gStart) {
        request.input('startDate', sql.DateTime, gStart);
        whereClauses.push(`TRY_CAST(ACTION_START_DATE AS DATETIME) >= @startDate`);
      }
    }
    if (endDate) {
      const gEnd = parseShamsiToGregorian(endDate);
      if (gEnd) {
        gEnd.setHours(23, 59, 59, 999);
        request.input('endDate', sql.DateTime, gEnd);
        whereClauses.push(`TRY_CAST(ACTION_START_DATE AS DATETIME) <= @endDate`);
      }
    }
    if (preimpelValues && preimpelValues.length > 0) {
      const pNames = [];
      preimpelValues.forEach((val, idx) => {
        const pName = `p_${idx}`;
        request.input(pName, sql.NVarChar, val);
        pNames.push(`@${pName}`);
      });
      whereClauses.push(`PREIMPEL_DESC IN (${pNames.join(',')})`);
    }

    const whereStr = whereClauses.length > 0 ? `WHERE ${whereClauses.join(' AND ')}` : '';
    const query = `
      SELECT ISNULL(GROUPNAME, N'نامشخص') AS GROUPNAME, COUNT(*) AS [Count]
      FROM [SDM].[dbo].[TotalDownTime] WITH (NOLOCK)
      ${whereStr}
      GROUP BY GROUPNAME
      ORDER BY COUNT(*) DESC
    `;

    const result = await request.query(query);
    const rawGroups = result.recordset || [];
    const totalCount = rawGroups.reduce((sum, g) => sum + g.Count, 0);

    // Classified Groups
    const classifiedMap = {};
    rawGroups.forEach(g => {
      const domain = classifyGroupName(g.GROUPNAME);
      classifiedMap[domain] = (classifiedMap[domain] || 0) + g.Count;
    });

    const classifiedList = Object.keys(classifiedMap).map(domain => ({
      domain,
      count: classifiedMap[domain],
      percentage: totalCount > 0 ? Number(((classifiedMap[domain] * 100) / totalCount).toFixed(2)) : 0
    })).sort((a, b) => b.count - a.count);

    const detailedList = rawGroups.map(g => ({
      groupName: g.GROUPNAME,
      domain: classifyGroupName(g.GROUPNAME),
      count: g.Count,
      percentage: totalCount > 0 ? Number(((g.Count * 100) / totalCount).toFixed(2)) : 0
    }));

    res.json({
      success: true,
      totalRecords: totalCount,
      classified: classifiedList,
      detailed: detailedList
    });

  } catch (err) {
    console.error('Error fetching group stats:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 6. POST /api/totaldt/import-csv - Upload & Staging Merge
// -------------------------------------------------------------
router.post('/import-csv', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    const { csvContent } = req.body;
    if (!csvContent || typeof csvContent !== 'string') {
      return res.status(400).json({ success: false, error: 'محتوای فایل CSV ارسال نشده است' });
    }

    // Parse CSV lines
    const lines = csvContent.split(/\r?\n/).filter(line => line.trim() !== '');
    if (lines.length < 2) {
      return res.status(400).json({ success: false, error: 'فایل CSV خالی یا فاقد ردیف داده است' });
    }

    // Parse header
    const headers = lines[0].split(',').map(h => h.trim().replace(/^["']|["']$/g, ''));

    // Step 1: Clear Staging Table
    await pool.request().query("DELETE FROM [SDM].[dbo].[TotalDownTime_Staging]");

    // Step 2: Insert into Staging Table in chunks
    const dataRows = [];
    for (let i = 1; i < lines.length; i++) {
      const line = lines[i];
      if (!line.trim()) continue;
      
      // Simple CSV regex splitter that respects quotes
      const values = [];
      let inQuotes = false;
      let curVal = '';
      for (let c = 0; c < line.length; c++) {
        const char = line[c];
        if (char === '"') {
          inQuotes = !inQuotes;
        } else if (char === ',' && !inQuotes) {
          values.push(curVal.trim().replace(/^["']|["']$/g, ''));
          curVal = '';
        } else {
          curVal += char;
        }
      }
      values.push(curVal.trim().replace(/^["']|["']$/g, ''));

      const rowObj = {};
      headers.forEach((h, idx) => {
        const v = values[idx];
        rowObj[h] = (v === undefined || v === '' || v === 'NULL') ? null : v;
      });
      dataRows.push(rowObj);
    }

    // Insert batches into Staging table
    const batchSize = 100;
    for (let i = 0; i < dataRows.length; i += batchSize) {
      const chunk = dataRows.slice(i, i + batchSize);
      const reqBatch = pool.request();
      const insertSqlList = [];

      chunk.forEach((row, rowIdx) => {
        const colNames = [];
        const paramNames = [];

        Object.keys(row).forEach((col, colIdx) => {
          // Verify valid column names
          const safeCol = col.replace(/[^a-zA-Z0-9_]/g, '');
          if (!safeCol) return;
          const param = `p_${rowIdx}_${colIdx}`;
          colNames.push(`[${safeCol}]`);
          paramNames.push(`@${param}`);
          reqBatch.input(param, sql.NVarChar, row[col]);
        });

        if (colNames.length > 0) {
          insertSqlList.push(`INSERT INTO [SDM].[dbo].[TotalDownTime_Staging] (${colNames.join(',')}) VALUES (${paramNames.join(',')})`);
        }
      });

      if (insertSqlList.length > 0) {
        await reqBatch.query(insertSqlList.join(';\n'));
      }
    }

    // Step 3: Execute Merge Procedure
    try {
      await pool.request().execute('[SDM].[dbo].[sp_MergeTotalDownTime]');
    } catch (spErr) {
      console.warn('sp_MergeTotalDownTime not found or failed, falling back to direct merge/insert:', spErr.message);
      // Direct merge if stored procedure does not exist
      await pool.request().query(`
        MERGE [SDM].[dbo].[TotalDownTime] AS Target
        USING [SDM].[dbo].[TotalDownTime_Staging] AS Source
        ON (Target.ORDERID = Source.ORDERID)
        WHEN MATCHED THEN
          UPDATE SET 
            Target.TITLE = Source.TITLE,
            Target.PROCESSSTATUS = Source.PROCESSSTATUS,
            Target.IMPEL_DESC = Source.IMPEL_DESC,
            Target.PREIMPEL_DESC = Source.PREIMPEL_DESC,
            Target.EXECUTOR_NAME = Source.EXECUTOR_NAME
        WHEN NOT MATCHED BY TARGET THEN
          INSERT (ORDERID, PROCESSID, CREATETIME, GROUPNAME, TITLE, TARGET_NODE, ACTION_START_DATE, ACTION_END_DATE, DOWNTIME, PREIMPEL_DESC, IMPEL_DESC, CREATOR_NAME, EXECUTOR_NAME)
          VALUES (Source.ORDERID, Source.PROCESSID, Source.CREATETIME, Source.GROUPNAME, Source.TITLE, Source.TARGET_NODE, Source.ACTION_START_DATE, Source.ACTION_END_DATE, Source.DOWNTIME, Source.PREIMPEL_DESC, Source.IMPEL_DESC, Source.CREATOR_NAME, Source.EXECUTOR_NAME);
      `);
    }

    logActivity('TOTALDT_CSV_IMPORT', `ایمپورت ${dataRows.length} سطر فایل CSV به TotalDownTime`, req.session?.user?.username);

    res.json({
      success: true,
      message: `تعداد ${dataRows.length} سطر از فایل CSV با موفقیت در دیتابیس بارگذاری و ادغام گردید.`
    });

  } catch (err) {
    console.error('Error importing CSV to TotalDt:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 7. GET /api/totaldt/export-excel - Professional Excel Export
// -------------------------------------------------------------
router.get('/export-excel', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    const { startDate, endDate, preimpel, search } = req.query;
    const request = pool.request();
    let whereClauses = [];

    if (startDate) {
      const gStart = parseShamsiToGregorian(startDate);
      if (gStart) {
        request.input('startDate', sql.DateTime, gStart);
        whereClauses.push(`TRY_CAST(T1.ACTION_START_DATE AS DATETIME) >= @startDate`);
      }
    }
    if (endDate) {
      const gEnd = parseShamsiToGregorian(endDate);
      if (gEnd) {
        gEnd.setHours(23, 59, 59, 999);
        request.input('endDate', sql.DateTime, gEnd);
        whereClauses.push(`TRY_CAST(T1.ACTION_START_DATE AS DATETIME) <= @endDate`);
      }
    }
    if (preimpel && preimpel.trim() !== '') {
      const preimpelList = preimpel.split('|||').map(p => p.trim()).filter(Boolean);
      if (preimpelList.length > 0) {
        const pNames = [];
        preimpelList.forEach((val, idx) => {
          const pName = `pre_${idx}`;
          request.input(pName, sql.NVarChar, val);
          pNames.push(`@${pName}`);
        });
        whereClauses.push(`T1.PREIMPEL_DESC IN (${pNames.join(',')})`);
      }
    }
    if (search && search.trim() !== '') {
      request.input('searchParam', sql.NVarChar, `%${search.trim()}%`);
      whereClauses.push(`(
        T1.ORDERID LIKE @searchParam OR
        T1.PROCESSID LIKE @searchParam OR
        T1.TARGET_NODE LIKE @searchParam OR
        T1.TITLE LIKE @searchParam OR
        T1.GROUPNAME LIKE @searchParam OR
        T1.CREATOR_NAME LIKE @searchParam OR
        T1.EXECUTOR_NAME LIKE @searchParam OR
        T2.[نام سایت] LIKE @searchParam
      )`);
    }

    const whereStr = whereClauses.length > 0 ? `WHERE ${whereClauses.join(' AND ')}` : '';

    const query = `
      SELECT TOP 50000
        T1.[ORDERID] AS [شماره سفارش],
        T1.[PROCESSID] AS [شماره فرایند],
        T1.[CREATETIME] AS [زمان ایجاد میلادی],
        T1.[TITLE] AS [عنوان],
        T1.[TARGET_NODE] AS [کد سایت],
        T2.[نام سایت] AS [نام سایت],
        T2.[منطقه] AS [منطقه],
        T1.[GROUPNAME] AS [نام گروه],
        T1.[PROCESSSTATUS] AS [وضعیت فرایند],
        T1.[DOWNTIME] AS [مدت قطعی],
        T1.[ACTION_START_DATE] AS [تاریخ شروع],
        T1.[ACTION_START_TIME] AS [زمان شروع],
        T1.[ACTION_END_DATE] AS [تاریخ پایان],
        T1.[ACTION_END_TIME] AS [زمان پایان],
        T1.[CREATOR_NAME] AS [نام سازنده],
        T1.[EXECUTOR_NAME] AS [نام انجام‌دهنده],
        T1.[PREIMPEL_DESC] AS [شرح پیش از اجرا],
        T1.[IMPEL_DESC] AS [شرح حین اجرا],
        T1.[CONFIRM_DESC] AS [شرح تایید]
      FROM [SDM].[dbo].[TotalDownTime] AS T1 WITH (NOLOCK)
      LEFT JOIN [DB].[dbo].[tblDb] AS T2 WITH (NOLOCK) ON T1.TARGET_NODE = T2.[کد سایت]
      ${whereStr}
      ORDER BY TRY_CAST(T1.CREATETIME AS DATETIME) DESC
    `;

    const result = await request.query(query);
    const rows = result.recordset || [];

    const workbook = new ExcelJS.Workbook();
    workbook.creator = 'PE Web System';
    workbook.created = new Date();

    const sheet = workbook.addWorksheet('TotalDownTime Report', {
      views: [{ rightToLeft: true }]
    });

    if (rows.length > 0) {
      const headers = Object.keys(rows[0]);
      sheet.columns = headers.map(h => ({
        header: h,
        key: h,
        width: Math.max(h.length + 5, 15)
      }));

      // Add rows with Shamsi date conversions
      rows.forEach(r => {
        const rowData = { ...r };
        if (rowData['تاریخ شروع']) rowData['تاریخ شروع'] = toShamsi(rowData['تاریخ شروع']);
        if (rowData['تاریخ پایان']) rowData['تاریخ پایان'] = toShamsi(rowData['تاریخ پایان']);
        sheet.addRow(rowData);
      });

      // Style header row
      const headerRow = sheet.getRow(1);
      headerRow.font = { name: 'Vazirmatn', bold: true, color: { argb: 'FFFFFFFF' }, size: 10 };
      headerRow.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FF0284C7' } // Blue header
      };
      headerRow.alignment = { vertical: 'middle', horizontal: 'center' };
      headerRow.height = 25;

      // Style data rows
      sheet.eachRow((row, rowNum) => {
        if (rowNum > 1) {
          row.font = { name: 'Vazirmatn', size: 9 };
          row.alignment = { vertical: 'middle', horizontal: 'right' };
          row.eachCell(cell => {
            cell.border = {
              top: { style: 'thin', color: { argb: 'FFE2E8F0' } },
              bottom: { style: 'thin', color: { argb: 'FFE2E8F0' } },
              left: { style: 'thin', color: { argb: 'FFE2E8F0' } },
              right: { style: 'thin', color: { argb: 'FFE2E8F0' } }
            };
          });
        }
      });
    }

    res.setHeader('Content-Type', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
    res.setHeader('Content-Disposition', `attachment; filename=TotalDt_Report_${Date.now()}.xlsx`);

    await workbook.xlsx.write(res);
    res.end();

  } catch (err) {
    console.error('Error exporting TotalDt to Excel:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 8. POST /api/totaldt/clear-all - Delete all records
// -------------------------------------------------------------
router.post('/clear-all', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    await pool.request().query(`
      DELETE FROM [SDM].[dbo].[TotalDownTime];
      DELETE FROM [SDM].[dbo].[TotalDownTime_Staging];
    `);

    logActivity('TOTALDT_CLEAR_ALL', 'پاکسازی کامل جداول TotalDownTime و Staging', req.session?.user?.username);

    res.json({ success: true, message: 'کلیه داده‌های TotalDownTime و جدول Staging با موفقیت پاکسازی شدند.' });
  } catch (err) {
    console.error('Error clearing TotalDt data:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

// -------------------------------------------------------------
// 9. BACKUP & RESTORE ROUTES
// -------------------------------------------------------------
const BACKUP_DIR = 'C:\\TotalDt_Backups';

router.get('/backups', requireAuth, async (req, res) => {
  try {
    if (!fs.existsSync(BACKUP_DIR)) {
      fs.mkdirSync(BACKUP_DIR, { recursive: true });
    }
    const files = fs.readdirSync(BACKUP_DIR)
      .filter(f => f.toLowerCase().endsWith('.bak'))
      .map(f => {
        const fullPath = path.join(BACKUP_DIR, f);
        const stats = fs.statSync(fullPath);
        return {
          fileName: f,
          fullPath,
          sizeBytes: stats.size,
          sizeFormatted: (stats.size / (1024 * 1024)).toFixed(2) + ' MB',
          createdAt: stats.birthtime,
          shamsiDate: toShamsi(stats.birthtime)
        };
      })
      .sort((a, b) => b.createdAt - a.createdAt);

    res.json({ success: true, backups: files, backupFolder: BACKUP_DIR });
  } catch (err) {
    console.error('Error listing backups:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

router.post('/backup-now', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    if (!fs.existsSync(BACKUP_DIR)) {
      fs.mkdirSync(BACKUP_DIR, { recursive: true });
    }

    const timestamp = new Date().toISOString().replace(/[-:T]/g, '_').substring(0, 15);
    const backupFileName = `SDM_Backup_${timestamp}.bak`;
    const backupFilePath = path.join(BACKUP_DIR, backupFileName);

    const query = `
      BACKUP DATABASE [SDM]
      TO DISK = '${backupFilePath.replace(/'/g, "''")}'
      WITH FORMAT, INIT, NAME = N'SDM-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10
    `;

    await pool.request().query(query);

    logActivity('TOTALDT_BACKUP_CREATED', `تهیه فایل پشتیبان ${backupFileName}`, req.session?.user?.username);

    res.json({
      success: true,
      message: `فایل پشتیبان با موفقیت ایجاد گردید: ${backupFileName}`,
      fileName: backupFileName,
      filePath: backupFilePath
    });
  } catch (err) {
    console.error('Error creating backup:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

router.post('/restore', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ success: false, error: 'اتصال به دیتابیس برقرار نیست' });

    const { fileName } = req.body;
    if (!fileName) return res.status(400).json({ success: false, error: 'نام فایل بک‌آپ ارسال نشده است' });

    const backupFilePath = path.join(BACKUP_DIR, fileName);
    if (!fs.existsSync(backupFilePath)) {
      return res.status(404).json({ success: false, error: 'فایل پشتیبان در سرور یافت نشد' });
    }

    const restoreQuery = `
      USE [master];
      ALTER DATABASE [SDM] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
      RESTORE DATABASE [SDM] FROM DISK = '${backupFilePath.replace(/'/g, "''")}' WITH REPLACE;
      ALTER DATABASE [SDM] SET MULTI_USER;
    `;

    await pool.request().query(restoreQuery);

    logActivity('TOTALDT_DATABASE_RESTORED', `بازیابی دیتابیس SDM از فایل ${fileName}`, req.session?.user?.username);

    res.json({
      success: true,
      message: `دیتابیس SDM با موفقیت از فایل ${fileName} بازیابی گردید.`
    });
  } catch (err) {
    console.error('Error restoring database:', err);
    res.status(500).json({ success: false, error: err.message });
  }
});

module.exports = router;
