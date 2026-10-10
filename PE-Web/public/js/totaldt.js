// ============================================================================
//                   ماژول جامع TotalDt (توتال دان‌تایم) برای وب
//                         PE-Web TotalDt Module
// ============================================================================

let totalDtState = {
  page: 1,
  pageSize: 50,
  search: '',
  startDate: '',
  endDate: '',
  preimpel: [],
  sortCol: 'CREATETIME',
  sortDir: 'DESC',
  rawData: [],
  totalRecords: 0
};

// ============================================================================
// 1. MODAL OPENERS & CLOSERS
// ============================================================================

function openTotalDtMainModal() {
  const modal = document.getElementById('totalDtMainModal');
  if (!modal) return;
  modal.style.display = 'flex';
  loadTotalDtData(1);
}

function closeTotalDtMainModal(e) {
  if (e && e.target !== e.currentTarget && !e.target.classList.contains('modal-close') && !e.target.classList.contains('btn-cancel')) return;
  const modal = document.getElementById('totalDtMainModal');
  if (modal) modal.style.display = 'none';
}

function openTotalDtFilterModal() {
  const modal = document.getElementById('totalDtFilterSelectorModal');
  if (!modal) return;
  modal.style.display = 'flex';
  
  // Set default dates if empty
  const txtStart = document.getElementById('totalDtFilterStartDate');
  const txtEnd = document.getElementById('totalDtFilterEndDate');
  if (txtStart && !txtStart.value) txtStart.value = totalDtState.startDate || '';
  if (txtEnd && !txtEnd.value) txtEnd.value = totalDtState.endDate || '';

  loadPreimpelList();
}

function closeTotalDtFilterModal(e) {
  if (e && e.target !== e.currentTarget && !e.target.classList.contains('modal-close') && !e.target.classList.contains('btn-cancel')) return;
  const modal = document.getElementById('totalDtFilterSelectorModal');
  if (modal) modal.style.display = 'none';
}

function openTotalDtExecutorModal() {
  const modal = document.getElementById('totalDtExecutorModal');
  if (!modal) return;
  modal.style.display = 'flex';
  loadTotalDtExecutorAnalysis();
}

function closeTotalDtExecutorModal(e) {
  if (e && e.target !== e.currentTarget && !e.target.classList.contains('modal-close') && !e.target.classList.contains('btn-cancel')) return;
  const modal = document.getElementById('totalDtExecutorModal');
  if (modal) modal.style.display = 'none';
}

function openTotalDtGroupStatsModal() {
  const modal = document.getElementById('totalDtGroupStatsModal');
  if (!modal) return;
  modal.style.display = 'flex';
  loadTotalDtGroupStats();
}

function closeTotalDtGroupStatsModal(e) {
  if (e && e.target !== e.currentTarget && !e.target.classList.contains('modal-close') && !e.target.classList.contains('btn-cancel')) return;
  const modal = document.getElementById('totalDtGroupStatsModal');
  if (modal) modal.style.display = 'none';
}

function openTotalDtImportModal() {
  const modal = document.getElementById('totalDtImportModal');
  if (!modal) return;
  modal.style.display = 'flex';
  const fileInput = document.getElementById('totalDtCsvFileInput');
  if (fileInput) fileInput.value = '';
  const statusDiv = document.getElementById('totalDtImportStatus');
  if (statusDiv) statusDiv.innerHTML = '';
}

function closeTotalDtImportModal(e) {
  if (e && e.target !== e.currentTarget && !e.target.classList.contains('modal-close') && !e.target.classList.contains('btn-cancel')) return;
  const modal = document.getElementById('totalDtImportModal');
  if (modal) modal.style.display = 'none';
}

function openTotalDtBackupModal() {
  const modal = document.getElementById('totalDtBackupModal');
  if (!modal) return;
  modal.style.display = 'flex';
  loadTotalDtBackups();
}

function closeTotalDtBackupModal(e) {
  if (e && e.target !== e.currentTarget && !e.target.classList.contains('modal-close') && !e.target.classList.contains('btn-cancel')) return;
  const modal = document.getElementById('totalDtBackupModal');
  if (modal) modal.style.display = 'none';
}

// ============================================================================
// 2. DATA LOADING & GRID RENDERING
// ============================================================================

