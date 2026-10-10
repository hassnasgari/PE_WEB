// public/js/db.js - Complete DB Management ported from C# PE - Ver 0009
let rawDbData = [];
let filteredDbData = [];
let dbColumnFilters = {};
let selectedDbRowIndex = -1;
let selectedDbRowData = null;
let leafletMapInstance = null;
let leafletMarkerInstance = null;

// --------------------------------------------------------------------------------
// 1. DATA LOADING & RENDERING
// --------------------------------------------------------------------------------

async function loadDB() {
  const loading = document.getElementById('dbLoading');
  const container = document.getElementById('dbTableContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/db/list');
    const result = await res.json();

    if (!result.success) {
      alert('خطا در دریافت اطلاعات دیتابیس سایت‌ها: ' + (result.error || 'خطای ناشناخته'));
      return;
    }

    rawDbData = result.data || [];
    selectedDbRowIndex = -1;
    selectedDbRowData = null;

    applyDbFilters();
    renderDbTable();
    loadDbLastUpdateDate();
  } catch (err) {
    console.error('loadDB error:', err);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

async function loadDbLastUpdateDate() {
  try {
    const res = await fetch('/api/db/last-update-date');
    const data = await res.json();
    const dbLastUpdate = document.getElementById('dbLastUpdate');
    if (dbLastUpdate && data.success) {
      if (data.dateOfLastDbUp) {
        const d = new Date(data.dateOfLastDbUp);
        const dateStr = d.toLocaleDateString('fa-IR') + ' ' + d.toLocaleTimeString('fa-IR');
        dbLastUpdate.innerHTML = `🕒 آخرین زمان بروز رسانی دیتابیس سایت‌ها: <strong>${dateStr}</strong>`;
      } else {
        dbLastUpdate.textContent = '🕒 تاریخ آخرین بروزرسانی: نامشخص';
      }
    }
  } catch (e) {
    console.warn('loadDbLastUpdateDate error:', e);
  }
}

function applyDbFilters() {
  const globalSearch = (document.getElementById('dbSearchInput')?.value || '').trim().toLowerCase();

  filteredDbData = rawDbData.filter(row => {
    // Global Search
    if (globalSearch) {
      const matchGlobal = Object.values(row).some(val => 
        val !== null && val !== undefined && String(val).toLowerCase().includes(globalSearch)
      );
      if (!matchGlobal) return false;
    }

    // Column Filters
    for (const [colKey, filterVal] of Object.entries(dbColumnFilters)) {
      if (filterVal) {
        const cellVal = String(row[colKey] || '').toLowerCase();
        if (!cellVal.includes(filterVal.toLowerCase())) {
          return false;
        }
      }
    }

    return true;
  });
}

function filterDbTableGlobal(query) {
  applyDbFilters();
  renderDbTable();
}

function updateDbColumnFilter(colKey, value, inputEl) {
  dbColumnFilters[colKey] = value;

  const cursorStart = inputEl ? inputEl.selectionStart : null;
  const cursorEnd = inputEl ? inputEl.selectionEnd : null;

  applyDbFilters();
  renderDbTable();

  const newInput = document.querySelector(`#tab-DB .column-filter-input[data-col="${colKey}"]`);
  if (newInput) {
    newInput.focus();
    if (cursorStart !== null && cursorEnd !== null) {
      try { newInput.setSelectionRange(cursorStart, cursorEnd); } catch (e) {}
    }
  }
}

function selectDbRow(idx) {
  selectedDbRowIndex = idx;
  selectedDbRowData = filteredDbData[idx] || null;

  const rows = document.querySelectorAll('#dbTableBody tr');
  rows.forEach((r, i) => {
    if (i === idx) {
      r.classList.add('selected-row');
    } else {
      r.classList.remove('selected-row');
    }
  });
}

function renderDbTable() {
  const head = document.getElementById('dbTableHead');
  const body = document.getElementById('dbTableBody');
  const foot = document.getElementById('dbTableFoot');

  if (!head || !body) return;

  if (filteredDbData.length === 0) {
    head.innerHTML = '';
    body.innerHTML = '<tr><td colspan="15" style="text-align:center;padding:25px;color:var(--text-secondary);">هیچ سایتی با مشخصات وارد شده پیدا نشد</td></tr>';
    if (foot) foot.innerHTML = '';
    return;
  }

  const columns = Object.keys(rawDbData[0] || {});

  function stickyClass(idx) {
    if (idx === 0) return 'sticky-col-0';
    if (idx === 1) return 'sticky-col-1';
    return '';
  }

  // 1. Header Row
  let headerHtml = '<tr>';
  columns.forEach((col, idx) => {
    const cls = stickyClass(idx);
    headerHtml += `<th class="${cls}">${col}</th>`;
  });
  headerHtml += '</tr>';

  // Auto Filter Row
  headerHtml += '<tr class="filter-row">';
  columns.forEach((col, idx) => {
    const cls = stickyClass(idx);
    const val = dbColumnFilters[col] || '';
    headerHtml += `<th class="filter-cell ${cls}"><input class="column-filter-input" data-col="${col}" type="text" value="${val}" oninput="updateDbColumnFilter('${col}', this.value, this)" placeholder="فیلتر..."></th>`;
  });
  headerHtml += '</tr>';

  head.innerHTML = headerHtml;

  // 2. Body Rows
  let bodyHtml = '';
  filteredDbData.forEach((row, rIdx) => {
    const isSelected = rIdx === selectedDbRowIndex;
    const bgClass = isSelected ? 'selected-row' : (rIdx % 2 === 0 ? 'row-even' : 'row-odd');
    bodyHtml += `<tr class="${bgClass}" onclick="selectDbRow(${rIdx})" ondblclick="openDbMapModal()" style="cursor: pointer;">`;
    columns.forEach((col, idx) => {
      const cls = stickyClass(idx);
      const val = String(row[col] ?? '').trim();
      const titleAttr = val ? `title="${col}: ${val.replace(/"/g, '&quot;')}"` : '';
      bodyHtml += `<td class="${cls}" ${titleAttr}>${row[col] ?? ''}</td>`;
    });
    bodyHtml += '</tr>';
  });

  body.innerHTML = bodyHtml;

  // 3. Footer Summary Row
  if (foot) {
    let footHtml = '<tr>';
    columns.forEach((col, idx) => {
      const cls = stickyClass(idx);
      if (idx === 0) {
        footHtml += `<td class="${cls}"><strong>تعداد: ${filteredDbData.length}</strong></td>`;
      } else {
        footHtml += `<td class="${cls}"></td>`;
      }
    });
    footHtml += '</tr>';
    foot.innerHTML = footHtml;
  }

  const countBadge = document.getElementById('dbRecordCount');
  if (countBadge) {
    countBadge.textContent = `${filteredDbData.length} رکورد دیتابیس سایت‌ها`;
  }

  if (typeof updateStatusRecordCount === 'function') {
    updateStatusRecordCount(filteredDbData.length);
  }

  // Adjust second frozen column right offset
  requestAnimationFrame(() => {
    const table = document.getElementById('dbTable');
    if (!table) return;
    const firstCol0 = table.querySelector('th.sticky-col-0, td.sticky-col-0');
    if (!firstCol0) return;
    const col0Width = firstCol0.getBoundingClientRect().width;

    const allCol1 = table.querySelectorAll('.sticky-col-1');
    allCol1.forEach(el => {
      el.style.right = col0Width + 'px';
    });
  });
}

// --------------------------------------------------------------------------------
// 2. EXCEL EXPORT & BACKUP
// --------------------------------------------------------------------------------

function exportDbToExcel() {
  const dataToExport = filteredDbData && filteredDbData.length > 0 ? filteredDbData : rawDbData;

  if (!dataToExport || dataToExport.length === 0) {
    alert('هیچ رکوردی جهت خروجی به اکسل وجود ندارد.');
    return;
  }

  const columns = Object.keys(dataToExport[0] || {});
  if (columns.length === 0) {
    alert('ساختار ستون‌های دیتابیس معتبر نیست.');
    return;
  }

  const now = new Date();
  const dateStr = now.toLocaleDateString('fa-IR').replace(/\//g, '-');
  const fileName = `دیتابیس_سایتها_${dateStr}.xls`;

  // Build clean HTML table format for Excel (with full Persian RTL formatting)
  let tableHtml = `<!DOCTYPE html><html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40" dir="rtl"><head><meta charset="utf-8"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Database</x:Name><x:WorksheetOptions><x:DisplayRightToLeft/><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><style>
    body { font-family: 'Calibri', 'Vazirmatn', sans-serif; direction: rtl; text-align: right; }
    table { border-collapse: collapse; width: 100%; direction: rtl; }
    th { background-color: #107C41; color: #ffffff; font-weight: bold; font-size: 11pt; border: 1px solid #0b5c30; padding: 8px; text-align: center; }
    td { font-size: 10pt; border: 1px solid #d1d5db; padding: 6px; text-align: center; mso-number-format:"\\@"; }
    tr:nth-child(even) { background-color: #f9fafb; }
    .bold-cell { font-weight: bold; }
  </style></head><body><table>`;

  // Header
  tableHtml += '<thead><tr>';
  tableHtml += '<th>ردیف</th>';
  columns.forEach(col => {
    tableHtml += `<th>${col}</th>`;
  });
  tableHtml += '</tr></thead><tbody>';

  // Rows
  dataToExport.forEach((row, idx) => {
    tableHtml += `<tr><td>${idx + 1}</td>`;
    columns.forEach(col => {
      let val = row[col] !== undefined && row[col] !== null ? String(row[col]) : '';
      val = val.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
      tableHtml += `<td>${val}</td>`;
    });
    tableHtml += '</tr>';
  });

  tableHtml += '</tbody></table></body></html>';

  const blob = new Blob(['\uFEFF' + tableHtml], { type: 'application/vnd.ms-excel;charset=utf-8' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
}

function backupDbExcel() {
  if (confirm('آیا مایل به تهیه و دریافت مستقیم نسخه پشتیبان اکسل (.xlsx) از دیتابیس سایت‌ها هستید؟')) {
    window.location.href = '/api/db/backup-excel';
  }
}

function backupDbSql() {
  if (confirm('آیا مایل به دانلود اسکریپت کامل SQL دیتابیس سایت‌ها (.sql) شامل کدهای ایجاد جدول و تمام رکوردهای فعلی هستید؟')) {
    window.location.href = '/api/db/backup-sql';
  }
}

async function backupDbNativeSql() {
  if (!confirm('آیا مایل به اجرای دستور BACKUP DATABASE در اس‌کیو‌ال سرور و ذخیره فایل پشتیبان بومی (.BAK) روی سرور هستید؟')) {
    return;
  }

  try {
    const res = await fetch('/api/db/backup-native-sql', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ dbName: 'DB' })
    });

    const contentType = res.headers.get('content-type') || '';
    if (!contentType.includes('application/json')) {
      if (!res.ok) {
        alert('⚠️ سرور نود (Node.js) نیاز به راه‌اندازی مجدد (Restart) دارد تا روت‌های جدید پشتیبان‌گیری بارگذاری شوند.\n\nلطفاً در محیط ترمینال سرور، یک بار کلیدهای Ctrl + C را زده و سپس دستور node server.js را اجرا فرمایید.');
        return;
      }
    }

    const result = await res.json();

    if (result.success) {
      alert(`✅ ${result.message}\n\nمسیر فایل در سرور:\n${result.backupLocation}`);
    } else {
      alert(`❌ ${result.error}`);
    }
  } catch (err) {
    alert('خطا در ارتباط با سرور: ' + err.message);
  }
}

// --------------------------------------------------------------------------------
// 3. DB EXCEL UPDATE MODAL (بروز رسانی دیتابیس جدید)
// --------------------------------------------------------------------------------

let selectedDbUpdateFile = null;

function openDbUpdateModal() {
  selectedDbUpdateFile = null;
  const input = document.getElementById('dbUploadFileInput');
  if (input) input.value = '';
  const nameDisp = document.getElementById('dbUploadFileNameDisplay');
  if (nameDisp) nameDisp.textContent = 'برای انتخاب فایل کلیک کنید یا فایل را اینجا رها کنید';
  
  const progress = document.getElementById('dbUploadProgress');
  if (progress) progress.style.display = 'none';
  const banner = document.getElementById('dbUploadResultBanner');
  if (banner) banner.style.display = 'none';

  const modal = document.getElementById('dbUpdateModal');
  if (modal) modal.style.display = 'flex';
}

function closeDbUpdateModal(event) {
  if (event && event.target !== event.currentTarget) return;
  const modal = document.getElementById('dbUpdateModal');
  if (modal) modal.style.display = 'none';
}

function handleDbFileSelected(input) {
  if (input.files && input.files[0]) {
    selectedDbUpdateFile = input.files[0];
    const nameDisp = document.getElementById('dbUploadFileNameDisplay');
    if (nameDisp) {
      nameDisp.innerHTML = `📄 <strong>${selectedDbUpdateFile.name}</strong> (${(selectedDbUpdateFile.size / 1024).toFixed(1)} KB)`;
    }
  }
}

async function executeDbExcelUpload() {
  if (!selectedDbUpdateFile) {
    alert('لطفاً ابتدا فایل اکسل دیتابیس را انتخاب نمایید.');
    return;
  }

  if (!confirm('توجه: با اجرای این عملیات تمام اطلاعات جدول دیتابیس سایت‌ها با داده‌های فایل انتخابی جایگزین خواهد شد. آیا مطمئن هستید؟')) {
    return;
  }

  const btn = document.getElementById('btnSubmitDbUpload');
  const progress = document.getElementById('dbUploadProgress');
  const banner = document.getElementById('dbUploadResultBanner');

  if (btn) btn.disabled = true;
  if (progress) progress.style.display = 'block';
  if (banner) banner.style.display = 'none';

  const formData = new FormData();
  formData.append('dbFile', selectedDbUpdateFile);

  try {
    const res = await fetch('/api/db/upload-excel', {
      method: 'POST',
      body: formData
    });
    const result = await res.json();

    if (result.success) {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box success';
      banner.innerHTML = `✅ <strong>عملیات با موفقیت انجام شد:</strong> ${result.message}`;
      
      setTimeout(() => {
        closeDbUpdateModal();
        loadDB();
      }, 2000);
    } else {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box error';
      banner.innerHTML = `❌ <strong>خطا:</strong> ${result.error || 'خطا در بروزرسانی دیتابیس'}`;
    }
  } catch (err) {
    banner.style.display = 'block';
    banner.className = 'modal-alert-box error';
    banner.innerHTML = `❌ <strong>خطای ارتباط با سرور:</strong> ${err.message}`;
  } finally {
    if (btn) btn.disabled = false;
    if (progress) progress.style.display = 'none';
  }
}

// --------------------------------------------------------------------------------
// 4. CONTRACTOR UPDATE MODAL (بروز رسانی پیمانکار - شرکت)
// --------------------------------------------------------------------------------

let selectedContractorFile = null;

function openDbContractorUpdateModal() {
  selectedContractorFile = null;
  const input = document.getElementById('dbContractorFileInput');
  if (input) input.value = '';
  const nameDisp = document.getElementById('dbContractorFileNameDisplay');
  if (nameDisp) nameDisp.textContent = 'انتخاب فایل اکسل پیمانکاران';
  const banner = document.getElementById('dbContractorResultBanner');
  if (banner) banner.style.display = 'none';

  const modal = document.getElementById('dbContractorModal');
  if (modal) modal.style.display = 'flex';
}

function closeDbContractorModal(event) {
  if (event && event.target !== event.currentTarget) return;
  const modal = document.getElementById('dbContractorModal');
  if (modal) modal.style.display = 'none';
}

function handleContractorFileSelected(input) {
  if (input.files && input.files[0]) {
    selectedContractorFile = input.files[0];
    const nameDisp = document.getElementById('dbContractorFileNameDisplay');
    if (nameDisp) {
      nameDisp.innerHTML = `📄 <strong>${selectedContractorFile.name}</strong> (${(selectedContractorFile.size / 1024).toFixed(1)} KB)`;
    }
  }
}

async function executeDbContractorUpload() {
  if (!selectedContractorFile) {
    alert('لطفاً فایل اکسل پیمانکاران را انتخاب نمایید.');
    return;
  }

  const btn = document.getElementById('btnSubmitContractorUpload');
  const banner = document.getElementById('dbContractorResultBanner');
  if (btn) btn.disabled = true;

  const formData = new FormData();
  formData.append('contractorFile', selectedContractorFile);

  try {
    const res = await fetch('/api/db/update-contractor', {
      method: 'POST',
      body: formData
    });
    const result = await res.json();

    if (result.success) {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box success';
      banner.innerHTML = `✅ ${result.message}`;
      setTimeout(() => {
        closeDbContractorModal();
        loadDB();
      }, 1800);
    } else {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box error';
      banner.innerHTML = `❌ ${result.error || 'خطا در بروزرسانی پیمانکاران'}`;
    }
  } catch (err) {
    banner.style.display = 'block';
    banner.className = 'modal-alert-box error';
    banner.innerHTML = `❌ ${err.message}`;
  } finally {
    if (btn) btn.disabled = false;
  }
}

// --------------------------------------------------------------------------------
// 5. NEW SITES MODAL (طرح‌های جدید - frmDbNewSite)
// --------------------------------------------------------------------------------

function openDbNewSitesModal() {
  const modal = document.getElementById('dbNewSitesModal');
  if (modal) modal.style.display = 'flex';
  
  populateDatalists();
  loadDbNewSites();
}

function closeDbNewSitesModal(event) {
  if (event && event.target !== event.currentTarget) return;
  const modal = document.getElementById('dbNewSitesModal');
  if (modal) modal.style.display = 'none';
}

function populateDatalists() {
  const contractors = new Set();
  const regions = new Set();

  rawDbData.forEach(row => {
    if (row['پیمانکار']) contractors.add(String(row['پیمانکار']).trim());
    if (row['منطقه']) regions.add(String(row['منطقه']).trim());
  });

  const cList = document.getElementById('contractorList');
  if (cList) {
    cList.innerHTML = Array.from(contractors).map(c => `<option value="${c}">`).join('');
  }

  const rList = document.getElementById('regionList');
  if (rList) {
    rList.innerHTML = Array.from(regions).map(r => `<option value="${r}">`).join('');
  }
}

function handleNewSiteCodeInput(val) {
  const cleanVal = val.replace(/[^0-9]/g, '');
  document.getElementById('newSiteCode').value = cleanVal;

  const alertBox = document.getElementById('newSiteStatusAlert');
  if (cleanVal.length === 4) {
    const fullCode = 'ES' + cleanVal;
    const existing = rawDbData.find(r => String(r['کد سایت'] || r['لوکیشن'] || '').toUpperCase() === fullCode);
    if (existing) {
      alertBox.style.display = 'block';
      alertBox.className = 'modal-alert-box error';
      alertBox.textContent = `⚠️ توجه: سایت ${fullCode} هم‌اکنون با نام «${existing['نام سایت'] || ''}» در دیتابیس وجود دارد.`;
      const nameInput = document.getElementById('newSiteName');
      if (nameInput) nameInput.value = existing['نام سایت'] || '';
    } else {
      alertBox.style.display = 'block';
      alertBox.className = 'modal-alert-box success';
      alertBox.textContent = `✅ کد سایت ${fullCode} در دیتابیس جدید است و تداخلی ندارد.`;
    }
  } else {
    alertBox.style.display = 'none';
  }
}

async function loadDbNewSites() {
  const tbody = document.getElementById('newSitesTableBody');
  const badge = document.getElementById('newSitesCountBadge');
  if (!tbody) return;

  tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px;">در حال بارگذاری...</td></tr>';

  try {
    const res = await fetch('/api/db/new-sites');
    const result = await res.json();

    if (!result.success) {
      tbody.innerHTML = `<tr><td colspan="7" style="text-align: center; color: red;">خطا: ${result.error}</td></tr>`;
      return;
    }

    const sites = result.data || [];
    if (badge) badge.textContent = `${sites.length} سایت جدید`;

    if (sites.length === 0) {
      tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px; color: var(--text-secondary);">هیچ سایت جدیدی در این بخش ثبت نشده است</td></tr>';
      return;
    }

    let html = '';
    sites.forEach((s, idx) => {
      const loc = s.location || '';
      html += `
        <tr>
          <td style="font-weight: bold; color: #64748b;">${idx + 1}</td>
          <td style="direction: ltr; font-family: monospace; font-weight: 800; color: #4f46e5;">${loc}</td>
          <td style="font-weight: 700; color: #0f172a; text-align: right;">${s.siteName || ''}</td>
          <td style="color: #334155;">${s.contractor || '-'}</td>
          <td style="color: #334155;">${s.region || '-'}</td>
          <td style="color: #475569; font-size: 12px; text-align: right;">${s.address || '-'}</td>
          <td>
            <button class="btn-delete-style" onclick="deleteDbNewSite('${loc}')" style="padding: 4px 10px; font-size: 11.5px; background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); border-color: #dc2626; color: #ffffff;">🗑️ حذف</button>
          </td>
        </tr>
      `;
    });
    tbody.innerHTML = html;
  } catch (err) {
    tbody.innerHTML = `<tr><td colspan="7" style="text-align: center; color: red;">خطا: ${err.message}</td></tr>`;
  }
}

async function submitDbNewSite() {
  const code = (document.getElementById('newSiteCode')?.value || '').trim();
  const siteName = (document.getElementById('newSiteName')?.value || '').trim();
  const contractor = (document.getElementById('newSiteContractor')?.value || '').trim();
  const region = (document.getElementById('newSiteRegion')?.value || '').trim();
  const address = (document.getElementById('newSiteAddress')?.value || '').trim();

  if (code.length !== 4) {
    alert('کد سایت باید یک عدد ۴ رقمی باشد.');
    document.getElementById('newSiteCode')?.focus();
    return;
  }

  if (siteName.length < 2) {
    alert('لطفاً نام سایت را به صورت کامل وارد نمایید.');
    document.getElementById('newSiteName')?.focus();
    return;
  }

  try {
    const res = await fetch('/api/db/new-sites', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        location: 'ES' + code,
        siteName,
        contractor,
        region,
        address
      })
    });
    const result = await res.json();

    if (result.success) {
      alert(result.message);
      document.getElementById('newSiteCode').value = '';
      document.getElementById('newSiteName').value = '';
      document.getElementById('newSiteContractor').value = '';
      document.getElementById('newSiteRegion').value = '';
      document.getElementById('newSiteAddress').value = '';
      document.getElementById('newSiteStatusAlert').style.display = 'none';
      loadDbNewSites();
    } else {
      alert('خطا در ثبت سایت: ' + result.error);
    }
  } catch (err) {
    alert('خطا در ارتباط با سرور: ' + err.message);
  }
}

