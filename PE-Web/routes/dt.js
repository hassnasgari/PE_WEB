const express = require('express');
const router = express.Router();
const fs = require('fs');
const path = require('path');
const sql = require('mssql');
const xlsx = require('xlsx');
const ExcelJS = require('exceljs');
const { logActivity } = require('../utils/logger');



function requireAuth(req, res, next) {
  if (!req.session.user) return res.status(401).json({ error: 'لاگین نشده‌اید' });
  next();
}

function getShamsiDateMinusDays(daysAgo) {
  try {
    const d = new Date();
    d.setDate(d.getDate() - daysAgo);
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
    return null;
  }
}

// GET /api/dt/list?period=15days|month|twomonth|all&filter=...
router.get('/list', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const { period, filter } = req.query;
    let whereClauses = [];

    // فیلتر بازه زمانی واقعی (بر اساس تاریخ ثبت شمسی)
    if (period === 'today') {
      const today = getShamsiDateMinusDays(0);
      if (today) whereClauses.push(`registerDate = '${today}'`);
    } else if (period === 'week') {
      const minDate = getShamsiDateMinusDays(7);
      if (minDate) whereClauses.push(`registerDate >= '${minDate}'`);
    } else if (period === '15days') {
      const minDate = getShamsiDateMinusDays(15);
      if (minDate) whereClauses.push(`registerDate >= '${minDate}'`);
    } else if (period === 'month') {
      const minDate = getShamsiDateMinusDays(30);
      if (minDate) whereClauses.push(`registerDate >= '${minDate}'`);
    } else if (period === 'twomonth') {
      const minDate = getShamsiDateMinusDays(60);
      if (minDate) whereClauses.push(`registerDate >= '${minDate}'`);
    }

    // فیلتر وضعیت بر اساس ستون status مطبق با نسخه دسکتاپ C#
    if (filter === 'running') {
      const today = getShamsiDateMinusDays(0);
      const now = new Date();
      const curTime = `${String(now.getHours()).padStart(2, '0')}:${String(now.getMinutes()).padStart(2, '0')}`;
      if (today) {
        whereClauses.push(`(startDate = '${today}' AND '${curTime}' >= startTime AND '${curTime}' <= endTime AND status = N'تائید')`);
      } else {
        whereClauses.push(`(status = N'تائید')`);
      }
    } else if (filter === 'user_registered') {
      whereClauses.push(`status = N'ثبت توسط کاربر'`);
    } else if (filter === 'session') {
      whereClauses.push(`status = N'منتظر ارائه در جلسه'`);
    } else if (filter === 'sdm') {
      whereClauses.push(`status = N'ثبت در (SDM)'`);
    } else if (filter === 'processing') {
      whereClauses.push(`status = N'در حال پردازش درخواست'`);
    } else if (filter === 'repeated') {
      whereClauses.push(`[DT].[dbo].[tblDownTimeShort].row IN (SELECT repeatedRow FROM [DT].[dbo].[tblRepetedDt])`);
    } else if (filter === 'needcheck') {
      whereClauses.push(`status IN (N'ثبت در (SDM)', N'ثبت توسط کاربر', N'پردازش فنی', N'در حال پردازش درخواست', N'عدم تائید(موقت)', N'در انتظار تایید درخواست مرتبط')`);
    } else if (filter === 'finalcheck') {
      whereClauses.push(`status IN (N'ثبت در (SDM)', N'ثبت توسط کاربر', N'پردازش فنی', N'در حال پردازش درخواست', N'عدم تائید(موقت)', N'در انتظار تایید درخواست مرتبط', N'منتظر ارائه در جلسه')`);
    }

    const whereStr = whereClauses.length > 0 ? 'WHERE ' + whereClauses.join(' AND ') : '';

    const query = `
      SELECT
        row AS [ردیف],
        haveAttachedFile AS [ضمیمه],
        registerDate AS [تاریخ ثبت],
        registerTime AS [ثبت],
        siteCode AS [کد سایت],
        siteName AS [نام سایت],
        region AS [منطقه],
        startDate AS [تاریخ شروع],
        startTime AS [زمان شروع],
        endDate AS [تاریخ پایان],
        endTime AS [زمان پایان],
        outageMin AS [زمان قطعی],
        applicant AS [درخواست کننده],
        subject AS [موضوع],
        performer AS [انجام دهنده],
        approver AS [ثبت کننده],
        dtNo AS [شماره],
        status AS [وضعیت],
        Note AS [یادداشت],
        Activity AS [فعالیت],
        domain AS [دامنه],
        curBSC AS [BSC فعلی]
      FROM [DT].[dbo].[tblDownTimeShort]
      ${whereStr}
      ORDER BY TRY_CAST(LTRIM(RTRIM([row])) AS bigint) DESC
    `;

    const result = await pool.request().query(query);
    res.json({ success: true, data: result.recordset, count: result.recordset.length });
  } catch (err) {
    console.error('DT list error:', err);
    res.status(500).json({ error: err.message });
  }
});

// GET /api/dt/stats - آمار خلاصه
router.get('/stats', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const result = await pool.request().query(`
      SELECT status AS [وضعیت], COUNT(*) AS [تعداد]
      FROM [DT].[dbo].[tblDownTimeShort]
      GROUP BY status
      ORDER BY COUNT(*) DESC
    `);
    res.json({ success: true, data: result.recordset });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET /api/dt/detail/:row
router.get('/detail/:row', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });
    const rowId = parseInt(req.params.row) || 0;
    const result = await pool.request()
      .input('rowId', sql.Int, rowId)
      .query(`
        SELECT
          row AS [ردیف],
          haveAttachedFile AS [ضمیمه],
          registerDate AS [تاریخ ثبت],
          registerTime AS [ثبت],
          siteCode AS [کد سایت],
          siteName AS [نام سایت],
          region AS [منطقه],
          startDate AS [تاریخ شروع],
          startTime AS [زمان شروع],
          endDate AS [تاریخ پایان],
          endTime AS [زمان پایان],
          outageMin AS [زمان قطعی],
          applicant AS [درخواست کننده],
          subject AS [موضوع],
          performer AS [انجام دهنده],
          approver AS [ثبت کننده],
          dtNo AS [شماره],
          status AS [وضعیت],
          Note AS [یادداشت],
          Activity AS [فعالیت],
          domain AS [دامنه],
          curBSC AS [BSC فعلی]
        FROM [DT].[dbo].[tblDownTimeShort]
        WHERE row = @rowId
      `);
    if (result.recordset.length > 0) {
      res.json({ success: true, data: result.recordset[0] });
    } else {
      res.json({ success: false, error: 'ردیف مورد نظر یافت نشد' });
    }
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET /api/dt/subjects - دریافت لیست موضوعات ثبت شده در دیتابیس
router.get('/subjects', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });
    const result = await pool.request().query(`
      SELECT DISTINCT subject 
      FROM [DT].[dbo].[tblDownTimeShort] 
      WHERE subject IS NOT NULL AND LTRIM(RTRIM(subject)) <> ''
      ORDER BY subject
    `);
    const subjects = result.recordset.map(r => r.subject.trim());
    res.json({ success: true, subjects });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET /api/dt/performers - دریافت انجام دهندگان از دفتر تلفن و دیتابیس
router.get('/performers', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });
    
    // Fetch from TEL database phonebook
    const telRes = await pool.request().query(`
      SELECT Name AS performer, WorkPlace AS Office_PhoneBook, tel AS Mobile_Personnel
      FROM [TEL].[dbo].[tblTel]
      WHERE Name IS NOT NULL AND LTRIM(RTRIM(Name)) <> ''
      ORDER BY Name
    `);

    // Fetch distinct performers from DT records
    const dtRes = await pool.request().query(`
      SELECT DISTINCT performer
      FROM [DT].[dbo].[tblDownTimeShort]
      WHERE performer IS NOT NULL AND LTRIM(RTRIM(performer)) <> ''
    `);

    const performersMap = new Map();
    (telRes.recordset || []).forEach(p => {
      if (p.performer) {
        performersMap.set(p.performer.trim(), {
          performer: p.performer.trim(),
          Office_PhoneBook: p.Office_PhoneBook || '---',
          Mobile_Personnel: p.Mobile_Personnel || '---'
        });
      }
    });

    (dtRes.recordset || []).forEach(p => {
      const name = p.performer.trim();
      if (name && !performersMap.has(name)) {
        performersMap.set(name, {
          performer: name,
          Office_PhoneBook: 'سوابق دان‌تایم',
          Mobile_Personnel: '---'
        });
      }
    });

    res.json({ success: true, performers: Array.from(performersMap.values()) });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET /api/dt/download/:row
router.get('/download/:row', requireAuth, async (req, res) => {
  const row = req.params.row;
  const candidateDirs = [
    process.env.ATTACHMENT_PATH,
    'Y:\\DtAttachFile',
    '\\\\172.16.3.116\\PE_Share\\DtAttachFile',
    'Y:\\DtAttachFile_New',
    '\\\\172.16.3.116\\PE_Share\\DtAttachFile_New',
    path.join(__dirname, '..', 'DtAttachFile'),
    path.join(__dirname, '..', 'uploads')
  ].filter(Boolean);

  try {
    let matchedFile = null;
    let foundDir = null;

    const rowStr = String(row).trim();
    const numRegex = new RegExp('(?:^|[^0-9])' + rowStr + '(?:[^0-9]|$)');

    for (const dir of candidateDirs) {
      try {
        if (fs.existsSync(dir)) {
          const files = fs.readdirSync(dir);
          const match = files.find(f => {
            if (f.startsWith('.')) return false;
            const lower = f.toLowerCase();
            const rowLower = rowStr.toLowerCase();
            return lower.startsWith(rowLower + '.') ||
                   lower.startsWith(rowLower + '_') ||
                   lower.startsWith(rowLower + '-') ||
                   lower.startsWith(rowLower + ' ') ||
                   numRegex.test(f);
          });
          if (match) {
            matchedFile = match;
            foundDir = dir;
            break;
          }
        }
      } catch (e) {
        console.warn(`[Attachment] Directory access warning (${dir}):`, e.message);
      }
    }

    if (!matchedFile || !foundDir) {
      if (req.accepts('html')) {
        return res.status(404).send(`<div dir="rtl" style="font-family:Vazirmatn, Tahoma, sans-serif; text-align:center; padding:50px; color:#e11d48;"><h2>📁 فایل ضمیمه یافت نشد</h2><p>فایل ضمیمه‌ای برای ردیف ${row} در سرور شبکه موجود نیست.</p></div>`);
      }
      return res.status(404).json({ error: `فایل ضمیمه‌ای برای ردیف ${row} پیدا نشد.` });
    }

    const filePath = path.join(foundDir, matchedFile);

    res.download(filePath, matchedFile, err => {
      if (err && !res.headersSent) {
        console.error('Download attachment error:', err);
        res.status(500).json({ error: 'خطا در دانلود فایل ضمیمه: ' + err.message });
      }
    });
  } catch (err) {
    console.error('Download attachment error:', err);
    res.status(500).json({ error: 'خطا در بارگیری فایل ضمیمه: ' + err.message });
  }
});