async function loadTotalDtData(page = 1) {
  totalDtState.page = page;
  const loading = document.getElementById('totalDtLoading');
  const tableContainer = document.getElementById('totalDtTableContainer');
  const countBadge = document.getElementById('totalDtRecordCount');

  if (loading) loading.style.display = 'flex';
  if (tableContainer) tableContainer.style.display = 'none';

  try {
    const params = new URLSearchParams({
      page: totalDtState.page,
      pageSize: totalDtState.pageSize,
      search: totalDtState.search,
      startDate: totalDtState.startDate,
      endDate: totalDtState.endDate,
      preimpel: (totalDtState.preimpel || []).join('|||'),
      sortCol: totalDtState.sortCol,
      sortDir: totalDtState.sortDir
    });

    const res = await fetch(`/api/totaldt/data?${params.toString()}`);
    const json = await res.json();

    if (!json.success) {
      throw new Error(json.error || 'خطا در دریافت اطلاعات');
    }

    totalDtState.rawData = json.data || [];
    totalDtState.totalRecords = json.total || 0;

    if (countBadge) {
      countBadge.textContent = `${totalDtState.totalRecords.toLocaleString('fa-IR')} رکورد`;
    }

    renderTotalDtTable(json.data || []);
    renderTotalDtPagination(json.page, json.totalPages, json.total);

  } catch (err) {
    console.error('Error in loadTotalDtData:', err);
    alert('خطا در بارگذاری اطلاعات TotalDt: ' + err.message);
  } finally {
    if (loading) loading.style.display = 'none';
    if (tableContainer) tableContainer.style.display = 'block';
  }
}

function renderTotalDtTable(rows) {
  const thead = document.getElementById('totalDtTableHead');
  const tbody = document.getElementById('totalDtTableBody');
  if (!thead || !tbody) return;

  // Define Columns with exact widths and alignments to prevent overlap
  const columns = [
    { key: 'ORDERID', label: 'شماره سفارش', minWidth: '175px', align: 'center' },
    { key: 'PROCESSID', label: 'شماره فرایند', minWidth: '190px', align: 'center' },
    { key: 'CREATETIME_SHAMSI', label: 'زمان ایجاد', minWidth: '105px', align: 'center', tooltipKey: 'CREATETIME_MILADI' },
    { key: 'TARGET_NODE', label: 'کد سایت', minWidth: '90px', align: 'center', isBold: true },
    { key: 'نام سایت', label: 'نام سایت', minWidth: '130px', align: 'right' },
    { key: 'منطقه', label: 'منطقه', minWidth: '80px', align: 'center' },
    { key: 'GROUPNAME', label: 'نام گروه', minWidth: '175px', align: 'right' },
    { key: 'TITLE', label: 'عنوان اقدام', minWidth: '320px', maxWidth: '480px', align: 'right' },
    { key: 'PROCESSSTATUS', label: 'وضعیت', minWidth: '95px', align: 'center' },
    { key: 'DOWNTIME', label: 'مدت قطعی', minWidth: '90px', align: 'center' },
    { key: 'ACTION_START_DATE_SHAMSI', label: 'تاریخ شروع', minWidth: '95px', align: 'center', tooltipKey: 'ACTION_START_DATE_MILADI' },
    { key: 'ACTION_START_TIME', label: 'زمان شروع', minWidth: '80px', align: 'center' },
    { key: 'ACTION_END_DATE_SHAMSI', label: 'تاریخ پایان', minWidth: '95px', align: 'center', tooltipKey: 'ACTION_END_DATE_MILADI' },
    { key: 'ACTION_END_TIME', label: 'زمان پایان', minWidth: '80px', align: 'center' },
    { key: 'EXECUTOR_NAME', label: 'انجام‌دهنده', minWidth: '160px', align: 'right' },
    { key: 'CREATOR_NAME', label: 'سازنده', minWidth: '130px', align: 'right' },
    { key: 'PREIMPEL_DESC', label: 'شرح پیش از اجرا', minWidth: '280px', maxWidth: '420px', align: 'right' },
    { key: 'IMPEL_DESC', label: 'شرح حین اجرا', minWidth: '220px', maxWidth: '380px', align: 'right' },
    { key: 'CONFIRM_DESC', label: 'شرح تایید', minWidth: '180px', maxWidth: '320px', align: 'right' }
  ];

  // Render Thead
  let headHtml = '<tr>';
  headHtml += `<th style="width: 45px; min-width: 45px; max-width: 45px; text-align: center;">ردیف</th>`;
  columns.forEach(col => {
    const isSorted = (totalDtState.sortCol === col.key);
    const sortIcon = isSorted ? (totalDtState.sortDir === 'ASC' ? ' 🔼' : ' 🔽') : '';
    const styleStr = `min-width: ${col.minWidth}; ${col.maxWidth ? 'max-width:' + col.maxWidth + ';' : ''} text-align: ${col.align}; cursor: pointer; user-select: none; white-space: nowrap;`;
    headHtml += `<th onclick="sortTotalDt('${col.key}')" style="${styleStr}" title="جهت مرتب‌سازی کلیک کنید">
      ${col.label}${sortIcon}
    </th>`;
  });
  headHtml += '</tr>';
  thead.innerHTML = headHtml;

  // Render Tbody
  if (rows.length === 0) {
    tbody.innerHTML = `<tr><td colspan="${columns.length + 1}" style="text-align:center; padding: 30px; color: var(--text-muted);">رکوردی مطابق با فیلترهای جاری یافت نشد.</td></tr>`;
    return;
  }

  const startRowNumber = (totalDtState.page - 1) * totalDtState.pageSize + 1;
  let bodyHtml = '';

  rows.forEach((row, idx) => {
    bodyHtml += `<tr>`;
    bodyHtml += `<td style="width: 45px; min-width: 45px; max-width: 45px; text-align: center; color: var(--text-muted); font-weight: 600;">${(startRowNumber + idx).toLocaleString('fa-IR')}</td>`;

    columns.forEach(col => {
      let val = row[col.key] || '';
      let tooltip = '';
      if (col.tooltipKey && row[col.tooltipKey]) {
        tooltip = `title="تاریخ میلادی: ${row[col.tooltipKey]}"`;
      }

      // Highlight for 'cancel by system'
      let isSystem = typeof val === 'string' && val.toLowerCase().includes('cancel by system');
      let colorStyle = isSystem ? 'color: #dc2626 !important; font-weight: bold;' : '';
      if (col.isBold) colorStyle += ' font-weight: bold;';

      const styleStr = `min-width: ${col.minWidth}; ${col.maxWidth ? 'max-width:' + col.maxWidth + ';' : ''} text-align: ${col.align}; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; ${colorStyle}`;

      bodyHtml += `<td ${tooltip} style="${styleStr}">${escapeHtml(String(val))}</td>`;
    });

    bodyHtml += `</tr>`;
  });

  tbody.innerHTML = bodyHtml;
}