async function deleteDbNewSite(location) {
  if (!confirm(`آیا از حذف سایت ${location} از لیست طرح‌های جدید اطمینان دارید؟`)) return;

  try {
    const res = await fetch(`/api/db/new-sites/${encodeURIComponent(location)}`, { method: 'DELETE' });
    const result = await res.json();
    if (result.success) {
      loadDbNewSites();
    } else {
      alert('خطا در حذف: ' + result.error);
    }
  } catch (err) {
    alert('خطا در ارتباط با سرور: ' + err.message);
  }
}

// --------------------------------------------------------------------------------
// 6. DELETED SITES MODAL (طرح‌های حذف شده - frmDbDeletedSite)
// --------------------------------------------------------------------------------

function openDbDeletedSitesModal() {
  const modal = document.getElementById('dbDeletedSitesModal');
  if (modal) modal.style.display = 'flex';
  loadDbDeletedSites();
}

function closeDbDeletedSitesModal(event) {
  if (event && event.target !== event.currentTarget) return;
  const modal = document.getElementById('dbDeletedSitesModal');
  if (modal) modal.style.display = 'none';
}

function handleDeletedSiteCodeInput(val) {
  const cleanVal = val.replace(/[^0-9]/g, '');
  document.getElementById('delSiteCode').value = cleanVal;

  const alertBox = document.getElementById('delSiteStatusAlert');
  if (cleanVal.length === 4) {
    const fullCode = 'ES' + cleanVal;
    const existing = rawDbData.find(r => String(r['کد سایت'] || r['لوکیشن'] || '').toUpperCase() === fullCode);
    if (existing) {
      alertBox.style.display = 'block';
      alertBox.className = 'modal-alert-box info';
      alertBox.textContent = `📍 نام سایت در دیتابیس: ${existing['نام سایت'] || ''}`;
      const nameInput = document.getElementById('delSiteName');
      if (nameInput) nameInput.value = existing['نام سایت'] || '';
    } else {
      alertBox.style.display = 'block';
      alertBox.className = 'modal-alert-box error';
      alertBox.textContent = `⚠️ این کد سایت (${fullCode}) در دیتابیس فعلی پیدا نشد.`;
    }
  } else {
    alertBox.style.display = 'none';
  }
}