// 161 Real Performers Cache from EMPLOYEES.dbo.staff1
const REAL_PERFORMERS_DB = [
  {"performer":"آرمان مرفاوی","Office_PhoneBook":"---","Mobile_Personnel":"09901877263"},
  {"performer":"آیت موسوی","Office_PhoneBook":"---","Mobile_Personnel":"09138258743"},
  {"performer":"ابراهیم شاهین","Office_PhoneBook":"---","Mobile_Personnel":"09162333545"},
  {"performer":"ابوالفضل حاجی پور","Office_PhoneBook":"---","Mobile_Personnel":"09136960394"},
  {"performer":"احسان طالبی","Office_PhoneBook":"---","Mobile_Personnel":"09138276389"},
  {"performer":"احمد خراجی","Office_PhoneBook":"---","Mobile_Personnel":"09130613676"},
  {"performer":"احمد رضا خدامی","Office_PhoneBook":"بوستان هدفمند","Mobile_Personnel":"09137272500"},
  {"performer":"احمد رضا شاکران","Office_PhoneBook":"---","Mobile_Personnel":"09131123597"},
  {"performer":"اسماعیل زنجانی","Office_PhoneBook":"فیکا","Mobile_Personnel":"09134468511"},
  {"performer":"اصغر گورتانیان","Office_PhoneBook":"پایا ارتباط شایان","Mobile_Personnel":"09134104428"},
  {"performer":"اصغر یزدانی","Office_PhoneBook":"پشتیبانی","Mobile_Personnel":"09133168820"},
  {"performer":"اعظم تیموری","Office_PhoneBook":"---","Mobile_Personnel":"09139592269"},
  {"performer":"افشین ویسی زاده","Office_PhoneBook":"---","Mobile_Personnel":"09130908809"},
  {"performer":"الهام تاجی","Office_PhoneBook":"رجایی","Mobile_Personnel":"09131283419"},
  {"performer":"امید ملک محمودی","Office_PhoneBook":"---","Mobile_Personnel":"09930417742"},
  {"performer":"امیر عباس حیدری","Office_PhoneBook":"کاشان","Mobile_Personnel":"09132625658"},
  {"performer":"امیر عباس قائلی","Office_PhoneBook":"---","Mobile_Personnel":"09133170008"},
  {"performer":"امیر فرزاد","Office_PhoneBook":"---","Mobile_Personnel":"09131315323"},
  {"performer":"امیر محسن روانبخش","Office_PhoneBook":"پیمانکار","Mobile_Personnel":"09131072673"},
  {"performer":"امیر مسعود اعرابی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09134085440"},
  {"performer":"امیر منوچهریان","Office_PhoneBook":"---","Mobile_Personnel":"09013058904"},
  {"performer":"امیر مهدی خوانساری","Office_PhoneBook":"---","Mobile_Personnel":"09132087002"},
  {"performer":"امین آقاسی","Office_PhoneBook":"---","Mobile_Personnel":"09134223121"},
  {"performer":"امین علیزاده زیاری","Office_PhoneBook":"طاها","Mobile_Personnel":"09132008802"},
  {"performer":"ایمان امیری","Office_PhoneBook":"مپسا","Mobile_Personnel":"09137876971"},
  {"performer":"ایوب اسماعیل وند","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09163617390"},
  {"performer":"بهزاد پوری شعبان","Office_PhoneBook":"---","Mobile_Personnel":"09920430523"},
  {"performer":"بهزاد کیماسی","Office_PhoneBook":"کاوش گستر مرکز","Mobile_Personnel":"09138957140"},
  {"performer":"پریسا علیپور","Office_PhoneBook":"---","Mobile_Personnel":"09133089987"},
  {"performer":"ثریا مهاجری","Office_PhoneBook":"---","Mobile_Personnel":"09139584215"},
  {"performer":"جلال توکلی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09132616569"},
  {"performer":"جلال کاوه","Office_PhoneBook":"---","Mobile_Personnel":"09131868082"},
  {"performer":"جواد یزدانبخش","Office_PhoneBook":"شرکت صنام","Mobile_Personnel":"09134032214"},
  {"performer":"حامد حسینی","Office_PhoneBook":"---","Mobile_Personnel":"09140109474"},
  {"performer":"حامد سلطانی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09132227664"},
  {"performer":"حامد طالبی","Office_PhoneBook":"کاوش گستر غرب","Mobile_Personnel":"09135593742"},
  {"performer":"حامد مهدوی دوست","Office_PhoneBook":"---","Mobile_Personnel":"09133291827"},
  {"performer":"حجت اله علینقی","Office_PhoneBook":"شهرضا","Mobile_Personnel":"09133212554"},
  {"performer":"حسن اسماعیلی","Office_PhoneBook":"---","Mobile_Personnel":"09137858138"},
  {"performer":"حسن حشمتی","Office_PhoneBook":"پیمانکار","Mobile_Personnel":"09133286929"},
  {"performer":"حسن رحمانی","Office_PhoneBook":"---","Mobile_Personnel":"09132073684"},
  {"performer":"حسن عبائیان","Office_PhoneBook":"---","Mobile_Personnel":"09120772360"},
  {"performer":"حسین خدادادی","Office_PhoneBook":"---","Mobile_Personnel":"09135592676"},
  {"performer":"حسین رهنمایی","Office_PhoneBook":"پیمانکاری روانبخش","Mobile_Personnel":"09914610503"},
  {"performer":"حسین عظیمی","Office_PhoneBook":"---","Mobile_Personnel":"09140325703"},
  {"performer":"حسین گرجی","Office_PhoneBook":"---","Mobile_Personnel":"09137991464"},
  {"performer":"حشمت الله نوری","Office_PhoneBook":"مپسا","Mobile_Personnel":"09131701407"},
  {"performer":"حمید براتی","Office_PhoneBook":"---","Mobile_Personnel":"09353792518"},
  {"performer":"حمید رجبی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09135701916"},
  {"performer":"حمید رضا آقایی","Office_PhoneBook":"پایا ارتباط شایان","Mobile_Personnel":"09356004096"},
  {"performer":"حمید رضا سخایی","Office_PhoneBook":"---","Mobile_Personnel":"09103591702"},
  {"performer":"حمید رضا شیرانی","Office_PhoneBook":"---","Mobile_Personnel":"09133139033"},
  {"performer":"حمید سبحانی","Office_PhoneBook":"کارشناس مسول بررسی عملکرد","Mobile_Personnel":"09133312819"},
  {"performer":"حمید مصری پور","Office_PhoneBook":"---","Mobile_Personnel":"09130825597"},
  {"performer":"حمیدرضا یادگاری","Office_PhoneBook":"پشتیبانی موبایل","Mobile_Personnel":"09131231618"},
  {"performer":"خسرو کیماسی","Office_PhoneBook":"شرکت کاوش گستر","Mobile_Personnel":"09138373345"},
  {"performer":"دینا سادات نوابی","Office_PhoneBook":"---","Mobile_Personnel":"09132865869"},
  {"performer":"رامین نرئی","Office_PhoneBook":"---","Mobile_Personnel":"09162423661"},
  {"performer":"رضا انصاری پور","Office_PhoneBook":"نگهداری","Mobile_Personnel":"09132115493"},
  {"performer":"رضا باغبان","Office_PhoneBook":"---","Mobile_Personnel":"09132666560"},
  {"performer":"رضا ترابی","Office_PhoneBook":"پایا ارتباط شایان","Mobile_Personnel":"09136266936"},
  {"performer":"رضا سورانی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09136430895"},
  {"performer":"رضا شاهزیدی","Office_PhoneBook":"مرکز مدرس","Mobile_Personnel":"09131281851"},
  {"performer":"رضا صفری","Office_PhoneBook":"مپسا","Mobile_Personnel":"09127553795"},
  {"performer":"رضا مالکی","Office_PhoneBook":"---","Mobile_Personnel":"09190984802"},
  {"performer":"روح الله آهنگرزاده","Office_PhoneBook":"صنام","Mobile_Personnel":"09140022292"},
  {"performer":"روح الله کاوه","Office_PhoneBook":"ارتباط هدف سپاهان","Mobile_Personnel":"09131660468"},
  {"performer":"زهرا اصغری","Office_PhoneBook":"---","Mobile_Personnel":"09913347454"},
  {"performer":"سارا نعیمی","Office_PhoneBook":"---","Mobile_Personnel":"09128549018"},
  {"performer":"سجاد شیرازی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09140103082"},
  {"performer":"سجاد هدایتی","Office_PhoneBook":"کارشناس نگهداری اعرابی خوانسار","Mobile_Personnel":"09196619707"},
  {"performer":"سجاد کریمیان","Office_PhoneBook":"---","Mobile_Personnel":"09130869338"},
  {"performer":"سعید عبدالله زاده","Office_PhoneBook":"پایا ارتباط شایان","Mobile_Personnel":"09131257853"},
  {"performer":"سید حسام سلطان زاده","Office_PhoneBook":"---","Mobile_Personnel":"09133655332"},
  {"performer":"سید مجتبی زیباکلام","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09122765211"},
  {"performer":"سید مسعود احمدی","Office_PhoneBook":"BSS","Mobile_Personnel":"09103152542"},
  {"performer":"سید مسعود موسوی نژاد","Office_PhoneBook":"شایان","Mobile_Personnel":"09137383064"},
  {"performer":"سید موسی رضوی","Office_PhoneBook":"---","Mobile_Personnel":"09131147186"},
  {"performer":"شادی تبریزی","Office_PhoneBook":"---","Mobile_Personnel":"09133090816"},
  {"performer":"شبنم خدابخشیان","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09132299920"},
  {"performer":"شهاب دهقانپور","Office_PhoneBook":"اداره انتقال موبایل","Mobile_Personnel":"09131130995"},
  {"performer":"طهماسب کیماسی","Office_PhoneBook":"پیمانکاری روانبخش","Mobile_Personnel":"09138772003"},
  {"performer":"عادل امیدی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09934931623"},
  {"performer":"عباس آقایان","Office_PhoneBook":"مپسا","Mobile_Personnel":"09133296327"},
  {"performer":"عباس بهره دار","Office_PhoneBook":"---","Mobile_Personnel":"09132101093"},
  {"performer":"عباس عزیزی","Office_PhoneBook":"---","Mobile_Personnel":"09133153009"},
  {"performer":"عبدالحسین جعفری","Office_PhoneBook":"---","Mobile_Personnel":"09133263397"},
  {"performer":"علی ایزدی","Office_PhoneBook":"فیکا","Mobile_Personnel":"09133182448"},
  {"performer":"علی بابایی","Office_PhoneBook":"---","Mobile_Personnel":"09131391408"},
  {"performer":"علی رفیعی","Office_PhoneBook":"---","Mobile_Personnel":"09160851029"},
  {"performer":"علی سورانی","Office_PhoneBook":"---","Mobile_Personnel":"09131704973"},
  {"performer":"علی طاهری","Office_PhoneBook":"---","Mobile_Personnel":"09216887518"},
  {"performer":"علی عظیمی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09138766117"},
  {"performer":"علیرضا دهقان نصیری","Office_PhoneBook":"پایا ارتباط شایان","Mobile_Personnel":"09103264982"},
  {"performer":"علیرضا صالحی","Office_PhoneBook":"کاوش گستر مرکز","Mobile_Personnel":"09103257527"},
  {"performer":"علیرضا نیری","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09137422083"},
  {"performer":"عماد آهنگرزاده","Office_PhoneBook":"پایا ارتباط شایان","Mobile_Personnel":"09140085403"},
  {"performer":"عیسی قربانی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09138219216"},
  {"performer":"غلامرضا پاکروان","Office_PhoneBook":"---","Mobile_Personnel":"09133080771"},
  {"performer":"فرهاد اباذری","Office_PhoneBook":"نظارت","Mobile_Personnel":"09133690370"},
  {"performer":"فرهاد محمدپور","Office_PhoneBook":"---","Mobile_Personnel":"09191162404"},
  {"performer":"قاسم کیماسی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09137389613"},
  {"performer":"مجتبی آیت","Office_PhoneBook":"نگهداری","Mobile_Personnel":"09131130994"},
  {"performer":"مجتبی اسکندری","Office_PhoneBook":"---","Mobile_Personnel":"09131011441"},
  {"performer":"مجید جمشیدی","Office_PhoneBook":"کاوش مرکز","Mobile_Personnel":"09134470339"},
  {"performer":"مجید جمشیدیان","Office_PhoneBook":"---","Mobile_Personnel":"09131281502"},
  {"performer":"مجید مسگری","Office_PhoneBook":"---","Mobile_Personnel":"09132607238"},
  {"performer":"محسن ارتجایی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09134305026"},
  {"performer":"محسن خدابخشیان","Office_PhoneBook":"فیکا","Mobile_Personnel":"09133145166"},
  {"performer":"محسن سلمانی","Office_PhoneBook":"---","Mobile_Personnel":"09131150031"},
  {"performer":"محسن میر احمدی","Office_PhoneBook":"---","Mobile_Personnel":"09133779722"},
  {"performer":"محسن نصر","Office_PhoneBook":"---","Mobile_Personnel":"09134429562"},
  {"performer":"محمد امینی","Office_PhoneBook":"---","Mobile_Personnel":"09103240695"},
  {"performer":"محمد بدیعی","Office_PhoneBook":"کاوش گستر غرب","Mobile_Personnel":"09131088616"},
  {"performer":"محمد پرورش","Office_PhoneBook":"---","Mobile_Personnel":"09133122001"},
  {"performer":"محمد رضا جودکی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09135647363"},
  {"performer":"محمد رضا خدامی","Office_PhoneBook":"رجایی","Mobile_Personnel":"09133869030"},
  {"performer":"محمد رضا قاسمی","Office_PhoneBook":"پایا ارتباط شایان","Mobile_Personnel":"09139812096"},
  {"performer":"محمد رضا نوری","Office_PhoneBook":"صنام","Mobile_Personnel":"09135612807"},
  {"performer":"محمد عبائیان","Office_PhoneBook":"---","Mobile_Personnel":"09131081916"},
  {"performer":"محمد عرب","Office_PhoneBook":"مپسا","Mobile_Personnel":"09134333194"},
  {"performer":"محمد گورتانیان نژاد","Office_PhoneBook":"---","Mobile_Personnel":"09130206890"},
  {"performer":"محمد موسوی","Office_PhoneBook":"پیمانکار","Mobile_Personnel":"09133327398"},
  {"performer":"محمد میلاد آرمیده","Office_PhoneBook":"ITMC","Mobile_Personnel":"09131634569"},
  {"performer":"محمدحسین نارکوب","Office_PhoneBook":"فیکا","Mobile_Personnel":"09215613062"},
  {"performer":"محمود شویدی","Office_PhoneBook":"کاشان","Mobile_Personnel":"09133617327"},
  {"performer":"محمود صیدی مقدم","Office_PhoneBook":"---","Mobile_Personnel":"09169544415"},
  {"performer":"مرتضی اسماعیلی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09137181190"},
  {"performer":"مرتضی اعتصامی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09132867458"},
  {"performer":"مرضیه محمدی","Office_PhoneBook":"نگهداری موبایل","Mobile_Personnel":"09133233537"},
  {"performer":"مرضیه محمدی کجیدی","Office_PhoneBook":"---","Mobile_Personnel":"09133175793"},
  {"performer":"مریم حسینی","Office_PhoneBook":"---","Mobile_Personnel":"09901877327"},
  {"performer":"مسعود فروغی","Office_PhoneBook":"صنام","Mobile_Personnel":"09138700431"},
  {"performer":"مصطفی امین زاده","Office_PhoneBook":"مرکز رجایی","Mobile_Personnel":"09133195318"},
  {"performer":"مصطفی حدادی","Office_PhoneBook":"WLL","Mobile_Personnel":"09133080058"},
  {"performer":"معین احمدی","Office_PhoneBook":"---","Mobile_Personnel":"09929136638"},
  {"performer":"مهتاب هاشمی","Office_PhoneBook":"---","Mobile_Personnel":"09134303018"},
  {"performer":"مهدی بدیعی","Office_PhoneBook":"صنام","Mobile_Personnel":"09133017262"},
  {"performer":"مهدی بهارلوئی","Office_PhoneBook":"---","Mobile_Personnel":"09903102790"},
  {"performer":"مهدی شیرازی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09137277105"},
  {"performer":"مهدی عزیزی","Office_PhoneBook":"---","Mobile_Personnel":"09139366864"},
  {"performer":"مهدی مرادی","Office_PhoneBook":"شرکت فیکا","Mobile_Personnel":"09131623850"},
  {"performer":"مهدی همایی نیا","Office_PhoneBook":"---","Mobile_Personnel":"09131024000"},
  {"performer":"مهدی یزدان پرست","Office_PhoneBook":"---","Mobile_Personnel":"09126003360"},
  {"performer":"مهران آقاجانی","Office_PhoneBook":"---","Mobile_Personnel":"09133694005"},
  {"performer":"مهران فریدونی","Office_PhoneBook":"مپسا","Mobile_Personnel":"09132385233"},
  {"performer":"مهران قدوسی","Office_PhoneBook":"مرکز رجایی- معاونت ارتباطات سیار","Mobile_Personnel":"09131180008"},
  {"performer":"مهرداد اعرابی","Office_PhoneBook":"پیمانکار","Mobile_Personnel":"09133057207"},
  {"performer":"مهرداد روشنائیان","Office_PhoneBook":"---","Mobile_Personnel":"09138012131"},
  {"performer":"میثم شاهمرادی","Office_PhoneBook":"---","Mobile_Personnel":"09133099829"},
  {"performer":"میثم مسعودی","Office_PhoneBook":"صنام","Mobile_Personnel":"09138745166"},
  {"performer":"میلاد آتشین پنجه","Office_PhoneBook":"کاوش گستر مرکز","Mobile_Personnel":"09137871848"},
  {"performer":"میلاد خیاط","Office_PhoneBook":"کاوش گستر غرب","Mobile_Personnel":"09196400795"},
  {"performer":"میلاد کریمی حسن آبادی","Office_PhoneBook":"---","Mobile_Personnel":"09148922349"},
  {"performer":"ندا قویدل","Office_PhoneBook":"---","Mobile_Personnel":"09131172195"},
  {"performer":"هوشنگ فتاحی","Office_PhoneBook":"کاوش گستر","Mobile_Personnel":"09137193614"},
  {"performer":"وحید جنگ روی","Office_PhoneBook":"شرکت صنام","Mobile_Personnel":"09139719126"},
  {"performer":"کیانوش قادری","Office_PhoneBook":"نظارت","Mobile_Personnel":"09131251491"},
  {"performer":"یاور چراغی","Office_PhoneBook":"صنام","Mobile_Personnel":"09132320450"},
  {"performer":"یاور کیماسی","Office_PhoneBook":"---","Mobile_Personnel":"09136037241"},
  {"performer":"یونس طاهری","Office_PhoneBook":"---","Mobile_Personnel":"09130324585"}
];

