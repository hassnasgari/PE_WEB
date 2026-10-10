let rawDtData = [];
let filteredDtData = [];
let dtColumnFilters = {};
let currentDtPeriod = '15days';
let currentDtFilter = 'none';

async function loadDT(goToEnd = false) {
  const loading = document.getElementById('dtLoading');
  const container = document.getElementById('dtTableContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch(`/api/dt/list?period=${currentDtPeriod}&filter=${currentDtFilter}`);
    const result = await res.json();

    if (!result.success) {
      alert('خطا در دریافت دان تایم‌ها: ' + result.error);
      return;
    }

    rawDtData = result.data || [];
    applyDtFilters();
    renderDtTable();

    const dtLastRefresh = document.getElementById('dtLastRefresh');
    if (dtLastRefresh) {
      const now = new Date().toLocaleTimeString('fa-IR');
      dtLastRefresh.textContent = 'آخرین بروزرسانی: ' + now;
    }

    if (goToEnd && container) {
      setTimeout(() => {
        container.scrollTop = container.scrollHeight;
      }, 100);
    }
  } catch (err) {
    console.error('loadDT error:', err);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

let currentScopeText = '۱۵ روز اخیر';

function getPeriodLabel(period) {
  switch(period) {
    case 'today':    return 'امروز';
    case 'week':     return 'هفته اخیر';
    case '15days':   return '۱۵ روز اخیر';
    case 'month':    return 'ماه اخیر';
    case 'twomonth': return 'دو ماه اخیر';
    case 'all':      return 'کل دیتابیس';
    default:         return '۱۵ روز اخیر';
  }
}

function setPeriod(period, btn) {
  currentDtPeriod = period;
  currentScopeText = getPeriodLabel(period);
  document.querySelectorAll('#tab-DT .menu-dropdown a').forEach(a => a.classList.remove('active'));
  if (btn) btn.classList.add('active');
  loadDT();
}

function setDtFilter(filter, btn) {
  currentDtFilter = filter;
  const menuContainer = btn ? btn.closest('.dropdown-menu') : null;
  if (menuContainer) {
    menuContainer.querySelectorAll('a').forEach(a => a.classList.remove('active'));
    if (btn) btn.classList.add('active');
  }
  loadDT();
}

let dtSortColumn = 'ردیف';
let dtSortDirection = 'desc';

function sortDtColumn(colKey) {
  if (window.isColumnJustResized) {
    return;
  }
  if (dtSortColumn === colKey) {
    if (dtSortDirection === 'desc') {
      dtSortDirection = 'asc';
    } else if (dtSortDirection === 'asc') {
      dtSortColumn = null;
      dtSortDirection = 'desc';
    }
  } else {
    dtSortColumn = colKey;
    dtSortDirection = 'desc';
  }
  applyDtFilters();
  renderDtTable();
}

function applyDtFilters() {
  const globalSearch = (document.getElementById('dtSearchInput')?.value || '').trim().toLowerCase();

  filteredDtData = rawDtData.filter(row => {
    // Global Search
    if (globalSearch) {
      const matchGlobal = Object.values(row).some(val => 
        val !== null && val !== undefined && String(val).toLowerCase().includes(globalSearch)
      );
      if (!matchGlobal) return false;
    }

    // Column Filters (Auto Filter Row)
    for (const [colKey, filterVal] of Object.entries(dtColumnFilters)) {
      if (filterVal) {
        const cellVal = String(row[colKey] || '').toLowerCase();
        if (!cellVal.includes(filterVal.toLowerCase())) {
          return false;
        }
      }
    }

    return true;
  });

  // Apply Sorting (Ascending / Descending)
  if (dtSortColumn) {
    filteredDtData.sort((a, b) => {
      let valA = a[dtSortColumn] ?? '';
      let valB = b[dtSortColumn] ?? '';

      const numA = Number(valA);
      const numB = Number(valB);
      if (!isNaN(numA) && !isNaN(numB) && String(valA).trim() !== '' && String(valB).trim() !== '') {
        return dtSortDirection === 'asc' ? numA - numB : numB - numA;
      }

      return dtSortDirection === 'asc'
        ? String(valA).localeCompare(String(valB), 'fa', { numeric: true })
        : String(valB).localeCompare(String(valA), 'fa', { numeric: true });
    });
  }
}

function filterDtTableGlobal(query) {
  applyDtFilters();
  renderDtTable();
}

function updateDtColumnFilter(colKey, value, inputEl) {
  dtColumnFilters[colKey] = value;

  const cursorStart = inputEl ? inputEl.selectionStart : null;
  const cursorEnd = inputEl ? inputEl.selectionEnd : null;

  applyDtFilters();
  renderDtTable();

  const newInput = document.querySelector(`#tab-DT .column-filter-input[data-col="${colKey}"]`);
  if (newInput) {
    newInput.focus();
    if (cursorStart !== null && cursorEnd !== null) {
      try { newInput.setSelectionRange(cursorStart, cursorEnd); } catch (e) {}
    }
  }
}

// DevExpress gridViewDt_RowStyle Color Engine
function getDtRowStyle(row) {
  const status = String(row['وضعیت'] || '').trim();
  const activity = String(row['فعالیت'] || '').trim();
  const note = String(row['یادداشت'] || '').trim();

  let combined = (!activity || activity === 'خالی') ? `${status} - بدون فعالیت` : `${status} - ${activity}`;

  let bg = '#ffffff';
  let fg = '#000000';

  if (note.includes('کنسل')) fg = 'orange';
  if (status.includes('در حال ویرایش')) fg = 'red';

  if (combined === 'تائید - انجام') { bg = '#008080'; fg = '#0000A0'; }
  else if (combined === 'تائید - لغو اضطراری') { bg = '#008080'; fg = '#FF1CFF'; }
  else if (combined === 'تائید - لغو توسط سیستم') { bg = '#008080'; fg = '#FFFF00'; }
  else if (combined === 'تائید - لغو توسط کاربر') { bg = '#008080'; fg = '#FF8000'; }
  else if (combined === 'تائید - لغو - شبکه فریز') { bg = '#008080'; fg = '#FFFFFF'; }
  else if (combined === 'تائید - بدون فعالیت' || status === 'تائید') { bg = '#008080'; fg = '#000000'; }
  else if (combined === 'تائید مشروط - انجام') { bg = '#00B3B3'; fg = '#0000A0'; }
  else if (combined === 'تائید مشروط - لغو اضطراری') { bg = '#00B3B3'; fg = '#BF00BF'; }
  else if (combined === 'تائید مشروط - بدون فعالیت' || status === 'تائید مشروط') { bg = '#00B3B3'; fg = '#000000'; }
  else if (combined === 'ثبت در (SDM) - بدون فعالیت' || status === 'ثبت در (SDM)') { bg = '#80FF80'; fg = '#000000'; }
  else if (combined === 'در حال پردازش درخواست - بدون فعالیت' || status.includes('در حال پردازش')) { bg = '#FFFF00'; fg = '#000000'; }
  else if (combined === 'عدم تائید - بدون فعالیت' || status === 'عدم تائید') { bg = '#FF0000'; fg = '#000000'; }
  else if (combined === 'عدم تائید(موقت) - بدون فعالیت' || status === 'عدم تائید(موقت)') { bg = '#FF2D96'; fg = '#000000'; }
  else if (combined === 'منتظر ارائه در جلسه - بدون فعالیت' || status.includes('منتظر')) { bg = '#FFB347'; fg = '#000000'; }
  else if (status === 'پردازش فنی') { bg = '#7CFC00'; fg = '#000000'; }
  else if (status === 'لغو') { bg = '#E090C0'; fg = '#000000'; }
  else if (status === 'شبکه فریز') { bg = '#B0A8E0'; fg = '#000000'; }
  else if (status === 'حذف') { bg = '#808080'; fg = '#ffffff'; }
  else if (status === 'نیاز به اصلاح درخواست' || status.includes('نیاز به اصلاح')) { bg = '#FFDEAD'; fg = '#000000'; }
  else if (status === 'درخواست منقضی' || status.includes('منقضی')) { bg = '#D2B48C'; fg = '#000000'; }

  // Gradient from right (light/white) to left (dark/status color)
  return `background: linear-gradient(to left, var(--bg-card) 0%, ${bg} 100%); color: ${fg};`;
}

let selectedDtRow = null;
let selectedDtRows = new Set();
let lastSelectedDtIndex = -1;

let isDtMouseDown = false;
let dragStartDtIndex = -1;

document.addEventListener('mouseup', () => {
  isDtMouseDown = false;
  dragStartDtIndex = -1;
});

function handleDtRowMouseDown(event, trEl, rowNum, idx) {
  if (event.button !== 0) return; // Left click only

  isDtMouseDown = true;
  dragStartDtIndex = idx;

  if (event.ctrlKey || event.metaKey) {
    // Ctrl + Click: toggle individual row selection
    if (selectedDtRows.has(rowNum)) {
      selectedDtRows.delete(rowNum);
    } else {
      selectedDtRows.add(rowNum);
    }
    lastSelectedDtIndex = idx;
  } else if (event.shiftKey && lastSelectedDtIndex >= 0) {
    // Shift + Click: range select from lastSelectedDtIndex to idx
    const start = Math.min(lastSelectedDtIndex, idx);
    const end = Math.max(lastSelectedDtIndex, idx);
    selectedDtRows.clear();
    for (let i = start; i <= end; i++) {
      if (filteredDtData[i]) {
        selectedDtRows.add(filteredDtData[i]['ردیف']);
      }
    }
  } else {
    // Single Click down: start selection with clicked row
    selectedDtRows.clear();
    selectedDtRows.add(rowNum);
    lastSelectedDtIndex = idx;
  }

  if (selectedDtRows.size > 0) {
    selectedDtRow = Array.from(selectedDtRows).pop();
  } else {
    selectedDtRow = null;
  }

  updateRowSelectionHighlights();
  showSelectionTooltip(event, selectedDtRows.size);
}

function handleDtRowMouseEnter(event, trEl, rowNum, idx) {
  if (!isDtMouseDown || dragStartDtIndex < 0) return;

  const start = Math.min(dragStartDtIndex, idx);
  const end = Math.max(dragStartDtIndex, idx);

  if (!event.ctrlKey && !event.metaKey) {
    selectedDtRows.clear();
  }

  for (let i = start; i <= end; i++) {
    if (filteredDtData[i]) {
      selectedDtRows.add(filteredDtData[i]['ردیف']);
    }
  }

  if (selectedDtRows.size > 0) {
    selectedDtRow = Array.from(selectedDtRows).pop();
  } else {
    selectedDtRow = null;
  }

  updateRowSelectionHighlights();
  showSelectionTooltip(event, selectedDtRows.size);
}

function selectDtRow(event, trEl, rowNum, idx) {
  // All selection handling is executed on mousedown and mouseenter.
  // Click event does nothing so mouse release NEVER wipes selection.
}

function updateRowSelectionHighlights() {
  const selectedSetStrings = new Set(Array.from(selectedDtRows).map(v => String(v).trim()));

  document.querySelectorAll('#dtTableBody tr').forEach(tr => {
    const rawVal = tr.getAttribute('data-row');
    const rNumStr = rawVal ? String(rawVal).trim() : '';

    const isSelected = selectedSetStrings.has(rNumStr);

    if (isSelected) {
      tr.classList.add('selected-row');
      tr.querySelectorAll('td').forEach(td => {
        td.style.setProperty('background-color', '#38bdf8', 'important');
        td.style.setProperty('background-image', 'none', 'important');
        td.style.setProperty('color', '#000000', 'important');
        td.style.setProperty('font-weight', '700', 'important');
      });
    } else {
      tr.classList.remove('selected-row');
      tr.querySelectorAll('td').forEach(td => {
        td.style.removeProperty('background-color');
        td.style.removeProperty('background-image');
        td.style.removeProperty('color');
        td.style.removeProperty('font-weight');
      });
    }
  });
}

function clearDtRowSelection() {
  selectedDtRows.clear();
  selectedDtRow = null;
  lastSelectedDtIndex = -1;
  updateRowSelectionHighlights();
  showSelectionTooltip(null, 0);
}

// Deselect on Escape key
document.addEventListener('keydown', (e) => {
  if (e.key === 'Escape') {
    clearDtRowSelection();
    hideDtContextMenu();
  }
});

// Close context menu on click & clear selection on clicking outside table
document.addEventListener('click', (e) => {
  const contextMenu = document.getElementById('dtContextMenu');
  if (contextMenu && contextMenu.style.display !== 'none' && !contextMenu.contains(e.target)) {
    hideDtContextMenu();
  }
  if (!e.target.closest('#dtTableBody') && !e.target.closest('#dtContextMenu') && !e.target.closest('.dt-context-menu')) {
    if (selectedDtRows.size > 0) {
      clearDtRowSelection();
    }
  }
});

let _selectionTooltipTimer = null;

function showSelectionTooltip(event, count) {
  let tooltip = document.getElementById('selectionTooltip');
  if (!tooltip) {
    tooltip = document.createElement('div');
    tooltip.id = 'selectionTooltip';
    tooltip.className = 'selection-tooltip';
    document.body.appendChild(tooltip);
  }

  if (_selectionTooltipTimer) {
    clearTimeout(_selectionTooltipTimer);
    _selectionTooltipTimer = null;
  }

  if (count > 0) {
    tooltip.innerHTML = `📌 ${count.toLocaleString('fa-IR')} سطر انتخاب شده است`;
    tooltip.style.display = 'flex';
    tooltip.style.opacity = '1';

    if (event && event.clientX) {
      let x = event.clientX + 15;
      let y = event.clientY + 15;
      if (x + 180 > window.innerWidth) x = event.clientX - 180;
      if (y + 40 > window.innerHeight) y = event.clientY - 40;
      tooltip.style.left = `${x}px`;
      tooltip.style.top = `${y}px`;
    }

    // Auto-hide after 2.5 seconds
    _selectionTooltipTimer = setTimeout(() => {
      tooltip.style.opacity = '0';
      setTimeout(() => { if (tooltip) tooltip.style.display = 'none'; }, 300);
    }, 2500);
  } else {
    tooltip.style.opacity = '0';
    tooltip.style.display = 'none';
  }
}

function hasAttachment(row) {
  if (!row) return false;
  const val = String(row['haveAttachedFile'] ?? row['ضمیمه'] ?? '').trim();
  return val === 'دارد' || val === '1' || val === 'true' || (val !== '' && val !== '0' && val !== 'ندارد' && val !== 'false' && val !== '-');
}

function handleAttachmentClick(event, rowNum) {
  if (event) {
    event.stopPropagation();
    event.preventDefault();
  }
  const shouldOpen = confirm(`فایل پیوست مربوط به ردیف ${rowNum} آماده دریافت است.\nآیا مایلید فایل پیوست را از سرور دانلود و باز کنید؟`);
  if (shouldOpen) {
    window.open(`/api/dt/download/${rowNum}`, '_blank');
  }
}

/* JALALI TO GREGORIAN CONVERTER & HOVER TOOLTIP */
function jalaliToGregorian(j_y, j_m, j_d) {
  j_y = parseInt(j_y, 10);
  j_m = parseInt(j_m, 10);
  j_d = parseInt(j_d, 10);

  let jy = j_y - 979;
  let jm = j_m - 1;
  let jd = j_d - 1;

  let j_day_no = 365 * jy + Math.floor(jy / 33) * 8 + Math.floor((jy % 33 + 3) / 4);
  for (let i = 0; i < jm; ++i) {
    j_day_no += (i < 6) ? 31 : 30;
  }
  j_day_no += jd;

  let g_day_no = j_day_no + 79;

  let gy = 1600 + 400 * Math.floor(g_day_no / 146097);
  g_day_no = g_day_no % 146097;

  let leap = true;
  if (g_day_no >= 36525) {
    g_day_no--;
    gy += 100 * Math.floor(g_day_no / 36524);
    g_day_no = g_day_no % 36524;

    if (g_day_no >= 365) {
      g_day_no++;
    } else {
      leap = false;
    }
  }

  gy += 4 * Math.floor(g_day_no / 1461);
  g_day_no = g_day_no % 1461;

  if (g_day_no >= 366) {
    leap = false;
    g_day_no--;
    gy += Math.floor(g_day_no / 365);
    g_day_no = g_day_no % 365;
  }

  const g_days_in_month = [31, (leap ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
  let i = 0;
  for (i = 0; i < 12; i++) {
    let dim = g_days_in_month[i];
    if (g_day_no < dim) break;
    g_day_no -= dim;
  }
  let gm = i + 1;
  let gd = g_day_no + 1;

  return new Date(gy, gm - 1, gd);
}

function formatGregorianTooltip(shamsiDateStr) {
  if (!shamsiDateStr || typeof shamsiDateStr !== 'string') return null;
  const cleaned = shamsiDateStr.trim();
  if (cleaned === '0000/00/00' || cleaned === '0000-00-00' || !cleaned) return null;

  const match = cleaned.match(/^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})$/);
  if (!match) return null;

  const y = parseInt(match[1], 10);
  const m = parseInt(match[2], 10);
  const d = parseInt(match[3], 10);

  if (y < 1300 || y > 1500 || m < 1 || m > 12 || d < 1 || d > 31) return null;

  const gDate = jalaliToGregorian(y, m, d);
  if (isNaN(gDate.getTime())) return null;

  const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
  const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  const dayOfWeek = dayNames[gDate.getDay()];
  const dayOfMonth = String(gDate.getDate()).padStart(2, '0');
  const monthName = monthNames[gDate.getMonth()];
  const year = gDate.getFullYear();

  return `${dayOfWeek} , ${dayOfMonth}  ${monthName}  ${year}`;
}

function handleDateCellMouseEnter(event, cellText) {
  const gregText = formatGregorianTooltip(cellText);
  if (!gregText) return;

  let tooltip = document.getElementById('gregorianDateTooltip');
  if (!tooltip) {
    tooltip = document.createElement('div');
    tooltip.id = 'gregorianDateTooltip';
    tooltip.className = 'gregorian-date-tooltip';
    document.body.appendChild(tooltip);
  }

  tooltip.innerText = gregText;
  tooltip.style.display = 'block';

  let x = event.clientX + 10;
  let y = event.clientY + 15;
  if (x + 220 > window.innerWidth) x = event.clientX - 220;
  if (y + 40 > window.innerHeight) y = event.clientY - 40;

  tooltip.style.left = `${x}px`;
  tooltip.style.top = `${y}px`;
}

function handleDateCellMouseLeave() {
  const tooltip = document.getElementById('gregorianDateTooltip');
  if (tooltip) {
    tooltip.style.display = 'none';
  }
}

/* HELPER FUNCTION: COPY TEXT TO CLIPBOARD WITH FALLBACK */
function copyTextToClipboard(text) {
  if (!text) return false;
  if (navigator.clipboard && window.isSecureContext) {
    navigator.clipboard.writeText(text).catch(() => {
      fallbackCopyTextToClipboard(text);
    });
    return true;
  } else {
    return fallbackCopyTextToClipboard(text);
  }
}

function fallbackCopyTextToClipboard(text) {
  const textArea = document.createElement('textarea');
  textArea.value = text;
  textArea.style.top = '0';
  textArea.style.left = '0';
  textArea.style.position = 'fixed';
  textArea.style.opacity = '0';
  document.body.appendChild(textArea);
  textArea.focus();
  textArea.select();
  let successful = false;
  try {
    successful = document.execCommand('copy');
  } catch (err) {
    successful = false;
  }
  document.body.removeChild(textArea);
  return successful;
}

/* CUSTOM RIGHT-CLICK CONTEXT MENU (MATCHING DESKTOP C# APP IMAGE 1) */
let rightClickedCellData = { rowNum: null, colKey: null, cellValue: '' };

function handleDtCellContextMenu(event, trEl, rowNum, rIdx, colKey, cellValue) {
  event.preventDefault();
  event.stopPropagation();

  const rNumStr = String(rowNum).trim();
  const selectedSetStrings = new Set(Array.from(selectedDtRows).map(v => String(v).trim()));

  // Preserve multi-selection if the right-clicked row is already selected
  if (!selectedSetStrings.has(rNumStr)) {
    selectedDtRows.clear();
    selectedDtRows.add(rowNum);
    selectedDtRow = rowNum;
    lastSelectedDtIndex = rIdx;
    updateRowSelectionHighlights();
    showSelectionTooltip(event, selectedDtRows.size);
  }

  rightClickedCellData = {
    rowNum,
    colKey,
    cellValue: String(cellValue || '').trim()
  };

  showDtContextMenu(event);
}

function showDtContextMenu(event) {
  let menu = document.getElementById('dtContextMenu');
  if (!menu) {
    menu = document.createElement('div');
    menu.id = 'dtContextMenu';
    menu.className = 'dt-context-menu';
    document.body.appendChild(menu);
  }

  menu.innerHTML = `
    <div class="context-menu-header">
      <span>لطفا مورد درخواستی را کلیک کنید</span>
      <span style="font-size:10px;">▶</span>
    </div>
    <div class="context-menu-item" onclick="onContextEditRow()">
      <img src="/ico/edit1.png" width="16" height="16" alt="Edit">
      <span>ویرایش و حذف ردیف یا ردیفهای انتخابی</span>
    </div>
    <div class="context-menu-item" onclick="onContextCopyCell()">
      <img src="/ico/copySubjects.png" width="16" height="16" alt="CopyCell">
      <span>کپی محتوای این خانه به حافظه موقت</span>
    </div>
    <div class="context-menu-item" onclick="onContextCopySiteCodes()">
      <img src="/ico/copySubjects.png" width="16" height="16" alt="CopySites">
      <span>کپی کد سایتهای منتخب به حافظه موقت</span>
    </div>
    <div class="context-menu-item" onclick="onContextCopySubjects()">
      <img src="/ico/copySubjects.png" width="16" height="16" alt="CopySubjects">
      <span>کپی موضوعات منتخب به حافظه موقت</span>
    </div>
  `;

  menu.style.display = 'block';

  let x = event.clientX;
  let y = event.clientY;
  const menuWidth = 250;
  const menuHeight = 170;

  if (x + menuWidth > window.innerWidth) x = window.innerWidth - menuWidth - 10;
  if (y + menuHeight > window.innerHeight) y = window.innerHeight - menuHeight - 10;

  menu.style.left = `${x}px`;
  menu.style.top = `${y}px`;
}

function hideDtContextMenu() {
  const menu = document.getElementById('dtContextMenu');
  if (menu) menu.style.display = 'none';
}

function onContextEditRow() {
  hideDtContextMenu();
  editSelectedDt();
}

function onContextCopyCell() {
  hideDtContextMenu();
  const val = String(rightClickedCellData.cellValue || '').trim();
  if (val) {
    copyTextToClipboard(val);
    alert(`محتوای خانه "${val}" به حافظه موقت کپی شد.`);
  } else {
    alert('! خانه انتخاب شده خالی است');
  }
}

function onContextCopySiteCodes() {
  hideDtContextMenu();
  const selectedSetStrings = new Set(Array.from(selectedDtRows).map(v => String(v).trim()));

  if (selectedSetStrings.size > 1) {
    const siteCodes = [];
    selectedSetStrings.forEach(rNumStr => {
      const rowData = rawDtData.find(r => String(r['ردیف']).trim() === rNumStr);
      if (rowData && rowData['کد سایت']) {
        const cleanedCode = String(rowData['کد سایت']).replace(/XX/gi, '').trim();
        if (cleanedCode) siteCodes.push(cleanedCode);
      }
    });

    const distinctSiteCodes = [...new Set(siteCodes)];
    const joinedStr = distinctSiteCodes.join(',');

    copyTextToClipboard(joinedStr);
    const msg = `${distinctSiteCodes.length} عدد کدسایت بدون تکرار در کلیپ برد کپی شد  \n${joinedStr}`;
    alert(msg);
  } else {
    alert('!فقط یک سطر انتخاب شده و سری قابل ساخت نیست لطفا حداقل دو سطر انتخاب کنید');
  }
}

function onContextCopySubjects() {
  hideDtContextMenu();
  const selectedSetStrings = new Set(Array.from(selectedDtRows).map(v => String(v).trim()));

  if (selectedSetStrings.size >= 1) {
    const subjects = [];
    selectedSetStrings.forEach(rNumStr => {
      const rowData = rawDtData.find(r => String(r['ردیف']).trim() === rNumStr);
      if (rowData && rowData['موضوع']) {
        subjects.push(String(rowData['موضوع']).trim());
      }
    });

    const joinedStr = subjects.join('\r\n');
    copyTextToClipboard(joinedStr);
    const msg = `${subjects.length} عدد موضوع تکراری و غیر تکراری در کلیپ برد کپی شد . \n\n${joinedStr}`;
    alert(msg);
  } else {
    alert('!هیچ سطری انتخاب نشده است');
  }
}

function renderDtTable() {
  const head = document.getElementById('dtTableHead');
  const body = document.getElementById('dtTableBody');
  const foot = document.getElementById('dtTableFoot');

  if (!head || !body) return;

  const columns = [
    { key: 'ردیف', label: 'ردیف', width: '60px' },
    { key: 'تاریخ ثبت', label: 'تاریخ ثبت', width: '70px' },
    { key: 'ثبت', label: 'زمان ثبت', width: '75px' },
    { key: 'کد سایت', label: 'کد سایت', width: '75px' },
    { key: 'نام سایت', label: 'نام سایت', width: '120px' },
    { key: 'منطقه', label: 'منطقه', width: '55px' },
    { key: 'تاریخ شروع', label: 'تاریخ شروع', width: '70px' },
    { key: 'زمان شروع', label: 'زمان شروع', width: '75px' },
    { key: 'تاریخ پایان', label: 'تاریخ پایان', width: '70px' },
    { key: 'زمان پایان', label: 'زمان پایان', width: '75px' },
    { key: 'زمان قطعی', label: 'زمان قطعی', width: '75px' },
    { key: 'درخواست کننده', label: 'درخواست کننده', width: '95px' },
    { key: 'موضوع', label: 'موضوع', width: '95px' },
    { key: 'انجام دهنده', label: 'انجام دهنده', width: '95px' },
    { key: 'ثبت کننده', label: 'ثبت کننده', width: '85px' },
    { key: 'شماره', label: 'شماره', width: '105px' },
    { key: 'وضعیت', label: 'وضعیت', width: '105px' },
    { key: 'فعالیت', label: 'فعالیت', width: '90px' },
    { key: 'یادداشت', label: 'یادداشت', width: '140px' }
  ];

  // 1. Header Row
  let headerHtml = '<tr>';
  columns.forEach(col => {
    let sortIconSvg = `<svg width="8" height="10" viewBox="0 0 8 10" fill="none" style="margin-right:2px; flex-shrink:0; opacity:0.35;"><path d="M4 0.5L7 3.5H1L4 0.5Z" fill="#000"/><path d="M4 9.5L1 6.5H7L4 9.5Z" fill="#000"/></svg>`;
    if (dtSortColumn === col.key) {
      if (dtSortDirection === 'asc') {
        sortIconSvg = `<svg width="9" height="11" viewBox="0 0 9 11" fill="none" style="margin-right:2px; flex-shrink:0;"><path d="M4.5 1L8 5H1L4.5 1Z" fill="#0284c7"/><path d="M4.5 10L1.5 6.5H7.5L4.5 10Z" fill="#cbd5e1" opacity="0.4"/></svg>`;
      } else {
        sortIconSvg = `<svg width="9" height="11" viewBox="0 0 9 11" fill="none" style="margin-right:2px; flex-shrink:0;"><path d="M4.5 1L8 4.5H1L4.5 1Z" fill="#cbd5e1" opacity="0.4"/><path d="M4.5 10L1 6H8L4.5 10Z" fill="#0284c7"/></svg>`;
      }
    }
    headerHtml += `<th onclick="sortDtColumn('${col.key}')" style="width:${col.width}; min-width:${col.width}; padding: 5px 3px; font-size: 11px; cursor: pointer; user-select: none;" title="کلیک برای مرتب‌سازی ${col.label}"><div style="display:flex; align-items:center; justify-content:center; gap:2px; white-space:nowrap;"><span>${col.label}</span>${sortIconSvg}</div></th>`;
  });
  headerHtml += '</tr>';

  // Auto Filter Row
  headerHtml += '<tr class="filter-row">';
  columns.forEach(col => {
    const val = dtColumnFilters[col.key] || '';
    headerHtml += `<th class="filter-cell" style="width:${col.width}; min-width:${col.width};"><input class="column-filter-input" data-col="${col.key}" type="text" value="${val}" oninput="updateDtColumnFilter('${col.key}', this.value, this)" placeholder="فیلتر..."></th>`;
  });
  headerHtml += '</tr>';

  head.innerHTML = headerHtml;

  // 2. Body Rows
  let bodyHtml = '';
  let totalOutageMin = 0;

  filteredDtData.forEach((row, rIdx) => {
    const style = getDtRowStyle(row);
    const outage = parseInt(row['زمان قطعی']) || 0;
    totalOutageMin += outage;

    const rowNum = row['ردیف'];
    const hasAttach = hasAttachment(row);
    const isSelected = selectedDtRows.has(rowNum) ? 'selected-row' : '';

    bodyHtml += `<tr class="${isSelected}" style="${style}" onmousedown="handleDtRowMouseDown(event, this, ${rowNum}, ${rIdx})" onmouseenter="handleDtRowMouseEnter(event, this, ${rowNum}, ${rIdx})" onclick="selectDtRow(event, this, ${rowNum}, ${rIdx})" data-row="${rowNum}">`;
    columns.forEach(col => {
      let cellVal = row[col.key] ?? '';
      const cellRawVal = String(cellVal).replace(/'/g, "\\'").replace(/"/g, '&quot;');

      if (col.key === 'ردیف') {
        if (hasAttach) {
          cellVal = `<a class="dt-row-attach-link" href="#" onclick="handleAttachmentClick(event, ${rowNum}); return false;" title="ردیف ${rowNum} دارای فایل پیوست است. کلیک برای دانلود">${rowNum} 📎</a>`;
        } else {
          cellVal = rowNum;
        }
      }

      const isDateCol = (col.key === 'تاریخ ثبت' || col.key === 'تاریخ شروع' || col.key === 'تاریخ پایان');
      const mouseEnterAttr = isDateCol ? `onmouseenter="handleDateCellMouseEnter(event, '${cellRawVal}')" onmouseleave="handleDateCellMouseLeave()"` : '';
      const plainTextVal = String(row[col.key] ?? '').replace(/<[^>]*>/g, '').trim();
      const cellTitleAttr = plainTextVal ? `title="${col.label}: ${plainTextVal.replace(/"/g, '&quot;')}"` : '';

      bodyHtml += `<td ${cellTitleAttr} ${mouseEnterAttr} oncontextmenu="handleDtCellContextMenu(event, this, ${rowNum}, ${rIdx}, '${col.key}', '${cellRawVal}')" style="width:${col.width}; min-width:${col.width}; text-align:center; white-space:nowrap; overflow:hidden; text-overflow:ellipsis; padding: 3px 5px; font-size: 11px;">${cellVal}</td>`;
    });
    bodyHtml += '</tr>';
  });

  body.innerHTML = bodyHtml;

  // 3. Clear Footer
  if (foot) {
    foot.innerHTML = '';
  }

  // Update Status Bar Record Count
  if (typeof updateStatusRecordCount === 'function') {
    updateStatusRecordCount(filteredDtData.length, currentScopeText);
  }

  // Enable Resizable Column Widths
  setTimeout(() => enableTableColumnResizing('dtTable'), 50);
}

function enableTableColumnResizing(tableId) {
  const table = document.getElementById(tableId);
  if (!table) return;

  const ths = table.querySelectorAll('thead th');
  ths.forEach(th => {
    if (th.querySelector('.column-resizer')) return;

    const resizer = document.createElement('div');
    resizer.className = 'column-resizer';
    th.style.position = 'relative';
    th.appendChild(resizer);

    let startX, startWidth;

    resizer.addEventListener('click', function (e) {
      e.preventDefault();
      e.stopPropagation();
    });

    resizer.addEventListener('mousedown', function (e) {
      e.preventDefault();
      e.stopPropagation();
      startX = e.pageX;
      startWidth = th.offsetWidth;
      resizer.classList.add('resizing');
      window.isColumnJustResized = true;

      function onMouseMove(e) {
        window.isColumnJustResized = true;
        const diff = startX - e.pageX;
        const newWidth = Math.max(40, startWidth + diff);
        th.style.width = newWidth + 'px';
        th.style.minWidth = newWidth + 'px';
      }

      function onMouseUp() {
        resizer.classList.remove('resizing');
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
        setTimeout(() => { window.isColumnJustResized = false; }, 250);
      }

      document.addEventListener('mousemove', onMouseMove);
      document.addEventListener('mouseup', onMouseUp);
    });
  });
}

// ==========================================================================
// DOWNTIME REGISTRATION FORM LOGIC (frmDtRegDownTime - EXACT WINFORMS LOGIC)
// ==========================================================================

let _selectedAttachmentFile = null;
let _uploadedTempAttachmentId = null;

function toggleAttachmentBox(hasAttach) {
  const box = document.getElementById('attachmentFileBox');
  if (box) {
    box.style.display = hasAttach ? 'block' : 'none';
  }
  if (!hasAttach) {
    clearSelectedAttachment();
  }
}

async function handleAttachmentSelected(input) {
  if (!input || !input.files || !input.files[0]) return;

  const file = input.files[0];
  if (file.size > 20 * 1024 * 1024) {
    alert('حجم فایل انتخابی بیش از حد مجاز (20 مگابایت) می‌باشد.');
    input.value = '';
    return;
  }

  _selectedAttachmentFile = file;
  _uploadedTempAttachmentId = null;

  const fileNameEl = document.getElementById('selectedFileName');
  const fileSizeEl = document.getElementById('selectedFileSize');
  const fileInfoBox = document.getElementById('selectedFileInfo');
  const dropArea = document.getElementById('fileDropArea');

  if (fileNameEl) fileNameEl.textContent = `⏳ ${file.name} (در حال ارسال به سرور...)`;
  if (fileSizeEl) fileSizeEl.textContent = (file.size / 1024).toFixed(1) + ' KB';
  if (fileInfoBox) {
    fileInfoBox.style.display = 'flex';
    fileInfoBox.style.borderColor = '#0284c7';
    fileInfoBox.style.background = '#f0f9ff';
  }
  if (dropArea) dropArea.style.display = 'none';

  try {
    const base64Data = await readFileAsBase64(file);
    const res = await fetch('/api/dt/upload-temp', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        fileName: file.name,
        fileData: base64Data
      })
    });

    const responseText = await res.text();
    let result;
    try {
      result = JSON.parse(responseText);
    } catch (parseErr) {
      console.error('Non-JSON response received:', responseText);
      alert('خطا در بارگذاری فایل پیوست (پاسخ سرور مجاز نبود). لطفا مجددا تلاش فرمایید.');
      clearSelectedAttachment();
      return;
    }

    if (result.success && result.tempId) {
      _uploadedTempAttachmentId = result.tempId;
      if (fileNameEl) fileNameEl.innerHTML = `✅ ${file.name} <span class="badge" style="background:#16a34a; color:#fff; font-size:10.5px; padding:2px 6px; border-radius:4px; margin-right:6px;">آپلود و تایید شد</span>`;
      if (fileInfoBox) {
        fileInfoBox.style.borderColor = '#16a34a';
        fileInfoBox.style.background = '#f0fdf4';
      }
    } else {
      alert('خطا در بارگذاری فایل روی سرور: ' + (result.error || 'پاسخ نامشخص'));
      clearSelectedAttachment();
    }
  } catch (err) {
    console.error('Instant upload error:', err);
    alert('خطا در ارتباط با سرور هنگام بارگذاری فایل پیوست.');
    clearSelectedAttachment();
  }
}