async function loadDbDeletedSites() {
  const tbody = document.getElementById('delSitesTableBody');
  const badge = document.getElementById('delSitesCountBadge');
  if (!tbody) return;

  tbody.innerHTML = '<tr><td colspan="5" style="text-align: center; padding: 20px;">در حال بارگذاری...</td></tr>';

  try {
    const res = await fetch('/api/db/deleted-sites');
    const result = await res.json();

    if (!result.success) {
      tbody.innerHTML = `<tr><td colspan="5" style="text-align: center; color: red;">خطا: ${result.error}</td></tr>`;
      return;
    }

    const sites = result.data || [];
    if (badge) badge.textContent = `${sites.length} سایت حذف شده`;

    if (sites.length === 0) {
      tbody.innerHTML = '<tr><td colspan="5" style="text-align: center; padding: 20px; color: var(--text-secondary);">هیچ سایتی در لیست حذف شده‌ها ثبت نشده است</td></tr>';
      return;
    }

    let html = '';
    sites.forEach((s, idx) => {
      const loc = s.location || '';
      const d = s.deletedDate ? new Date(s.deletedDate).toLocaleDateString('fa-IR') : '-';
      html += `
        <tr>
          <td style="font-weight: bold; color: #64748b;">${idx + 1}</td>
          <td style="direction: ltr; font-family: monospace; font-weight: 800; color: #dc2626;">${loc}</td>
          <td style="font-weight: 700; color: #0f172a; text-align: right;">${s.siteName || ''}</td>
          <td style="color: #475569; font-size: 12px;">${d}</td>
          <td>
            <button class="btn-delete-style" onclick="deleteDbDeletedSite('${loc}')" style="padding: 4px 10px; font-size: 11.5px; background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); border-color: #dc2626; color: #ffffff;">🗑️ حذف</button>
          </td>
        </tr>
      `;
    });
    tbody.innerHTML = html;
  } catch (err) {
    tbody.innerHTML = `<tr><td colspan="5" style="text-align: center; color: red;">خطا: ${err.message}</td></tr>`;
  }
}