// GET /api/dt/performers - دریافت لیست 161 انجام‌دهنده از دیتابیس EMPLOYEES.dbo.staff1
router.get('/performers', async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    let performers = [];
    
    if (pool) {
      // Try 1: EMPLOYEES.dbo.staff1 (Main phonebook table with 161 staff records)
      try {
        const r1 = await pool.request().query(`
          SELECT 
            LTRIM(RTRIM(ISNULL([Name_PhoneBook], '') + ' ' + ISNULL([Family_PhoneBook], ''))) AS performer, 
            ISNULL(NULLIF(LTRIM(RTRIM([Office_PhoneBook])), ''), '---') AS Office_PhoneBook, 
            ISNULL(NULLIF(LTRIM(RTRIM([Mobile_Personnel])), ''), '---') AS Mobile_Personnel
          FROM [EMPLOYEES].[dbo].[staff1]
          WHERE [Name_PhoneBook] IS NOT NULL OR [Family_PhoneBook] IS NOT NULL
          ORDER BY performer
        `);
        performers = (r1.recordset || []).filter(r => r.performer && r.performer.trim() !== '');
      } catch (e1) {
        console.warn('Performers Try 1 failed, trying TEL.dbo.tblTel...', e1.message);
      }

      // Try 2: TEL.dbo.tblTel (fallback)
      if (performers.length === 0) {
        try {
          const r2 = await pool.request().query(`
            SELECT 
              Name AS performer, 
              ISNULL(WorkPlace, '---') AS Office_PhoneBook, 
              ISNULL(Tel, '---') AS Mobile_Personnel
            FROM [TEL].[dbo].[tblTel]
            WHERE Name IS NOT NULL AND LTRIM(RTRIM(Name)) <> ''
            ORDER BY Name
          `);
          performers = (r2.recordset || []).filter(r => r.performer && r.performer.trim() !== '');
        } catch (e2) {
          console.warn('Performers Try 2 failed:', e2.message);
        }
      }
    }

    if (performers.length === 0) {
      performers = REAL_PERFORMERS_DB;
    }

    res.json({ success: true, performers });
  } catch (err) {
    console.error('DT Performers Error:', err);
    res.json({ success: true, performers: REAL_PERFORMERS_DB });
  }
});