function renderTotalDtPagination(currentPage, totalPages, totalRecords) {
  const container = document.getElementById('totalDtPagination');
  if (!container) return;

  if (totalPages <= 1) {
    container.innerHTML = '';
    return;
  }

  let html = `<div class="pagination-wrapper">`;
  
  html += `<button class="page-btn" onclick="loadTotalDtData(1)" ${currentPage === 1 ? 'disabled' : ''}>« اول</button>`;
  html += `<button class="page-btn" onclick="loadTotalDtData(${currentPage - 1})" ${currentPage === 1 ? 'disabled' : ''}>‹ قبلی</button>`;

  const maxVisiblePages = 5;
  let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
  let endPage = Math.min(totalPages, startPage + maxVisiblePages - 1);
  if (endPage - startPage < maxVisiblePages - 1) {
    startPage = Math.max(1, endPage - maxVisiblePages + 1);
  }

  for (let i = startPage; i <= endPage; i++) {
    html += `<button class="page-btn ${i === currentPage ? 'active' : ''}" onclick="loadTotalDtData(${i})">${i.toLocaleString('fa-IR')}</button>`;
  }

  html += `<button class="page-btn" onclick="loadTotalDtData(${currentPage + 1})" ${currentPage === totalPages ? 'disabled' : ''}>بعدی ›</button>`;
  html += `<button class="page-btn" onclick="loadTotalDtData(${totalPages})" ${currentPage === totalPages ? 'disabled' : ''}>آخر »</button>`;

  html += `<span class="page-info">صفحه ${currentPage.toLocaleString('fa-IR')} از ${totalPages.toLocaleString('fa-IR')} (مجموع ${totalRecords.toLocaleString('fa-IR')} رکورد)</span>`;
  html += `</div>`;

  container.innerHTML = html;
}