async function submitDbDeletedSite() {
  const code = (document.getElementById('delSiteCode')?.value || '').trim();
  const siteName = (document.getElementById('delSiteName')?.value || '').trim();

  if (code.length !== 4) {
    alert('کد سایت باید ۴ رقم باشد.');
    document.getElementById('delSiteCode')?.focus();
    return;
  }

  try {
    const res = await fetch('/api/db/deleted-sites', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        location: 'ES' + code,
        siteName
      })
    });
    const result = await res.json();

    if (result.success) {
      alert(result.message);
      document.getElementById('delSiteCode').value = '';
      document.getElementById('delSiteName').value = '';
      document.getElementById('delSiteStatusAlert').style.display = 'none';
      loadDbDeletedSites();
    } else {
      alert('خطا: ' + result.error);
    }
  } catch (err) {
    alert('خطای سرور: ' + err.message);
  }
}

async function deleteDbDeletedSite(location) {
  if (!confirm(`آیا از حذف سایت ${location} از لیست حذف شده‌ها اطمینان دارید؟`)) return;

  try {
    const res = await fetch(`/api/db/deleted-sites/${encodeURIComponent(location)}`, { method: 'DELETE' });
    const result = await res.json();
    if (result.success) {
      loadDbDeletedSites();
    } else {
      alert('خطا: ' + result.error);
    }
  } catch (err) {
    alert('خطا: ' + err.message);
  }
}

