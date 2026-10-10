let rawTelData = [];
let filteredTelData = [];
let telColumnFilters = {};

async function loadTEL() {
  const loading = document.getElementById('telLoading');
  const container = document.getElementById('telTableContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/tel/list');
    const result = await res.json();

    if (!result.success) {
      alert('خطا در دریافت دفترچه تلفن: ' + result.error);
      return;
    }

    rawTelData = result.data || [];
    applyTelFilters();
    renderTelTable();
  } catch (err) {
    console.error('loadTEL error:', err);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

function applyTelFilters() {
  const globalSearch = (document.getElementById('telSearchInput')?.value || '').trim().toLowerCase();

  filteredTelData = rawTelData.filter(row => {
    if (globalSearch) {
      const matchGlobal = Object.values(row).some(val => 
        val !== null && val !== undefined && String(val).toLowerCase().includes(globalSearch)
      );
      if (!matchGlobal) return false;
    }

    for (const [colKey, filterVal] of Object.entries(telColumnFilters)) {
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

function filterTelTableGlobal(query) {
  applyTelFilters();
  renderTelTable();
}

function updateTelColumnFilter(colKey, value, inputEl) {
  telColumnFilters[colKey] = value;

  const cursorStart = inputEl ? inputEl.selectionStart : null;
  const cursorEnd = inputEl ? inputEl.selectionEnd : null;

  applyTelFilters();
  renderTelTable();

  const newInput = document.querySelector(`#tab-PB .column-filter-input[data-col="${colKey}"]`);
  if (newInput) {
    newInput.focus();
    if (cursorStart !== null && cursorEnd !== null) {
      try { newInput.setSelectionRange(cursorStart, cursorEnd); } catch (e) {}
    }
  }
}

function renderTelTable() {
  const head = document.getElementById('telTableHead');
  const body = document.getElementById('telTableBody');
  const foot = document.getElementById('telTableFoot');

  if (!head || !body) return;

  const columns = [
    { key: 'ردیف', label: 'ردیف' },
    { key: 'نام و نام خانوادگی', label: 'نام و نام خانوادگی' },
    { key: 'شماره تلفن', label: 'شماره تلفن' },
    { key: 'محل کار', label: 'محل کار' }
  ];

  // Header
  let headerHtml = '<tr>';
  columns.forEach(col => {
    headerHtml += `<th>${col.label}</th>`;
  });
  headerHtml += '</tr>';

  // Auto filter
  headerHtml += '<tr class="filter-row">';
  columns.forEach(col => {
    const val = telColumnFilters[col.key] || '';
    headerHtml += `<th class="filter-cell"><input class="column-filter-input" data-col="${col.key}" type="text" value="${val}" oninput="updateTelColumnFilter('${col.key}', this.value, this)" placeholder="فیلتر..."></th>`;
  });
  headerHtml += '</tr>';

  head.innerHTML = headerHtml;

  // Body
  let bodyHtml = '';
  filteredTelData.forEach((row, idx) => {
    bodyHtml += `<tr>
      <td>${row['ردیف'] || (idx + 1)}</td>
      <td>${row['نام و نام خانوادگی'] || row['name'] || ''}</td>
      <td dir="ltr" style="text-align:right">${row['شماره تلفن'] || row['tel'] || ''}</td>
      <td>${row['محل کار'] || row['workPlace'] || ''}</td>
    </tr>`;
  });

  body.innerHTML = bodyHtml;

  // Footer
  if (foot) {
    foot.innerHTML = `<tr><td>مجموع مخاطبین: ${filteredTelData.length}</td><td></td><td></td><td></td></tr>`;
  }

  const countBadge = document.getElementById('telRecordCount');
  if (countBadge) {
    countBadge.textContent = `${filteredTelData.length} مخاطب در دفترچه تلفن`;
  }

  // Update Status Bar Record Count
  if (typeof updateStatusRecordCount === 'function') {
    updateStatusRecordCount(filteredTelData.length);
  }
}

function showAddTelModal() {
  const modal = document.getElementById('addTelModal');
  if (modal) modal.style.display = 'flex';
}

function closeAddTelModal(e) {
  if (e && e.target !== e.currentTarget) return;
  const modal = document.getElementById('addTelModal');
  if (modal) modal.style.display = 'none';
}

async function saveTel() {
  const name = document.getElementById('addTelName').value.trim();
  const tel = document.getElementById('addTelNum').value.trim();
  const workPlace = document.getElementById('addTelWp').value.trim();

  if (!name || !tel) {
    alert('لطفاً نام و شماره تلفن را وارد نمایید.');
    return;
  }

  try {
    const res = await fetch('/api/tel/add', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, tel, workPlace })
    });

    const result = await res.json();
    if (result.success) {
      alert('شماره تلفن با موفقیت ثبت گردید.');
      closeAddTelModal();
      loadTEL();
    } else {
      alert('خطا در ثبت شماره: ' + result.error);
    }
  } catch (err) {
    console.error('saveTel error:', err);
    alert('خطا در ارتباط با سرور');
  }
}