function clearSelectedAttachment() {
  _selectedAttachmentFile = null;
  _uploadedTempAttachmentId = null;
  const input = document.getElementById('dtRegAttachmentInput');
  if (input) input.value = '';

  const fileInfoBox = document.getElementById('selectedFileInfo');
  const dropArea = document.getElementById('fileDropArea');
  if (fileInfoBox) fileInfoBox.style.display = 'none';
  if (dropArea) dropArea.style.display = 'block';
}

function readFileAsBase64(file) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
    reader.readAsDataURL(file);
  });
}

// Get Shamsi Today String (e.g., 1403/11/12)
function getShamsiTodayString(offsetDays = 0) {
  try {
    const d = new Date();
    if (offsetDays !== 0) d.setDate(d.getDate() + offsetDays);
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
    return '';
  }
}

// =========================================================================
// FRMADDDOWNTIME WIZARD CONTROLLER & LOGIC
// =========================================================================

let _performersList = [];
let _selectedPerformer = null;
let _serverTimeInfo = null;

function populateShamsiDays(selectId, selectedDayStr) {
  const select = document.getElementById(selectId);
  if (!select) return;
  let html = '';
  for (let i = 1; i <= 31; i++) {
    const dStr = String(i).padStart(2, '0');
    const isSel = (dStr === selectedDayStr) ? 'selected' : '';
    html += `<option value="${dStr}" ${isSel}>${dStr}</option>`;
  }
  select.innerHTML = html;
}