// --------------------------------------------------------------------------------
// 7. DB MAP MODAL (نمایش لوکیشن روی نقشه - frmDbMaps)
// --------------------------------------------------------------------------------

function parseCoordinates(row) {
  let lat = null;
  let lng = null;

  // Search for latitude / longitude keys
  const latKey = Object.keys(row).find(k => k.includes('عرض') || k.toLowerCase().includes('lat') || k === 'Y');
  const lngKey = Object.keys(row).find(k => k.includes('طول') || k.toLowerCase().includes('long') || k.toLowerCase().includes('lng') || k === 'X');

  if (latKey && row[latKey]) {
    lat = parseFloat(String(row[latKey]).replace('/', '.').trim());
  }
  if (lngKey && row[lngKey]) {
    lng = parseFloat(String(row[lngKey]).replace('/', '.').trim());
  }

  // Fallback defaults for Isfahan center if invalid
  if (!lat || isNaN(lat) || lat < 25 || lat > 40) lat = 32.6546;
  if (!lng || isNaN(lng) || lng < 45 || lng > 65) lng = 51.6680;

  return { lat, lng };
}

function openDbMapModal() {
  if (!selectedDbRowData) {
    if (filteredDbData.length > 0) {
      selectDbRow(0);
    } else {
      alert('هیچ سایتی جهت نمایش روی نقشه انتخاب نشده است.');
      return;
    }
  }

  const row = selectedDbRowData;
  const siteCode = row['کد سایت'] || row['لوکیشن'] || '-';
  const siteName = row['نام سایت'] || '-';
  const region = row['منطقه'] || '-';
  const contractor = row['پیمانکار'] || '-';

  const { lat, lng } = parseCoordinates(row);

  document.getElementById('mapSiteCode').textContent = siteCode;
  document.getElementById('mapSiteName').textContent = siteName;
  document.getElementById('mapSiteRegion').textContent = region;
  document.getElementById('mapSiteContractor').textContent = contractor;
  document.getElementById('mapSiteCoords').textContent = `${lat.toFixed(5)}, ${lng.toFixed(5)}`;

  // Navigation Links
  document.getElementById('btnOpenGoogleMaps').href = `https://www.google.com/maps?q=${lat},${lng}`;
  document.getElementById('btnOpenWaze').href = `https://waze.com/ul?ll=${lat},${lng}&navigate=yes`;

  const modal = document.getElementById('dbMapModal');
  if (modal) modal.style.display = 'flex';

  // Initialize or update Leaflet map after modal becomes visible
  setTimeout(() => {
    initOrUpdateLeafletMap(lat, lng, siteCode, siteName);
  }, 200);
}