function sortTotalDt(colKey) {
  if (totalDtState.sortCol === colKey) {
    totalDtState.sortDir = (totalDtState.sortDir === 'ASC') ? 'DESC' : 'ASC';
  } else {
    totalDtState.sortCol = colKey;
    totalDtState.sortDir = 'DESC';
  }
  loadTotalDtData(1);
}

function handleTotalDtSearch(val) {
  totalDtState.search = val.trim();
  loadTotalDtData(1);
}

// ============================================================================
// 3. PREIMPEL FILTER & SELECTOR
// ============================================================================

let allPreimpelItems = [];

async function loadPreimpelList() {
  const container = document.getElementById('totalDtPreimpelListContainer');
  if (!container) return;

  const startDate = document.getElementById('totalDtFilterStartDate')?.value || '';
  const endDate = document.getElementById('totalDtFilterEndDate')?.value || '';

  container.innerHTML = `<div style="text-align:center; padding: 20px; color: var(--text-muted);">در حال دریافت مقادیر...</div>`;

  try {
    const params = new URLSearchParams({ startDate, endDate });
    const res = await fetch(`/api/totaldt/preimpel-list?${params.toString()}`);
    const json = await res.json();

    if (!json.success) throw new Error(json.error || 'خطا در واکشی اطلاعات');

    allPreimpelItems = json.list || [];
    renderPreimpelChecklist(allPreimpelItems);

  } catch (err) {
    console.error('Error in loadPreimpelList:', err);
    container.innerHTML = `<div style="color: #ef4444; padding: 10px;">خطا در دریافت مقادیر: ${err.message}</div>`;
  }
}

function renderPreimpelChecklist(items) {
  const container = document.getElementById('totalDtPreimpelListContainer');
  if (!container) return;

  if (items.length === 0) {
    container.innerHTML = `<div style="padding: 15px; color: var(--text-muted); text-align: center;">هیچ موردی یافت نشد.</div>`;
    return;
  }

  let html = `<div class="preimpel-checklist">`;
  items.forEach((item, idx) => {
    const isChecked = totalDtState.preimpel.includes(item);
    const isSys = item.toLowerCase() === 'cancel by system';
    const labelStyle = isSys ? 'color: #ef4444; font-weight: bold;' : '';

    html += `
      <label class="preimpel-item" style="${labelStyle}">
        <input type="checkbox" value="${escapeHtml(item)}" ${isChecked ? 'checked' : ''} class="preimpel-checkbox" onchange="togglePreimpelItem(this)">
        <span>${escapeHtml(item)}</span>
      </label>
    `;
  });
  html += `</div>`;
  container.innerHTML = html;
}

function filterPreimpelSearch(searchTxt) {
  const q = searchTxt.trim().toLowerCase();
  const filtered = allPreimpelItems.filter(item => item.toLowerCase().includes(q));
  renderPreimpelChecklist(filtered);
}

function selectAllPreimpel(select = true) {
  const checkboxes = document.querySelectorAll('.preimpel-checkbox');
  checkboxes.forEach(cb => {
    cb.checked = select;
  });
  if (select) {
    totalDtState.preimpel = [...allPreimpelItems];
  } else {
    totalDtState.preimpel = [];
  }
}

function togglePreimpelItem(cb) {
  const val = cb.value;
  if (cb.checked) {
    if (!totalDtState.preimpel.includes(val)) totalDtState.preimpel.push(val);
  } else {
    totalDtState.preimpel = totalDtState.preimpel.filter(x => x !== val);
  }
}

function applyTotalDtFilters() {
  const startDate = document.getElementById('totalDtFilterStartDate')?.value || '';
  const endDate = document.getElementById('totalDtFilterEndDate')?.value || '';

  // Collect checked items
  const checked = [];
  document.querySelectorAll('.preimpel-checkbox:checked').forEach(cb => {
    checked.push(cb.value);
  });

  totalDtState.startDate = startDate;
  totalDtState.endDate = endDate;
  totalDtState.preimpel = checked;

  closeTotalDtFilterModal();
  openTotalDtMainModal();
}