function getSelectedShamsiDate(prefix) {
  const y = document.getElementById(`dtReg${prefix}Year`).value;
  const m = document.getElementById(`dtReg${prefix}Month`).value;
  const d = document.getElementById(`dtReg${prefix}Day`).value;
  return `${y}/${m}/${d}`;
}

function setSelectedShamsiDate(prefix, shamsiStr) {
  if (!shamsiStr) return;
  const parts = shamsiStr.split('/');
  if (parts.length === 3) {
    const y = parts[0];
    const m = parts[1].padStart(2, '0');
    const d = parts[2].padStart(2, '0');
    
    const yEl = document.getElementById(`dtReg${prefix}Year`);
    const mEl = document.getElementById(`dtReg${prefix}Month`);
    if (yEl) yEl.value = y;
    if (mEl) mEl.value = m;
    populateShamsiDays(`dtReg${prefix}Day`, d);
  }
}

async function initializeShamsiDatesToToday() {
  try {
    const res = await fetch('/api/dt/server-time');
    const data = await res.json();
    if (data.success && data.shamsiDate) {
      _serverTimeInfo = data;
      setSelectedShamsiDate('Start', data.shamsiDate);
      setSelectedShamsiDate('End', data.shamsiDate);
      return;
    }
  } catch (e) {}

  const d = new Date();
  const faParts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
    year: 'numeric', month: '2-digit', day: '2-digit'
  }).formatToParts(d);
  const y = faParts.find(p => p.type === 'year').value;
  const m = faParts.find(p => p.type === 'month').value;
  const day = faParts.find(p => p.type === 'day').value;
  const todayStr = `${y}/${m}/${day}`;
  setSelectedShamsiDate('Start', todayStr);
  setSelectedShamsiDate('End', todayStr);
}