function closeDbMapModal(event) {
  if (event && event.target !== event.currentTarget) return;
  const modal = document.getElementById('dbMapModal');
  if (modal) modal.style.display = 'none';
}

function initOrUpdateLeafletMap(lat, lng, siteCode, siteName) {
  const mapContainer = document.getElementById('dbLeafletMap');
  if (!mapContainer || typeof L === 'undefined') return;

  if (!leafletMapInstance) {
    leafletMapInstance = L.map('dbLeafletMap').setView([lat, lng], 15);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      attribution: '© OpenStreetMap contributors'
    }).addTo(leafletMapInstance);
  } else {
    leafletMapInstance.invalidateSize();
    leafletMapInstance.setView([lat, lng], 15);
  }

  if (leafletMarkerInstance) {
    leafletMarkerInstance.remove();
  }

  leafletMarkerInstance = L.marker([lat, lng]).addTo(leafletMapInstance)
    .bindPopup(`<strong>${siteCode}</strong><br>${siteName}`)
    .openPopup();
}

// --------------------------------------------------------------------------------
// 8. DB STATS MODAL (آمار و سطرهای گروه‌بندی شده - frmDbSearch)
// --------------------------------------------------------------------------------

function openDbStatsModal() {
  const modal = document.getElementById('dbStatsModal');
  if (modal) modal.style.display = 'flex';
  loadDbStats();
}