function resetTotalDtFilters() {
  totalDtState.startDate = '';
  totalDtState.endDate = '';
  totalDtState.preimpel = [];
  totalDtState.search = '';

  const txtStart = document.getElementById('totalDtFilterStartDate');
  const txtEnd = document.getElementById('totalDtFilterEndDate');
  if (txtStart) txtStart.value = '';
  if (txtEnd) txtEnd.value = '';

  selectAllPreimpel(false);
  closeTotalDtFilterModal();
  loadTotalDtData(1);
}

// ============================================================================
// 4. EXECUTOR ANALYSIS & PROGRESS BARS
// ============================================================================

async function loadTotalDtExecutorAnalysis() {
  const container = document.getElementById('totalDtExecutorContainer');
  const summary = document.getElementById('totalDtExecutorSummary');
  if (!container) return;

  container.innerHTML = `<div class="loading-spinner"><div class="spinner"></div><p>در حال اجرای الگوریتم هوشمند تطابق پرسنلی و محاسبه آمار...</p></div>`;

  try {
    const res = await fetch('/api/totaldt/executor-analysis', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        startDate: totalDtState.startDate,
        endDate: totalDtState.endDate,
        preimpelValues: totalDtState.preimpel
      })
    });

    const json = await res.json();
    if (!json.success) throw new Error(json.error || 'خطا در تحلیل انجام‌دهنده‌ها');

    if (summary) {
      summary.innerHTML = `مجموع رکوردهای بررسی شده: <strong>${json.totalRecords.toLocaleString('fa-IR')}</strong> | تعداد انجام‌دهندگان متمایز: <strong>${json.uniqueExecutors.toLocaleString('fa-IR')}</strong>`;
    }

    renderExecutorTable(json.data || [], json.totalRecords);

  } catch (err) {
    console.error('Error in loadTotalDtExecutorAnalysis:', err);
    container.innerHTML = `<div style="color: #ef4444; padding: 20px; text-align: center;">خطا در تحلیل: ${err.message}</div>`;
  }
}

function renderExecutorTable(list, totalRecords) {
  const container = document.getElementById('totalDtExecutorContainer');
  if (!container) return;

  if (list.length === 0) {
    container.innerHTML = `<div style="padding: 30px; text-align: center; color: var(--text-muted);">هیچ رکوردی برای نمایش وجود ندارد.</div>`;
    return;
  }

  let html = `
    <table class="data-table executor-table" style="width: 100%; border-collapse: separate !important; border-spacing: 0 !important;">
      <thead>
        <tr>
          <th style="width: 55px; min-width: 55px; max-width: 55px; text-align: center; white-space: nowrap;">رتبه</th>
          <th style="min-width: 280px; text-align: right; white-space: nowrap;">نام انجام‌دهنده (تطابق هوشمند پرسنلی)</th>
          <th style="width: 110px; min-width: 110px; text-align: center; white-space: nowrap;">تعداد تکرار</th>
          <th style="width: 90px; min-width: 90px; text-align: center; white-space: nowrap;">درصد از کل</th>
          <th style="min-width: 320px; text-align: right; white-space: nowrap;">نمودار میله‌ای فراوانی</th>
        </tr>
      </thead>
      <tbody>
  `;

  list.forEach((item, idx) => {
    const isSys = item.isSystem;
    const rowClass = isSys ? 'system-cancel-row' : '';
    const barColor = isSys ? 'linear-gradient(90deg, #ef4444, #f87171)' : 'linear-gradient(90deg, #0284c7, #38bdf8)';

    html += `
      <tr class="${rowClass}">
        <td style="width: 55px; min-width: 55px; max-width: 55px; text-align: center; font-weight: bold; color: var(--text-muted);">${(idx + 1).toLocaleString('fa-IR')}</td>
        <td style="min-width: 280px; font-weight: 600; text-align: right; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${escapeHtml(item.executor)}</td>
        <td style="width: 110px; min-width: 110px; text-align: center; font-weight: bold;">${item.count.toLocaleString('fa-IR')}</td>
        <td style="width: 90px; min-width: 90px; text-align: center; color: var(--text-muted);">${item.percentage.toLocaleString('fa-IR')}%</td>
        <td style="min-width: 320px;">
          <div class="executor-progress-track">
            <div class="executor-progress-fill" style="width: ${item.barPercent}%; background: ${barColor};">
              <span class="progress-label">${item.count.toLocaleString('fa-IR')}</span>
            </div>
          </div>
        </td>
      </tr>
    `;
  });

  html += `
      </tbody>
    </table>
  `;

  container.innerHTML = html;

  // Set fixed footer summary
  const footerSummary = document.getElementById('totalDtExecutorFooterSummary');
  if (footerSummary) {
    footerSummary.innerHTML = `مجموع کل سطرها: <strong>${list.length.toLocaleString('fa-IR')}</strong> نفر متمایز | مجموع کل قطعی‌ها: <strong>${totalRecords.toLocaleString('fa-IR')}</strong> (۱۰۰٪)`;
  }
}