async function openDtRegModal() {
  const modal = document.getElementById('dtRegModal');
  if (!modal) return;
  modal.style.display = 'flex';

  if (!modal.dataset.enterListenerAttached) {
    modal.dataset.enterListenerAttached = 'true';
    modal.addEventListener('keydown', (e) => {
      if (e.key === 'Enter') {
        const activeEl = document.activeElement;
        if (activeEl && activeEl.id !== 'btnSubmit') {
          e.preventDefault();
          e.stopPropagation();

          if (activeEl.id === 'txtSiteCode') {
            const btnNextSite = document.getElementById('btnNextSite');
            if (btnNextSite && !btnNextSite.disabled) btnNextSite_Click();
          } else if (activeEl.id === 'txtSubject' || activeEl.id === 'sluePerformer' || (activeEl.id && activeEl.id.startsWith('tme')) || activeEl.id === 'chkInBsyHour') {
            const btnNextDetails = document.getElementById('btnNextDetails');
            if (btnNextDetails && !btnNextDetails.disabled) btnNextDetails_Click();
          } else if (activeEl.tagName === 'SELECT' || (activeEl.id && activeEl.id.startsWith('dtReg'))) {
            btnNextDate_Click();
          } else if (activeEl.name === 'rdoDomain') {
            btnNextDomain_Click();
          } else if (activeEl.name === 'rdoCategory') {
            btnNextCategory_Click();
          } else if (activeEl.name === 'rdoAttachmentChoice') {
            const btnSubmit = document.getElementById('btnSubmit');
            if (btnSubmit && !btnSubmit.disabled) btnSubmit.focus();
          }
        }
      }
    });
  }

  await initializeShamsiDatesToToday();
  await loadPerformersList();
  await loadDtSubjectsList();
  await btnReset_Click();
  loadUserRecentDtSubmissions();
}

function closeDtRegModal(event) {
  if (event && event.target && event.target.id !== 'dtRegModal' && !event.target.classList.contains('modal-close') && !event.target.classList.contains('btn-action-close')) {
    return;
  }
  const modal = document.getElementById('dtRegModal');
  if (modal) modal.style.display = 'none';
}

async function btnReset_Click() {
  try {
    const res = await fetch('/api/dt/server-time');
    const data = await res.json();
    if (data.success) {
      _serverTimeInfo = data;
      setSelectedShamsiDate('Start', data.shamsiDate);
      setSelectedShamsiDate('End', data.shamsiDate);
      
      const curH = String(data.hour).padStart(2, '0');
      document.getElementById('tmeStartHour').value = curH;
      document.getElementById('tmeStartMin').value = '00';
      document.getElementById('tmeEndHour').value = curH;
      document.getElementById('tmeEndMin').value = '00';
    }
  } catch (e) {
    const today = getShamsiTodayString(0);
    setSelectedShamsiDate('Start', today);
    setSelectedShamsiDate('End', today);
  }

  // Clear domain
  const domainRadios = document.querySelectorAll('input[name="rdoDomain"]');
  domainRadios.forEach(r => r.checked = false);
  document.getElementById('btnNextDomain').disabled = true;

  // Clear site code
  const txtSiteCode = document.getElementById('txtSiteCode');
  txtSiteCode.value = '';
  txtSiteCode.disabled = false;
  document.getElementById('lblSiteNameResult').textContent = '--------';
  document.getElementById('lblSiteNameResult').style.color = 'var(--text-muted)';
  document.getElementById('btnNextSite').disabled = true;

  // Clear category (Default to Site Access)
  const rdoAccess = document.getElementById('rdoSiteAccess');
  if (rdoAccess) {
    rdoAccess.checked = true;
    Category_CheckedChanged(rdoAccess);
  }

  _isSiteValid = false;
  _selectedOutageMinutes = 0;
  _selectedMaxDuration = 0;
  _isCorrectingDateForStep5 = false;
  _selectedPerformer = null;

  document.getElementById('btnSubmit').disabled = true;
  const dateAlert = document.getElementById('dateCorrectionAlert');
  if (dateAlert) dateAlert.style.display = 'none';

  _maxUnlockedStep = 1;
  activateStep(1, true);
}

let _maxUnlockedStep = 1;

function activateStep(stepNum, forceUnlock = false) {
  if (forceUnlock) {
    _maxUnlockedStep = Math.max(_maxUnlockedStep, stepNum);
  }
  if (stepNum > _maxUnlockedStep) {
    return;
  }
  _currentWizardStep = stepNum;

  // Update Stepper Bar
  for (let i = 1; i <= 6; i++) {
    const indicator = document.getElementById(`stepIndicator${i}`);
    if (indicator) {
      if (i === stepNum) {
        indicator.className = 'step-item active';
      } else if (i <= _maxUnlockedStep) {
        indicator.className = 'step-item completed';
      } else {
        indicator.className = 'step-item';
      }
    }
  }

  // Update Cards State (Step-by-Step Progressive Enablement)
  const cards = [
    { num: 1, id: 'grpDates' },
    { num: 2, id: 'grpDomain' },
    { num: 3, id: 'grpSiteCode' },
    { num: 4, id: 'grpCategory' },
    { num: 5, id: 'grpTimeAndDetails' },
    { num: 6, id: 'grpAttachment' }
  ];

  cards.forEach(c => {
    const el = document.getElementById(c.id);
    if (!el) return;
    if (c.num <= _maxUnlockedStep) {
      el.classList.add('active-step-card');
      el.classList.remove('disabled-step-card');
    } else {
      el.classList.remove('active-step-card');
      el.classList.add('disabled-step-card');
    }
  });

  // Actions card (Submit button) is disabled until Step 6 is reached
  const grpActions = document.getElementById('grpActions');
  if (grpActions) {
    if (_maxUnlockedStep >= 6) {
      grpActions.classList.remove('disabled-step-card');
    } else {
      grpActions.classList.add('disabled-step-card');
    }
  }

  if (stepNum === 1) {
    const startDay = document.getElementById('dtRegStartDay');
    if (startDay) startDay.focus();
  } else if (stepNum === 2 || stepNum === 3) {
    const txtSiteCode = document.getElementById('txtSiteCode');
    if (txtSiteCode) {
      txtSiteCode.disabled = false;
      setTimeout(() => {
        txtSiteCode.focus();
        txtSiteCode.select();
      }, 60);
    }
  } else if (stepNum === 4) {
    const rdoAccess = document.getElementById('rdoSiteAccess');
    if (rdoAccess && !document.querySelector('input[name="rdoCategory"]:checked')) {
      rdoAccess.checked = true;
      Category_CheckedChanged(rdoAccess);
    }
    const btnNextCat = document.getElementById('btnNextCategory');
    if (btnNextCat) {
      btnNextCat.disabled = false;
      setTimeout(() => btnNextCat.focus(), 60);
    }
  } else if (stepNum === 5) {
    // Auto Time Initialization when Step 5 is activated
    const startDate = getSelectedShamsiDate('Start');
    const endDate = getSelectedShamsiDate('End');

    if (_serverTimeInfo && startDate === _serverTimeInfo.shamsiDate) {
      let curM = _serverTimeInfo.minute;
      let minutesToAdd = 15 - (curM % 15);
      let targetHour = _serverTimeInfo.hour;
      let targetMin = curM + minutesToAdd;
      if (targetMin >= 60) {
        targetMin = 0;
        targetHour = (targetHour + 1) % 24;
      }
      document.getElementById('tmeStartHour').value = String(targetHour).padStart(2, '0');
      document.getElementById('tmeStartMin').value = String(targetMin).padStart(2, '0');
    }

    if (startDate === endDate) {
      let stH = parseInt(document.getElementById('tmeStartHour').value) || 8;
      let stM = parseInt(document.getElementById('tmeStartMin').value) || 0;
      let endM = stM + 15;
      let endH = stH;
      if (endM >= 60) {
        endM = 0;
        endH = (endH + 1) % 24;
      }
      document.getElementById('tmeEndHour').value = String(endH).padStart(2, '0');
      document.getElementById('tmeEndMin').value = String(endM).padStart(2, '0');
    }

    const stH = document.getElementById('tmeStartHour');
    if (stH) setTimeout(() => stH.focus(), 60);
  }
}

function jumpToWizardStep(stepNum) {
  if (stepNum <= _maxUnlockedStep) {
    activateStep(stepNum, false);
  }
}

function handleStartDateChange() {
  const startDate = getSelectedShamsiDate('Start');
  const endDate = getSelectedShamsiDate('End');
  if (endDate < startDate) {
    setSelectedShamsiDate('End', startDate);
  }
}

function validateDateStep() {
  const alertBox = document.getElementById('dateCorrectionAlert');
  if (alertBox) alertBox.style.display = 'none';
}

function btnNextDate_Click() {
  _isCorrectingDateForStep5 = false;
  const alertBox = document.getElementById('dateCorrectionAlert');
  if (alertBox) alertBox.style.display = 'none';

  let startDate = getSelectedShamsiDate('Start');
  let endDate = getSelectedShamsiDate('End');

  if (_serverTimeInfo && startDate < _serverTimeInfo.shamsiDate) {
    setSelectedShamsiDate('Start', _serverTimeInfo.shamsiDate);
    if (endDate < _serverTimeInfo.shamsiDate) {
      setSelectedShamsiDate('End', _serverTimeInfo.shamsiDate);
    }
    startDate = getSelectedShamsiDate('Start');
    endDate = getSelectedShamsiDate('End');
  }

  if (endDate < startDate) {
    setSelectedShamsiDate('End', startDate);
  }

  const domainRadio = document.querySelector('input[name="rdoDomain"][value="SITE"]');
  if (domainRadio) {
    domainRadio.checked = true;
  }
  rdoDomain_EditValueChanged('SITE');

  activateStep(3, true);

  const txtSiteCode = document.getElementById('txtSiteCode');
  if (txtSiteCode) {
    txtSiteCode.disabled = false;
    txtSiteCode.style.background = '#ffffff';
    txtSiteCode.focus();
    txtSiteCode.select();
  }
}

function rdoDomain_EditValueChanged(val) {
  document.getElementById('btnNextDomain').disabled = !val;
  const txtSiteCode = document.getElementById('txtSiteCode');
  const lblSiteNameResult = document.getElementById('lblSiteNameResult');

  if (val === 'SITE') {
    txtSiteCode.disabled = false;
    txtSiteCode.style.background = '#ffffff';
  } else {
    txtSiteCode.disabled = true;
    txtSiteCode.style.background = 'var(--row-hover)';
    txtSiteCode.value = '';
    lblSiteNameResult.textContent = '--------';
    lblSiteNameResult.style.color = '#8b5cf6';
  }
}

function btnNextDomain_Click() {
  let domainEl = document.querySelector('input[name="rdoDomain"]:checked');
  if (!domainEl) {
    const siteRadio = document.querySelector('input[name="rdoDomain"][value="SITE"]');
    if (siteRadio) {
      siteRadio.checked = true;
      rdoDomain_EditValueChanged('SITE');
      domainEl = siteRadio;
    }
  }
  const domainVal = domainEl ? domainEl.value : 'SITE';

  if (domainVal === 'SITE') {
    activateStep(3, true);
  } else {
    _isSiteValid = true;
    activateStep(4, true);
    const rdoOther = document.getElementById('rdoOther');
    if (rdoOther) {
      rdoOther.checked = true;
      Category_CheckedChanged(rdoOther);
    }
  }
}