function closeDbStatsModal(event) {
  if (event && event.target !== event.currentTarget) return;
  const modal = document.getElementById('dbStatsModal');
  if (modal) modal.style.display = 'none';
}

async function loadDbStats() {
  const totalSitesEl = document.getElementById('statsTotalSites');
  const contractorBody = document.getElementById('statsContractorBody');
  const regionBody = document.getElementById('statsRegionBody');

  if (totalSitesEl) totalSitesEl.textContent = String(rawDbData.length);

  try {
    const res = await fetch('/api/db/stats');
    const result = await res.json();

    if (result.success) {
      if (totalSitesEl) totalSitesEl.textContent = String(result.total || rawDbData.length);

      // Contractor table
      if (contractorBody) {
        if (result.byContractor.length === 0) {
          contractorBody.innerHTML = '<tr><td colspan="2" style="text-align:center;">داده‌ای یافت نشد</td></tr>';
        } else {
          contractorBody.innerHTML = result.byContractor.map(item => `
            <tr>
              <td><strong>${item.item}</strong></td>
              <td style="text-align: center; color: var(--accent-primary); font-weight: bold;">${item.count}</td>
            </tr>
          `).join('');
        }
      }

      // Region table
      if (regionBody) {
        if (result.byRegion.length === 0) {
          regionBody.innerHTML = '<tr><td colspan="2" style="text-align:center;">داده‌ای یافت نشد</td></tr>';
        } else {
          regionBody.innerHTML = result.byRegion.map(item => `
            <tr>
              <td><strong>${item.item}</strong></td>
              <td style="text-align: center; color: var(--accent-primary); font-weight: bold;">${item.count}</td>
            </tr>
          `).join('');
        }
      }
    }
  } catch (err) {
    console.error('loadDbStats error:', err);
  }
}

// --------------------------------------------------------------------------------
// 9. DB RESTORE MODAL (بازیابی دیتابیس از فایل‌های پشتیبان)
// --------------------------------------------------------------------------------

let latestDbBackupFile = null;
let customRestoreSelectedFile = null;

function openDbRestoreModal() {
  customRestoreSelectedFile = null;
  const fileInput = document.getElementById('dbRestoreCustomFileInput');
  if (fileInput) fileInput.value = '';

  const progress = document.getElementById('dbRestoreProgress');
  if (progress) progress.style.display = 'none';
  const banner = document.getElementById('dbRestoreResultBanner');
  if (banner) banner.style.display = 'none';

  const modal = document.getElementById('dbRestoreModal');
  if (modal) modal.style.display = 'flex';

  loadDbBackupsList();
}

function closeDbRestoreModal(event) {
  if (event && event.target !== event.currentTarget) return;
  const modal = document.getElementById('dbRestoreModal');
  if (modal) modal.style.display = 'none';
}

