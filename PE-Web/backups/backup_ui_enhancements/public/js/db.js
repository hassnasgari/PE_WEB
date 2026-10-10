let rawDbData = [];
let filteredDbData = [];
let dbColumnFilters = {};

async function loadDB() {
  const loading = document.getElementById('dbLoading');
  const container = document.getElementById('dbTableContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/db/list');
    const result = await res.json();

    if (!result.success) {
      alert('خطا در دریافت اطلاعات دیتابیس سایت‌ها: ' + result.error);
      return;
    }

    rawDbData = result.data || [];
    applyDbFilters();
    renderDbTable();

    const dbLastUpdate = document.getElementById('dbLastUpdate');
    if (dbLastUpdate) {
      const now = new Date().toLocaleTimeString('fa-IR');
      dbLastUpdate.textContent = 'آخرین بروزرسانی دیتابیس جدید: ' + now;
    }
  } catch (err) {
    console.error('loadDB error:', err);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
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

function renderDbTable() {
  const head = document.getElementById('dbTableHead');
  const body = document.getElementById('dbTableBody');
  const foot = document.getElementById('dbTableFoot');

  if (!head || !body) return;

  if (filteredDbData.length === 0) {
    head.innerHTML = '';
    body.innerHTML = '<tr><td colspan="10" style="text-align:center;padding:20px;">هیچ سایتی پیدا نشد</td></tr>';
    if (foot) foot.innerHTML = '';
    return;
  }

  const columns = Object.keys(rawDbData[0] || {});

  // Helper: return sticky class for frozen columns (first 2 columns)
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
    const bgClass = rIdx % 2 === 0 ? 'row-even' : 'row-odd';
    bodyHtml += `<tr class="${bgClass}">`;
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
        footHtml += `<td class="${cls}">تعداد سایت‌ها: ${filteredDbData.length}</td>`;
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

  // Update Status Bar Record Count
  if (typeof updateStatusRecordCount === 'function') {
    updateStatusRecordCount(filteredDbData.length);
  }

  // 4. Compute right offset for the second frozen column (sticky-col-1)
  //    It needs to be pushed right by the width of sticky-col-0
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

function exportDbToExcel() {
  alert('در حال آماده‌سازی فایل اکسل دیتابیس سایت‌ها...');
}