// ============================================================================
// 5. GROUP STATISTICS & CHARTS
// ============================================================================

async function loadTotalDtGroupStats() {
  const container = document.getElementById('totalDtGroupStatsContainer');
  if (!container) return;

  container.innerHTML = `<div class="loading-spinner"><div class="spinner"></div><p>در حال محاسبه آمار و گروه‌بندی حوزه‌ها...</p></div>`;

  try {
    const res = await fetch('/api/totaldt/group-stats', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        startDate: totalDtState.startDate,
        endDate: totalDtState.endDate,
        preimpelValues: totalDtState.preimpel
      })
    });

    const json = await res.json();
    if (!json.success) throw new Error(json.error || 'خطا در دریافت آمار گروه‌ها');

    renderGroupStats(json);

  } catch (err) {
    console.error('Error in loadTotalDtGroupStats:', err);
    container.innerHTML = `<div style="color: #ef4444; padding: 20px; text-align: center;">خطا در دریافت آمار: ${err.message}</div>`;
  }
}

function renderGroupStats(data) {
  const container = document.getElementById('totalDtGroupStatsContainer');
  if (!container) return;

  const classified = data.classified || [];
  const detailed = data.detailed || [];
  const total = data.totalRecords || 0;

  let html = `
    <div class="group-stats-layout">
      <!-- SUMMARY CARDS / DOMAIN DISTRIBUTION -->
      <div class="group-stats-section">
        <h4 class="section-title">📊 توزیع قطعی‌ها بر اساس حوزه‌های تخصصی (طبقه‌بندی هوشمند)</h4>
        <div class="domain-grid">
  `;

  const domainColors = [
    '#3b82f6', '#10b981', '#f59e0b', '#ec4899', '#8b5cf6',
    '#06b6d4', '#6366f1', '#14b8a6', '#f97316', '#64748b'
  ];

  classified.forEach((c, idx) => {
    const color = domainColors[idx % domainColors.length];
    html += `
      <div class="domain-stat-card" style="border-right: 4px solid ${color};">
        <div class="domain-card-header">
          <span class="domain-name">${escapeHtml(c.domain)}</span>
          <span class="domain-pct" style="color: ${color};">${c.percentage.toLocaleString('fa-IR')}%</span>
        </div>
        <div class="domain-card-count">
          <strong>${c.count.toLocaleString('fa-IR')}</strong> <span class="unit">قطعی</span>
        </div>
        <div class="domain-card-bar">
          <div class="domain-bar-fill" style="width: ${c.percentage}%; background: ${color};"></div>
        </div>
      </div>
    `;
  });

  html += `
        </div>
      </div>

      <!-- DETAILED GROUP TABLE -->
      <div class="group-stats-section" style="margin-top: 25px;">
        <h4 class="section-title">📋 جزئیات آماری کلیه گروه‌های اجرایی (Group Names)</h4>
        <table class="data-table">
          <thead>
            <tr>
              <th style="width: 45px; text-align: center;">ردیف</th>
              <th>نام گروه اجرایی (GROUPNAME)</th>
              <th>حوزه تخصصی مربوطه</th>
              <th style="width: 120px; text-align: center;">تعداد قطعی</th>
              <th style="width: 100px; text-align: center;">درصد از کل</th>
            </tr>
          </thead>
          <tbody>
  `;

  detailed.forEach((g, idx) => {
    html += `
      <tr>
        <td style="text-align: center; color: var(--text-muted);">${(idx + 1).toLocaleString('fa-IR')}</td>
        <td style="font-weight: 600;">${escapeHtml(g.groupName)}</td>
        <td><span class="badge badge-info">${escapeHtml(g.domain)}</span></td>
        <td style="text-align: center; font-weight: bold;">${g.count.toLocaleString('fa-IR')}</td>
        <td style="text-align: center;">${g.percentage.toLocaleString('fa-IR')}%</td>
      </tr>
    `;
  });

  html += `
          </tbody>
          <tfoot>
            <tr style="background: var(--bg-tertiary); font-weight: bold;">
              <td colspan="3" style="text-align: right; padding-right: 15px;">مجموع کل قطعی‌ها:</td>
              <td style="text-align: center;">${total.toLocaleString('fa-IR')}</td>
              <td style="text-align: center;">۱۰۰٪</td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  `;

  container.innerHTML = html;
}