// GET /api/dt/subjects - دریافت لیست موضوعات از دیتابیس (مطابق C# WinForms: Select subject from tblDownTimeShort GROUP BY subject)
router.get('/subjects', async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const result = await pool.request().query(`
      SELECT subject 
      FROM [DT].[dbo].[tblDownTimeShort] 
      WHERE subject IS NOT NULL AND TRIM(subject) <> ''
      GROUP BY subject 
      ORDER BY COUNT(subject) DESC
    `);

    const defaultSubjects = ['PM-Cleaning', 'PM-Site', 'PM-Tower', 'مراجعه و بازدید', 'رفع خرابی', 'متفرقه'];
    let dbSubjects = (result.recordset || []).map(r => r.subject ? r.subject.trim() : '').filter(Boolean);

    // Merge defaults with DB subjects removing duplicates
    const combined = Array.from(new Set([...defaultSubjects, ...dbSubjects]));
    res.json({ success: true, subjects: combined });
  } catch (err) {
    console.error('DT Subjects Error:', err);
    res.json({ success: true, subjects: ['PM-Cleaning', 'PM-Site', 'PM-Tower', 'مراجعه و بازدید', 'رفع خرابی', 'متفرقه'] });
  }
});

// GET /api/dt/site-info/:siteCode - استعلام نام سایت، منطقه و دان‌تایم‌های فعال تداخلی
router.get('/site-info/:siteCode', async (req, res) => {


  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const sql = req.app.locals.sql;
    let rawCode = decodeURIComponent(req.params.siteCode || '').trim();
    let cleanDigits = rawCode.replace(/[^0-9]/g, '');
    if (cleanDigits.length > 4) cleanDigits = cleanDigits.slice(-4);
    let padded4 = cleanDigits.padStart(4, '0');
    let esCode = 'ES' + padded4;

    const request = pool.request();
    request.input('cleanDigits', sql.NVarChar, cleanDigits);
    request.input('padded4', sql.NVarChar, padded4);

    const siteRes = await request.query(`
      SELECT TOP 1 [نام سایت] AS siteName, [منطقه] AS region
      FROM [DB].[dbo].[tblDb]
      WHERE [کد سایت] LIKE N'%' + @padded4 + N'%' 
         OR [کد سایت] LIKE N'%' + @cleanDigits + N'%'
    `);

    let siteName = '';
    let region = '';
    if (siteRes.recordset && siteRes.recordset.length > 0) {
      siteName = siteRes.recordset[0].siteName || '';
      region = siteRes.recordset[0].region || '';
    }





    // 2. بررسی دان‌تایم‌های فعال/تداخلی این سایت
    const activeDtRes = await pool.request()
      .input('esCode', esCode)
      .input('rawCode', rawCode)
      .input('padded4', padded4)
      .query(`
        SELECT TOP 5 row, startDate, startTime, endDate, endTime, subject, status
        FROM [DT].[dbo].[tblDownTimeShort]
        WHERE (siteCode = @esCode OR siteCode = @rawCode OR siteCode LIKE '%' + @padded4 + '%')
          AND status IN (N'تائید', N'ثبت توسط کاربر', N'ثبت در (SDM)', N'در حال پردازش درخواست')
        ORDER BY TRY_CAST(LTRIM(RTRIM([row])) AS bigint) DESC
      `);


    res.json({
      found: siteRes.recordset.length > 0,
      siteName,
      region,
      activeDownTimes: activeDtRes.recordset
    });
  } catch (err) {
    console.error('Site Info Error:', err);
    res.status(500).json({ error: err.message });
  }
});




// GET /api/dt/user-recent - دریافت لیست دان‌تایم‌های کاربر جاری مطابق کد سی‌شارپ
router.get('/user-recent', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const user = req.session.user || {};
    const fullName = (user.fullName || '').trim();
    const username = (user.username || '').trim();
    const role = (user.role || '').trim().toLowerCase();

    // Candidates for applicant matching
    const applicantCandidates = new Set();
    if (fullName) applicantCandidates.add(fullName);
    if (username) applicantCandidates.add(username);

    // If admin or administrator, include standard admin applicant names
    if (role === 'administrator' || username.toLowerCase() === 'admin') {
      applicantCandidates.add('مدیر سیستم');
      applicantCandidates.add('مدیر سامانه');
      applicantCandidates.add('مدیر سیستم (دسترسی نامحدود)');
    }

    // Clean name without parenthesis e.g. "مدیر سیستم (دسترسی نامحدود)" -> "مدیر سیستم"
    const cleanName = fullName.replace(/\s*\(.*?\)\s*/g, '').trim();
    if (cleanName) applicantCandidates.add(cleanName);

    const candidatesList = Array.from(applicantCandidates).filter(Boolean);

    // Condition based on C# code in frmDtRegDownTime.cs:
    // If role != "ts" -> status = 'ثبت توسط کاربر'
    // If role == "ts" -> status = 'تائید'
    const statusVal = (role === 'ts') ? 'تائید' : 'ثبت توسط کاربر';

    const reqQuery = pool.request();
    reqQuery.input('status', statusVal);

    const orClauses = [];
    candidatesList.forEach((c, idx) => {
      reqQuery.input(`app_${idx}`, c);
      orClauses.push(`applicant = @app_${idx}`);
    });
    if (cleanName) {
      reqQuery.input('cleanNameLike', `%${cleanName}%`);
      orClauses.push(`applicant LIKE @cleanNameLike`);
    }

    const whereApplicant = orClauses.length > 0 ? `(${orClauses.join(' OR ')})` : `1=1`;

    const recentRes = await reqQuery.query(`
      SELECT
        [row],
        [registerDate],
        [registerTime],
        [domain],
        [siteCode],
        [siteName],
        [region],
        [startDate],
        [startTime],
        [endDate],
        [endTime],
        [outageMin],
        [subject],
        [performer],
        [Activity],
        [status],
        [haveAttachedFile],
        [applicant]
      FROM [DT].[dbo].[tblDownTimeShort]
      WHERE ${whereApplicant}
        AND (status = @status OR (status = N'تایید' AND @status = N'تائید'))
      ORDER BY TRY_CAST(LTRIM(RTRIM([row])) AS bigint) DESC
    `);

    res.json(recentRes.recordset || []);
  } catch (err) {
    console.error('User Recent DT Error:', err);
    res.status(500).json({ error: err.message });
  }
});

function getAttachmentTargetDir() {
  const candidateDirs = [
    process.env.ATTACHMENT_PATH,
    'Y:\\DtAttachFile',
    'Y:\\DtAttachFile_New',
    path.join(__dirname, '..', 'DtAttachFile')
  ].filter(Boolean);

  for (const dir of candidateDirs) {
    try {
      if (!fs.existsSync(dir)) {
        fs.mkdirSync(dir, { recursive: true });
      }
      return dir;
    } catch (e) {
      console.warn(`[Attachment Dir Warning] (${dir}):`, e.message);
    }
  }
  const fallback = path.join(__dirname, '..', 'DtAttachFile');
  if (!fs.existsSync(fallback)) fs.mkdirSync(fallback, { recursive: true });
  return fallback;
}

// POST /api/dt/upload-temp - آپلود و بارگذاری آنی فایل پیوست در باکس ثبت
router.post('/upload-temp', async (req, res) => {
  try {
    const { fileName, fileData } = req.body;
    if (!fileName || !fileData) {
      return res.status(400).json({ error: 'نام یا محتوای فایل پیوست ارسال نشده است.' });
    }

    const targetDir = getAttachmentTargetDir();
    const ext = path.extname(fileName || '').toLowerCase() || '.pdf';
    const tempId = `temp_${Date.now()}_${Math.random().toString(36).substring(2, 7)}${ext}`;
    const saveFilePath = path.join(targetDir, tempId);

    const base64Content = fileData.replace(/^data:[^;]+;base64,/, '');
    const fileBuffer = Buffer.from(base64Content, 'base64');

    fs.writeFileSync(saveFilePath, fileBuffer);

    res.json({
      success: true,
      message: 'فایل پیوست با موفقیت در سرور آپلود و تایید گردید.',
      tempId: tempId,
      originalName: fileName,
      fileSize: (fileBuffer.length / 1024).toFixed(1) + ' KB'
    });
  } catch (err) {
    console.error('Upload temp error:', err);
    res.status(500).json({ error: 'خطا در آپلود آنلاین فایل: ' + err.message });
  }
});