function txtSiteCode_TextChanged(val) {
  const cleanDigits = val.replace(/[^0-9]/g, '').trim();
  const lblSiteNameResult = document.getElementById('lblSiteNameResult');
  const btnNextSite = document.getElementById('btnNextSite');

  if (cleanDigits.length === 4) {
    performSiteLookup(cleanDigits);
  } else {
    _isSiteValid = false;
    lblSiteNameResult.textContent = '--------';
    lblSiteNameResult.style.color = 'var(--text-muted)';
    btnNextSite.disabled = true;
  }
}

let _siteRegion = '';

async function performSiteLookup(siteCode) {
  const lblSiteNameResult = document.getElementById('lblSiteNameResult');
  const btnNextSite = document.getElementById('btnNextSite');

  try {
    const res = await fetch(`/api/dt/site-info/${siteCode}`);
    const data = await res.json();

    if (data.found && data.siteName && data.siteName !== 'نامشخص') {
      _isSiteValid = true;
      _siteRegion = data.region || '';
      lblSiteNameResult.textContent = data.siteName + ' ✅';
      lblSiteNameResult.style.color = '#16a34a';
      btnNextSite.disabled = false;
    } else {
      const confirmContinue = confirm('این کد سایت در دیتابیس موجود نیست آیا ادامه می دهید؟');
      if (confirmContinue) {
        _isSiteValid = true;
        _siteRegion = '';
        lblSiteNameResult.textContent = 'نامشخص (تایید شده) ⚠️';
        lblSiteNameResult.style.color = '#ea580c';
        btnNextSite.disabled = false;
      } else {
        _isSiteValid = false;
        _siteRegion = '';
        lblSiteNameResult.textContent = '--------';
        lblSiteNameResult.style.color = '#dc2626';
        btnNextSite.disabled = true;
        document.getElementById('txtSiteCode').value = '';
        document.getElementById('txtSiteCode').focus();
      }
    }
  } catch (err) {
    console.error('Site lookup error:', err);
  }
}

function btnNextSite_Click() {
  if (_isSiteValid) {
    activateStep(4, true);
    const rdoAccess = document.getElementById('rdoSiteAccess');
    if (rdoAccess) {
      rdoAccess.checked = true;
      Category_CheckedChanged(rdoAccess);
      rdoAccess.focus();
    }
  }
}

function Category_CheckedChanged(el) {
  if (!el || !el.checked) return;

  document.getElementById('btnNextCategory').disabled = false;
  const domainEl = document.querySelector('input[name="rdoDomain"]:checked');
  const domainVal = domainEl ? domainEl.value : 'SITE';
  const siteResultText = document.getElementById('lblSiteNameResult').textContent.replace(' ✅', '').replace(' (تایید شده) ⚠️', '');
  const siteNameStr = (domainVal === 'SITE') ? siteResultText : domainVal;

  const txtOutage = document.getElementById('txtOutage');
  const txtSubject = document.getElementById('txtSubject');

  if (el.id === 'rdoSiteAccess') {
    _selectedOutageMinutes = 0;
    _selectedMaxDuration = 4;
    txtOutage.readOnly = true;
    txtOutage.value = 'Outage = 0 (اجباری)';
  } else if (el.id === 'rdoSiteCleanup') {
    _selectedOutageMinutes = 60;
    _selectedMaxDuration = 6;
    txtOutage.readOnly = true;
    txtOutage.value = 'Outage = 60';
  } else {
    _selectedOutageMinutes = -1;
    _selectedMaxDuration = 0;
    txtOutage.readOnly = false;
    txtOutage.value = '0';
    txtOutage.placeholder = 'دقایق Outage (مثلاً 0 یا 30)';
  }

  // Subject is ALWAYS EMPTY initially by user directive
  txtSubject.value = '';
  txtSubject.placeholder = 'جستجو و انتخاب موضوع...';
}

function btnNextCategory_Click() {
  const startDate = getSelectedShamsiDate('Start');
  const endDate = getSelectedShamsiDate('End');

  // Vital Check: Start and End Dates MUST be on the same day
  if (startDate !== endDate) {
    const catAccess = document.getElementById('rdoSiteAccess');
    const catCleanup = document.getElementById('rdoSiteCleanup');
    const catName = catAccess.checked ? "Site Access" : (catCleanup.checked ? "Site Cleanup" : "سایر موارد");

    const errorMessage = `چون درخواست شما از نوع « ${catName} » می‌باشد،\nتاریخ شروع و پایان باید در یک روز باشد.\nلطفاً تاریخ درخواست خود را اصلاح نمایید.`;
    alert(errorMessage);

    _isCorrectingDateForStep5 = true;
    activateStep(1);
    const alertBox = document.getElementById('dateCorrectionAlert');
    if (alertBox) {
      alertBox.textContent = `توجه: به دلیل انتخاب «${catName}»، تاریخ شروع و پایان باید کاملاً یکسان باشد.`;
      alertBox.style.display = 'block';
    }
    return;
  }

  activateStep(5, true);
}

function TimeEdit_Spin(type, field) {
  let hEl = document.getElementById(`tme${type === 'start' ? 'Start' : 'End'}Hour`);
  let mEl = document.getElementById(`tme${type === 'start' ? 'Start' : 'End'}Min`);
  
  let h = parseInt(hEl.value) || 0;
  let m = parseInt(mEl.value) || 0;

  if (m % 15 !== 0) {
    m = Math.round(m / 15) * 15;
    if (m >= 60) {
      m = 0;
      h = (h + 1) % 24;
    }
  }
  hEl.value = String(h).padStart(2, '0');
  mEl.value = String(m).padStart(2, '0');
}

function TimeEdit_Validating(type) {
  let hEl = document.getElementById(`tme${type === 'start' ? 'Start' : 'End'}Hour`);
  let mEl = document.getElementById(`tme${type === 'start' ? 'Start' : 'End'}Min`);
  
  let h = parseInt(hEl.value) || 0;
  let m = parseInt(mEl.value) || 0;

  if (m % 15 !== 0) {
    m = (m % 15 >= 8) ? m + (15 - (m % 15)) : m - (m % 15);
    if (m >= 60) {
      m = 0;
      h = (h + 1) % 24;
    }
  }
  hEl.value = String(h).padStart(2, '0');
  mEl.value = String(m).padStart(2, '0');

  if (type === 'start') {
    if (h < 6) {
      alert('زمان شروع نمی‌تواند قبل از ساعت 06:00 صبح باشد.\nلطفاً زمان را اصلاح کنید.');
      hEl.value = '06';
      hEl.focus();
    }
  }
}

const DEFAULT_PERFORMERS_LIST = [
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

_performersList = [...DEFAULT_PERFORMERS_LIST];

async function loadPerformersList() {
  try {
    const res = await fetch('/api/dt/performers');
    const data = await res.json();
    if (data.success && Array.isArray(data.performers) && data.performers.length > 0) {
      _performersList = data.performers;
    }
  } catch (err) {
    console.error('loadPerformersList error:', err);
  }
}

function openPerformerDropdown() {
  filterPerformerDropdown(document.getElementById('sluePerformer').value);
}

function filterPerformerDropdown(query) {
  const dropdown = document.getElementById('sluePerformerDropdown');
  if (!dropdown) return;

  if (!_performersList || _performersList.length === 0) {
    _performersList = [...DEFAULT_PERFORMERS_LIST];
  }

  const q = (query || '').trim().toLowerCase();
  const filtered = _performersList.filter(p => {
    return (p.performer || '').toLowerCase().includes(q) ||
           (p.Office_PhoneBook || '').toLowerCase().includes(q) ||
           (p.Mobile_Personnel || '').toLowerCase().includes(q);
  }).slice(0, 30);

  if (filtered.length === 0) {
    dropdown.innerHTML = '<div class="p-2 text-muted text-center">انجام‌دهنده‌ای یافت نشد.</div>';
  } else {
    let html = '';
    filtered.forEach(p => {
      html += `
        <div class="slue-item" onclick="selectPerformerItem('${p.performer.replace(/'/g, "\\'")}', '${(p.Office_PhoneBook || '---').replace(/'/g, "\\'")}', '${(p.Mobile_Personnel || '---').replace(/'/g, "\\'")}')">
          <span><strong>${p.performer}</strong> (${p.Office_PhoneBook || '---'})</span>
          <span style="color:#16a34a; direction:ltr;">${p.Mobile_Personnel || ''}</span>
        </div>
      `;
    });
    dropdown.innerHTML = html;
  }
  dropdown.style.display = 'block';

  if (!window.slueOutsideClickListener) {
    window.slueOutsideClickListener = function(e) {
      if (!e.target.closest('.slue-performer-wrapper')) {
        dropdown.style.display = 'none';
      }
    };
    document.addEventListener('click', window.slueOutsideClickListener);
  }
}

function selectPerformerItem(name, workPlace, phone) {
  document.getElementById('sluePerformer').value = name;
  document.getElementById('lblWorkPlaceResult').textContent = workPlace || '---';
  document.getElementById('lblPhoneResult').textContent = phone || '---';
  _selectedPerformer = { name, workPlace, phone };

  document.getElementById('sluePerformerDropdown').style.display = 'none';
  const btnNextDetails = document.getElementById('btnNextDetails');
  if (btnNextDetails) btnNextDetails.disabled = false;
}

function btnNextDetails_Click() {
  const subject = document.getElementById('txtSubject').value.trim();
  const performer = document.getElementById('sluePerformer').value.trim();

  if (!subject) {
    alert('لطفاً موضوع درخواست را انتخاب نمایید.');
    document.getElementById('txtSubject').focus();
    return;
  }
  if (!performer) {
    alert('لطفاً انجام دهنده را انتخاب نمایید.');
    document.getElementById('sluePerformer').focus();
    return;
  }

  activateStep(6, true);
  const btnSubmit = document.getElementById('btnSubmit');
  if (btnSubmit) btnSubmit.disabled = false;
}

async function btnSubmit_Click() {
  const startDate = getSelectedShamsiDate('Start');
  const endDate = getSelectedShamsiDate('End');
  
  const stH = document.getElementById('tmeStartHour').value.padStart(2, '0');
  const stM = document.getElementById('tmeStartMin').value.padStart(2, '0');
  const endH = document.getElementById('tmeEndHour').value.padStart(2, '0');
  const endM = document.getElementById('tmeEndMin').value.padStart(2, '0');

  const startTime = `${stH}:${stM}`;
  const endTime = `${endH}:${endM}`;

  // Server Time Check
  if (_serverTimeInfo && startDate === _serverTimeInfo.shamsiDate) {
    if (startTime < _serverTimeInfo.time) {
      alert('شما تاریخ امروز را انتخاب کرده‌اید، بنابراین زمان شروع نمی‌تواند در گذشته باشد.');
      return;
    }
  }

  const startMinutesTotal = parseInt(stH) * 60 + parseInt(stM);
  const endMinutesTotal = parseInt(endH) * 60 + parseInt(endM);

  if (startDate === endDate && endMinutesTotal <= startMinutesTotal) {
    alert('زمان پایان باید حتماً پس از زمان شروع باشد.');
    return;
  }

  // Max Duration Check
  const totalHours = (endMinutesTotal - startMinutesTotal) / 60;
  if (_selectedMaxDuration > 0 && totalHours > _selectedMaxDuration) {
    alert(`در دسته‌بندی دان‌تایمی که شما انتخاب کردید نباید کل زمان دان‌تایم بیش از ${_selectedMaxDuration} ساعت باشد و شما ${totalHours.toFixed(1)} ساعت رو انتخاب کردید.`);
    return;
  }

  let performer = document.getElementById('sluePerformer').value.trim();
  if (!performer) {
    alert('لطفاً شخص انجام دهنده را مشخص کنید.');
    return;
  }

  const phoneVal = document.getElementById('lblPhoneResult').textContent.trim();
  if (phoneVal && phoneVal !== '---' && !performer.includes('-')) {
    performer = `${performer} - ${phoneVal}`;
  }

  const domainEl = document.querySelector('input[name="rdoDomain"]:checked');
  const domain = domainEl ? domainEl.value : 'SITE';
  const siteCodeRaw = document.getElementById('txtSiteCode').value.trim();
  const siteCode = (domain === 'SITE') ? ('ES' + siteCodeRaw) : domain;
  const siteNameText = document.getElementById('lblSiteNameResult').textContent.replace(' ✅', '').replace(' (تایید شده) ⚠️', '');
  const siteName = (domain === 'SITE') ? siteNameText : domain;
  const subject = document.getElementById('txtSubject').value;
  const workPlace = document.getElementById('lblWorkPlaceResult').textContent;

  const rdoAttachYes = document.getElementById('rdoAttachYes');
  const hasAttachmentChoice = rdoAttachYes ? rdoAttachYes.checked : false;

  if (hasAttachmentChoice && !_selectedAttachmentFile) {
    alert('شما گزینه‌ی «دارای فایل پیوست» را انتخاب کرده‌اید، لطفاً فایل پیوست را انتخاب نمایید.');
    return;
  }
  if (hasAttachmentChoice && !_uploadedTempAttachmentId) {
    alert('لطفاً تا اتمام بارگذاری و تایید آنلاین فایل پیوست روی سرور صبور باشید.');
    return;
  }

  // Confirmation Alert
  const attachText = (hasAttachmentChoice && _selectedAttachmentFile) ? `📎 فایل پیوست: ${_selectedAttachmentFile.name} (آپلود شده ✅)` : '❌ فاقد فایل پیوست';
  const confirmMsg =
    `آیا از ثبت درخواست دان‌تایم زیر اطمینان دارید؟\n\n` +
    `📍 کد / حوزه: ${siteCode} (${siteName})\n` +
    `📑 موضوع: ${subject}\n` +
    `📅 تاریخ: از ${startDate} ساعت ${startTime} تا ${endDate} ساعت ${endTime}\n` +
    `👤 انجام‌دهنده: ${performer}\n` +
    `${attachText}\n\n` +
    `آیا تایید می‌فرمایید؟`;

  if (!confirm(confirmMsg)) {
    return;
  }

  try {
    const res = await fetch('/api/dt/add', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        siteCode, siteName, region: _siteRegion || '', domain, subject,
        startDate, startTime, endDate, endTime,
        applicant: (window.currentUser ? (window.currentUser.fullName || window.currentUser.username) : 'کاربر وب'),
        performer, workPlace, activity: '', inBsyHour: '0', note: '',
        tempAttachmentId: _uploadedTempAttachmentId,
        hasAttachedFile: (hasAttachmentChoice && _uploadedTempAttachmentId ? true : false)
      })
    });

    const result = await res.json();
    if (result.success) {
      if (hasAttachmentChoice && _selectedAttachmentFile) {
        alert(`✅ درخواست دان‌تایم (ردیف ${result.row}) و فایل پیوست «${_selectedAttachmentFile.name}» با موفقیت در سامانه ثبت گردید.`);
      } else {
        alert(`✅ درخواست دان‌تایم (ردیف ${result.row}) با موفقیت در سامانه ثبت گردید.`);
      }

      loadUserRecentDts();
      if (domain === 'SITE') {
        btnReset_Click();
      } else {
        closeDtRegModal();
      }
      if (typeof loadDT === 'function') {
        await loadDT(true);
      }
    } else {
      alert('خطا در ثبت درخواست: ' + result.error);
    }
  } catch (err) {
    console.error('btnSubmit_Click error:', err);
    alert('خطا در ارتباط با سرور.');
  }
}