// ============================================================================
// 6. CSV IMPORT
// ============================================================================

async function submitTotalDtCsvImport() {
  const fileInput = document.getElementById('totalDtCsvFileInput');
  const statusDiv = document.getElementById('totalDtImportStatus');
  const btn = document.getElementById('btnSubmitTotalDtImport');

  if (!fileInput || !fileInput.files || fileInput.files.length === 0) {
    alert('لطفاً ابتدا یک فایل CSV انتخاب نمایید.');
    return;
  }

  const file = fileInput.files[0];
  if (!file.name.toLowerCase().endsWith('.csv')) {
    alert('فرمت فایل انتخابی باید CSV باشد.');
    return;
  }

  if (btn) btn.disabled = true;
  if (statusDiv) {
    statusDiv.innerHTML = `<div class="loading-spinner" style="padding: 10px;"><div class="spinner"></div><p>در حال خواندن فایل، پاکسازی Staging و ادغام در دیتابیس SDM...</p></div>`;
  }

  try {
    const text = await file.text();

    const res = await fetch('/api/totaldt/import-csv', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ csvContent: text })
    });

    const json = await res.json();
    if (!json.success) throw new Error(json.error || 'خطا در پردازش فایل');

    if (statusDiv) {
      statusDiv.innerHTML = `<div style="color: #10b981; font-weight: bold; padding: 10px; background: rgba(16,185,129,0.1); border-radius: 6px; border: 1px solid #10b981;">✅ ${json.message}</div>`;
    }

    setTimeout(() => {
      closeTotalDtImportModal();
      loadTotalDtData(1);
    }, 2000);

  } catch (err) {
    console.error('Error importing CSV:', err);
    if (statusDiv) {
      statusDiv.innerHTML = `<div style="color: #ef4444; font-weight: bold; padding: 10px; background: rgba(239,68,68,0.1); border-radius: 6px; border: 1px solid #ef4444;">❌ خطا: ${err.message}</div>`;
    }
  } finally {
    if (btn) btn.disabled = false;
  }
}

// ============================================================================
// 7. EXPORT TO EXCEL
// ============================================================================

function exportTotalDtToExcel() {
  const params = new URLSearchParams({
    startDate: totalDtState.startDate,
    endDate: totalDtState.endDate,
    preimpel: (totalDtState.preimpel || []).join('|||'),
    search: totalDtState.search
  });

  window.location.href = `/api/totaldt/export-excel?${params.toString()}`;
}

// ============================================================================
// 8. CLEAR ALL DATA
// ============================================================================

async function clearTotalDtData() {
  const confirm1 = confirm('⚠️ هشدار بسیار مهم:\n\nآیا از حذف کامل تمامی داده‌های جدول TotalDownTime و جدول موقت Staging در دیتابیس SDM اطمینان قطعی دارید؟\n\nاین عملیات غیرقابل بازگشت است!');
  if (!confirm1) return;

  const confirm2 = prompt('جهت تایید نهایی حذف، عبارت DELETE را با حروف بزرگ تایپ کنید:');
  if (confirm2 !== 'DELETE') {
    alert('عملیات حذف لغو گردید.');
    return;
  }

  try {
    const res = await fetch('/api/totaldt/clear-all', { method: 'POST' });
    const json = await res.json();
    if (!json.success) throw new Error(json.error || 'خطا در پاکسازی');

    alert('✅ ' + json.message);
    loadTotalDtData(1);
  } catch (err) {
    console.error('Error clearing data:', err);
    alert('خطا در پاکسازی داده‌ها: ' + err.message);
  }
}

// ============================================================================
// 9. BACKUP & RESTORE
// ============================================================================