// POST /api/dt/add - ثبت دان تایم جدید (frmDtRegDownTime)
router.post('/add', async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    let {
      siteCode, siteName, region, domain, subject,
      startDate, startTime, endDate, endTime,
      applicant, performer, workPlace, activity,
      inBsyHour, note, tempAttachmentId
    } = req.body;

    const currentUser = (req.session && req.session.user)
      ? (req.session.user.fullName || req.session.user.username)
      : 'کاربر وب';

    const finalApplicant = applicant || currentUser;
    const finalApprover = currentUser;
    const status = 'ثبت توسط کاربر';

    // 1. Calculate Shamsi Register Date & HH:mm Register Time
    const dNow = new Date();
    const faParts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric', month: '2-digit', day: '2-digit'
    }).formatToParts(dNow);
    const regYear = faParts.find(p => p.type === 'year').value;
    const regMonth = faParts.find(p => p.type === 'month').value;
    const regDay = faParts.find(p => p.type === 'day').value;
    const shamsiRegisterDate = `${regYear}/${regMonth}/${regDay}`;

    const hoursNow = String(dNow.getHours()).padStart(2, '0');
    const minutesNow = String(dNow.getMinutes()).padStart(2, '0');
    const formattedRegisterTime = `${hoursNow}:${minutesNow}`;

    // Region Fallback Lookup if region is empty
    if (!region || region.trim() === '') {
      try {
        const cleanDigits = (siteCode || '').replace(/[^0-9]/g, '');
        if (cleanDigits.length > 0) {
          const padded4 = cleanDigits.slice(-4).padStart(4, '0');
          const rRes = await pool.request()
            .input('p4', padded4)
            .query(`SELECT TOP 1 [منطقه] AS region FROM [DB].[dbo].[tblDb] WHERE [کد سایت] LIKE '%' + @p4 + '%'`);
          if (rRes.recordset && rRes.recordset.length > 0 && rRes.recordset[0].region) {
            region = rRes.recordset[0].region;
          }
        }
      } catch (e) {
        console.warn('Region fallback error:', e.message);
      }
    }

    const hasAttachedFile = (tempAttachmentId || req.body.hasAttachedFile === true || req.body.hasAttachedFile === 'true' || req.body.hasAttachedFile === 'دارد') ? 'دارد' : 'ندارد';

    const insertQuery = `
      INSERT INTO [DT].[dbo].[tblDownTimeShort] (
        [row], [mysqlFlag], siteCode, siteName, region, domain, subject,
        startDate, startTime, endDate, endTime, outageMin,
        applicant, performer, approver, Activity,
        InBsyHour, Note, status, registerDate, registerTime, haveAttachedFile
      )
      VALUES (
        (ISNULL((SELECT MAX(TRY_CAST([row] AS bigint)) FROM [DT].[dbo].[tblDownTimeShort]), 0) + 1),
        'I',
        @siteCode, @siteName, @region, @domain, @subject,
        @startDate, @startTime, @endDate, @endTime, @outageMin,
        @applicant, @performer, @approver, @activity,
        @inBsyHour, @note, @status,
        @registerDate,
        @registerTime,
        @haveAttachedFile
      )
    `;

    const request = pool.request();
    request.input('siteCode', sql.NVarChar, siteCode || '');
    request.input('siteName', sql.NVarChar, siteName || '');
    request.input('region', sql.NVarChar, region || '');
    request.input('domain', sql.NVarChar, domain || 'SITE');
    request.input('subject', sql.NVarChar, subject || 'متفرقه');
    request.input('startDate', sql.NVarChar, startDate || '');
    request.input('startTime', sql.NVarChar, startTime || '08:00');
    request.input('endDate', sql.NVarChar, endDate || '');
    request.input('endTime', sql.NVarChar, endTime || '12:00');
    request.input('outageMin', sql.NVarChar, String(req.body.outageMin || req.body.outage || '0'));
    request.input('applicant', sql.NVarChar, finalApplicant);
    request.input('performer', sql.NVarChar, performer || '');
    request.input('approver', sql.NVarChar, finalApprover);
    request.input('activity', sql.NVarChar, activity || '');
    request.input('inBsyHour', sql.NVarChar, inBsyHour || '0');
    request.input('note', sql.NVarChar, note || '');
    request.input('status', sql.NVarChar, status);
    request.input('registerDate', sql.NVarChar, shamsiRegisterDate);
    request.input('registerTime', sql.NVarChar, formattedRegisterTime);
    request.input('haveAttachedFile', sql.NVarChar, hasAttachedFile);

    await request.query(insertQuery);

    const maxRowRes = await pool.request().query(`SELECT MAX(TRY_CAST([row] AS bigint)) AS newRow FROM [DT].[dbo].[tblDownTimeShort]`);
    const newRow = (maxRowRes.recordset && maxRowRes.recordset.length > 0) ? String(maxRowRes.recordset[0].newRow) : null;

    // Handle staged temp attachment renaming & desktop tblAttachedFile sync
    if (newRow && hasAttachedFile === 'دارد') {
      try {
        if (tempAttachmentId) {
          const targetDir = getAttachmentTargetDir();
          const tempPath = path.join(targetDir, tempAttachmentId);
          const ext = path.extname(tempAttachmentId) || '.pdf';
          const finalFileName = `${newRow}${ext}`;
          const finalPath = path.join(targetDir, finalFileName);

          if (fs.existsSync(tempPath)) {
            fs.renameSync(tempPath, finalPath);
          }
        }

        // Insert into Desktop tblAttachedFile table
        const faPartsAtt = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
          year: 'numeric', month: '2-digit', day: '2-digit', weekday: 'long'
        }).formatToParts(dNow);
        const attY = faPartsAtt.find(p => p.type === 'year').value;
        const attM = faPartsAtt.find(p => p.type === 'month').value;
        const attD = faPartsAtt.find(p => p.type === 'day').value;
        const attW = faPartsAtt.find(p => p.type === 'weekday')?.value || '';
        const dateTimeStr = `${attW}  ${attY}/${attM}/${attD}`;

        await pool.request()
          .input('row', sql.NVarChar, newRow)
          .input('dtStr', sql.NVarChar, dateTimeStr)
          .query(`
            IF NOT EXISTS (SELECT 1 FROM [DT].[dbo].[tblAttachedFile] WHERE [rowWithAttachedFileInDt] = @row)
            BEGIN
              INSERT INTO [DT].[dbo].[tblAttachedFile] ([rowWithAttachedFileInDt], [dateTime])
              VALUES (@row, @dtStr)
            END
          `);
      } catch (tempErr) {
        console.warn('[Add DT] Attachment finalize warning:', tempErr.message);
      }
    }

    // Log Activity
    await logActivity(req, `ثبت درخواست دان تایم جدید - ردیف: ${newRow || 'نامشخص'} (سایت: ${siteCode || ''} - ${siteName || ''})`);

    res.json({ success: true, row: newRow, message: 'دان تایم با موفقیت ثبت گردید' });
  } catch (err) {
    console.error('DT Add Error:', err);
    res.status(500).json({ error: err.message });
  }
});

// POST /api/dt/upload-attachment - آپلود فایل پیوست بر اساس شماره ردیف
router.post('/upload-attachment', async (req, res) => {
  try {
    const { row, fileName, fileData } = req.body;
    if (!row || !fileData) {
      return res.status(400).json({ error: 'شماره ردیف یا داده‌های فایل پیوست ارسال نشده است.' });
    }

    const targetDir = getAttachmentTargetDir();
    const ext = path.extname(fileName || '').toLowerCase() || '.pdf';
    const cleanRow = String(row).trim();
    const saveFileName = `${cleanRow}${ext}`;
    const saveFilePath = path.join(targetDir, saveFileName);

    const base64Content = fileData.replace(/^data:[^;]+;base64,/, '');
    const fileBuffer = Buffer.from(base64Content, 'base64');

    fs.writeFileSync(saveFilePath, fileBuffer);

    const pool = req.app.locals.getPool();
    if (pool) {
      try {
        await pool.request()
          .input('row', sql.NVarChar, cleanRow)
          .input('haveAttachedFile', sql.NVarChar, 'دارد')
          .query(`UPDATE [DT].[dbo].[tblDownTimeShort] SET [haveAttachedFile] = @haveAttachedFile WHERE [row] = @row`);

        const dNowAtt = new Date();
        const faPartsAtt = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
          year: 'numeric', month: '2-digit', day: '2-digit', weekday: 'long'
        }).formatToParts(dNowAtt);
        const attY = faPartsAtt.find(p => p.type === 'year').value;
        const attM = faPartsAtt.find(p => p.type === 'month').value;
        const attD = faPartsAtt.find(p => p.type === 'day').value;
        const attW = faPartsAtt.find(p => p.type === 'weekday')?.value || '';
        const dateTimeStr = `${attW}  ${attY}/${attM}/${attD}`;

        await pool.request()
          .input('row', sql.NVarChar, cleanRow)
          .input('dtStr', sql.NVarChar, dateTimeStr)
          .query(`
            IF NOT EXISTS (SELECT 1 FROM [DT].[dbo].[tblAttachedFile] WHERE [rowWithAttachedFileInDt] = @row)
            BEGIN
              INSERT INTO [DT].[dbo].[tblAttachedFile] ([rowWithAttachedFileInDt], [dateTime])
              VALUES (@row, @dtStr)
            END
          `);
      } catch (dbErr) {
        console.warn('[Attachment Upload] DB Update warning:', dbErr.message);
      }
    }

    res.json({ success: true, message: 'فایل پیوست با موفقیت ذخیره گردید', fileName: saveFileName });
  } catch (err) {
    console.error('Upload attachment error:', err);
    res.status(500).json({ error: 'خطا در بارگذاری و ذخیره فایل پیوست: ' + err.message });
  }
});