async function loadDbBackupsList() {
  const tbody = document.getElementById('dbBackupsTableBody');
  const countBadge = document.getElementById('backupsCountBadge');
  const latestCard = document.getElementById('latestBackupHighlightCard');

  if (tbody) {
    tbody.innerHTML = '<tr><td colspan="6" style="text-align: center; padding: 20px;">در حال بارگذاری لیست فایل‌های پشتیبان...</td></tr>';
  }

  try {
    const res = await fetch('/api/db/backup-list');
    const result = await res.json();

    if (!result.success) {
      if (tbody) tbody.innerHTML = `<tr><td colspan="6" style="text-align:center; color:red;">خطا: ${result.error}</td></tr>`;
      return;
    }

    const backups = result.backups || [];
    if (countBadge) countBadge.textContent = `${backups.length} فایل پشتیبان`;

    // Highlight latest backup card
    if (result.latestBackup && latestCard) {
      latestDbBackupFile = result.latestBackup;
      latestCard.style.display = 'block';
      document.getElementById('latestBackupName').textContent = result.latestBackup.filename;
      document.getElementById('latestBackupDate').textContent = result.latestBackup.dateJalali;
      document.getElementById('latestBackupSize').textContent = result.latestBackup.sizeFormatted;
      document.getElementById('latestBackupType').textContent = result.latestBackup.fileTypeLabel;
    } else if (latestCard) {
      latestDbBackupFile = null;
      latestCard.style.display = 'none';
    }

    // Render Table
    if (tbody) {
      if (backups.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" style="text-align: center; padding: 20px; color: var(--text-secondary);">هیچ فایل پشتیبانی در پوشه BackUp_DB یافت نشد.</td></tr>';
        return;
      }

      let html = '';
      backups.forEach((b, idx) => {
        const isLatestBadge = b.isLatest ? '<span style="background:#10b981; color:#fff; font-size:10px; font-weight:800; padding:1px 6px; border-radius:10px; margin-right:6px;">جدیدترین</span>' : '';
        html += `
          <tr ${b.isLatest ? 'style="background: #f0fdf4;"' : ''}>
            <td style="font-weight: bold; color: #64748b;">${idx + 1}</td>
            <td style="text-align: right; direction: ltr; font-family: monospace; font-weight: bold; color: #0369a1;">
              ${b.filename}
              ${isLatestBadge}
            </td>
            <td><span style="font-size: 11.5px; font-weight: 600; color: #334155;">${b.fileTypeLabel}</span></td>
            <td style="direction: ltr; font-family: monospace; font-weight: bold; color: #047857;">${b.sizeFormatted}</td>
            <td style="font-size: 12px; color: #475569;">${b.dateJalali}</td>
            <td>
              <button class="btn-primary-action" onclick="executeRestoreBackupFile('${b.filename}')" style="padding: 4px 12px; font-size: 11.5px; background: linear-gradient(180deg, #10b981 0%, #059669 100%); border-color: #059669;">
                📥 بازیابی
              </button>
            </td>
          </tr>
        `;
      });
      tbody.innerHTML = html;
    }
  } catch (err) {
    if (tbody) tbody.innerHTML = `<tr><td colspan="6" style="text-align:center; color:red;">خطای ارتباط با سرور: ${err.message}</td></tr>`;
  }
}

async function executeRestoreFromLatest() {
  if (!latestDbBackupFile || !latestDbBackupFile.filename) {
    alert('فایل آخرین بک‌آپ در دسترس نیست.');
    return;
  }
  executeRestoreBackupFile(latestDbBackupFile.filename);
}

async function executeRestoreBackupFile(filename) {
  if (!confirm(`⚠️ هشدار مهم:\nبا بازیابی فایل «${filename}»، تمام داده‌های فعلی جدول دیتابیس سایت‌ها با اطلاعات این فایل پشتیبان جایگزین خواهند شد.\n\nآیا از ادامه عملیات بازیابی اطمینان کامل دارید؟`)) {
    return;
  }

  const progress = document.getElementById('dbRestoreProgress');
  const banner = document.getElementById('dbRestoreResultBanner');

  if (progress) progress.style.display = 'block';
  if (banner) banner.style.display = 'none';

  try {
    const res = await fetch('/api/db/restore-backup', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ filename })
    });
    const result = await res.json();

    if (result.success) {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box success';
      banner.innerHTML = `✅ <strong>عملیات بازیابی با موفقیت انجام شد:</strong> ${result.message} (${result.rowCount} رکورد ثبت شد)`;

      setTimeout(() => {
        closeDbRestoreModal();
        loadDB();
      }, 2000);
    } else {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box error';
      banner.innerHTML = `❌ <strong>خطا در بازیابی:</strong> ${result.error || 'خطای ناشناخته'}`;
    }
  } catch (err) {
    banner.style.display = 'block';
    banner.className = 'modal-alert-box error';
    banner.innerHTML = `❌ <strong>خطای ارتباط با سرور:</strong> ${err.message}`;
  } finally {
    if (progress) progress.style.display = 'none';
  }
}

function handleCustomRestoreFileSelected(input) {
  if (input.files && input.files[0]) {
    customRestoreSelectedFile = input.files[0];
  }
}

async function executeRestoreFromUpload() {
  if (!customRestoreSelectedFile) {
    alert('لطفاً ابتدا یک فایل پشتیبان (.sql, .xlsx, .bak, .json) انتخاب نمایید.');
    return;
  }

  if (!confirm(`⚠️ هشدار مهم:\nبا بازیابی فایل «${customRestoreSelectedFile.name}»، تمام اطلاعات دیتابیس با این فایل جایگزین خواهد شد. آیا مطمئن هستید؟`)) {
    return;
  }

  const progress = document.getElementById('dbRestoreProgress');
  const banner = document.getElementById('dbRestoreResultBanner');

  if (progress) progress.style.display = 'block';
  if (banner) banner.style.display = 'none';

  const formData = new FormData();
  formData.append('restoreFile', customRestoreSelectedFile);

  try {
    const res = await fetch('/api/db/restore-backup', {
      method: 'POST',
      body: formData
    });
    const result = await res.json();

    if (result.success) {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box success';
      banner.innerHTML = `✅ <strong>عملیات بازیابی با موفقیت انجام شد:</strong> ${result.message} (${result.rowCount} رکورد)`;

      setTimeout(() => {
        closeDbRestoreModal();
        loadDB();
      }, 2000);
    } else {
      banner.style.display = 'block';
      banner.className = 'modal-alert-box error';
      banner.innerHTML = `❌ <strong>خطا در بازیابی:</strong> ${result.error || 'خطای ناشناخته'}`;
    }
  } catch (err) {
    banner.style.display = 'block';
    banner.className = 'modal-alert-box error';
    banner.innerHTML = `❌ <strong>خطای ارتباط با سرور:</strong> ${err.message}`;
  } finally {
    if (progress) progress.style.display = 'none';
  }
}