async function loadTotalDtBackups() {
  const container = document.getElementById('totalDtBackupListContainer');
  if (!container) return;

  container.innerHTML = `<div class="loading-spinner"><div class="spinner"></div><p>در حال دریافت لیست فایل‌های پشتیبان...</p></div>`;

  try {
    const res = await fetch('/api/totaldt/backups');
    const json = await res.json();
    if (!json.success) throw new Error(json.error || 'خطا در دریافت لیست بک‌آپ‌ها');

    renderBackupList(json.backups || [], json.backupFolder);

  } catch (err) {
    console.error('Error loading backups:', err);
    container.innerHTML = `<div style="color: #ef4444; padding: 15px;">خطا: ${err.message}</div>`;
  }
}

function renderBackupList(backups, folderPath) {
  const container = document.getElementById('totalDtBackupListContainer');
  if (!container) return;

  let html = `
    <div style="margin-bottom: 12px; font-size: 0.85rem; color: var(--text-muted);">
      📁 مسیر ذخیره‌سازی فایل‌های پشتیبان در سرور: <code>${escapeHtml(folderPath || '')}</code>
    </div>
  `;

  if (backups.length === 0) {
    html += `<div style="padding: 20px; text-align: center; color: var(--text-muted); background: var(--bg-tertiary); border-radius: 6px;">هیچ فایل پشتیبانی در مسیر مشخص‌شده یافت نشد.</div>`;
    container.innerHTML = html;
    return;
  }

  html += `
    <table class="data-table">
      <thead>
        <tr>
          <th style="width: 40px; text-align: center;">#</th>
          <th>نام فایل پشتیبان (.BAK)</th>
          <th style="width: 100px; text-align: center;">حجم</th>
          <th style="width: 140px; text-align: center;">تاریخ ایجاد (شمسی)</th>
          <th style="width: 110px; text-align: center;">عملیات</th>
        </tr>
      </thead>
      <tbody>
  `;

  backups.forEach((b, idx) => {
    html += `
      <tr>
        <td style="text-align: center; color: var(--text-muted);">${(idx + 1).toLocaleString('fa-IR')}</td>
        <td style="font-weight: 600; direction: ltr; text-align: right;">${escapeHtml(b.fileName)}</td>
        <td style="text-align: center;">${escapeHtml(b.sizeFormatted)}</td>
        <td style="text-align: center;">${escapeHtml(b.shamsiDate || '')}</td>
        <td style="text-align: center;">
          <button class="btn-sm btn-danger" onclick="restoreTotalDtBackup('${escapeHtml(b.fileName)}')">
            📥 بازیابی
          </button>
        </td>
      </tr>
    `;
  });

  html += `
      </tbody>
    </table>
  `;

  container.innerHTML = html;
}

async function createTotalDtBackupNow() {
  const btn = document.getElementById('btnCreateTotalDtBackup');
  if (btn) btn.disabled = true;

  try {
    const res = await fetch('/api/totaldt/backup-now', { method: 'POST' });
    const json = await res.json();
    if (!json.success) throw new Error(json.error || 'خطا در ایجاد بک‌آپ');

    alert('✅ ' + json.message);
    loadTotalDtBackups();
  } catch (err) {
    console.error('Error creating backup:', err);
    alert('خطا در تهیه فایل پشتیبان: ' + err.message);
  } finally {
    if (btn) btn.disabled = false;
  }
}

async function restoreTotalDtBackup(fileName) {
  const confirm1 = confirm(`⚠️ هشدار جدی:\n\nآیا مطمئن هستید که می‌خواهید دیتابیس SDM را از روی فایل پشتیبان زیر بازیابی نمایید؟\n${fileName}\n\nکلیه اتصالات کاربران به دیتابیس موقتاً قطع و اطلاعات دیتابیس به زمان بک‌آپ بازگردانده خواهد شد.`);
  if (!confirm1) return;

  try {
    const res = await fetch('/api/totaldt/restore', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ fileName })
    });

    const json = await res.json();
    if (!json.success) throw new Error(json.error || 'خطا در بازیابی');

    alert('✅ ' + json.message);
    closeTotalDtBackupModal();
    loadTotalDtData(1);
  } catch (err) {
    console.error('Error restoring backup:', err);
    alert('خطا در بازیابی دیتابیس: ' + err.message);
  }
}

// Utility HTML escape
function escapeHtml(str) {
  if (!str) return '';
  return str
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#039;');
}