// GET /api/dt/server-time - Get current server time from SQL Server (Exact SQL Server System Time)
router.get('/server-time', async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });

    const result = await pool.request().query(`
      SELECT 
        CONVERT(varchar(10), GETDATE(), 111) AS dateStr,
        CONVERT(varchar(8), GETDATE(), 108) AS timeStr
    `);

    const { dateStr, timeStr } = result.recordset[0];
    const [year, month, day] = dateStr.split('/').map(Number);
    const [hour, minute, second] = timeStr.split(':').map(Number);

    const gDate = new Date(year, month - 1, day);
    const parts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric', month: '2-digit', day: '2-digit', weekday: 'long'
    }).formatToParts(gDate);

    const y = parts.find(p => p.type === 'year').value;
    const m = parts.find(p => p.type === 'month').value;
    const d = parts.find(p => p.type === 'day').value;
    const weekdayObj = parts.find(p => p.type === 'weekday');
    const weekdayStr = weekdayObj ? weekdayObj.value : '';

    res.json({
      success: true,
      shamsiDate: `${y}/${m}/${d}`,
      dayOfWeek: weekdayStr,
      time: timeStr,
      hour: hour,
      minute: minute,
      second: second
    });
  } catch (err) {
    console.error('Server time error:', err);
    res.status(500).json({ error: err.message });
  }
});

// ===== FREEZE ANNOUNCEMENTS API =====
async function ensureAnnouncementsTable(pool) {
  try {
    await pool.request().query(`
      IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'tblDtAnnouncements')
      BEGIN
        CREATE TABLE [dbo].[tblDtAnnouncements] (
          [id] INT IDENTITY(1,1) PRIMARY KEY,
          [messageText] NVARCHAR(MAX) NOT NULL,
          [startDate] NVARCHAR(10) NULL,
          [endDate] NVARCHAR(10) NULL,
          [isActive] BIT NOT NULL DEFAULT 0,
          [createdDate] NVARCHAR(10) NULL,
          [createdTime] NVARCHAR(8) NULL,
          [createdBy] NVARCHAR(100) NULL
        );
      END
      ELSE
      BEGIN
        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tblDtAnnouncements') AND name = 'startDate')
        BEGIN
          ALTER TABLE [dbo].[tblDtAnnouncements] ADD [startDate] NVARCHAR(10) NULL;
        END
        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tblDtAnnouncements') AND name = 'endDate')
        BEGIN
          ALTER TABLE [dbo].[tblDtAnnouncements] ADD [endDate] NVARCHAR(10) NULL;
        END
      END
    `);
  } catch (err) {
    console.error('Error ensuring tblDtAnnouncements table:', err.message);
  }
}

// GET /api/dt/announcement/active
router.get('/announcement/active', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });
    await ensureAnnouncementsTable(pool);

    const result = await pool.request().query(`
      SELECT TOP 1 * FROM [dbo].[tblDtAnnouncements]
      WHERE [isActive] = 1
      ORDER BY [id] DESC
    `);
    res.json({ success: true, activeAnnouncement: result.recordset[0] || null });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET /api/dt/announcements/history
router.get('/announcements/history', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });
    await ensureAnnouncementsTable(pool);

    const result = await pool.request().query(`
      SELECT * FROM [dbo].[tblDtAnnouncements]
      ORDER BY [id] DESC
    `);
    res.json({ success: true, history: result.recordset });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// POST /api/dt/announcement
router.post('/announcement', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });
    await ensureAnnouncementsTable(pool);

    const { messageText, startDate, endDate, setAsActive } = req.body;
    if (!messageText || !messageText.trim()) {
      return res.status(400).json({ error: 'متن پیام نمی‌تواند خالی باشد' });
    }

    const todayShamsi = getShamsiDateMinusDays(0) || '';
    const d = new Date();
    const nowTime = d.toTimeString().split(' ')[0];
    const createdBy = req.session.user ? (req.session.user.fullName || req.session.user.username) : 'مدیر سیستم';
    const isActiveVal = setAsActive ? 1 : 0;

    if (setAsActive) {
      await pool.request().query(`UPDATE [dbo].[tblDtAnnouncements] SET [isActive] = 0`);
    }

    const result = await pool.request()
      .input('messageText', sql.NVarChar(sql.MAX), messageText.trim())
      .input('startDate', sql.NVarChar(10), startDate || null)
      .input('endDate', sql.NVarChar(10), endDate || null)
      .input('isActive', sql.Bit, isActiveVal)
      .input('createdDate', sql.NVarChar(10), todayShamsi)
      .input('createdTime', sql.NVarChar(8), nowTime)
      .input('createdBy', sql.NVarChar(100), createdBy)
      .query(`
        INSERT INTO [dbo].[tblDtAnnouncements] ([messageText], [startDate], [endDate], [isActive], [createdDate], [createdTime], [createdBy])
        VALUES (@messageText, @startDate, @endDate, @isActive, @createdDate, @createdTime, @createdBy);
        SELECT SCOPE_IDENTITY() AS newId;
      `);

    res.json({ success: true, id: result.recordset[0].newId, message: 'اعلان با موفقیت ثبت شد' });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// PUT /api/dt/announcement/deactivate-all
router.put('/announcement/deactivate-all', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });
    await ensureAnnouncementsTable(pool);

    await pool.request().query(`UPDATE [dbo].[tblDtAnnouncements] SET [isActive] = 0`);
    res.json({ success: true, message: 'تمامی اعلان‌ها غیرفعال شدند' });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// PUT /api/dt/announcement/:id/activate
router.put('/announcement/:id/activate', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });
    await ensureAnnouncementsTable(pool);

    const id = parseInt(req.params.id);
    await pool.request().query(`UPDATE [dbo].[tblDtAnnouncements] SET [isActive] = 0`);
    await pool.request()
      .input('id', sql.Int, id)
      .query(`UPDATE [dbo].[tblDtAnnouncements] SET [isActive] = 1 WHERE [id] = @id`);

    res.json({ success: true, message: 'پیام مورد نظر فعال گردید' });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// DELETE /api/dt/announcement/:id
router.delete('/announcement/:id', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });
    await ensureAnnouncementsTable(pool);

    const id = parseInt(req.params.id);
    await pool.request()
      .input('id', sql.Int, id)
      .query(`DELETE FROM [dbo].[tblDtAnnouncements] WHERE [id] = @id`);

    res.json({ success: true, message: 'اعلان با موفقیت حذف شد' });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// POST /api/dt/multi-update - Edit multiple DT records matching C# frmDtMultiChange logic
router.post('/multi-update', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const { rowNumbers, updates, activeFields } = req.body;
    if (!Array.isArray(rowNumbers) || rowNumbers.length === 0) {
      return res.status(400).json({ error: 'هیچ ردیفی جهت ویرایش انتخاب نشده است.' });
    }
    if (!Array.isArray(activeFields) || activeFields.length === 0) {
      return res.status(400).json({ error: 'هیچ فیلدی جهت تغییر انتخاب نشده است.' });
    }

    const fieldMap = {
      row: 'row',
      registerDate: 'registerDate',
      registerTime: 'registerTime',
      region: 'region',
      startDate: 'startDate',
      startTime: 'startTime',
      endDate: 'endDate',
      endTime: 'endTime',
      siteCode: 'siteCode',
      siteName: 'siteName',
      applicant: 'applicant',
      subject: 'subject',
      performer: 'performer',
      approver: 'approver',
      dtNo: 'dtNo',
      status: 'status',
      activity: 'Activity',
      note: 'Note',
      outageMin: 'outageMin'
    };

    const setClauses = [];
    const request = pool.request();

    activeFields.forEach((fieldKey, idx) => {
      const dbCol = fieldMap[fieldKey];
      if (dbCol && updates[fieldKey] !== undefined) {
        const paramName = `val_${idx}`;
        setClauses.push(`[${dbCol}] = @${paramName}`);
        request.input(paramName, sql.NVarChar, String(updates[fieldKey]));
      }
    });

    if (setClauses.length === 0) {
      return res.status(400).json({ error: 'فیلدهای معتبری جهت ویرایش ارسال نشده است.' });
    }

    const rowParams = rowNumbers.map((rowNum, idx) => {
      const rParam = `r_${idx}`;
      request.input(rParam, sql.NVarChar, String(rowNum));
      return `@${rParam}`;
    });

    const updateQuery = `
      UPDATE [DT].[dbo].[tblDownTimeShort]
      SET ${setClauses.join(', ')}
      WHERE [row] IN (${rowParams.join(', ')})
    `;

    await request.query(updateQuery);

    // Log Activity
    await logActivity(req, `ویرایش گروهی دان تایم‌ها (${rowNumbers.length} رکورد - ردیف‌ها: ${rowNumbers.slice(0, 5).join(', ')}${rowNumbers.length > 5 ? '...' : ''})`);

    res.json({
      success: true,
      message: `تعداد ${rowNumbers.length} رکورد دان‌تایم با موفقیت بروزرسانی شد.`
    });
  } catch (err) {
    console.error('multi-update error:', err);
    res.status(500).json({ error: err.message });
  }
});