async function loadUserRecentDts() {
  const tbody = document.getElementById('userRecentDtBody');
  if (!tbody) return;

  try {
    const res = await fetch('/api/dt/list?limit=15');
    const data = await res.json();

    if (data.success && Array.isArray(data.data) && data.data.length > 0) {
      let html = '';
      data.data.slice(0, 15).forEach(row => {
        html += `
          <tr>
            <td>${row['ردیف'] || ''}</td>
            <td><strong>${row['کد سایت'] || ''}</strong></td>
            <td>${row['نام سایت'] || '-'}</td>
            <td>${row['دامنه'] || '-'}</td>
            <td>${row['موضوع'] || ''}</td>
            <td>${row['تاریخ شروع'] || ''}</td>
            <td>${row['زمان شروع'] || ''}</td>
            <td>${row['تاریخ پایان'] || ''}</td>
            <td>${row['زمان پایان'] || ''}</td>
            <td><span class="status-badge" style="font-size:10px;">${row['وضعیت'] || ''}</span></td>
          </tr>
        `;
      });
      tbody.innerHTML = html;
    } else {
      tbody.innerHTML = '<tr><td colspan="10" class="text-center py-2 text-muted">سابقه ثبت‌شده‌ای یافت نشد.</td></tr>';
    }
  } catch (err) {
    console.error('loadUserRecentDts error:', err);
  }
}

function btnReset_Click() {
  // 1. Step 1: Reset Domain Radio
  const rdoSite = document.getElementById('rdoSite');
  if (rdoSite) {
    rdoSite.checked = true;
  }
  _siteRegion = '';

  // 2. Step 2: Clear Site Code & Name
  const txtSiteCode = document.getElementById('txtSiteCode');
  const lblSiteNameResult = document.getElementById('lblSiteNameResult');
  const btnNextSite = document.getElementById('btnNextSite');
  if (txtSiteCode) txtSiteCode.value = '';
  if (lblSiteNameResult) {
    lblSiteNameResult.textContent = '--------';
    lblSiteNameResult.style.color = 'var(--text-muted)';
  }
  _isSiteValid = false;
  if (btnNextSite) btnNextSite.disabled = true;

  // 3. Step 3: Reset Category & Clear Subject
  const rdoSiteAccess = document.getElementById('rdoSiteAccess');
  if (rdoSiteAccess) {
    rdoSiteAccess.checked = true;
    Category_CheckedChanged(rdoSiteAccess);
  }
  const txtSubject = document.getElementById('txtSubject');
  if (txtSubject) txtSubject.value = '';

  // 4. Step 4: Reset Dates & Times
  const todayShamsi = getShamsiTodayString();
  setSelectedShamsiDate('Start', todayShamsi);
  setSelectedShamsiDate('End', todayShamsi);

  const tmeStartHour = document.getElementById('tmeStartHour');
  const tmeStartMin = document.getElementById('tmeStartMin');
  const tmeEndHour = document.getElementById('tmeEndHour');
  const tmeEndMin = document.getElementById('tmeEndMin');
  if (tmeStartHour) tmeStartHour.value = '08';
  if (tmeStartMin) tmeStartMin.value = '00';
  if (tmeEndHour) tmeEndHour.value = '12';
  if (tmeEndMin) tmeEndMin.value = '00';

  // 5. Step 5: Reset Performer
  const sluePerformer = document.getElementById('sluePerformer');
  const lblWorkPlaceResult = document.getElementById('lblWorkPlaceResult');
  const lblPhoneResult = document.getElementById('lblPhoneResult');
  const btnSubmit = document.getElementById('btnSubmit');

  const btnNextDetails = document.getElementById('btnNextDetails');
  if (sluePerformer) sluePerformer.value = '';
  if (lblWorkPlaceResult) lblWorkPlaceResult.textContent = '---';
  if (lblPhoneResult) lblPhoneResult.textContent = '---';
  _selectedPerformer = null;
  if (btnNextDetails) btnNextDetails.disabled = true;
  if (btnSubmit) btnSubmit.disabled = true;

  // 6. Step 6: Reset Attachment State
  const rdoAttachNo = document.getElementById('rdoAttachNo');
  if (rdoAttachNo) rdoAttachNo.checked = true;
  toggleAttachmentBox(false);
  clearSelectedAttachment();

  // 7. Reset Step & Focus to Step 1 (Date)
  activateStep(1);
  const dtRegStartYear = document.getElementById('dtRegStartYear');
  if (dtRegStartYear) dtRegStartYear.focus();
}

function closeDtRegModal(event) {
  if (event && event.target && event.target.id !== 'dtRegModal' && !event.target.classList.contains('modal-close') && !event.target.classList.contains('btn-cancel')) {
    return;
  }
  const modal = document.getElementById('dtRegModal');
  if (modal) modal.style.display = 'none';
}

// =========================================================================
// SUBJECT & PERFORMER SEARCHABLE DROPDOWN WITH KEYBOARD NAVIGATION
// =========================================================================

let _subjectsList = [
  'مراجعه و بازدید از سایتهای موبایل',
  'Site Cleanup',
  'PM-Cleaning',
  'PM-Site',
  'PM-Tower',
  'رفع خرابی',
  'بهینه‌سازی',
  'قطعی کابل',
  'دستکاری تجهیزات',
  'سایر موارد'
];

let _subjectHighlightedIdx = -1;
let _performerHighlightedIdx = -1;

async function loadDtSubjectsList() {
  try {
    const res = await fetch('/api/dt/subjects');
    const data = await res.json();
    if (data.success && Array.isArray(data.subjects)) {
      _subjectsList = Array.from(new Set([..._subjectsList, ...data.subjects]));
    }
  } catch (e) {
    console.warn('loadDtSubjectsList error:', e);
  }
}

function openSubjectDropdown() {
  _subjectHighlightedIdx = -1;
  filterSubjectDropdown(document.getElementById('txtSubject').value);
}

function filterSubjectDropdown(query) {
  const dropdown = document.getElementById('txtSubjectDropdown');
  if (!dropdown) return;

  const q = (query || '').trim().toLowerCase();
  const filtered = _subjectsList.filter(s => s.toLowerCase().includes(q)).slice(0, 30);

  if (filtered.length === 0) {
    dropdown.innerHTML = '<div class="p-2 text-muted text-center" style="font-size:12px;">موضوعی یافت نشد.</div>';
  } else {
    let html = '';
    filtered.forEach((s, idx) => {
      html += `<div class="slue-item ${idx === _subjectHighlightedIdx ? 'autocomplete-item-active' : ''}" data-idx="${idx}" onclick="selectSubjectItem('${s.replace(/'/g, "\\'")}')">
        <span><strong>${s}</strong></span>
      </div>`;
    });
    dropdown.innerHTML = html;
  }
  dropdown.style.display = 'block';

  if (!window.subjectOutsideClickListener) {
    window.subjectOutsideClickListener = function(e) {
      if (!e.target.closest('.subject-search-wrapper')) {
        dropdown.style.display = 'none';
        _subjectHighlightedIdx = -1;
      }
    };
    document.addEventListener('click', window.subjectOutsideClickListener);
  }
}

function handleSubjectKeydown(e) {
  const dropdown = document.getElementById('txtSubjectDropdown');
  if (!dropdown) return;

  const items = dropdown.querySelectorAll('.slue-item');
  const count = items.length;

  if (e.key === 'ArrowDown') {
    e.preventDefault();
    if (dropdown.style.display === 'none') {
      openSubjectDropdown();
      return;
    }
    _subjectHighlightedIdx = (_subjectHighlightedIdx < count - 1) ? _subjectHighlightedIdx + 1 : 0;
    updateSubjectHighlight(items);
  } else if (e.key === 'ArrowUp') {
    e.preventDefault();
    if (dropdown.style.display === 'none') return;
    _subjectHighlightedIdx = (_subjectHighlightedIdx > 0) ? _subjectHighlightedIdx - 1 : count - 1;
    updateSubjectHighlight(items);
  } else if (e.key === 'Enter') {
    if (dropdown.style.display === 'block' && _subjectHighlightedIdx >= 0 && _subjectHighlightedIdx < count) {
      e.preventDefault();
      const val = items[_subjectHighlightedIdx].textContent.trim();
      selectSubjectItem(val);
      const perfEl = document.getElementById('sluePerformer');
      if (perfEl) perfEl.focus();
    }
  } else if (e.key === 'Escape') {
    dropdown.style.display = 'none';
    _subjectHighlightedIdx = -1;
  }
}

function updateSubjectHighlight(items) {
  items.forEach((item, i) => {
    if (i === _subjectHighlightedIdx) {
      item.style.background = 'rgba(2, 132, 199, 0.15)';
      item.style.color = '#0284c7';
      item.scrollIntoView({ block: 'nearest' });
    } else {
      item.style.background = '';
      item.style.color = '';
    }
  });
}

function selectSubjectItem(val) {
  document.getElementById('txtSubject').value = val;
  const dropdown = document.getElementById('txtSubjectDropdown');
  if (dropdown) dropdown.style.display = 'none';
  _subjectHighlightedIdx = -1;
}

function handlePerformerKeydown(e) {
  const dropdown = document.getElementById('sluePerformerDropdown');
  if (!dropdown) return;

  const items = dropdown.querySelectorAll('.slue-item');
  const count = items.length;

  if (e.key === 'ArrowDown') {
    e.preventDefault();
    if (dropdown.style.display === 'none') {
      openPerformerDropdown();
      return;
    }
    _performerHighlightedIdx = (_performerHighlightedIdx < count - 1) ? _performerHighlightedIdx + 1 : 0;
    updatePerformerHighlight(items);
  } else if (e.key === 'ArrowUp') {
    e.preventDefault();
    if (dropdown.style.display === 'none') return;
    _performerHighlightedIdx = (_performerHighlightedIdx > 0) ? _performerHighlightedIdx - 1 : count - 1;
    updatePerformerHighlight(items);
  } else if (e.key === 'Enter') {
    if (dropdown.style.display === 'block' && _performerHighlightedIdx >= 0 && _performerHighlightedIdx < count) {
      e.preventDefault();
      items[_performerHighlightedIdx].click();
      _performerHighlightedIdx = -1;
    }
  } else if (e.key === 'Escape') {
    dropdown.style.display = 'none';
    _performerHighlightedIdx = -1;
  }
}

