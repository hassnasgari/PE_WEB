const express = require('express');
const router = express.Router();
const fs = require('fs');
const path = require('path');
const sql = require('mssql');

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
      ORDER BY row DESC
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

// GET /api/dt/freeze
router.get('/freeze', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'DB not connected' });
    const result = await pool.request().query(`SELECT isFreezeNetwork, causeText FROM [DT].[dbo].[tblFreeze]`);
    const row = result.recordset[0] || {};
    res.json({ 
      success: true, 
      isFrozen: !!row.isFreezeNetwork, 
      causeText: row.causeText || '' 
    });
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
    '\\\\172.16.3.116\\PE_Share\\DtAttachFile_New'
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
        ORDER BY row DESC
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




// GET /api/dt/user-recent - دریافت لیست سوابق اخیر ثبت‌شده توسط کاربر جاری
router.get('/user-recent', requireAuth, async (req, res) => {
  try {
    const pool = req.app.locals.getPool();
    if (!pool) return res.status(503).json({ error: 'اتصال به دیتابیس برقرار نیست' });

    const approver = req.session.user ? req.session.user.username : 'web_user';

    const recentRes = await pool.request()
      .input('approver', approver)
      .query(`
        SELECT TOP 15 row, siteCode, siteName, domain, subject, startDate, startTime, endDate, endTime, status, registerDate
        FROM [DT].[dbo].[tblDownTimeShort]
        WHERE approver = @approver
        ORDER BY row DESC
      `);

    res.json(recentRes.recordset);
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
        startDate, startTime, endDate, endTime,
        applicant, performer, approver, Activity,
        InBsyHour, Note, status, registerDate, registerTime, haveAttachedFile
      )
      VALUES (
        (ISNULL((SELECT MAX(TRY_CAST([row] AS bigint)) FROM [DT].[dbo].[tblDownTimeShort]), 0) + 1),
        'I',
        @siteCode, @siteName, @region, @domain, @subject,
        @startDate, @startTime, @endDate, @endTime,
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

module.exports = router;