// POST /api/dt/multi-delete - Delete DT records (Physical DELETE for last record, Soft DELETE for others)
router.post('/multi-delete', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const { rowNumbers } = req.body;
    if (!Array.isArray(rowNumbers) || rowNumbers.length === 0) {
      return res.status(400).json({ error: 'هیچ ردیفی جهت حذف انتخاب نشده است.' });
    }

    const convertFaToEnDigits = (str) => {
      if (!str) return '';
      return String(str)
        .replace(/[۰٠]/g, '0')
        .replace(/[۱١]/g, '1')
        .replace(/[۲٢]/g, '2')
        .replace(/[۳٣]/g, '3')
        .replace(/[۴٤]/g, '4')
        .replace(/[۵٥]/g, '5')
        .replace(/[۶٦]/g, '6')
        .replace(/[۷٧]/g, '7')
        .replace(/[۸٨]/g, '8')
        .replace(/[۹٩]/g, '9')
        .replace(/[^0-9]/g, '');
    };

    const cleanRowNumbers = rowNumbers.map(r => convertFaToEnDigits(r)).filter(r => r.length > 0);
    if (cleanRowNumbers.length === 0) {
      return res.status(400).json({ error: 'شماره ردیف عددی معتبری دریافت نشد.' });
    }

    const currentUser = req.session.user ? (req.session.user.fullName || req.session.user.username) : 'کاربر';
    const request = pool.request();

    const rowConditions = [];
    cleanRowNumbers.forEach((rowNum, idx) => {
      const pStr = `r_str_${idx}`;
      const pNum = `r_num_${idx}`;
      request.input(pStr, sql.NVarChar, rowNum);
      request.input(pNum, sql.BigInt, parseInt(rowNum, 10));
      rowConditions.push(`LTRIM(RTRIM([row])) = @${pStr} OR TRY_CAST(LTRIM(RTRIM([row])) AS bigint) = @${pNum}`);
    });

    request.input('currentUser', sql.NVarChar, String(currentUser));

    // Get absolute maximum row number currently in DB
    const maxTotalResult = await pool.request().query(`
      SELECT ISNULL(MAX(TRY_CAST(LTRIM(RTRIM([row])) AS bigint)), 0) AS maxTotalRow 
      FROM [DT].[dbo].[tblDownTimeShort]
    `);

    const maxTotal = parseInt(maxTotalResult.recordset[0]?.maxTotalRow || 0, 10);

    // Check if selected row(s) contain or equal the max row in DB
    const isLastRowSelected = cleanRowNumbers.some(r => {
      const num = parseInt(r, 10);
      return num > 0 && num >= maxTotal;
    });

    if (isLastRowSelected) {
      // Execute physical SQL DELETE for the last record
      const deleteQuery = `
        DELETE FROM [DT].[dbo].[tblDownTimeShort]
        WHERE ${rowConditions.join(' OR ')}
      `;
      const deleteRes = await request.query(deleteQuery);
      const rowsDeleted = deleteRes.rowsAffected ? deleteRes.rowsAffected.reduce((a, b) => a + b, 0) : 0;

      if (rowsDeleted > 0) {
        await logActivity(req, `حذف فیزیکی ردیف دان تایم (${cleanRowNumbers.join(', ')})`);
        return res.json({
          success: true,
          message: `آخرین ردیف ثبت‌شده (ردیف ${cleanRowNumbers.join(', ')}) به طور کامل و فیزیکی از دیتابیس حذف گردید.`
        });
      } else {
        return res.status(404).json({
          error: `هیچ ردیفی با شماره ${cleanRowNumbers.join(', ')} در دیتابیس یافت نشد.`
        });
      }
    } else {
      // Soft delete for middle records: set status = 'حذف'
      const softDeleteQuery = `
        UPDATE [DT].[dbo].[tblDownTimeShort]
        SET [status] = N'حذف', [Note] = @currentUser
        WHERE ${rowConditions.join(' OR ')}
      `;
      await request.query(softDeleteQuery);

      await logActivity(req, `حذف دان تایم‌ها (${cleanRowNumbers.length} رکورد - ردیف‌ها: ${cleanRowNumbers.slice(0, 5).join(', ')}${cleanRowNumbers.length > 5 ? '...' : ''})`);

      return res.json({
        success: true,
        message: `تعداد ${cleanRowNumbers.length} دان‌تایم با موفقیت غیرفعال و به وضعیت حذف تغییر یافت.`
      });
    }
  } catch (err) {
    console.error('multi-delete error:', err);
    res.status(500).json({ error: err.message });
  }
});

// =========================================================================
// CAB SITE LIST MAPPING & EXCEL GENERATOR (Ported from SiteMappingApp C#)
// =========================================================================

const independentBands = {
  '2G900': 'ES1G',
  '2G1800': 'ES2G',
  '3G900': 'ES2U',
  '3G2100': 'ES1U',
  '4G1800': 'ES2L',
  '4G2600': 'ES1L',
  '4G900': 'ESAL',
  '4G2100': 'ES3L',
  'TDLTE2300': 'ES5L',
  '5G3500': 'ESNL'
};

function extractNode(rawData, prefix) {
  if (!rawData || typeof rawData !== 'string' || !rawData.trim()) return '-';
  const parts = rawData.split(/[,/\s&\-]+/);
  for (const part of parts) {
    const cleanPart = part.trim().toUpperCase();
    if (cleanPart.startsWith(prefix.toUpperCase())) {
      return cleanPart;
    }
  }
  return '-';
}

function analyzeFrequencies(siteCode, freqRaw) {
  const results = [];
  const content = (freqRaw || '').replace(/\s+/g, '').toUpperCase();
  const last4 = siteCode.length >= 4 ? siteCode.substring(siteCode.length - 4) : siteCode;

  for (const [bandKey, bandPrefix] of Object.entries(independentBands)) {
    if (content.includes(bandKey.toUpperCase())) {
      const siteName = `${bandPrefix}${last4}`;
      results.push({ siteName, tech: bandKey });
    }
  }
  if (results.length === 0) {
    results.push({ siteName: '-', tech: 'Unknown' });
  }
  return results;
}

// POST /api/dt/cab-site-list - تشکیل لیست سایتهای جلسه CAB و خروجی اکسل
router.post('/cab-site-list', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'عدم اتصال به دیتابیس' });

    let { siteCodes } = req.body;
    if (!Array.isArray(siteCodes) || siteCodes.length === 0) {
      return res.status(400).json({ error: 'هیچ کد سایتی انتخاب یا ارسال نشده است' });
    }

    // Clean, split and deduplicate site codes
    const allParsedCodes = [];
    siteCodes.forEach(code => {
      if (!code) return;
      const cleaned = String(code).replace(/XX/gi, '').trim();
      if (cleaned) {
        cleaned.split(/[,;\/\s]+/).forEach(part => {
          const s = part.trim().toUpperCase();
          if (s.length > 0) allParsedCodes.push(s);
        });
      }
    });

    const distinctCodes = [...new Set(allParsedCodes)];
    if (distinctCodes.length === 0) {
      return res.status(400).json({ error: 'کد سایت معتبری در ردیف‌های انتخابی یافت نشد' });
    }

    const reportRows = [];
    let rowCounter = 1;

    for (const code of distinctCodes) {
      const request = pool.request();
      request.input('code', sql.NVarChar, code);
      const dbRes = await request.query(`
        SELECT [باند فرکانسی فعال] AS freqRaw, 
               [شهر (براساس تقسیمات کشوری)] AS city, 
               [BSC&RNC] AS rawNodeData
        FROM [DB].[dbo].[tblDb]
        WHERE [کد سایت] = @code
      `);

      if (dbRes.recordset && dbRes.recordset.length > 0) {
        const row = dbRes.recordset[0];
        const freqRaw = row.freqRaw || '';
        const city = row.city || '';
        const rawNodeData = row.rawNodeData || '';

        const processedSites = analyzeFrequencies(code, freqRaw);
        for (const item of processedSites) {
          let finalNode = '-';
          if (item.tech.includes('4G') || item.tech.includes('5G') || item.tech.includes('TDLTE')) {
            finalNode = '-';
          } else if (item.tech.includes('2G')) {
            finalNode = extractNode(rawNodeData, 'B');
          } else if (item.tech.includes('3G')) {
            finalNode = extractNode(rawNodeData, 'R');
          }

          reportRows.push({
            row: rowCounter++,
            province: 'Esfahan',
            city: city,
            location: code,
            siteName: item.siteName,
            node: finalNode
          });
        }
      } else {
        reportRows.push({
          row: rowCounter++,
          province: 'Esfahan',
          city: '-',
          location: code,
          siteName: '-',
          node: '-'
        });
      }
    }

    if (reportRows.length === 0) {
      return res.status(404).json({ error: 'هیچ رکوردی برای تشکیل لیست سایتها ایجاد نشد' });
    }

    // Build Styled Excel Workbook matching exact user screenshot
    const workbook = new ExcelJS.Workbook();
    workbook.creator = 'PE-Web';
    workbook.created = new Date();

    const worksheet = workbook.addWorksheet('Database for CR', {
      views: [{ showGridLines: true }]
    });

    worksheet.columns = [
      { header: 'Row', key: 'row', width: 9 },
      { header: 'Province', key: 'province', width: 14 },
      { header: 'City', key: 'city', width: 15 },
      { header: 'Location', key: 'location', width: 14 },
      { header: 'Site Name', key: 'siteName', width: 16 },
      { header: 'Node (RNC/BSC)', key: 'node', width: 20 }
    ];

    // Set AutoFilter on header row
    worksheet.autoFilter = 'A1:F1';

    // Style Header Row (Row 1) - Peach/Light-Orange (#FCD5B4), Calibri 10 Bold, Solid Black Borders
    const headerRow = worksheet.getRow(1);
    headerRow.height = 24;
    headerRow.eachCell((cell) => {
      cell.font = { name: 'Calibri', size: 10, bold: true, color: { argb: 'FF000000' } };
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFFCD5B4' } // Exact Peach/Cream color from screenshot
      };
      cell.alignment = { vertical: 'middle', horizontal: 'center' };
      cell.border = {
        top: { style: 'thin', color: { argb: 'FF000000' } },
        left: { style: 'thin', color: { argb: 'FF000000' } },
        bottom: { style: 'medium', color: { argb: 'FF000000' } },
        right: { style: 'thin', color: { argb: 'FF000000' } }
      };
    });

    // Add & Style Data Rows - Pure White background, Calibri 10 Bold for ALL columns, Solid Black Borders
    reportRows.forEach((item) => {
      const dataRow = worksheet.addRow(item);
      dataRow.height = 19;
      dataRow.eachCell((cell) => {
        cell.font = {
          name: 'Calibri',
          size: 10,
          bold: true,
          color: { argb: 'FF000000' }
        };
        cell.alignment = { vertical: 'middle', horizontal: 'center' };
        cell.border = {
          top: { style: 'thin', color: { argb: 'FF000000' } },
          left: { style: 'thin', color: { argb: 'FF000000' } },
          bottom: { style: 'thin', color: { argb: 'FF000000' } },
          right: { style: 'thin', color: { argb: 'FF000000' } }
        };
      });
    });

    // 2. Sheet 2: Node (Distinct Node list in a single cell separated by comma)
    const distinctNodes = [...new Set(
      reportRows
        .map(r => r.node ? String(r.node).trim() : '')
        .filter(n => n && n !== '-')
    )];
    const joinedNodes = distinctNodes.join(', ');

    const nodeSheet = workbook.addWorksheet('Node', {
      views: [{ showGridLines: true }]
    });
    nodeSheet.columns = [
      { header: 'Node (RNC/BSC)', key: 'nodes', width: Math.max(30, joinedNodes.length + 5) }
    ];

    const nodeHeaderRow = nodeSheet.getRow(1);
    nodeHeaderRow.height = 24;
    nodeHeaderRow.eachCell((cell) => {
      cell.font = { name: 'Calibri', size: 10, bold: true, color: { argb: 'FF000000' } };
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFFCD5B4' }
      };
      cell.alignment = { vertical: 'middle', horizontal: 'center' };
      cell.border = {
        top: { style: 'thin', color: { argb: 'FF000000' } },
        left: { style: 'thin', color: { argb: 'FF000000' } },
        bottom: { style: 'medium', color: { argb: 'FF000000' } },
        right: { style: 'thin', color: { argb: 'FF000000' } }
      };
    });

    const nodeDataRow = nodeSheet.addRow({ nodes: joinedNodes });
    nodeDataRow.height = 22;
    nodeDataRow.eachCell((cell) => {
      cell.font = { name: 'Calibri', size: 10, bold: true, color: { argb: 'FF000000' } };
      cell.alignment = { vertical: 'middle', horizontal: 'left' };
      cell.border = {
        top: { style: 'thin', color: { argb: 'FF000000' } },
        left: { style: 'thin', color: { argb: 'FF000000' } },
        bottom: { style: 'thin', color: { argb: 'FF000000' } },
        right: { style: 'thin', color: { argb: 'FF000000' } }
      };
    });

    const firstCode = distinctCodes[0] || 'Site';
    const fileName = `Site List - ${firstCode}.xlsx`;



    // Auto-save directly to Desktop and open with Excel if running on local Windows
    let savedToDesktop = false;
    let desktopFilePath = '';
    try {
      const desktopDir = path.join(process.env.USERPROFILE || 'C:\\Users\\ASG', 'Desktop');
      if (fs.existsSync(desktopDir)) {
        desktopFilePath = path.join(desktopDir, fileName);
        await workbook.xlsx.writeFile(desktopFilePath);
        savedToDesktop = true;
        const { exec } = require('child_process');
        exec(`cmd.exe /c start "" "${desktopFilePath}"`, (launchErr) => {
          if (launchErr) console.warn('Auto-launch error:', launchErr.message);
        });
      }
    } catch (saveErr) {
      console.warn('Desktop auto-save/launch warning:', saveErr.message);
    }

    // Log user activity
    await logActivity(req, `تشکیل و خروجی فایل اکسل لیست سایتهای جلسه CAB (${distinctCodes.length} سایت / نام فایل: ${fileName})`);

    return res.json({
      success: true,
      fileName,
      desktopFilePath,
      savedToDesktop,
      count: distinctCodes.length,
      siteCodes: distinctCodes
    });



  } catch (err) {
    console.error('CAB site list error:', err);
    res.status(500).json({ error: 'خطا در تشکیل لیست سایتهای CAB: ' + err.message });
  }
});