function updatePerformerHighlight(items) {
  items.forEach((item, i) => {
    if (i === _performerHighlightedIdx) {
      item.style.background = 'rgba(2, 132, 199, 0.15)';
      item.style.color = '#0284c7';
      item.scrollIntoView({ block: 'nearest' });
    } else {
      item.style.background = '';
      item.style.color = '';
    }
  });
}

// Global Enter Key Step Progression (Bulletproof Step Navigation)
document.addEventListener('keydown', function(e) {
  const modal = document.getElementById('dtRegModal');
  if (!modal || modal.style.display === 'none') return;
  if (e.key !== 'Enter') return;

  const activeEl = document.activeElement;
  if (!activeEl) return;

  // STEP 1: DATES CARD
  if (activeEl.closest('#grpDates')) {
    e.preventDefault();
    btnNextDate_Click();
    return;
  }

  // STEP 2: DOMAIN CARD
  if (activeEl.closest('#grpDomain') || activeEl.name === 'rdoDomain' || _currentWizardStep === 2) {
    e.preventDefault();
    const checkedDomain = document.querySelector('input[name="rdoDomain"]:checked');
    if (!checkedDomain) {
      const defaultSiteRadio = document.querySelector('input[name="rdoDomain"][value="SITE"]');
      if (defaultSiteRadio) {
        defaultSiteRadio.checked = true;
        rdoDomain_EditValueChanged("SITE");
      }
    }
    btnNextDomain_Click();
    return;
  }

  // STEP 3: SITE CODE CARD
  if (activeEl.closest('#grpSiteCode')) {
    e.preventDefault();
    const rawText = document.getElementById('txtSiteCode').value.replace(/[^0-9]/g, '').trim();
    if (!rawText) {
      alert('لطفاً کد سایت را وارد کنید.');
    } else if (rawText.length < 4) {
      alert('کد سایت باید حتماً 4 رقم باشد.');
    } else {
      const btnNextSite = document.getElementById('btnNextSite');
      if (btnNextSite && !btnNextSite.disabled) {
        btnNextSite_Click();
      }
    }
    return;
  }

  // STEP 4: CATEGORY CARD
  if (activeEl.closest('#grpCategory')) {
    e.preventDefault();
    const btnNextCategory = document.getElementById('btnNextCategory');
    if (btnNextCategory && !btnNextCategory.disabled) {
      btnNextCategory_Click();
    }
    return;
  }

  // STEP 5: DETAILS CARD
  if (activeEl.id === 'tmeStartHour' || activeEl.id === 'tmeStartMin') {
    e.preventDefault();
    document.getElementById('tmeEndHour').focus();
  } else if (activeEl.id === 'tmeEndHour' || activeEl.id === 'tmeEndMin') {
    e.preventDefault();
    document.getElementById('txtSubject').focus();
  } else if (activeEl.id === 'txtSubject') {
    const dropdown = document.getElementById('txtSubjectDropdown');
    if (!dropdown || dropdown.style.display === 'none' || _subjectHighlightedIdx < 0) {
      e.preventDefault();
      document.getElementById('sluePerformer').focus();
    }
  } else if (activeEl.id === 'sluePerformer') {
    const dropdown = document.getElementById('sluePerformerDropdown');
    if (!dropdown || dropdown.style.display === 'none' || _performerHighlightedIdx < 0) {
      e.preventDefault();
      const btnSubmit = document.getElementById('btnSubmit');
      if (btnSubmit && !btnSubmit.disabled) {
        btnSubmit_Click();
      }
    }
  }
});

// Load Site Database Cache if needed

async function ensureSiteDbCache() {
  if (!window.cachedSiteMap) {
    window.cachedSiteMap = {};
    try {
      const res = await fetch('/api/db/list');
      const data = await res.json();
      if (data.data && Array.isArray(data.data)) {
        data.data.forEach(item => {
          const code = (item['کد سایت'] || item['siteCode'] || '').toString();
          const digits = code.replace(/[^0-9]/g, '').slice(-4);
          if (digits) {
            window.cachedSiteMap[digits] = {
              name: item['نام سایت'] || item['Site Name'] || item['siteName'] || '',
              region: item['منطقه'] || item['region'] || ''
            };
          }
        });
      }
    } catch (e) {
      console.warn('ensureSiteDbCache error:', e);
    }
  }
}

// Handle site code typing (4 digits logic matching WinForms)
async function handleSiteCodeInput(val) {
  const code = (val || '').trim();
  const siteNameInput = document.getElementById('dtRegSiteName');
  const regionInput = document.getElementById('dtRegRegion');
  const conflictAlert = document.getElementById('siteConflictAlert');

  if (!siteNameInput || !regionInput) return;

  const cleanDigits = code.replace(/[^0-9]/g, '');
  if (cleanDigits.length < 4) {
    siteNameInput.value = '-----------------------------------------------';
    regionInput.value = '';
    if (conflictAlert) conflictAlert.style.display = 'none';
    return;
  }

  const padded4 = cleanDigits.slice(-4);
  const fullSiteCode = 'ES' + padded4;

  // 1. Instant check against pre-fetched cache
  if (window.cachedSiteMap && window.cachedSiteMap[padded4]) {
    const cached = window.cachedSiteMap[padded4];
    siteNameInput.value = cached.name;
    regionInput.value = cached.region || 'نامشخص';
    return;
  }

  // 2. Network query fallback
  try {
    const res = await fetch(`/api/dt/site-info/${encodeURIComponent(fullSiteCode)}`);
    const data = await res.json();

    let foundName = (data.found && data.siteName) ? data.siteName : '';
    let foundRegion = (data.found && data.region) ? data.region : '';

    if (!foundName) {
      const dbSearchRes = await fetch(`/api/db/search?q=${encodeURIComponent(padded4)}`);
      const dbSearchData = await dbSearchRes.json();
      if (dbSearchData.success && dbSearchData.data && dbSearchData.data.length > 0) {
        const item = dbSearchData.data[0];
        foundName = item['نام سایت'] || item['Site Name'] || item['siteName'] || '';
        foundRegion = item['منطقه'] || item['region'] || '';
      }
    }

    if (foundName) {
      siteNameInput.value = foundName;
      regionInput.value = foundRegion || 'نامشخص';
      if (!window.cachedSiteMap) window.cachedSiteMap = {};
      window.cachedSiteMap[padded4] = { name: foundName, region: foundRegion };
    } else {
      siteNameInput.value = 'نامشخص';
      regionInput.value = 'نامشخص';
    }


    // Fallback 2: Direct lookup in dbData array if present
    if (!foundName && typeof rawDbData !== 'undefined' && Array.isArray(rawDbData)) {
      const match = rawDbData.find(d => {
        const c = (d['کد سایت'] || d['siteCode'] || '').toString();
        return c === fullSiteCode || c === padded4 || c.endsWith(padded4);
      });
      if (match) {
        foundName = match['نام سایت'] || '';
        foundRegion = match['منطقه'] || '';
      }
    }

    if (foundName) {
      siteNameInput.value = foundName;
      regionInput.value = foundRegion || 'نامشخص';
    } else {
      siteNameInput.value = 'نامشخص';
      regionInput.value = 'نامشخص';
    }



    // Check Active Conflicts
    if (conflictAlert) {
      if (data.activeDownTimes && data.activeDownTimes.length > 0) {
        let conflictHtml = `⚠️ <strong>هشدار تداخل:</strong> برای این سایت تعداد ${data.activeDownTimes.length} دان‌تایم فعال/درخواستی دیگر ثبت شده است:`;
        conflictHtml += '<ul style="margin:5px 0 0 15px; padding:0;">';
        data.activeDownTimes.forEach(dt => {
          conflictHtml += `<li>ردیف ${dt.row}: ${dt.subject} (از ${dt.startDate} ${dt.startTime} تا ${dt.endDate} ${dt.endTime}) - وضعیت: ${dt.status}</li>`;
        });
        conflictHtml += '</ul>';
        conflictAlert.innerHTML = conflictHtml;
        conflictAlert.style.display = 'block';
      } else {
        conflictAlert.style.display = 'none';
      }
    }
  } catch (err) {
    console.error('handleSiteCodeInput error:', err);
    siteNameInput.value = 'نامشخص';
    regionInput.value = 'نامشخص';
  }
}




// Dynamically update day options based on Shamsi Month (Months 1-6 = 31 days, 7-11 = 30 days, 12 = 29/30 days)
function updateShamsiDayOptions(prefix) {
  const monthVal = parseInt(document.getElementById(prefix + 'Month')?.value || '1', 10);
  const yearVal = parseInt(document.getElementById(prefix + 'Year')?.value || '1405', 10);
  const daySelect = document.getElementById(prefix + 'Day');
  if (!daySelect) return;

  let maxDays = 31;
  if (monthVal >= 7 && monthVal <= 11) {
    maxDays = 30;
  } else if (monthVal === 12) {
    // Esfand leap year check (1403 & 1407 are leap years)
    const isLeap = (yearVal % 33 === 1 || yearVal % 33 === 5 || yearVal % 33 === 9 || yearVal % 33 === 13 || yearVal % 33 === 17 || yearVal % 33 === 22 || yearVal % 33 === 26 || yearVal % 33 === 30);
    maxDays = isLeap ? 30 : 29;
  }

  const currentSelectedDay = parseInt(daySelect.value || '1', 10);
  daySelect.innerHTML = '';
  for (let i = 1; i <= maxDays; i++) {
    const dayStr = String(i).padStart(2, '0');
    const opt = document.createElement('option');
    opt.value = dayStr;
    opt.textContent = dayStr;
    if (i === Math.min(currentSelectedDay, maxDays)) {
      opt.selected = true;
    }
    daySelect.appendChild(opt);
  }
}

// Sync Shamsi Date from Year, Month, Day Dropdowns
function syncShamsiFromDropdowns() {
  updateShamsiDayOptions('dtRegStart');
  updateShamsiDayOptions('dtRegEnd');

  const sY = document.getElementById('dtRegStartYear')?.value || '1405';
  const sM = document.getElementById('dtRegStartMonth')?.value || '05';
  const sD = document.getElementById('dtRegStartDay')?.value || '13';
  if (document.getElementById('dtRegStartDate')) {
    document.getElementById('dtRegStartDate').value = `${sY}/${sM}/${sD}`;
  }

  const eY = document.getElementById('dtRegEndYear')?.value || '1405';
  const eM = document.getElementById('dtRegEndMonth')?.value || '05';
  const eD = document.getElementById('dtRegEndDay')?.value || '13';
  if (document.getElementById('dtRegEndDate')) {
    document.getElementById('dtRegEndDate').value = `${eY}/${eM}/${eD}`;
  }

  calculateOutageDuration();
}




// Convert Gregorian Picked Date to Shamsi String Format (yyyy/mm/dd)
function syncShamsiDate(fieldId, gDateVal) {
  if (!gDateVal) return;
  try {
    const d = new Date(gDateVal);
    const parts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit'
    }).formatToParts(d);

    const y = parts.find(p => p.type === 'year').value;
    const m = parts.find(p => p.type === 'month').value;
    const day = parts.find(p => p.type === 'day').value;

    const shamsiStr = `${y}/${m}/${day}`;
    const targetInput = document.getElementById(fieldId);
    if (targetInput) {
      targetInput.value = shamsiStr;
      calculateOutageDuration();
    }
  } catch (e) {
    console.warn('syncShamsiDate error:', e);
  }
}

// Toggle Visit Shift Presets (صبح / عصر)

function toggleVisitShift(shift) {
  const container = document.getElementById('visitPresetsContainer');
  if (!container) return;
  if (shift === 'sobh') {
    container.innerHTML = `
      <button type="button" class="btn-preset" onclick="applyTimePreset('07:00', '11:00')">07-11</button>
      <button type="button" class="btn-preset" onclick="applyTimePreset('08:00', '12:00')">08-12</button>
      <button type="button" class="btn-preset" onclick="applyTimePreset('09:00', '13:00')">09-13</button>
    `;
    applyTimePreset('07:00', '11:00');
  } else {
    container.innerHTML = `
      <button type="button" class="btn-preset" onclick="applyTimePreset('13:00', '17:00')">13-17</button>
      <button type="button" class="btn-preset" onclick="applyTimePreset('14:00', '18:00')">14-18</button>
      <button type="button" class="btn-preset" onclick="applyTimePreset('15:00', '19:00')">15-19</button>
    `;
    applyTimePreset('13:00', '17:00');
  }
}