// GET /api/dt/copy-excel (دقیقاً مطابق متد کپیToolStripMenuItem_Click در MainForm.cs نسخه دسکتاپ PE)
router.get('/copy-excel', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const sqlQuery = `
      SELECT 
        row AS [ردیف],
        haveAttachedFile AS [ضمیمه],
        registerDate AS [تاریخ ثبت],
        registerTime AS [ثبت],
        siteCode AS [کد سایت],
        siteName AS [نام سایت],
        region AS [منطقه],
        startDate AS [تاریخ شروع],
        startTime AS [زمان شروع],
        endDate AS [تاریخ پایان],
        endTime AS [زمان پایان],
        outageMin AS [زمان قطعی],
        applicant AS [درخواست کننده],
        subject AS [موضوع],
        performer AS [انجام دهنده],
        approver AS [ثبت کننده],
        dtNo AS [شماره],
        status AS [وضعیت],
        Note AS [یادداشت]
      FROM [DT].[dbo].tblDownTimeShort 
      WHERE
        status=N'پردازش فنی'  OR 
        status=N'عدم تائید(موقت)'  OR 
        status=N'ثبت در (SDM)' OR 
        status=N'ثبت توسط کاربر' OR 
        status=N'در حال پردازش درخواست' OR
        status=N'نیاز به اصلاح درخواست' OR
        status=N'منتظر ارائه در جلسه'
      ORDER BY row DESC;
    `;

    const result = await pool.request().query(sqlQuery);
    const rows = result.recordset || [];

    const workbook = new ExcelJS.Workbook();
    workbook.creator = 'PE System';
    workbook.created = new Date();

    const worksheet = workbook.addWorksheet('Sheet1', {
      views: [{ rtl: true, state: 'normal' }]
    });

    const columns = [
      { header: 'ردیف', key: 'ردیف', width: 10 },
      { header: 'ضمیمه', key: 'ضمیمه', width: 10 },
      { header: 'تاریخ ثبت', key: 'تاریخ ثبت', width: 13 },
      { header: 'ثبت', key: 'ثبت', width: 10 },
      { header: 'کد سایت', key: 'کد سایت', width: 12 },
      { header: 'نام سایت', key: 'نام سایت', width: 22 },
      { header: 'منطقه', key: 'منطقه', width: 12 },
      { header: 'تاریخ شروع', key: 'تاریخ شروع', width: 13 },
      { header: 'زمان شروع', key: 'زمان شروع', width: 11 },
      { header: 'تاریخ پایان', key: 'تاریخ پایان', width: 13 },
      { header: 'زمان پایان', key: 'زمان پایان', width: 11 },
      { header: 'زمان قطعی', key: 'زمان قطعی', width: 12 },
      { header: 'درخواست کننده', key: 'درخواست کننده', width: 18 },
      { header: 'موضوع', key: 'موضوع', width: 35 },
      { header: 'انجام دهنده', key: 'انجام دهنده', width: 18 },
      { header: 'ثبت کننده', key: 'ثبت کننده', width: 18 },
      { header: 'شماره', key: 'شماره', width: 10 },
      { header: 'وضعیت', key: 'وضعیت', width: 18 },
      { header: 'یادداشت', key: 'یادداشت', width: 30 }
    ];

    worksheet.columns = columns;

    // Header styling matching C# CreateExcelFile
    const headerRow = worksheet.getRow(1);
    headerRow.font = { name: 'Vazirmatn', size: 10.5, bold: true, color: { argb: 'FFFFFFFF' } };
    headerRow.fill = {
      type: 'pattern',
      pattern: 'solid',
      fgColor: { argb: 'FF007BFF' } // Blue #007bff
    };
    headerRow.alignment = { vertical: 'middle', horizontal: 'center' };
    headerRow.height = 28;

    // Add rows and format using explicit array values (جلوگیری قطعی از خالی ماندن ستون‌ها)
    rows.forEach((r, idx) => {
      const rowValues = columns.map(c => {
        const val = r[c.key] !== undefined ? r[c.key] : (r[c.header] !== undefined ? r[c.header] : '');
        return val !== null ? val : '';
      });

      const row = worksheet.addRow(rowValues);
      row.height = 22;
      row.font = { name: 'Vazirmatn', size: 10 };
      row.alignment = { vertical: 'middle', horizontal: 'center' };

      // Alternating row color (#ffffcc - light yellow on even data rows)
      if (idx % 2 === 1) {
        row.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'FFFFFFCC' }
        };
      }

      // Green and bold if 'ضمیمه' === 'دارد'
      const attachVal = String(r['ضمیمه'] || rowValues[1] || '').trim();
      if (attachVal === 'دارد') {
        const attachCell = row.getCell(2);
        if (attachCell) {
          attachCell.font = { name: 'Vazirmatn', size: 10, bold: true, color: { argb: 'FF008000' } };
        }
      }

      // Border for each cell
      row.eachCell({ includeEmpty: true }, (cell) => {
        cell.border = {
          top: { style: 'thin', color: { argb: 'FFD1D5DB' } },
          left: { style: 'thin', color: { argb: 'FFD1D5DB' } },
          bottom: { style: 'thin', color: { argb: 'FFD1D5DB' } },
          right: { style: 'thin', color: { argb: 'FFD1D5DB' } }
        };
      });
    });

    const now = new Date();
    const pad = (n) => String(n).padStart(2, '0');
    const ts = `${now.getFullYear()}${pad(now.getMonth() + 1)}${pad(now.getDate())}_${pad(now.getHours())}${pad(now.getMinutes())}${pad(now.getSeconds())}`;
    const fileName = `Current_DT_${ts}.xlsx`;

    // Save directly to Desktop and immediately launch in Excel
    let desktopFilePath = '';
    const desktopDir = path.join(process.env.USERPROFILE || 'C:\\Users\\ASG', 'Desktop');
    
    if (fs.existsSync(desktopDir)) {
      desktopFilePath = path.join(desktopDir, fileName);
      await workbook.xlsx.writeFile(desktopFilePath);

      const { exec } = require('child_process');
      // PowerShell Start-Process launches the registered default app (Excel) in interactive GUI session
      exec(`powershell -NoProfile -Command "Start-Process '${desktopFilePath}'"`, (launchErr) => {
        if (launchErr) {
          exec(`cmd.exe /c start "" "${desktopFilePath}"`);
        }
      });
    } else {
      desktopFilePath = path.join(__dirname, '..', 'public', fileName);
      await workbook.xlsx.writeFile(desktopFilePath);
    }

    await logActivity(req, `اجرای عملیات کپی دان‌تایم و صدور فایل اکسل ${fileName} (${rows.length} ردیف فعال)`);

    return res.json({
      success: true,
      fileName: fileName,
      desktopFilePath: desktopFilePath,
      rowCount: rows.length
    });
  } catch (err) {
    console.error('copy-excel error:', err);
    res.status(500).json({ error: 'خطا در کپی و صدور اکسل: ' + err.message });
  }
});

module.exports = router;