// Sync Time Spinners (Hour & Min) to Hidden Time Values
function updateTimeFromSpinners() {
  const stH = String(document.getElementById('dtStHour').value || 0).padStart(2, '0');
  const stM = String(document.getElementById('dtStMin').value || 0).padStart(2, '0');
  const endH = String(document.getElementById('dtEndHour').value || 0).padStart(2, '0');
  const endM = String(document.getElementById('dtEndMin').value || 0).padStart(2, '0');

  document.getElementById('dtRegStartTime').value = `${stH}:${stM}`;
  document.getElementById('dtRegEndTime').value = `${endH}:${endM}`;
  calculateOutageDuration();
}

// Validate end date+time > start date+time on blur
function validateEndTimeAfterStart() {
  // First update hidden fields
  updateTimeFromSpinners();

  const startDate = (document.getElementById('dtRegStartDate').value || '').trim();
  const endDate = (document.getElementById('dtRegEndDate').value || '').trim();
  const startTime = (document.getElementById('dtRegStartTime').value || '').trim();
  const endTime = (document.getElementById('dtRegEndTime').value || '').trim();

  if (!startDate || !endDate || !startTime || !endTime) return;

  // Compare: startDate+startTime vs endDate+endTime
  const startFull = startDate + ' ' + startTime; // e.g. "1405/05/13 08:00"
  const endFull = endDate + ' ' + endTime;       // e.g. "1405/05/13 12:00"

  if (endFull <= startFull) {
    alert(`زمان پایان باید بزرگتر از زمان شروع باشد.\n\nشروع: ${startDate}  -  ${startTime}\nپایان: ${endDate}  -  ${endTime}`);
    document.getElementById('dtEndHour').focus();
  }
}

// Preset Buttons for Time Shortcuts
function applyTimePreset(st, et) {
  document.getElementById('dtRegStartTime').value = st;
  document.getElementById('dtRegEndTime').value = et;

  const [sH, sM] = st.split(':');
  const [eH, eM] = et.split(':');
  if (document.getElementById('dtStHour')) document.getElementById('dtStHour').value = parseInt(sH);
  if (document.getElementById('dtStMin')) document.getElementById('dtStMin').value = parseInt(sM);
  if (document.getElementById('dtEndHour')) document.getElementById('dtEndHour').value = parseInt(eH);
  if (document.getElementById('dtEndMin')) document.getElementById('dtEndMin').value = parseInt(eM);
}

// Select Outage Value
function selectOutageValue(val) {
  const txt = document.getElementById('txtSerchedOutage');
  if (txt) txt.value = ` ${val}  دقیقه`;
}

// Subject Change Handler
function handleSubjectChange(val) {
  if (val.startsWith('PM-')) {
    applyTimePreset('08:00', '14:00');
  }
}


// Calculate Outage Duration
function calculateOutageDuration() {
  const st = document.getElementById('dtRegStartTime').value;
  const et = document.getElementById('dtRegEndTime').value;
  const display = document.getElementById('outageDurationText');
  if (!display) return;

  if (!st || !et) {
    display.textContent = '---';
    return;
  }

  const [sH, sM] = st.split(':').map(Number);
  const [eH, eM] = et.split(':').map(Number);

  let startMinutes = sH * 60 + sM;
  let endMinutes = eH * 60 + eM;

  if (endMinutes < startMinutes) {
    endMinutes += 24 * 60; // Next day outage (e.g. 23:00 to 06:00)
  }

  const diffMinutes = endMinutes - startMinutes;
  const hours = Math.floor(diffMinutes / 60);
  const mins = diffMinutes % 60;

  let text = '';
  if (hours > 0) text += `${hours} ساعت `;
  if (mins > 0 || hours === 0) text += `${mins} دقیقه `;
  text += `(${diffMinutes} دقیقه)`;

  display.textContent = text;
}

// Search Performer in Phonebook
let phonebookDataCache = null;
async function searchPerformerPhonebook(query) {
  const dropdown = document.getElementById('performerDropdown');
  if (!dropdown) return;

  const q = (query || '').trim();

  try {
    if (!phonebookDataCache) {
      const res = await fetch('/api/tel/list');
      const responseData = await res.json();
      phonebookDataCache = Array.isArray(responseData) ? responseData : (responseData.data || []);
    }


    let filtered = phonebookDataCache;
    if (q.length > 0) {
      filtered = phonebookDataCache.filter(item => {
        const name = (item['نام و نام خانوادگی'] || item['Name'] || item['name'] || '').toString();
        const wp = (item['محل کار'] || item['WorkPlace'] || item['workPlace'] || '').toString();
        return name.includes(q) || wp.includes(q);
      });
    }
    filtered = filtered.slice(0, 30);

    if (filtered.length === 0) {
      dropdown.style.display = 'none';
      return;
    }

    let html = '';
    filtered.forEach(item => {
      const name = item['نام و نام خانوادگی'] || item['Name'] || item['name'] || '';
      const wp = item['محل کار'] || item['WorkPlace'] || item['workPlace'] || '';
      const tel = item['شماره تلفن'] || item['tel'] || '';
      html += `<div class="autocomplete-item" onclick="selectPerformerFromDropdown('${name.replace(/'/g, "\\'")}', '${wp.replace(/'/g, "\\'")}')">
                 <strong>${name}</strong>
                 <span style="color:var(--text-muted); font-size:10px;">${wp} ${tel ? '(' + tel + ')' : ''}</span>
               </div>`;
    });

    dropdown.innerHTML = html;
    dropdown.style.display = 'block';

    // Global listener to close dropdown on click outside
    if (!window.performerOutsideClickListener) {
      window.performerOutsideClickListener = function(evt) {
        const wrapper = document.querySelector('.performer-search-wrapper');
        if (wrapper && !wrapper.contains(evt.target)) {
          const dd = document.getElementById('performerDropdown');
          if (dd) dd.style.display = 'none';
        }
      };
      document.addEventListener('click', window.performerOutsideClickListener);
    }
  } catch (err) {
    console.error('searchPerformerPhonebook error:', err);
  }
}



function selectPerformerFromDropdown(name, workPlace) {
  document.getElementById('dtRegPerformer').value = name;
  if (workPlace) {
    document.getElementById('dtRegWorkPlace').value = workPlace;
  }
  const dropdown = document.getElementById('performerDropdown');
  if (dropdown) dropdown.style.display = 'none';
}

// Load Recent User Submissions into Modal Table
async function loadUserRecentDtSubmissions() {
  const tbody = document.getElementById('userRecentDtBody');
  if (!tbody) return;

  try {
    const res = await fetch('/api/dt/user-recent');
    const list = await res.json();

    if (!Array.isArray(list) || list.length === 0) {
      tbody.innerHTML = '<tr><td colspan="10" class="text-center py-2 text-muted">سابقه ثبت‌شده‌ای برای شما یافت نشد.</td></tr>';
      return;
    }

    let html = '';
    list.forEach(r => {
      html += `<tr>
        <td>${r.row}</td>
        <td><strong>${r.siteCode}</strong></td>
        <td>${r.siteName || '-'}</td>
        <td>${r.domain}</td>
        <td>${r.subject}</td>
        <td>${r.startDate}</td>
        <td>${r.startTime}</td>
        <td>${r.endDate}</td>
        <td>${r.endTime}</td>
        <td><span class="status-badge status-pending">${r.status}</span></td>
      </tr>`;
    });
    tbody.innerHTML = html;
  } catch (err) {
    console.error('loadUserRecentDtSubmissions error:', err);
    tbody.innerHTML = '<tr><td colspan="10" class="text-center py-2 text-danger">خطا در دریافت سوابق.</td></tr>';
  }
}

// Submit Downtime Registration Form
async function submitDtRegistration() {
  const codeVal = document.getElementById('dtRegCodeSite').value.trim();
  const siteName = document.getElementById('dtRegSiteName').value.trim();
  const region = document.getElementById('dtRegRegion').value.trim();
  const domainEl = document.querySelector('input[name="dtDomain"]:checked');
  const domain = domainEl ? domainEl.value : '2G';
  const subject = document.getElementById('dtRegSubject').value;
  const startDate = document.getElementById('dtRegStartDate').value.trim();
  const startTime = document.getElementById('dtRegStartTime').value;
  const endDate = document.getElementById('dtRegEndDate').value.trim();
  const endTime = document.getElementById('dtRegEndTime').value;
  const applicant = document.getElementById('dtRegApplicant').value.trim();
  const performer = document.getElementById('dtRegPerformer').value.trim();
  const workPlace = document.getElementById('dtRegWorkPlace').value.trim();
  const activity = document.getElementById('dtRegActivity').value.trim();
  const inBsyHour = document.getElementById('dtRegInBsyHour').value;
  const note = document.getElementById('dtRegNote').value.trim();

  // Validate Site Code (Must be exactly 4 digits)
  if (!codeVal || codeVal.length !== 4) {
    alert('تعداد ارقام کد سایت باید 4 عدد باشد. لطفا اصلاح فرمایید.');
    document.getElementById('dtRegCodeSite').focus();
    return;
  }

  // Full site code with ES prefix
  const siteCode = 'ES' + codeVal;

  if (!startDate || !endDate || !applicant || !performer) {
    alert('لطفاً تمامی فیلدهای ضروری (کد سایت، تاریخ شروع/پایان، درخواست‌کننده، انجام‌دهنده) را وارد نمایید.');
    return;
  }

  // Validate: subject must not be empty
  if (!subject || subject.trim() === '') {
    alert('لطفاً موضوع را انتخاب نمایید.');
    document.getElementById('dtRegSubject').focus();
    return;
  }

  // Validate: Start datetime must be >= server current datetime
  try {
    const timeRes = await fetch('/api/dt/server-time');
    const timeData = await timeRes.json();
    if (timeData.success) {
      const serverDate = timeData.shamsiDate; // e.g. "1405/05/13"
      const serverTime = timeData.time;       // e.g. "13:30"

      // Compare date first, then time
      if (startDate < serverDate || (startDate === serverDate && startTime < serverTime)) {
        alert(`زمان شروع نمی‌تواند قبل از زمان فعلی سرور باشد.\n\nزمان فعلی سرور: ${serverDate}  -  ${serverTime}\nزمان شروع انتخابی: ${startDate}  -  ${startTime}`);
        document.getElementById('dtStHour').focus();
        return;
      }
    }
  } catch (timeErr) {
    console.warn('Could not validate server time:', timeErr);
  }

  try {
    const res = await fetch('/api/dt/add', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        siteCode, siteName, region, domain, subject,
        startDate, startTime, endDate, endTime,
        applicant, performer, workPlace, activity,
        inBsyHour, note
      })
    });

    const result = await res.json();
    if (result.success) {
      alert('دان تایم با موفقیت ثبت گردید.');
      closeDtRegModal();
      loadDT(true);
    } else {
      alert('خطا در ثبت دان تایم: ' + result.error);
    }
  } catch (err) {
    console.error('submitDtRegistration error:', err);
    alert('خطا در ارسال اطلاعات به سرور');
  }
}

function exportDtToExcel() {
  alert('در حال آماده‌سازی فایل اکسل دان‌تایم‌ها...');
}

function copySelectedDt() {
  if (!selectedDtRow) {
    alert('لطفاً ابتدا یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }
  const rowData = rawDtData.find(r => r['ردیف'] === selectedDtRow);
  if (rowData) {
    const textToCopy = `ردیف: ${rowData['ردیف']} | کد سایت: ${rowData['کد سایت']} | نام سایت: ${rowData['نام سایت']} | موضوع: ${rowData['موضوع']} | زمان قطعی: ${rowData['زمان قطعی']} دقیقه`;
    if (navigator.clipboard) {
      navigator.clipboard.writeText(textToCopy);
    }
    alert(`اطلاعات دان‌تایم ردیف ${selectedDtRow} کپی شد.`);
  }
}

function uploadDtAttachment() {
  if (!selectedDtRow) {
    alert('لطفاً ابتدا یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }
  alert(`فرم آپلود فایل پیوست برای ردیف ${selectedDtRow} آماده شد.`);
}

function editSelectedDt() {
  if (!selectedDtRow) {
    alert('لطفاً ابتدا یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }
  alert(`فرم ویرایش و حذف برای ردیف ${selectedDtRow} باز شد.`);
}

function openCreateSimilarDtModal() {
  if (!selectedDtRow) {
    alert('لطفاً ابتدا یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }
  alert(`ایجاد دان‌تایم مشابه برای ردیف ${selectedDtRow} آماده گردید.`);
}

