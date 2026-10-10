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

  // Gradient from right (light/white Day Mode) to left (status color)
  return `background: linear-gradient(to left, #ffffff 0%, ${bg} 100%); color: ${fg};`;
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
    <div class="context-menu-item" onclick="onContextEditRow('edit')">
      <img src="/ico/edit1.png" width="16" height="16" alt="Edit">
      <span>✏️ ویرایش ردیفهای انتخابی</span>
    </div>
    <div class="context-menu-item" onclick="onContextEditRow('delete')">
      <span style="color:#ef4444; font-weight:bold; font-size:14px; margin-left:4px;">🗑️</span>
      <span style="color:#f87171;">حذف ردیفهای انتخابی</span>
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
    <div class="context-menu-item context-menu-cab" onclick="onContextCabSiteList()">
      <span class="cab-icon">📊</span>
      <span>تشکیل لیست سایتهای جلسه ی CAB</span>
    </div>
  `;

  menu.style.display = 'block';

  let x = event.clientX;
  let y = event.clientY;
  const menuWidth = 260;
  const menuHeight = 240;

  if (x + menuWidth > window.innerWidth) x = window.innerWidth - menuWidth - 10;
  if (y + menuHeight > window.innerHeight) y = window.innerHeight - menuHeight - 10;

  menu.style.left = `${x}px`;
  menu.style.top = `${y}px`;
}

function hideDtContextMenu() {
  const menu = document.getElementById('dtContextMenu');
  if (menu) menu.style.display = 'none';
}

function onContextEditRow(tab = 'edit') {
  hideDtContextMenu();
  openDtMultiChangeModal(tab);
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

/* CAB SITE LIST GENERATOR & EXCEL EXPORT (Ported from SiteMappingApp C#) */
async function onContextCabSiteList() {
  hideDtContextMenu();
  const selectedSetStrings = new Set(Array.from(selectedDtRows).map(v => String(v).trim()));

  if (selectedSetStrings.size === 0) {
    alert('!لطفاً حداقل یک سطر از جدول دان‌تایم را انتخاب کنید');
    return;
  }

  const siteCodes = [];
  selectedSetStrings.forEach(rNumStr => {
    const rowData = rawDtData.find(r => String(r['ردیف']).trim() === rNumStr);
    if (rowData && rowData['کد سایت']) {
      const cleaned = String(rowData['کد سایت']).replace(/XX/gi, '').trim();
      if (cleaned) {
        cleaned.split(/[,;\/\s]+/).forEach(part => {
          const s = part.trim().toUpperCase();
          if (s.length > 0) siteCodes.push(s);
        });
      }
    }
  });

  const distinctCodes = [...new Set(siteCodes)];
  if (distinctCodes.length === 0) {
    alert('!هیچ کد سایتی در سطرهای انتخابی یافت نشد');
    return;
  }

  const firstSite = distinctCodes[0];
  const joinedCodes = distinctCodes.join(', ');
  const confirmMsg = `تعداد ${distinctCodes.length} کد سایت بدون تکرار انتخاب شد:\n${joinedCodes}\n\nآیا مایل به تشکیل، ذخیره روی دسکتاپ و باز شدن فایل اکسل جلسه CAB هستید؟\n(نام فایل: Site List - ${firstSite}.xlsx)`;
  if (!confirm(confirmMsg)) {

    return;
  }

  try {
    const res = await fetch('/api/dt/cab-site-list', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ siteCodes: distinctCodes })
    });

    const data = await res.json().catch(() => ({}));
    if (!res.ok || !data.success) {
      throw new Error(data.error || 'خطا در ارتباط با سرور یا دیتابیس');
    }

    const successMsg = `✅ فایل اکسل جلسه CAB با موفقیت روی دسکتاپ ذخیره و در اکسل باز شد:\n${data.fileName}\n\nکدهای سایت پردازش‌شده (${distinctCodes.length} سایت):\n${joinedCodes}`;
    alert(successMsg);
  } catch (err) {
    console.error('Error generating CAB site list:', err);
    alert(`❌ خطا در تشکیل لیست سایت‌های CAB:\n${err.message}`);
  }
}




function renderDtTable() {
  const head = document.getElementById('dtTableHead');
  const body = document.getElementById('dtTableBody');
  const foot = document.getElementById('dtTableFoot');

  if (!head || !body) return;

  const columns = [
    { key: 'ردیف', label: 'ردیف', width: '46px' },
    { key: 'تاریخ ثبت', label: 'تاریخ<br>ثبت', width: '74px' },
    { key: 'ثبت', label: 'زمان<br>ثبت', width: '50px' },
    { key: 'کد سایت', label: 'کد سایت', width: '64px' },
    { key: 'نام سایت', label: 'نام سایت', width: '90px' },
    { key: 'منطقه', label: 'منطقه', width: '42px' },
    { key: 'تاریخ شروع', label: 'تاریخ<br>شروع', width: '74px' },
    { key: 'زمان شروع', label: 'زمان<br>شروع', width: '50px' },
    { key: 'تاریخ پایان', label: 'تاریخ<br>پایان', width: '74px' },
    { key: 'زمان پایان', label: 'زمان<br>پایان', width: '50px' },
    { key: 'زمان قطعی', label: 'زمان<br>قطعی', width: '50px' },
    { key: 'درخواست کننده', label: 'درخواست<br>کننده', width: '80px' },
    { key: 'موضوع', label: 'موضوع', width: '88px' },
    { key: 'انجام دهنده', label: 'انجام<br>دهنده', width: '80px' },
    { key: 'ثبت کننده', label: 'ثبت<br>کننده', width: '75px' },
    { key: 'شماره', label: 'شماره', width: '90px' },
    { key: 'وضعیت', label: 'وضعیت', width: '90px' },
    { key: 'فعالیت', label: 'فعالیت', width: '75px' },
    { key: 'یادداشت', label: 'یادداشت', width: '55px' }
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
    headerHtml += `<th onclick="sortDtColumn('${col.key}')" style="width:${col.width}; min-width:${col.width}; padding: 4px 2px; font-size: 10.5px; line-height: 1.2; cursor: pointer; user-select: none;" title="کلیک برای مرتب‌سازی ${col.key}"><div style="display:flex; align-items:center; justify-content:center; gap:2px; text-align:center;"><span>${col.label}</span>${sortIconSvg}</div></th>`;
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
      const cleanLabel = String(col.label || '').replace(/<[^>]*>/g, ' ').replace(/\s+/g, ' ').trim();
      const cellTitleAttr = plainTextVal ? `title="${cleanLabel}: ${plainTextVal.replace(/"/g, '&quot;')}"` : '';

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
        if (activeEl && activeEl.id === 'btnSubmit') {
          // Allow enter to trigger submit directly
          return;
        }

        e.preventDefault();
        e.stopPropagation();

        if (_currentWizardStep === 1 || (activeEl && (activeEl.tagName === 'SELECT' || (activeEl.id && activeEl.id.startsWith('dtReg')) || activeEl.id === 'btnNextDate' || activeEl.closest?.('#grpDates')))) {
          btnNextDate_Click();
        } else if (_currentWizardStep === 2 || (activeEl && (activeEl.name === 'rdoDomain' || activeEl.id === 'btnNextDomain' || activeEl.closest?.('#grpDomain')))) {
          btnNextDomain_Click();
        } else if (_currentWizardStep === 3 || (activeEl && (activeEl.id === 'txtSiteCode' || activeEl.id === 'btnNextSite' || activeEl.closest?.('#grpSiteCode')))) {
          btnNextSite_Click();
        } else if (_currentWizardStep === 4 || (activeEl && (activeEl.name === 'rdoCategory' || activeEl.id === 'btnNextCategory' || activeEl.closest?.('#grpCategory')))) {
          btnNextCategory_Click();
        } else if (_currentWizardStep === 5 || (activeEl && ((activeEl.id && activeEl.id.startsWith('tme')) || activeEl.id === 'txtOutage' || activeEl.id === 'btnNextTime' || activeEl.closest?.('#grpTimeAndOutage')))) {
          btnNextTime_Click();
        } else if (_currentWizardStep === 6 || (activeEl && (activeEl.id === 'txtSubject' || activeEl.id === 'sluePerformer' || activeEl.id === 'chkInBsyHour' || activeEl.id === 'btnNextDetails' || activeEl.closest?.('#grpDetailsAndPerformer')))) {
          btnNextDetails_Click();
        } else if (_currentWizardStep === 7 || (activeEl && (activeEl.name === 'rdoHasAttachment' || activeEl.name === 'rdoAttachmentChoice' || activeEl.id === 'btnNextAttachment' || activeEl.closest?.('#grpAttachment')))) {
          btnNextAttachment_Click();
        } else if (_currentWizardStep === 8 || (activeEl && (activeEl.id === 'btnSubmit' || activeEl.closest?.('#grpActions')))) {
          btnSubmit_Click();
        }
      }
    });
  }

  await initializeShamsiDatesToToday();
  await loadPerformersList();
  await loadDtSubjectsList();
  if (!window._similarLoadedRowData) {
    await btnReset_Click();
  }
  loadUserRecentDtSubmissions();
  checkActiveFreezeAnnouncement();
}

function closeDtRegModal(event) {
  if (event && event.target && event.target.id !== 'dtRegModal' && !event.target.classList.contains('modal-close') && !event.target.classList.contains('btn-action-close')) {
    return;
  }
  const modal = document.getElementById('dtRegModal');
  if (modal) {
    modal.style.display = 'none';
    modal.classList.remove('similar-theme');
  }
  window._similarLoadedRowData = null;
}

async function btnReset_Click() {
  const mainTitleEl = document.querySelector('.wizard-main-title');
  if (mainTitleEl && !window._similarLoadedRowData) {
    mainTitleEl.innerHTML = '<span style="color: #ffffff; font-weight: 900;">ثبت درخواست دان تایم</span>';
  }

  if (window._similarLoadedRowData) {
    populateFormFromSimilarRowData(window._similarLoadedRowData);
    return;
  }

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
  const btnNextDomain = document.getElementById('btnNextDomain');
  if (btnNextDomain) btnNextDomain.disabled = true;

  // Clear site code
  const txtSiteCode = document.getElementById('txtSiteCode');
  if (txtSiteCode) {
    txtSiteCode.value = '';
    txtSiteCode.disabled = false;
  }
  const lblSiteName = document.getElementById('lblSiteNameResult');
  if (lblSiteName) {
    lblSiteName.textContent = '--------';
    lblSiteName.style.color = 'var(--text-muted)';
  }
  const btnNextSite = document.getElementById('btnNextSite');
  if (btnNextSite) btnNextSite.disabled = true;

  // Clear category (Default to Site Access)
  const rdoAccess = document.getElementById('rdoSiteAccess');
  if (rdoAccess) {
    rdoAccess.checked = true;
    Category_CheckedChanged(rdoAccess);
  } else {
    const txtSubject = document.getElementById('txtSubject');
    if (txtSubject) txtSubject.value = '';
  }

  // Reset performer
  const sluePerformer = document.getElementById('sluePerformer');
  const lblWorkPlaceResult = document.getElementById('lblWorkPlaceResult');
  const lblPhoneResult = document.getElementById('lblPhoneResult');
  if (sluePerformer) sluePerformer.value = '';
  if (lblWorkPlaceResult) lblWorkPlaceResult.textContent = '---';
  if (lblPhoneResult) lblPhoneResult.textContent = '---';

  // Reset attachment
  const rdoAttachNo = document.getElementById('rdoAttachNo');
  if (rdoAttachNo) rdoAttachNo.checked = true;
  toggleAttachmentBox(false);
  clearSelectedAttachment();

  _isSiteValid = false;
  _selectedOutageMinutes = 0;
  _selectedMaxDuration = 0;
  _isCorrectingDateForStep5 = false;
  _selectedPerformer = null;

  updateSubmitButtonState();
  const dateAlert = document.getElementById('dateCorrectionAlert');
  if (dateAlert) dateAlert.style.display = 'none';

  _maxUnlockedStep = 1;
  activateStep(1, true);
}

let _maxUnlockedStep = 1;

function activateStep(stepNum, forceUnlock = false) {
  // Hard barrier: Box 3 must be filled and validated before moving to step 4 or higher
  if (stepNum > 3) {
    const rawDigits = (document.getElementById('txtSiteCode')?.value || '').replace(/[^0-9]/g, '').trim();
    if (rawDigits.length < 4 || !_isSiteValid) {
      _maxUnlockedStep = Math.min(_maxUnlockedStep, 3);
      if (_currentWizardStep !== 3) {
        _currentWizardStep = 3;
      }
      const txt = document.getElementById('txtSiteCode');
      if (txt) {
        txt.focus();
        txt.select();
      }
      return;
    }
  }

  if (forceUnlock) {
    _maxUnlockedStep = Math.max(_maxUnlockedStep, stepNum);
  }
  if (stepNum > _maxUnlockedStep) {
    return;
  }
  _currentWizardStep = stepNum;

  // Update Stepper Bar
  for (let i = 1; i <= 8; i++) {
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
    { num: 5, id: 'grpTimeAndOutage' },
    { num: 6, id: 'grpDetailsAndPerformer' },
    { num: 7, id: 'grpAttachment' },
    { num: 8, id: 'grpActions' }
  ];

  cards.forEach(c => {
    const el = document.getElementById(c.id);
    if (!el) return;
    if (c.num === stepNum) {
      el.classList.add('active-step-card');
      el.classList.remove('disabled-step-card');
      el.classList.remove('completed-step-card');
    } else if (c.num < stepNum) {
      el.classList.remove('active-step-card');
      el.classList.remove('disabled-step-card');
      el.classList.add('completed-step-card');
    } else {
      el.classList.remove('active-step-card');
      el.classList.add('disabled-step-card');
      el.classList.remove('completed-step-card');
    }
    el.onclick = null;
    const header = el.querySelector('.grp-card-header');
    if (header) {
      header.onclick = (e) => {
        if (c.num < _currentWizardStep && c.num <= _maxUnlockedStep) {
          jumpToWizardStep(c.num);
        }
      };
    }
  });

  updateStepControlsState(stepNum);

  // Control Box 6 phone row: hide while in Box 6 or earlier (stepNum <= 6), show only after exiting Box 6 (stepNum > 6)
  const rowPhone = document.getElementById('rowPerformerPhone');
  if (rowPhone) {
    rowPhone.style.display = (stepNum > 6) ? 'flex' : 'none';
  }

  if (stepNum === 1) {
    const startDay = document.getElementById('dtRegStartDay');
    if (startDay) startDay.focus();
  } else if (stepNum === 2) {
    const checkedDomain = document.querySelector('input[name="rdoDomain"]:checked') || document.querySelector('input[name="rdoDomain"][value="SITE"]');
    const btnNextDomain = document.getElementById('btnNextDomain');
    if (checkedDomain) {
      setTimeout(() => checkedDomain.focus(), 60);
    } else if (btnNextDomain) {
      setTimeout(() => btnNextDomain.focus(), 60);
    }
  } else if (stepNum === 3) {
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
  } else if (stepNum === 6) {
    const txtSubj = document.getElementById('txtSubject');
    if (txtSubj && !txtSubj.readOnly) {
      setTimeout(() => txtSubj.focus(), 60);
    } else {
      const perf = document.getElementById('sluePerformer');
      if (perf) setTimeout(() => perf.focus(), 60);
    }
  }
}

function updateStepControlsState(stepNum) {
  // 1. DATES (STEP 1)
  const isStep1 = (stepNum === 1);
  ['dtRegStartYear', 'dtRegStartMonth', 'dtRegStartDay', 'dtRegEndYear', 'dtRegEndMonth', 'dtRegEndDay'].forEach(id => {
    const el = document.getElementById(id);
    if (el) el.disabled = !isStep1;
  });
  const btnNextDate = document.getElementById('btnNextDate');
  if (btnNextDate) btnNextDate.disabled = !isStep1;

  // 2. DOMAIN (STEP 2)
  const isStep2 = (stepNum === 2);
  document.querySelectorAll('input[name="rdoDomain"]').forEach(r => {
    r.disabled = !isStep2;
  });
  const btnNextDomain = document.getElementById('btnNextDomain');
  if (btnNextDomain) {
    btnNextDomain.disabled = !isStep2 || !document.querySelector('input[name="rdoDomain"]:checked');
  }

  // 3. SITE CODE (STEP 3)
  const isStep3 = (stepNum === 3);
  const txtSiteCode = document.getElementById('txtSiteCode');
  if (txtSiteCode) {
    txtSiteCode.disabled = !isStep3;
  }
  const btnNextSite = document.getElementById('btnNextSite');
  if (btnNextSite) {
    const rawDigits = (txtSiteCode?.value || '').replace(/[^0-9]/g, '');
    btnNextSite.disabled = !isStep3 || rawDigits.length < 4 || !_isSiteValid;
  }

  // 4. CATEGORY (STEP 4)
  const isStep4 = (stepNum === 4);
  document.querySelectorAll('input[name="rdoCategory"]').forEach(r => {
    r.disabled = !isStep4;
  });
  const btnNextCategory = document.getElementById('btnNextCategory');
  if (btnNextCategory) {
    btnNextCategory.disabled = !isStep4 || !document.querySelector('input[name="rdoCategory"]:checked');
  }

  // 5. TIME & OUTAGE (STEP 5)
  const isStep5 = (stepNum === 5);
  ['tmeStartHour', 'tmeStartMin', 'tmeEndHour', 'tmeEndMin', 'txtOutage'].forEach(id => {
    const el = document.getElementById(id);
    if (el) el.disabled = !isStep5;
  });
  const btnNextTime = document.getElementById('btnNextTime');
  if (btnNextTime) btnNextTime.disabled = !isStep5;

  // 6. DETAILS (STEP 6)
  const isStep6 = (stepNum === 6);
  ['txtSubject', 'sluePerformer'].forEach(id => {
    const el = document.getElementById(id);
    if (el) {
      if (id === 'txtSubject' && document.getElementById('rdoSiteAccess')?.checked) {
        el.disabled = !isStep6;
        el.readOnly = true;
      } else {
        el.disabled = !isStep6;
      }
    }
  });
  const btnNextDetails = document.getElementById('btnNextDetails');
  if (btnNextDetails) {
    const hasSubj = !!(document.getElementById('txtSubject')?.value?.trim());
    const hasPerf = !!(document.getElementById('sluePerformer')?.value?.trim());
    btnNextDetails.disabled = !isStep6 || !(hasSubj && hasPerf);
  }

  // 7. ATTACHMENT (STEP 7)
  const isStep7 = (stepNum === 7);
  document.querySelectorAll('input[name="rdoHasAttachment"]').forEach(r => {
    r.disabled = !isStep7 && stepNum < 7;
  });
  const dtRegAttachmentInput = document.getElementById('dtRegAttachmentInput');
  if (dtRegAttachmentInput) dtRegAttachmentInput.disabled = !isStep7 && stepNum < 7;
  const btnNextAttachment = document.getElementById('btnNextAttachment');
  if (btnNextAttachment) btnNextAttachment.disabled = !isStep7;

  // 8. ACTIONS / REGISTER (STEP 8)
  updateSubmitButtonState();
}

function checkSubmitEligibility() {
  const domainEl = document.querySelector('input[name="rdoDomain"]:checked');
  if (!domainEl) return false;

  const siteRaw = (document.getElementById('txtSiteCode')?.value || '').replace(/[^0-9]/g, '').trim();
  if (siteRaw.length < 4 || !_isSiteValid) return false;

  const catEl = document.querySelector('input[name="rdoCategory"]:checked');
  if (!catEl) return false;

  const subject = (document.getElementById('txtSubject')?.value || '').trim();
  if (!subject) return false;

  const performer = (document.getElementById('sluePerformer')?.value || '').trim();
  if (!performer) return false;

  return true;
}

function updateSubmitButtonState() {
  const btnSubmit = document.getElementById('btnSubmit');
  if (!btnSubmit) return;

  const isEligible = checkSubmitEligibility();
  if (isEligible) {
    btnSubmit.classList.remove('btn-action-submit-disabled');
    btnSubmit.style.opacity = '1';
    btnSubmit.style.filter = 'none';
    btnSubmit.title = 'ثبت نهایی درخواست دان‌تایم';
  } else {
    btnSubmit.classList.add('btn-action-submit-disabled');
    btnSubmit.style.opacity = '0.65';
    btnSubmit.style.filter = 'grayscale(0.2)';
    btnSubmit.title = 'لطفاً اطلاعات الزامی فرم را تکمیل فرمایید';
  }
  // Keep button interactive so clicks always give instant feedback or submit!
  btnSubmit.disabled = false;
}

function jumpToWizardStep(stepNum) {
  if (stepNum > 3) {
    const rawDigits = (document.getElementById('txtSiteCode')?.value || '').replace(/[^0-9]/g, '').trim();
    if (rawDigits.length < 4 || !_isSiteValid) {
      alert('برای ورود به مرحله بعد، ابتدا باید کد ۴ رقمی سایت در مرحله ۳ تکمیل و تأیید شود.');
      activateStep(3, false);
      const txt = document.getElementById('txtSiteCode');
      if (txt) { txt.focus(); txt.select(); }
      return;
    }
  }
  if (stepNum <= _maxUnlockedStep) {
    activateStep(stepNum, false);
  }
}

function handleSiteCodeKeydown(e) {
  if (e.key === 'Enter') {
    e.preventDefault();
    e.stopPropagation();
    btnNextSite_Click();
  } else if (e.key === 'Tab' && !e.shiftKey) {
    const rawDigits = (document.getElementById('txtSiteCode')?.value || '').replace(/[^0-9]/g, '').trim();
    if (rawDigits.length < 4 || !_isSiteValid) {
      e.preventDefault();
      e.stopPropagation();
      alert('لطفاً ابتدا کد ۴ رقمی سایت در مرحله ۳ را وارد و تأیید فرمایید.');
      const txt = document.getElementById('txtSiteCode');
      if (txt) { txt.focus(); txt.select(); }
    }
  }
}
window.handleSiteCodeKeydown = handleSiteCodeKeydown;

function isOnlyNumberKey(evt) {
  if (!evt) return true;
  const charCode = (evt.which) ? evt.which : evt.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57)) {
    return false;
  }
  return true;
}
window.isOnlyNumberKey = isOnlyNumberKey;

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

function btnNextDate_Click(e) {
  if (e && e.stopPropagation) e.stopPropagation();
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

  let domainRadio = document.querySelector('input[name="rdoDomain"]:checked');
  if (!domainRadio) {
    domainRadio = document.querySelector('input[name="rdoDomain"][value="SITE"]');
    if (domainRadio) {
      domainRadio.checked = true;
    }
  }
  if (domainRadio) {
    rdoDomain_EditValueChanged(domainRadio.value);
  }

  activateStep(2, true);
}

function rdoDomain_EditValueChanged(val, fromUserClick = false) {
  const btnNextDomain = document.getElementById('btnNextDomain');
  if (btnNextDomain) btnNextDomain.disabled = !val;
  const txtSiteCode = document.getElementById('txtSiteCode');

  if (txtSiteCode) {
    txtSiteCode.disabled = false;
    txtSiteCode.style.background = '#ffffff';
  }

  if (fromUserClick) {
    activateStep(3, true);
    if (txtSiteCode) {
      setTimeout(() => { txtSiteCode.focus(); txtSiteCode.select(); }, 60);
    }
  }
  updateSubmitButtonState();
}

function btnNextDomain_Click() {
  let domainEl = document.querySelector('input[name="rdoDomain"]:checked');
  if (!domainEl) {
    const siteRadio = document.querySelector('input[name="rdoDomain"][value="SITE"]');
    if (siteRadio) {
      siteRadio.checked = true;
      rdoDomain_EditValueChanged('SITE', false);
      domainEl = siteRadio;
    }
  }

  activateStep(3, true);
  const txt = document.getElementById('txtSiteCode');
  if (txt) {
    setTimeout(() => { txt.focus(); txt.select(); }, 60);
  }
  updateSubmitButtonState();
}

function txtSiteCode_TextChanged(val) {
  const cleanDigits = (val || '').replace(/[^0-9]/g, '').trim();
  const lblSiteNameResult = document.getElementById('lblSiteNameResult');
  const btnNextSite = document.getElementById('btnNextSite');

  if (cleanDigits.length === 4) {
    if (btnNextSite) btnNextSite.disabled = false;
    performSiteLookup(cleanDigits);
  } else {
    _isSiteValid = false;
    if (lblSiteNameResult) {
      lblSiteNameResult.textContent = '--------';
      lblSiteNameResult.style.color = 'var(--text-muted)';
    }
    if (btnNextSite) btnNextSite.disabled = true;
    updateSubmitButtonState();
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
      if (lblSiteNameResult) {
        lblSiteNameResult.textContent = data.siteName + ' ✅';
        lblSiteNameResult.style.color = '#16a34a';
      }
      if (btnNextSite) btnNextSite.disabled = false;

      // Auto progression: 3 -> 4
      activateStep(4, true);
      const rdoAccess = document.getElementById('rdoSiteAccess');
      if (rdoAccess) {
        rdoAccess.checked = true;
        Category_CheckedChanged(rdoAccess, false);
      }
      updateSubmitButtonState();
    } else {
      const confirmContinue = confirm('این کد سایت در دیتابیس موجود نیست آیا ادامه می دهید؟');
      if (confirmContinue) {
        _isSiteValid = true;
        _siteRegion = '';
        if (lblSiteNameResult) {
          lblSiteNameResult.textContent = 'نامشخص (تایید شده) ⚠️';
          lblSiteNameResult.style.color = '#ea580c';
        }
        if (btnNextSite) btnNextSite.disabled = false;

        // Auto progression: 3 -> 4
        activateStep(4, true);
        const rdoAccess = document.getElementById('rdoSiteAccess');
        if (rdoAccess) {
          rdoAccess.checked = true;
          Category_CheckedChanged(rdoAccess, false);
        }
        updateSubmitButtonState();
      } else {
        _isSiteValid = false;
        _siteRegion = '';
        if (lblSiteNameResult) {
          lblSiteNameResult.textContent = '--------';
          lblSiteNameResult.style.color = '#dc2626';
        }
        if (btnNextSite) btnNextSite.disabled = true;
        const txt = document.getElementById('txtSiteCode');
        if (txt) {
          txt.value = '';
          txt.focus();
        }
        updateSubmitButtonState();
      }
    }
  } catch (err) {
    console.error('Site lookup error:', err);
  }
}

async function btnNextSite_Click() {
  const rawText = (document.getElementById('txtSiteCode')?.value || '').replace(/[^0-9]/g, '').trim();
  if (!rawText) {
    alert('لطفاً کد ۴ رقمی سایت را وارد کنید.');
    const txt = document.getElementById('txtSiteCode');
    if (txt) { txt.focus(); txt.select(); }
    return;
  }
  if (rawText.length < 4) {
    alert('کد سایت باید حتماً ۴ رقم باشد.');
    const txt = document.getElementById('txtSiteCode');
    if (txt) { txt.focus(); txt.select(); }
    return;
  }
  if (!_isSiteValid) {
    await performSiteLookup(rawText);
    if (!_isSiteValid) return;
  }

  activateStep(4, true);
  const rdoAccess = document.getElementById('rdoSiteAccess');
  if (rdoAccess) {
    rdoAccess.checked = true;
    Category_CheckedChanged(rdoAccess, false);
    rdoAccess.focus();
  }
  updateSubmitButtonState();
}

function Category_CheckedChanged(el, fromUserClick = false) {
  if (!el || !el.checked) return;

  const btnNextCategory = document.getElementById('btnNextCategory');
  if (btnNextCategory) btnNextCategory.disabled = false;

  const txtOutage = document.getElementById('txtOutage');
  const txtSubject = document.getElementById('txtSubject');

  if (el.id === 'rdoSiteAccess') {
    _selectedOutageMinutes = 0;
    _selectedMaxDuration = 4;
    if (txtOutage) {
      txtOutage.readOnly = false;
      if (txtOutage.value === '' || txtOutage.value.includes('Outage')) txtOutage.value = '0';
    }
    if (txtSubject) {
      txtSubject.value = 'مراجعه و بازدید از سایتهای موبایل';
      txtSubject.readOnly = true;
      txtSubject.style.backgroundColor = 'var(--bg-muted, #f1f5f9)';
      txtSubject.style.cursor = 'not-allowed';
      txtSubject.title = 'موضوع برای Site Access به صورت خودکار و فیکس تعیین شده است';
    }
  } else if (el.id === 'rdoSiteCleanup') {
    _selectedOutageMinutes = 60;
    _selectedMaxDuration = 6;
    if (txtOutage) {
      txtOutage.readOnly = false;
      if (txtOutage.value === '' || txtOutage.value.includes('Outage')) txtOutage.value = '60';
    }
    if (txtSubject) {
      if (txtSubject.value === 'مراجعه و بازدید از سایتهای موبایل') txtSubject.value = '';
      txtSubject.readOnly = false;
      txtSubject.style.backgroundColor = '';
      txtSubject.style.cursor = '';
      txtSubject.title = '';
      txtSubject.placeholder = 'جستجو و انتخاب موضوع...';
    }
  } else {
    _selectedOutageMinutes = -1;
    _selectedMaxDuration = 0;
    if (txtOutage) {
      txtOutage.readOnly = false;
      if (txtOutage.value.includes('Outage')) txtOutage.value = '0';
      txtOutage.placeholder = 'دقایق Outage (مثلاً 0 یا 30)';
    }
    if (txtSubject) {
      if (txtSubject.value === 'مراجعه و بازدید از سایتهای موبایل') txtSubject.value = '';
      txtSubject.readOnly = false;
      txtSubject.style.backgroundColor = '';
      txtSubject.style.cursor = '';
      txtSubject.title = '';
      txtSubject.placeholder = 'جستجو و انتخاب موضوع...';
    }
  }

  updateSubmitButtonState();

  if (fromUserClick) {
    btnNextCategory_Click();
  }
}

function btnNextCategory_Click() {
  const rawDigits = (document.getElementById('txtSiteCode')?.value || '').replace(/[^0-9]/g, '').trim();
  if (rawDigits.length < 4 || !_isSiteValid) {
    alert('لطفاً ابتدا کد ۴ رقمی سایت در مرحله ۳ را تکمیل و تأیید نمایید.');
    activateStep(3, false);
    const txt = document.getElementById('txtSiteCode');
    if (txt) { txt.focus(); txt.select(); }
    return;
  }

  const startDate = getSelectedShamsiDate('Start');
  const endDate = getSelectedShamsiDate('End');

  const catAccess = document.getElementById('rdoSiteAccess');
  const catCleanup = document.getElementById('rdoSiteCleanup');
  const isAccessOrCleanup = (catAccess && catAccess.checked) || (catCleanup && catCleanup.checked);

  // Vital Check: Start and End Dates MUST be on the same day for Site Access and Site Cleanup
  if (isAccessOrCleanup && startDate !== endDate) {
    const catName = (catAccess && catAccess.checked) ? "Site Access" : "Site Cleanup";
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
  updateSubmitButtonState();
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
  
  updateSubmitButtonState();

  const subject = (document.getElementById('txtSubject')?.value || '').trim();
  if (subject) {
    activateStep(7, true);
    updateSubmitButtonState();
  }
}

function btnNextTime_Click() {
  const startDate = getSelectedShamsiDate('Start');
  const endDate = getSelectedShamsiDate('End');
  
  const stH = parseInt(document.getElementById('tmeStartHour')?.value) || 0;
  const stM = parseInt(document.getElementById('tmeStartMin')?.value) || 0;
  const endH = parseInt(document.getElementById('tmeEndHour')?.value) || 0;
  const endM = parseInt(document.getElementById('tmeEndMin')?.value) || 0;

  const startMinutesTotal = stH * 60 + stM;
  const endMinutesTotal = endH * 60 + endM;

  if (startDate === endDate && endMinutesTotal <= startMinutesTotal) {
    alert('زمان پایان باید حتماً پس از زمان شروع باشد.');
    return;
  }

  activateStep(6, true);
  updateSubmitButtonState();

  const txtSubj = document.getElementById('txtSubject');
  const sluePerf = document.getElementById('sluePerformer');
  setTimeout(() => {
    if (txtSubj && txtSubj.value.trim() && txtSubj.readOnly) {
      if (sluePerf) { sluePerf.focus(); }
    } else if (txtSubj) {
      txtSubj.focus();
    }
  }, 60);
}
window.btnNextTime_Click = btnNextTime_Click;

function btnNextDetails_Click() {
  const subject = (document.getElementById('txtSubject')?.value || '').trim();
  const performer = (document.getElementById('sluePerformer')?.value || '').trim();

  if (!subject) {
    alert('لطفاً موضوع درخواست را انتخاب نمایید.');
    document.getElementById('txtSubject')?.focus();
    return;
  }
  if (!performer) {
    alert('لطفاً انجام دهنده را انتخاب نمایید.');
    document.getElementById('sluePerformer')?.focus();
    return;
  }

  activateStep(7, true);
  updateSubmitButtonState();
}

function btnNextAttachment_Click() {
  activateStep(8, true);
  updateSubmitButtonState();
  const btnSubmit = document.getElementById('btnSubmit');
  if (btnSubmit) btnSubmit.focus();
}
window.btnNextAttachment_Click = btnNextAttachment_Click;

async function btnSubmit_Click() {
  const btnSubmit = document.getElementById('btnSubmit');

  try {
    // 1. Check Domain
    let domainEl = document.querySelector('input[name="rdoDomain"]:checked');
    if (!domainEl) {
      const siteRadio = document.querySelector('input[name="rdoDomain"][value="SITE"]');
      if (siteRadio) {
        siteRadio.checked = true;
        domainEl = siteRadio;
        rdoDomain_EditValueChanged('SITE', false);
      } else {
        alert('لطفاً ابتدا در مرحله ۲، دامنه را انتخاب فرمایید.');
        activateStep(2, true);
        return;
      }
    }
    const domain = domainEl.value;

    // 2. Check Site Code
    const siteCodeRaw = (document.getElementById('txtSiteCode')?.value || '').replace(/[^0-9]/g, '').trim();
    if (!siteCodeRaw || siteCodeRaw.length < 4) {
      alert('لطفاً در مرحله ۳، کد ۴ رقمی سایت را وارد فرمایید.');
      activateStep(3, true);
      const txt = document.getElementById('txtSiteCode');
      if (txt) { txt.focus(); txt.select(); }
      return;
    }
    if (!_isSiteValid) {
      await performSiteLookup(siteCodeRaw);
      if (!_isSiteValid) {
        alert('کد سایت در پایگاه داده تأیید نشده است.');
        activateStep(3, true);
        return;
      }
    }

    // 3. Check Category
    let catEl = document.querySelector('input[name="rdoCategory"]:checked');
    if (!catEl) {
      const rdoAccess = document.getElementById('rdoSiteAccess');
      if (rdoAccess) {
        rdoAccess.checked = true;
        Category_CheckedChanged(rdoAccess, false);
        catEl = rdoAccess;
      } else {
        alert('لطفاً در مرحله ۴، نوع دسته‌بندی را انتخاب فرمایید.');
        activateStep(4, true);
        return;
      }
    }

    // 4. Check Subject
    const subject = (document.getElementById('txtSubject')?.value || '').trim();
    if (!subject) {
      alert('لطفاً در مرحله ۶، موضوع درخواست را مشخص فرمایید.');
      activateStep(6, true);
      document.getElementById('txtSubject')?.focus();
      return;
    }

    // 5. Check Performer
    let performer = (document.getElementById('sluePerformer')?.value || '').trim();
    if (!performer) {
      alert('لطفاً در مرحله ۶، شخص انجام‌دهنده را از لیست مشخص فرمایید.');
      activateStep(6, true);
      document.getElementById('sluePerformer')?.focus();
      return;
    }

    const startDate = getSelectedShamsiDate('Start');
    const endDate = getSelectedShamsiDate('End');
    
    // NEW FINAL VALIDATION: Site Access and Site Cleanup must be on the same day
    if (catEl && (catEl.id === 'rdoSiteAccess' || catEl.id === 'rdoSiteCleanup' || catEl.value === 'Site Access' || catEl.value === 'Site Cleanup')) {
      if (startDate !== endDate) {
        const catName = catEl.value || "Site Access";
        alert(`چون درخواست شما از نوع « ${catName} » می‌باشد،\nتاریخ شروع و پایان باید در یک روز باشد.\nلطفاً تاریخ درخواست خود را اصلاح نمایید.`);
        activateStep(1, true); // هدایت به مرحله تاریخ
        return;
      }
    }

    const stH = (document.getElementById('tmeStartHour')?.value || '08').padStart(2, '0');
    const stM = (document.getElementById('tmeStartMin')?.value || '00').padStart(2, '0');
    const endH = (document.getElementById('tmeEndHour')?.value || '09').padStart(2, '0');
    const endM = (document.getElementById('tmeEndMin')?.value || '00').padStart(2, '0');

    // NEW FINAL VALIDATION: Start time cannot be before 06:00
    if (parseInt(stH) < 6) {
      alert('زمان شروع نمی‌تواند قبل از ساعت 06:00 صبح باشد.\nلطفاً زمان را اصلاح کنید.');
      activateStep(5, true); // هدایت به مرحله ساعت
      return;
    }

    const startTime = `${stH}:${stM}`;
    const endTime = `${endH}:${endM}`;

    const startMinutesTotal = parseInt(stH) * 60 + parseInt(stM);
    const endMinutesTotal = parseInt(endH) * 60 + parseInt(endM);

    if (startDate === endDate && endMinutesTotal <= startMinutesTotal) {
      alert('زمان پایان باید حتماً پس از زمان شروع باشد.');
      activateStep(5, true);
      return;
    }

    // Max Duration Check (bypassed in similar mode)
    const isSameDay = (startDate === endDate);
    // If not on same day, we assume it's > 24 hours to enforce max duration properly
    const totalHours = isSameDay ? ((endMinutesTotal - startMinutesTotal) / 60) : (24 + (endMinutesTotal - startMinutesTotal) / 60);

    if (!window._similarLoadedRowData && _selectedMaxDuration > 0) {
      if (!isSameDay || totalHours > _selectedMaxDuration) {
        alert(`در دسته‌بندی دان‌تایمی که شما انتخاب کردید نباید کل زمان دان‌تایم بیش از ${_selectedMaxDuration} ساعت باشد.\nلطفاً زمان را اصلاح کنید.`);
        activateStep(5, true);
        return;
      }
    }

    const phoneVal = (document.getElementById('lblPhoneResult')?.textContent || '').trim();
    if (phoneVal && phoneVal !== '---' && !performer.includes('-')) {
      performer = `${performer} - ${phoneVal}`;
    }

    const siteCode = 'ES' + siteCodeRaw.padStart(4, '0');
    const siteNameText = (document.getElementById('lblSiteNameResult')?.textContent || '').replace(' ✅', '').replace(' (تایید شده) ⚠️', '').trim();
    const siteName = siteNameText || domain;
    const workPlace = (document.getElementById('lblWorkPlaceResult')?.textContent || '').replace('---', '').trim();

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

    if (btnSubmit) {
      btnSubmit.disabled = true;
      btnSubmit.innerHTML = '<span>در حال ثبت اطلاعات... ⏳</span>';
    }

    const res = await fetch('/api/dt/add', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        siteCode, siteName, region: _siteRegion || '', domain, subject,
        startDate, startTime, endDate, endTime,
        outageMin: document.getElementById('txtOutage')?.value || '0',
        applicant: (typeof currentUser !== 'undefined' && currentUser ? (currentUser.fullName || currentUser.username) : 'کاربر وب'),
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

      await loadUserRecentDts();
      await btnReset_Click();
      if (typeof loadDT === 'function') {
        loadDT(true);
      }
      const recentContainer = document.getElementById('userRecentDtContainer');
      if (recentContainer && recentContainer.style.display !== 'none') {
        setTimeout(() => {
          recentContainer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }, 100);
      }
    } else {
      alert('خطا در ثبت درخواست: ' + (result.error || 'پاسخ ناموفق از سرور'));
    }
  } catch (err) {
    console.error('btnSubmit_Click error:', err);
    alert('خطا در فرآیند ثبت درخواست: ' + err.message);
  } finally {
    const btnSubmit = document.getElementById('btnSubmit');
    if (btnSubmit) {
      btnSubmit.innerHTML = '<span class="action-btn-icon">✅</span><span>ثبت</span>';
      updateSubmitButtonState();
    }
  }
}
window.btnSubmit_Click = btnSubmit_Click;

async function loadUserRecentDts() {
  await loadUserRecentDtSubmissions();
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
  const rdoAccess = document.getElementById('rdoSiteAccess');
  const txtSub = document.getElementById('txtSubject');
  if ((rdoAccess && rdoAccess.checked) || (txtSub && txtSub.readOnly)) {
    return;
  }
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
  if (activeEl.closest('#grpSiteCode') || _currentWizardStep === 3) {
    e.preventDefault();
    btnNextSite_Click();
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

  // STEP 5: TIME CARD
  if (activeEl.closest('#grpTimeAndOutage') || _currentWizardStep === 5) {
    if (activeEl.id === 'tmeStartHour' || activeEl.id === 'tmeStartMin') {
      e.preventDefault();
      document.getElementById('tmeEndHour')?.focus();
      return;
    }
    e.preventDefault();
    btnNextTime_Click();
    return;
  }

  // STEP 6: DETAILS CARD
  if (activeEl.closest('#grpDetailsAndPerformer') || _currentWizardStep === 6) {
    if (activeEl.id === 'txtSubject') {
      const dropdown = document.getElementById('txtSubjectDropdown');
      if (!dropdown || dropdown.style.display === 'none' || _subjectHighlightedIdx < 0) {
        e.preventDefault();
        document.getElementById('sluePerformer')?.focus();
        return;
      }
    } else if (activeEl.id === 'sluePerformer') {
      const dropdown = document.getElementById('sluePerformerDropdown');
      if (!dropdown || dropdown.style.display === 'none' || _performerHighlightedIdx < 0) {
        e.preventDefault();
        btnNextDetails_Click();
        return;
      }
    } else {
      e.preventDefault();
      btnNextDetails_Click();
      return;
    }
  }

  // STEP 7: ATTACHMENT CARD
  if (activeEl.closest('#grpAttachment') || _currentWizardStep === 7) {
    e.preventDefault();
    btnNextAttachment_Click();
    return;
  }

  // STEP 8: REGISTER CARD
  if (activeEl.closest('#grpActions') || _currentWizardStep === 8) {
    e.preventDefault();
    btnSubmit_Click();
    return;
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
  setOutagePreset(val);
}

function validateOutageStep(inputEl) {
  if (!inputEl) return;
  let val = parseInt(inputEl.value) || 0;
  if (val < 0) val = 0;
  if (val > 1440) val = 1440;
  if (val % 15 !== 0) {
    val = Math.round(val / 15) * 15;
  }
  inputEl.value = val;
}

function setOutagePreset(mins) {
  const txtOutage = document.getElementById('txtOutage');
  if (txtOutage) {
    txtOutage.value = mins;
    validateOutageStep(txtOutage);
  }
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
  const container = document.getElementById('userRecentDtContainer');
  const tbody = document.getElementById('userRecentDtBody');
  if (!container || !tbody) return;

  try {
    const res = await fetch('/api/dt/user-recent');
    const list = await res.json();

    if (!Array.isArray(list) || list.length === 0) {
      // If user has no unapproved records, DO NOT DISPLAY AT ALL (not even an empty table)
      container.style.display = 'none';
      tbody.innerHTML = '';
      return;
    }

    // Has records -> display full-width table container
    container.style.display = 'flex';
    const countBadge = document.getElementById('userRecentDtCount');
    if (countBadge) {
      countBadge.textContent = `${list.length} رکورد`;
    }

    let html = '';
    list.forEach(r => {
      const hasAttach = (r.haveAttachedFile === 'دارد' || r.haveAttachedFile === true || r.haveAttachedFile === 'true');
      const attachBadge = hasAttach
        ? `<a href="/api/dt/attachment/${r.row}" target="_blank" onclick="event.stopPropagation()" class="status-badge" style="background:#e0f2fe; color:#0369a1; border:1px solid #bae6fd; font-size:10.5px; padding:1px 6px; border-radius:10px; text-decoration:none; display:inline-flex; align-items:center; gap:2px; font-weight:700;" title="دانلود فایل ضمیمه">📎 دانلود</a>`
        : '<span style="color:#94a3b8; font-size:10.5px;">ندارد</span>';

      const safeRowJson = JSON.stringify(r).replace(/"/g, '&quot;');

      html += `<tr style="cursor: pointer;" title="دوبار کلیک برای بارگذاری به عنوان دان‌تایم مشابه" ondblclick="populateFormFromSimilarRowData(${safeRowJson})">
        <td style="font-weight: 800; color: #0284c7; font-family: monospace;">#${r.row || '-'}</td>
        <td style="font-size: 10.5px;">${r.registerDate || '-'}</td>
        <td dir="ltr" style="font-family: monospace; font-size: 10.5px; color: #475569;">${r.registerTime || '-'}</td>
        <td><span style="background: #f1f5f9; color: #334155; padding: 2px 6px; border-radius: 4px; font-weight: 700; font-size: 10px; border: 1px solid #cbd5e1;">${r.domain || '-'}</span></td>
        <td style="font-weight: 800; color: #0f172a; font-family: monospace; font-size: 11.5px;">${r.siteCode || '-'}</td>
        <td style="font-weight: 700; color: #047857; text-align: right;">${r.siteName || '-'}</td>
        <td style="color: #475569;">${r.region || '-'}</td>
        <td style="font-size: 10.5px;">${r.startDate || '-'}</td>
        <td dir="ltr" style="font-family: monospace; font-size: 10.5px;">${r.startTime || '-'}</td>
        <td style="font-size: 10.5px;">${r.endDate || '-'}</td>
        <td dir="ltr" style="font-family: monospace; font-size: 10.5px;">${r.endTime || '-'}</td>
        <td style="font-weight: 800; color: ${r.outageMin > 0 ? '#ea580c' : '#64748b'};">${r.outageMin ?? 0}</td>
        <td style="max-width: 220px; text-align: right; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;" title="${r.subject || ''}">${r.subject || '-'}</td>
        <td style="font-weight: 600; color: #7c3aed; text-align: right;">${r.performer || '-'}</td>
        <td style="color: #475569;">${r.Activity || '-'}</td>
        <td><span class="status-badge" style="background: #fef3c7; color: #92400e; border: 1px solid #fde68a; font-weight: 800; font-size: 10.5px; padding: 2px 8px; border-radius: 12px;">${r.status || 'ثبت توسط کاربر'}</span></td>
        <td>${attachBadge}</td>
      </tr>`;
    });
    tbody.innerHTML = html;
  } catch (err) {
    console.error('loadUserRecentDtSubmissions error:', err);
    container.style.display = 'none';
  }
}
window.loadUserRecentDtSubmissions = loadUserRecentDtSubmissions;

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
  const dataToExport = (filteredDtData && filteredDtData.length > 0) ? filteredDtData : rawDtData;
  if (!dataToExport || dataToExport.length === 0) {
    alert('اطلاعاتی برای خروجی اکسل وجود ندارد.');
    return;
  }

  const confirmExport = confirm(`آیا از خروجی گرفتن اکسل برای ${dataToExport.length} رکورد دان‌تایم اطمینان دارید؟`);
  if (!confirmExport) return;

  const columns = [
    { key: 'ردیف', label: 'ردیف' },
    { key: 'تاریخ ثبت', label: 'تاریخ ثبت' },
    { key: 'ثبت', label: 'زمان ثبت' },
    { key: 'کد سایت', label: 'کد سایت' },
    { key: 'نام سایت', label: 'نام سایت' },
    { key: 'منطقه', label: 'منطقه' },
    { key: 'تاریخ شروع', label: 'تاریخ شروع' },
    { key: 'زمان شروع', label: 'زمان شروع' },
    { key: 'تاریخ پایان', label: 'تاریخ پایان' },
    { key: 'زمان پایان', label: 'زمان پایان' },
    { key: 'زمان قطعی', label: 'زمان قطعی (دقیقه)' },
    { key: 'درخواست کننده', label: 'درخواست کننده' },
    { key: 'موضوع', label: 'موضوع' },
    { key: 'انجام دهنده', label: 'انجام دهنده' },
    { key: 'ثبت کننده', label: 'ثبت کننده' },
    { key: 'شماره', label: 'شماره' },
    { key: 'وضعیت', label: 'وضعیت' },
    { key: 'فعالیت', label: 'فعالیت' },
    { key: 'یادداشت', label: 'یادداشت' }
  ];

  let tableHtml = `<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40">`;
  tableHtml += `<head><meta charset="UTF-8"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>دان تایم ها</x:Name><x:WorksheetOptions><x:DisplayRightToLeft/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><style>th { background-color: #0284c7; color: #ffffff; font-weight: bold; text-align: center; border: 1px solid #cccccc; padding: 6px; } td { text-align: center; border: 1px solid #e5e7eb; padding: 4px; mso-number-format:"\\@"; }</style></head>`;
  tableHtml += `<body dir="rtl"><table><thead><tr>`;

  columns.forEach(col => {
    tableHtml += `<th>${col.label}</th>`;
  });
  tableHtml += `</tr></thead><tbody>`;

  dataToExport.forEach(row => {
    tableHtml += `<tr>`;
    columns.forEach(col => {
      let val = row[col.key] !== undefined && row[col.key] !== null ? String(row[col.key]) : '';
      val = val.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
      tableHtml += `<td>${val}</td>`;
    });
    tableHtml += `</tr>`;
  });

  tableHtml += `</tbody></table></body></html>`;

  const blob = new Blob(['\uFEFF' + tableHtml], { type: 'application/vnd.ms-excel;charset=utf-8' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  const now = new Date();
  const dateStr = now.toLocaleDateString('fa-IR').replace(/\//g, '-');
  a.download = `گزارش_دان_تایم_${dateStr}.xls`;
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);

  const wantToOpen = confirm('فایل اکسل دانلود شد. آیا می‌خواهید گزارش در تب جدید نیز باز شود؟');
  if (wantToOpen) {
    const win = window.open('', '_blank');
    if (win) {
      const pageTitle = `گزارش دان‌تایم - ${dateStr}`;
      let htmlDoc = `<!DOCTYPE html><html lang="fa" dir="rtl"><head><meta charset="UTF-8"><title>${pageTitle}</title>`;
      htmlDoc += `<link href="https://fonts.googleapis.com/css2?family=Vazirmatn:wght@400;600;700&display=swap" rel="stylesheet">`;
      htmlDoc += `<style>
        body { font-family: 'Vazirmatn', sans-serif; background-color: #0f172a; color: #f8fafc; padding: 20px; direction: rtl; }
        h2 { color: #38bdf8; text-align: center; margin-bottom: 8px; font-weight: 700; font-size: 18px; }
        .info-bar { text-align: center; margin-bottom: 18px; font-size: 12.5px; color: #94a3b8; }
        table { width: 100%; border-collapse: collapse; background-color: #1e293b; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.3); }
        th { background-color: #0284c7; color: #ffffff; padding: 9px 6px; font-size: 11.5px; font-weight: 700; border: 1px solid #0369a1; text-align: center; }
        td { padding: 7px 5px; font-size: 11px; border: 1px solid #334155; text-align: center; color: #e2e8f0; }
        tr:nth-child(even) { background-color: #0f172a; }
        tr:hover { background-color: #334155; }
      </style></head><body>`;
      htmlDoc += `<h2>📋 گزارش خروجی دان‌تایم‌های سامانه PE</h2>`;
      htmlDoc += `<div class="info-bar">تاریخ ثبت گزارش: ${now.toLocaleDateString('fa-IR')} | تعداد رکوردها: ${dataToExport.length}</div>`;
      htmlDoc += `<table><thead><tr>`;
      columns.forEach(col => { htmlDoc += `<th>${col.label}</th>`; });
      htmlDoc += `</tr></thead><tbody>`;
      dataToExport.forEach(row => {
        htmlDoc += `<tr>`;
        columns.forEach(col => {
          let val = row[col.key] !== undefined && row[col.key] !== null ? String(row[col.key]) : '';
          val = val.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
          htmlDoc += `<td>${val}</td>`;
        });
        htmlDoc += `</tr>`;
      });
      htmlDoc += `</tbody></table></body></html>`;

      win.document.open();
      win.document.write(htmlDoc);
      win.document.close();
    }
  }

  URL.revokeObjectURL(url);
}

function getSelectedDtRowSet() {
  const selectedSet = new Set(Array.from(selectedDtRows || []).map(v => String(v).trim()));
  if (selectedSet.size === 0 && selectedDtRow) {
    selectedSet.add(String(selectedDtRow).trim());
  }
  return selectedSet;
}

function copyDtSiteCodes() {
  const selectedSet = getSelectedDtRowSet();
  if (selectedSet.size === 0) {
    alert('لطفاً ابتدا حداقل یک سطر از جدول دان‌تایم را انتخاب کنید.');
    return;
  }

  const siteCodes = [];
  selectedSet.forEach(rNumStr => {
    const rowData = rawDtData.find(r => String(r['ردیف']).trim() === rNumStr);
    if (rowData && rowData['کد سایت']) {
      const cleanedCode = String(rowData['کد سایت']).replace(/XX/gi, '').trim();
      if (cleanedCode) siteCodes.push(cleanedCode);
    }
  });

  const distinctSiteCodes = [...new Set(siteCodes)];
  if (distinctSiteCodes.length === 0) {
    alert('هیچ کد سایتی در سطرهای انتخابی یافت نشد.');
    return;
  }

  const joinedStr = distinctSiteCodes.join(',');
  copyTextToClipboard(joinedStr);
  const msg = `${distinctSiteCodes.length} عدد کدسایت بدون تکرار در حافظه موقت کپی شد:\n\n${joinedStr}`;
  alert(msg);
}

function copyDtSubjects() {
  const selectedSet = getSelectedDtRowSet();
  if (selectedSet.size === 0) {
    alert('لطفاً ابتدا حداقل یک سطر از جدول دان‌تایم را انتخاب کنید.');
    return;
  }

  const subjects = [];
  selectedSet.forEach(rNumStr => {
    const rowData = rawDtData.find(r => String(r['ردیف']).trim() === rNumStr);
    if (rowData && rowData['موضوع']) {
      const subj = String(rowData['موضوع']).trim();
      if (subj) subjects.push(subj);
    }
  });

  if (subjects.length === 0) {
    alert('هیچ موضوعی در سطرهای انتخابی یافت نشد.');
    return;
  }

  const joinedStr = subjects.join('\r\n');
  copyTextToClipboard(joinedStr);
  const msg = `${subjects.length} عدد موضوع در حافظه موقت کپی شد:\n\n${joinedStr}`;
  alert(msg);
}

function copySelectedDtRowsFull() {
  const selectedSet = getSelectedDtRowSet();
  if (selectedSet.size === 0) {
    alert('لطفاً ابتدا حداقل یک سطر از جدول دان‌تایم را انتخاب کنید.');
    return;
  }

  const selectedRowsData = [];
  selectedSet.forEach(rNumStr => {
    const row = rawDtData.find(r => String(r['ردیف']).trim() === rNumStr);
    if (row) selectedRowsData.push(row);
  });

  if (selectedRowsData.length === 0) return;

  const cols = Object.keys(selectedRowsData[0]).filter(k => !k.startsWith('_'));
  const headerLine = cols.join('\t');
  const rowLines = selectedRowsData.map(r => cols.map(c => String(r[c] !== null && r[c] !== undefined ? r[c] : '')).join('\t'));
  const fullText = [headerLine, ...rowLines].join('\r\n');

  copyTextToClipboard(fullText);
  alert(`اطلاعات کامل ${selectedRowsData.length} ردیف با فرمت سازگار با اکسل در حافظه موقت کپی شد.`);
}

function copySelectedDtSummary() {
  const selectedSet = getSelectedDtRowSet();
  if (selectedSet.size === 0) {
    alert('لطفاً ابتدا یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }

  const firstRowId = Array.from(selectedSet)[0];
  const rowData = rawDtData.find(r => String(r['ردیف']).trim() === firstRowId);
  if (rowData) {
    const textToCopy = `ردیف: ${rowData['ردیف']} | کد سایت: ${rowData['کد سایت'] || '-'} | نام سایت: ${rowData['نام سایت'] || '-'} | موضوع: ${rowData['موضوع'] || '-'} | زمان قطعی: ${rowData['زمان قطعی'] || 0} دقیقه | وضعیت: ${rowData['وضعیت'] || '-'}`;
    copyTextToClipboard(textToCopy);
    alert(`خلاصه اطلاعات دان‌تایم ردیف ${firstRowId} در حافظه موقت کپی شد.`);
  }
}

async function executeDesktopDtCopy() {
  const loading = document.getElementById('dtLoading');
  if (loading) loading.style.display = 'flex';

  try {
    const res = await fetch('/api/dt/copy-excel');
    const data = await res.json().catch(() => ({}));
    if (!res.ok || !data.success) {
      alert(`خطا در اجرای عملیات کپی: ${data.error || res.statusText || 'خطای ناشناخته در سرور'}`);
      return;
    }

    const fileName = data.fileName || 'Current_DT.xlsx';
    const rowCount = data.rowCount || 0;
    alert(`✅ عملیات کپی انجام شد و فایل اکسل با موفقیت روی دسکتاپ ایجاد و در اکسل باز گردید:\n${fileName}\n(تعداد ردیف‌های فعال: ${rowCount})`);
  } catch (err) {
    console.error('executeDesktopDtCopy error:', err);
    alert('خطا در اجرای عملیات کپی: ' + err.message);
  } finally {
    if (loading) loading.style.display = 'none';
  }
}

function copySelectedDt() {
  executeDesktopDtCopy();
}

// Keyboard Shortcut: Ctrl + B for Copy (matching Desktop PE)
window.addEventListener('keydown', (e) => {
  if (e.ctrlKey && (e.key === 'b' || e.key === 'B' || e.code === 'KeyB' || e.keyCode === 66)) {
    e.preventDefault();
    e.stopPropagation();
    executeDesktopDtCopy();
  }
}, true);

function uploadDtAttachment() {
  if (!selectedDtRow) {
    alert('لطفاً ابتدا یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }
  alert(`فرم آپلود فایل پیوست برای ردیف ${selectedDtRow} آماده شد.`);
}

/* ==========================================================================
   DT MULTI EDIT & DELETE MODAL LOGIC (MATCHING C# DESKTOP frmDtMultiChange)
   ========================================================================== */

let dtMultiSelectedRows = [];

function openDtMultiChangeModal(initialTab = 'edit') {
  const modal = document.getElementById('dtMultiChangeModal');
  if (!modal) return;

  const selectedSet = new Set(Array.from(selectedDtRows).map(v => String(v).trim()));
  if (selectedSet.size === 0 && selectedDtRow) {
    selectedSet.add(String(selectedDtRow).trim());
  }

  if (selectedSet.size === 0) {
    alert('لطفاً ابتدا حداقل یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }

  dtMultiSelectedRows = [];
  selectedSet.forEach(rNumStr => {
    const rowObj = rawDtData.find(r => String(r['ردیف']).trim() === rNumStr);
    if (rowObj) dtMultiSelectedRows.push(rowObj);
  });

  if (dtMultiSelectedRows.length === 0) {
    alert('رکوردهای انتخابی یافت نشدند.');
    return;
  }

  const badgeEl = document.getElementById('dtMultiChangeRecordBadge');
  if (badgeEl) badgeEl.textContent = `${dtMultiSelectedRows.length} رکورد انتخابی`;

  const deleteCountEl = document.getElementById('dtDeleteRecordCountText');
  if (deleteCountEl) deleteCountEl.textContent = dtMultiSelectedRows.length;

  renderDtMultiGrid();
  populateDtMultiOutageSelect();
  populateDtMultiTimeSelects();
  populateDtMultiDropdowns();

  if (dtMultiSelectedRows.length > 0) {
    populateDtMultiFormFromRow(dtMultiSelectedRows[0]);
  }

  switchDtMultiTab(initialTab);

  const container = document.querySelector('.dt-multichange-container');
  if (container) {
    container.classList.add('is-maximized');
  }

  modal.style.display = 'flex';
}

function populateDtMultiTimeSelects() {
  const times = [];
  for (let h = 0; h < 24; h++) {
    for (let m = 0; m < 60; m += 15) {
      const hh = String(h).padStart(2, '0');
      const mm = String(m).padStart(2, '0');
      times.push(`${hh}:${mm}`);
    }
  }

  ['selectDtChgGrpStartTimePicker', 'selectDtChgGrpEndTimePicker', 'selectDtChgGrpRegTimePicker'].forEach(id => {
    const el = document.getElementById(id);
    if (el && el.children.length <= 1) {
      el.innerHTML = '<option value="">⏰ زمان</option>' + times.map(t => `<option value="${t}">${t}</option>`).join('');
    }
  });
}

function onDtMultiTimePicked(selectEl, targetInputId, chbId) {
  if (!selectEl || !selectEl.value) return;
  const targetInput = document.getElementById(targetInputId);
  const chb = document.getElementById(chbId);
  if (targetInput) targetInput.value = selectEl.value;
  if (chb) chb.checked = true;
  autoCalculateDtMultiOutageMin();
}

function setDtMultiDateToday(targetInputId, chbId) {
  try {
    const todayStr = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit'
    }).format(new Date()).replace(/\//g, '/');

    const targetInput = document.getElementById(targetInputId);
    const chb = document.getElementById(chbId);
    if (targetInput) targetInput.value = todayStr;
    if (chb) chb.checked = true;
  } catch (e) {}
}

function closeDtMultiChangeModal() {
  const modal = document.getElementById('dtMultiChangeModal');
  if (modal) modal.style.display = 'none';
}

function toggleDtMultiMaximized() {
  const container = document.querySelector('.dt-multichange-container');
  if (container) {
    container.classList.toggle('is-maximized');
  }
}

function switchDtMultiTab(tabName, btnEl) {
  const tabEdit = document.getElementById('dtMultiTabEdit');
  const tabDelete = document.getElementById('dtMultiTabDelete');
  const tabBtnEdit = document.getElementById('tabBtnEdit');
  const tabBtnDelete = document.getElementById('tabBtnDelete');

  if (tabName === 'edit') {
    if (tabEdit) tabEdit.classList.add('active');
    if (tabDelete) tabDelete.classList.remove('active');
    if (tabBtnEdit) tabBtnEdit.classList.add('active');
    if (tabBtnDelete) tabBtnDelete.classList.remove('active');
  } else if (tabName === 'delete') {
    if (tabDelete) tabDelete.classList.add('active');
    if (tabEdit) tabEdit.classList.remove('active');
    if (tabBtnDelete) tabBtnDelete.classList.add('active');
    if (tabBtnEdit) tabBtnEdit.classList.remove('active');
  }
}

function renderDtMultiGrid() {
  const tbody = document.getElementById('dtMultiChangeTableBody');
  if (!tbody) return;

  const columns = [
    'ردیف', 'تاریخ ثبت', 'ثبت', 'کد سایت', 'نام سایت', 'منطقه',
    'تاریخ شروع', 'زمان شروع', 'تاریخ پایان', 'زمان پایان', 'زمان قطعی',
    'درخواست کننده', 'موضوع', 'انجام دهنده', 'ثبت کننده', 'شماره', 'وضعیت', 'فعالیت', 'یادداشت'
  ];

  let html = '';
  dtMultiSelectedRows.forEach((row, idx) => {
    const isFirst = idx === 0 ? 'active-row' : '';
    html += `<tr class="${isFirst}" onclick="onDtMultiGridRowClick(this, ${idx})">`;
    columns.forEach(colKey => {
      const val = row[colKey] !== undefined && row[colKey] !== null ? String(row[colKey]) : '';
      html += `<td>${val}</td>`;
    });
    html += `</tr>`;
  });

  tbody.innerHTML = html;
}

function onDtMultiGridRowClick(trEl, idx) {
  document.querySelectorAll('#dtMultiChangeTableBody tr').forEach(r => r.classList.remove('active-row'));
  if (trEl) trEl.classList.add('active-row');

  if (dtMultiSelectedRows[idx]) {
    populateDtMultiFormFromRow(dtMultiSelectedRows[idx]);
  }
}

function populateDtMultiOutageSelect() {
  const select = document.getElementById('selectDtChgGrpOutageMin');
  if (!select || select.children.length > 0) return;

  const intervals = [0, 15, 30, 45, 60, 75, 90, 105, 120, 150, 180, 210, 240, 270, 300, 330, 360, 390, 420, 450, 480, 510, 540, 570, 600, 630, 660, 690, 720, 780, 840, 900, 960, 1020, 1080, 1140, 1200, 1260, 1320, 1380, 1440, 1500, 1560, 1620, 1680, 1740, 1800, 1860, 1920, 1980, 2040, 2100, 2160, 2220, 2280, 2340, 2400, 2460, 2520, 2580, 2640, 2700, 2760, 2820, 2880, 2940, 3000, 3060, 3120, 3180, 3240, 3300, 3360, 3420, 3480, 3540, 3600, 3660, 3720, 3780, 3840, 3900, 3960, 4020, 4080, 4140, 4200, 4260, 4320, 4380, 4440, 4500, 4560, 4620, 4680, 4740, 4800, 4860, 4920, 4980, 5040, 5100, 5160, 5220, 5280, 5340, 5400, 5460, 5520, 5580, 5640, 5700, 5760, 5820, 5880, 5940, 6000, 6060, 6120, 6180, 6240, 6300, 6360, 6420, 6480, 6540, 6600, 6660, 6720, 6780, 6840, 6900, 6960, 7020, 7080, 7140, 7200, 7260, 7320, 7380, 7440, 7500, 7560, 7620, 7680, 7740, 7800, 7860, 7920, 7980, 8040, 8100, 8160, 8220, 8280, 8340, 8400, 8460, 8520, 8580, 8640, 8700, 8760, 8820, 8880, 8940, 9000, 9060, 9120, 9180, 9240, 9300, 9360, 9420, 9480, 9540, 9600, 9660, 9720, 9780, 9840, 9900, 9960, 10020, 10080];
  let html = '';
  intervals.forEach(min => {
    html += `<option value="${min}">${min} دقیقه</option>`;
  });
  select.innerHTML = html;
}

async function populateDtMultiDropdowns() {
  const selectSubj = document.getElementById('selectDtChgGrpSubject');
  const selectPerf = document.getElementById('selectDtChgGrpPerformer');

  if (selectSubj && selectSubj.children.length === 0) {
    try {
      const res = await fetch('/api/dt/subjects');
      const data = await res.json();
      if (data.subjects) {
        selectSubj.innerHTML = data.subjects.map(s => `<option value="${s}">${s}</option>`).join('');
      }
    } catch(e) {}
  }

  if (selectPerf && selectPerf.children.length === 0) {
    try {
      const res = await fetch('/api/dt/performers');
      const data = await res.json();
      if (data.performers) {
        selectPerf.innerHTML = data.performers.map(p => `<option value="${p}">${p}</option>`).join('');
      }
    } catch(e) {}
  }
}

function populateDtMultiFormFromRow(row) {
  if (!row) return;

  const setVal = (id, val) => {
    const el = document.getElementById(id);
    if (el) el.value = val !== undefined && val !== null ? String(val) : '';
  };

  setVal('txtDtChgGrpRow', row['ردیف']);
  setVal('txtDtChgGrpRegDate', row['تاریخ ثبت']);
  setVal('txtDtChgGrpRegTime', row['ثبت']);
  setVal('txtDtChgGrpRegion', row['منطقه']);

  setVal('txtDtChgGrpStartDate', row['تاریخ شروع']);
  setVal('txtDtChgGrpStartTime', row['زمان شروع']);
  setVal('txtDtChgGrpEndDate', row['تاریخ پایان']);
  setVal('txtDtChgGrpEndTime', row['زمان پایان']);

  setVal('txtDtChgGrpSiteCode', row['کد سایت']);
  setVal('txtDtChgGrpSiteName', row['نام سایت']);
  setVal('txtDtChgGrpApplicant', row['درخواست کننده']);
  setVal('selectDtChgGrpSubject', row['موضوع']);
  setVal('selectDtChgGrpPerformer', row['انجام دهنده']);
  setVal('txtDtChgGrpApprover', row['ثبت کننده']);
  setVal('txtDtChgGrpDtNo', row['شماره']);
  setVal('selectDtChgGrpStatus', row['وضعیت']);
  setVal('selectDtChgGrpActivity', row['فعالیت']);
  setVal('txtDtChgGrpNote', row['یادداشت']);

  const outageVal = String(row['زمان قطعی'] || row['outageMin'] || '0').replace('_YES', '').replace('_NO', '').trim();
  setVal('selectDtChgGrpOutageMin', outageVal);

  const rawOutage = String(row['outageMin'] || '');
  if (rawOutage.includes('_YES')) {
    const rb = document.getElementById('rbBusyHourYes');
    if (rb) rb.checked = true;
  } else {
    const rb = document.getElementById('rbBusyHourNo');
    if (rb) rb.checked = true;
  }
}

function validateDtNoFormat(inputEl) {
  if (!inputEl) return;
  let val = inputEl.value.trim();
  if (val.includes('MCI-CR-')) {
    val = val.replace('MCI-CR-', '');
    inputEl.value = val;
  }

  const match = val.match(/^202\d{5}-\d{5}$/);
  if ((match && val.length === 14) || val.length === 0) {
    inputEl.style.borderColor = '#10b981';
    inputEl.style.color = '#10b981';
  } else {
    inputEl.style.borderColor = '#ef4444';
    inputEl.style.color = '#ef4444';
  }
}

function validateDtMultiTimeRules(updates, activeFields) {
  const dateRegex = /^\d{4}\/(0[1-9]|1[0-2])\/(0[1-9]|[12]\d|3[01])$/;
  const timeRegex = /^([01]\d|2[0-3]):[0-5]\d$/;

  if (activeFields.includes('startDate') && !dateRegex.test(updates.startDate)) {
    return 'فرمت تاریخ شروع نامعتبر است. نمونه معتبر: 1403/05/20';
  }
  if (activeFields.includes('endDate') && !dateRegex.test(updates.endDate)) {
    return 'فرمت تاریخ پایان نامعتبر است. نمونه معتبر: 1403/05/20';
  }
  if (activeFields.includes('registerDate') && !dateRegex.test(updates.registerDate)) {
    return 'فرمت تاریخ ثبت نامعتبر است. نمونه معتبر: 1403/05/20';
  }
  if (activeFields.includes('startTime') && !timeRegex.test(updates.startTime)) {
    return 'فرمت زمان شروع نامعتبر است. نمونه معتبر: 08:00';
  }
  if (activeFields.includes('endTime') && !timeRegex.test(updates.endTime)) {
    return 'فرمت زمان پایان نامعتبر است. نمونه معتبر: 10:30';
  }
  if (activeFields.includes('registerTime') && !timeRegex.test(updates.registerTime)) {
    return 'فرمت زمان ثبت نامعتبر است. نمونه معتبر: 14:30';
  }

  let nowShamsi = '';
  let nowTime = '';
  try {
    nowShamsi = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric', month: '2-digit', day: '2-digit'
    }).format(new Date()).replace(/\//g, '/');

    const d = new Date();
    const hh = String(d.getHours()).padStart(2, '0');
    const mm = String(d.getMinutes()).padStart(2, '0');
    nowTime = `${hh}:${mm}`;
  } catch(e) {}

  if (typeof _serverTimeInfo !== 'undefined' && _serverTimeInfo && _serverTimeInfo.shamsiDate) {
    nowShamsi = _serverTimeInfo.shamsiDate;
    nowTime = _serverTimeInfo.time || nowTime;
  }

  if (activeFields.includes('startDate') || activeFields.includes('startTime')) {
    const sDate = updates.startDate || (dtMultiSelectedRows[0] ? dtMultiSelectedRows[0]['تاریخ شروع'] : nowShamsi);
    const sTime = updates.startTime || (dtMultiSelectedRows[0] ? dtMultiSelectedRows[0]['زمان شروع'] : '00:00');

    const startDateTimeStr = `${sDate} ${sTime}`;
    const nowDateTimeStr = `${nowShamsi} ${nowTime}`;

    if (startDateTimeStr < nowDateTimeStr) {
      if (!confirm(`هشدار: زمان شروع انتخاب‌شده (${startDateTimeStr}) قبل از زمان حال (${nowDateTimeStr}) است.\nآیا مطمئن هستید که می‌خواهید ادامه دهید؟`)) {
        return 'اعمال تغییرات زمان شروع لغو شد.';
      }
    }
  }

  if (activeFields.includes('endDate') || activeFields.includes('endTime') || activeFields.includes('startDate') || activeFields.includes('startTime')) {
    const sDate = updates.startDate || (dtMultiSelectedRows[0] ? dtMultiSelectedRows[0]['تاریخ شروع'] : nowShamsi);
    const sTime = updates.startTime || (dtMultiSelectedRows[0] ? dtMultiSelectedRows[0]['زمان شروع'] : '00:00');
    const eDate = updates.endDate || (dtMultiSelectedRows[0] ? dtMultiSelectedRows[0]['تاریخ پایان'] : sDate);
    const eTime = updates.endTime || (dtMultiSelectedRows[0] ? dtMultiSelectedRows[0]['زمان پایان'] : sTime);

    const startDateTimeStr = `${sDate} ${sTime}`;
    const endDateTimeStr = `${eDate} ${eTime}`;

    if (endDateTimeStr <= startDateTimeStr) {
      return `خطا: زمان پایان (${endDateTimeStr}) باید بزرگتر از زمان شروع (${startDateTimeStr}) باشد.`;
    }
  }

  return null;
}

function autoCalculateDtMultiOutageMin() {
  const sDate = document.getElementById('txtDtChgGrpStartDate')?.value.trim();
  const sTime = document.getElementById('txtDtChgGrpStartTime')?.value.trim();
  const eDate = document.getElementById('txtDtChgGrpEndDate')?.value.trim();
  const eTime = document.getElementById('txtDtChgGrpEndTime')?.value.trim();

  if (!sDate || !sTime || !eDate || !eTime) return;

  const dateRegex = /^\d{4}\/(0[1-9]|1[0-2])\/(0[1-9]|[12]\d|3[01])$/;
  const timeRegex = /^([01]\d|2[0-3]):[0-5]\d$/;

  if (!dateRegex.test(sDate) || !dateRegex.test(eDate) || !timeRegex.test(sTime) || !timeRegex.test(eTime)) return;

  const sParts = sDate.split('/');
  const eParts = eDate.split('/');
  const stParts = sTime.split(':');
  const etParts = eTime.split(':');

  const sDays = (parseInt(sParts[0]) * 365) + (parseInt(sParts[1]) * 30) + parseInt(sParts[2]);
  const eDays = (parseInt(eParts[0]) * 365) + (parseInt(eParts[1]) * 30) + parseInt(eParts[2]);

  const sTotalMin = (sDays * 24 * 60) + (parseInt(stParts[0]) * 60) + parseInt(stParts[1]);
  const eTotalMin = (eDays * 24 * 60) + (parseInt(etParts[0]) * 60) + parseInt(etParts[1]);

  const diffMin = eTotalMin - sTotalMin;
  if (diffMin > 0) {
    const outageSelect = document.getElementById('selectDtChgGrpOutageMin');
    if (outageSelect) {
      let found = false;
      for (let opt of outageSelect.options) {
        if (parseInt(opt.value) === diffMin) {
          outageSelect.value = opt.value;
          found = true;
          break;
        }
      }
      if (!found) {
        const opt = document.createElement('option');
        opt.value = diffMin;
        opt.textContent = `${diffMin} دقیقه (محاسبه‌شده)`;
        opt.selected = true;
        outageSelect.appendChild(opt);
      }
      const chbOutage = document.getElementById('chbDtChgGrpOutageTime');
      if (chbOutage) chbOutage.checked = true;
    }
  }
}

async function executeDtMultiUpdate() {
  if (dtMultiSelectedRows.length === 0) {
    alert('هیچ دان‌تایمی انتخاب نشده است.');
    return;
  }

  const fieldChecks = [
    { chbId: 'chbDtChgGrpRow', key: 'row', inputId: 'txtDtChgGrpRow' },
    { chbId: 'chbDtChgGrpRegDate', key: 'registerDate', inputId: 'txtDtChgGrpRegDate' },
    { chbId: 'chbDtChgGrpRegTime', key: 'registerTime', inputId: 'txtDtChgGrpRegTime' },
    { chbId: 'chbDtChgGrpRegion', key: 'region', inputId: 'txtDtChgGrpRegion' },
    { chbId: 'chbDtChgGrpStartDate', key: 'startDate', inputId: 'txtDtChgGrpStartDate' },
    { chbId: 'chbDtChgGrpStartTime', key: 'startTime', inputId: 'txtDtChgGrpStartTime' },
    { chbId: 'chbDtChgGrpEndDate', key: 'endDate', inputId: 'txtDtChgGrpEndDate' },
    { chbId: 'chbDtChgGrpEndTime', key: 'endTime', inputId: 'txtDtChgGrpEndTime' },
    { chbId: 'chbDtChgGrpSiteCode', key: 'siteCode', inputId: 'txtDtChgGrpSiteCode' },
    { chbId: 'chbDtChgGrpSiteName', key: 'siteName', inputId: 'txtDtChgGrpSiteName' },
    { chbId: 'chbDtChgGrpApplicant', key: 'applicant', inputId: 'txtDtChgGrpApplicant' },
    { chbId: 'chbDtChgGrpSubject', key: 'subject', inputId: 'selectDtChgGrpSubject' },
    { chbId: 'chbDtChgGrpPerformer', key: 'performer', inputId: 'selectDtChgGrpPerformer' },
    { chbId: 'chbDtChgGrpApprover', key: 'approver', inputId: 'txtDtChgGrpApprover' },
    { chbId: 'chbDtChgGrpDtNo', key: 'dtNo', inputId: 'txtDtChgGrpDtNo' },
    { chbId: 'chbDtChgGrpStatus', key: 'status', inputId: 'selectDtChgGrpStatus' },
    { chbId: 'chbDtChgGrpActivity', key: 'activity', inputId: 'selectDtChgGrpActivity' },
    { chbId: 'chbDtChgGrpNote', key: 'note', inputId: 'txtDtChgGrpNote' },
    { chbId: 'chbDtChgGrpOutageTime', key: 'outageMin', inputId: 'selectDtChgGrpOutageMin' }
  ];

  const activeFields = [];
  const updates = {};

  fieldChecks.forEach(item => {
    const chb = document.getElementById(item.chbId);
    const input = document.getElementById(item.inputId);
    if (chb && chb.checked && input) {
      activeFields.push(item.key);
      updates[item.key] = input.value.trim();
    }
  });

  const chbBusy = document.getElementById('chbDtChgGrpBusyOver');
  if (chbBusy && chbBusy.checked && updates['outageMin'] !== undefined) {
    const isBusy = document.getElementById('rbBusyHourYes')?.checked;
    updates['outageMin'] = updates['outageMin'] + (isBusy ? '_YES' : '_NO');
  }

  if (activeFields.length === 0) {
    alert('لطفاً چک‌باکس حداقل یک فیلد را برای اعمال تغییرات فعال کنید.');
    return;
  }

  // Validate time and date logic rules
  const timeValidationError = validateDtMultiTimeRules(updates, activeFields);
  if (timeValidationError) {
    alert(timeValidationError);
    return;
  }

  const confirmMsg = `آیا از اعمال تغییرات برای تعداد ${dtMultiSelectedRows.length} دان‌تایم انتخابی اطمینان دارید؟`;
  if (!confirm(confirmMsg)) return;

  const rowNumbers = dtMultiSelectedRows.map(r => r['ردیف']);

  try {
    const res = await fetch('/api/dt/multi-update', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ rowNumbers, updates, activeFields })
    });

    const text = await res.text();
    let data;
    try {
      data = JSON.parse(text);
    } catch (e) {
      if (res.status === 404) {
        throw new Error('مسیر سرور ویرایش پیدا نشد. لطفاً سرور Node را مجدداً راه‌اندازی کنید.');
      } else {
        throw new Error(`پاسخ سرور نامعتبر است (${res.status}): ${text.substring(0, 100)}`);
      }
    }

    if (!res.ok || !data.success) {
      alert('خطا در اعمال تغییرات: ' + (data.error || 'خطای ناشناخته'));
      return;
    }

    alert(data.message || 'تغییرات گروهی با موفقیت اعمال گردید.');
    closeDtMultiChangeModal();
    loadDT();
  } catch (err) {
    console.error('executeDtMultiUpdate error:', err);
    alert('خطا در ارتباط با سرور: ' + err.message);
  }
}

async function executeDtMultiDelete() {
  if (dtMultiSelectedRows.length === 0) {
    alert('هیچ دان‌تایمی انتخاب نشده است.');
    return;
  }

  const rowNumbers = dtMultiSelectedRows.map(r => r['ردیف']);

  const confirm1 = confirm(`برای حذف به تعداد ${rowNumbers.length} رکورد انتخابی (ردیف ${rowNumbers.join(', ')}) موافق هستید؟`);
  if (!confirm1) return;

  const confirm2 = confirm(`آیا از حذف تعداد ${rowNumbers.length} رکورد دان‌تایم اطمینان دارید؟`);
  if (!confirm2) return;

  try {
    const res = await fetch('/api/dt/multi-delete', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ rowNumbers })
    });

    const text = await res.text();
    let data;
    try {
      data = JSON.parse(text);
    } catch (e) {
      if (res.status === 404) {
        throw new Error('مسیر سرور حذف پیدا نشد. لطفاً سرور Node را مجدداً راه‌اندازی کنید.');
      } else {
        throw new Error(`پاسخ سرور نامعتبر است (${res.status}): ${text.substring(0, 100)}`);
      }
    }

    if (!res.ok || !data.success) {
      alert('خطا در حذف دان‌تایم‌ها: ' + (data.error || 'خطای ناشناخته'));
      return;
    }

    alert(data.message || 'حذف دان‌تایم‌ها با موفقیت انجام شد.');
    closeDtMultiChangeModal();
    loadDT();
  } catch (err) {
    console.error('executeDtMultiDelete error:', err);
    alert('خطا در ارتباط با سرور: ' + err.message);
  }
}

function editSelectedDt() {
  openDtMultiChangeModal('edit');
}

window._similarLoadedRowData = null;

async function populateFormFromSimilarRowData(rowData) {
  if (!rowData) return;

  // 1. Set Header Title (subtle with parenthetical instruction)
  const mainTitleEl = document.querySelector('.wizard-main-title');
  if (mainTitleEl) {
    mainTitleEl.innerHTML = `<span style="color: #ffffff; font-weight: 800; font-size: 14.5px;">ثبت دان‌تایم مشابه (بر اساس ردیف #${rowData['ردیف'] || rowData.row})</span> <span style="font-size: 11px; color: #f8fafc; font-weight: 400; margin-right: 6px;">(موارد مورد نظر را تغییر داده و دکمه ثبت را بفشارید)</span>`;
  }

  // 2. Domain Radio (SITE / LRAN / HRAN / IPBB)
  const domainVal = (rowData['دامنه'] || rowData['حوزه'] || rowData['domain'] || 'SITE').toString().trim().toUpperCase();
  const domainRadio = document.querySelector(`input[name="rdoDomain"][value="${domainVal}"]`) || document.querySelector('input[name="rdoDomain"][value="SITE"]');
  if (domainRadio) {
    domainRadio.checked = true;
    rdoDomain_EditValueChanged(domainRadio.value);
  }

  // 3. Site Code & Site Name lookup
  let rawSiteCode = (rowData['کد سایت'] || rowData['siteCode'] || '').toString().trim();
  let cleanDigits = rawSiteCode.replace(/[^0-9]/g, '');
  if (cleanDigits) {
    const txtSiteCode = document.getElementById('txtSiteCode');
    if (txtSiteCode) {
      txtSiteCode.value = cleanDigits.padStart(4, '0');
      await txtSiteCode_TextChanged(txtSiteCode.value);
    }
  } else if (domainVal !== 'SITE') {
    _isSiteValid = true;
    const btnNextSite = document.getElementById('btnNextSite');
    if (btnNextSite) btnNextSite.disabled = false;
  }

  // 4. Category Radio (Site Access, Site Cleanup, Other)
  const activityVal = (rowData['فعالیت'] || rowData['Activity'] || rowData['category'] || '').toString().trim();
  let catRadioId = 'rdoOtherCategory';
  if (activityVal.includes('دسترسی') || activityVal.includes('Access')) {
    catRadioId = 'rdoSiteAccess';
  } else if (activityVal.includes('پاکسازی') || activityVal.includes('Cleanup')) {
    catRadioId = 'rdoSiteCleanup';
  }
  const catRadio = document.getElementById(catRadioId);
  if (catRadio) {
    catRadio.checked = true;
  }

  // 5. Start Date & End Date
  const startDateStr = (rowData['تاریخ شروع'] || rowData['startDate'] || '').toString().trim();
  if (startDateStr) {
    setSelectedShamsiDate('Start', startDateStr);
  } else {
    setSelectedShamsiDate('Start', getShamsiTodayString());
  }

  const endDateStr = (rowData['تاریخ پایان'] || rowData['endDate'] || '').toString().trim();
  if (endDateStr) {
    setSelectedShamsiDate('End', endDateStr);
  } else {
    setSelectedShamsiDate('End', getShamsiTodayString());
  }

  // 6. Start Time & End Time
  const startTimeStr = (rowData['زمان شروع'] || rowData['شروع'] || rowData['startTime'] || '').toString().trim();
  if (startTimeStr && startTimeStr.includes(':')) {
    const parts = startTimeStr.split(':');
    if (parts.length >= 2) {
      const hEl = document.getElementById('tmeStartHour');
      const mEl = document.getElementById('tmeStartMin');
      if (hEl) hEl.value = parts[0].padStart(2, '0');
      if (mEl) mEl.value = parts[1].padStart(2, '0');
    }
  }

  const endTimeStr = (rowData['زمان پایان'] || rowData['پایان'] || rowData['endTime'] || '').toString().trim();
  if (endTimeStr && endTimeStr.includes(':')) {
    const parts = endTimeStr.split(':');
    if (parts.length >= 2) {
      const hEl = document.getElementById('tmeEndHour');
      const mEl = document.getElementById('tmeEndMin');
      if (hEl) hEl.value = parts[0].padStart(2, '0');
      if (mEl) mEl.value = parts[1].padStart(2, '0');
    }
  }

  // 7. Outage Time (زمان قطعی)
  const outageVal = rowData['زمان قطعی'] || rowData['outageMin'] || rowData['outageTime'] || rowData['outage'] || '0';
  const txtOutage = document.getElementById('txtOutage');
  if (txtOutage) {
    const numOutage = parseInt(outageVal.toString().replace(/[^0-9]/g, '')) || 0;
    txtOutage.value = numOutage;
    validateOutageStep(txtOutage);
  }

  // 8. Busy Hour Checkbox
  const rawOutageStr = (rowData['outageMin'] || '').toString();
  const chkInBsyHour = document.getElementById('chkInBsyHour');
  if (chkInBsyHour) {
    chkInBsyHour.checked = rawOutageStr.includes('_YES');
  }

  // 9. Subject (موضوع)
  const subjectVal = rowData['موضوع'] || rowData['subject'] || '';
  const txtSubject = document.getElementById('txtSubject');
  if (txtSubject) txtSubject.value = subjectVal;

  // 10. Performer (انجام دهنده) & Phonebook details lookup
  const performerVal = rowData['انجام دهنده'] || rowData['performer'] || '';
  const sluePerformer = document.getElementById('sluePerformer');
  if (sluePerformer) sluePerformer.value = performerVal;

  const lblWorkPlaceResult = document.getElementById('lblWorkPlaceResult');
  const lblPhoneResult = document.getElementById('lblPhoneResult');
  if (lblWorkPlaceResult) lblWorkPlaceResult.textContent = '---';
  if (lblPhoneResult) lblPhoneResult.textContent = '---';

  if (performerVal && Array.isArray(_performersList) && _performersList.length > 0) {
    const cleanName = performerVal.split('-')[0].trim();
    const matchPerf = _performersList.find(p => p.performer === cleanName || cleanName.includes(p.performer));
    if (matchPerf) {
      if (lblWorkPlaceResult) lblWorkPlaceResult.textContent = matchPerf.Office_PhoneBook || '---';
      if (lblPhoneResult) lblPhoneResult.textContent = matchPerf.Mobile_Personnel || '---';
    }
  }

  // 11. Attachment State (Reset to No Attachment)
  const rdoAttachNo = document.getElementById('rdoAttachNo');
  if (rdoAttachNo) rdoAttachNo.checked = true;
  toggleAttachmentBox(false);
  clearSelectedAttachment();

  // 12. Unlock steps & ENABLE submit button & next buttons
  _maxUnlockedStep = 7;
  ['btnNextDate', 'btnNextDomain', 'btnNextSite', 'btnNextCategory', 'btnNextTime', 'btnNextDetails'].forEach(id => {
    const el = document.getElementById(id);
    if (el) el.disabled = false;
  });

  const btnSubmit = document.getElementById('btnSubmit');
  if (btnSubmit) btnSubmit.disabled = false;

  activateStep(5, true);
}

async function openCreateSimilarDtModal() {
  if (!selectedDtRow) {
    alert('لطفاً ابتدا یک سطر (دان‌تایم) را از جدول انتخاب کنید.');
    return;
  }

  const selStr = String(selectedDtRow).trim();
  let rowData = null;

  const checkMatch = (r) => {
    if (!r) return false;
    const rId = String(r['ردیف'] ?? r.row ?? r['شماره'] ?? r.dtNo ?? '').trim();
    return rId === selStr;
  };

  if (typeof rawDtData !== 'undefined' && Array.isArray(rawDtData)) {
    rowData = rawDtData.find(checkMatch);
  }
  if (!rowData && typeof filteredDtData !== 'undefined' && Array.isArray(filteredDtData)) {
    rowData = filteredDtData.find(checkMatch);
  }

  if (!rowData) {
    try {
      const res = await safeFetchJson(`/api/dt/detail/${selStr}`);
      if (res && res.success && res.data) {
        rowData = res.data;
      }
    } catch (e) {
      console.warn('Could not fetch DT detail from API:', e);
    }
  }

  if (!rowData) {
    alert(`اطلاعات دان‌تایم سطر انتخابی (${selectedDtRow}) پیدا نشد.`);
    return;
  }

  window._similarLoadedRowData = rowData;
  const modal = document.getElementById('dtRegModal');
  if (modal) modal.classList.add('similar-theme');

  await openDtRegModal();
  await populateFormFromSimilarRowData(rowData);
}

// ===== FREEZE ANNOUNCEMENT MANAGEMENT LOGIC =====

function initFreezeModalDates() {
  const d = new Date();
  const faParts = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
    year: 'numeric', month: '2-digit', day: '2-digit'
  }).formatToParts(d);
  
  const y = faParts.find(p => p.type === 'year').value;
  const m = faParts.find(p => p.type === 'month').value;
  const day = faParts.find(p => p.type === 'day').value;
  const todayStr = `${y}/${m}/${day}`;

  setFreezePickerDate('Start', todayStr);
  setFreezePickerDate('End', todayStr);

  generateDefaultFreezeMessage(false);
}

function setFreezePickerDate(prefix, dateStr) {
  if (!dateStr) return;
  const parts = dateStr.split('/');
  if (parts.length < 3) return;
  
  const y = parts[0];
  const m = parts[1].padStart(2, '0');
  const d = parts[2].padStart(2, '0');

  const yEl = document.getElementById(`freeze${prefix}Year`);
  const mEl = document.getElementById(`freeze${prefix}Month`);
  if (yEl) yEl.value = y;
  if (mEl) mEl.value = m;
  
  populateShamsiDays(`freeze${prefix}Day`, d);
}

function getFreezePickerDate(prefix) {
  const y = document.getElementById(`freeze${prefix}Year`)?.value;
  const m = document.getElementById(`freeze${prefix}Month`)?.value;
  const d = document.getElementById(`freeze${prefix}Day`)?.value;
  if (!y || !m || !d) return null;
  return `${y}/${m.padStart(2, '0')}/${d.padStart(2, '0')}`;
}

function generateDefaultFreezeMessage(force = false) {
  const startDate = getFreezePickerDate('Start');
  const endDate = getFreezePickerDate('End');
  const txtArea = document.getElementById('txtFreezeMessageInput');
  if (!txtArea) return;

  if (!force && txtArea.value.trim() !== '') return;

  let msg = `کاربر گرامی، شبکه از تاریخ ( ${startDate || '---'} ) تا ( ${endDate || '---'} ) فریز می‌باشد و هیچگونه فعالیت برنامه‌ریزی‌شده‌ای تأیید نخواهد شد\n (به جز فعالیتهای اضطراری با مجوز عملیات شبکه).`;
  txtArea.value = msg;
}

async function safeFetchJson(url, options = {}) {
  const res = await fetch(url, options);
  const contentType = res.headers.get('content-type') || '';
  if (!contentType.includes('application/json')) {
    throw new Error('سرور نود (Node.js) نیاز به بازخوانی دارد. لطفاً یک بار پنجره سرور را بسته و Start-PE-Server.bat را مجدداً اجرا کنید.');
  }
  return await res.json();
}

async function checkActiveFreezeAnnouncement() {
  try {
    const data = await safeFetchJson('/api/dt/announcement/active');
    
    const banner = document.getElementById('dtFreezeAlertBanner');
    const msgEl = document.getElementById('dtFreezeAlertMessage');
    
    const modalBanner = document.getElementById('modalFreezeAlertBanner');
    const modalMsgEl = document.getElementById('modalFreezeAlertMessage');

    const statusBadge = document.getElementById('freezeStatusBadge');
    const toggleBtn = document.getElementById('btnToggleFreezeStatus');
    const previewBox = document.getElementById('freezeActivePreviewBox');
    const previewText = document.getElementById('freezeActivePreviewText');
    const previewUser = document.getElementById('freezeActiveMetaUser');
    const previewDate = document.getElementById('freezeActiveMetaDate');

    if (data.success && data.activeAnnouncement) {
      const activeMsg = data.activeAnnouncement.messageText;
      const createdBy = data.activeAnnouncement.createdBy || 'مدیر سیستم';
      const createdDate = `${data.activeAnnouncement.createdDate || ''} ${data.activeAnnouncement.createdTime || ''}`;

      // Update main DT page banner
      if (banner && msgEl) {
        msgEl.innerText = activeMsg;
        banner.style.display = 'flex';
      }

      // Update DT registration modal banner
      if (modalBanner && modalMsgEl) {
        modalMsgEl.innerText = activeMsg;
        modalBanner.style.display = 'flex';
      }

      // Update Freeze Manager Modal preview
      if (statusBadge) {
        statusBadge.className = 'freeze-status-badge active';
        statusBadge.innerText = '🟢 فعال (در حال نمایش به کاربران)';
      }
      if (toggleBtn) {
        toggleBtn.style.display = 'inline-block';
      }
      if (previewBox) {
        previewBox.style.display = 'block';
        if (previewText) previewText.innerText = activeMsg;
        if (previewUser) previewUser.innerText = createdBy;
        if (previewDate) previewDate.innerText = createdDate;
      }
    } else {
      // Hide banners
      if (banner) banner.style.display = 'none';
      if (modalBanner) modalBanner.style.display = 'none';

      // Update status badge
      if (statusBadge) {
        statusBadge.className = 'freeze-status-badge inactive';
        statusBadge.innerText = '⚪ غیرفعال (هیچ پیامی نمایش داده نمی‌شود)';
      }
      if (toggleBtn) {
        toggleBtn.style.display = 'none';
      }
      if (previewBox) {
        previewBox.style.display = 'none';
      }
    }
  } catch (err) {
    console.error('Error checking active freeze announcement:', err.message);
  }
}

function openFreezeManagerModal() {
  const modal = document.getElementById('freezeManagerModal');
  if (!modal) return;
  modal.style.display = 'flex';
  initFreezeModalDates();
  checkActiveFreezeAnnouncement();
  loadFreezeHistoryUI();
}

function closeFreezeManagerModal(event) {
  if (event && event.target && event.target.id !== 'freezeManagerModal' && !event.target.classList.contains('modal-close') && !event.target.classList.contains('btn-modal-cancel')) {
    return;
  }
  const modal = document.getElementById('freezeManagerModal');
  if (modal) modal.style.display = 'none';
}

async function loadFreezeHistoryUI() {
  const tbody = document.getElementById('freezeHistoryTableBody');
  if (!tbody) return;
  
  tbody.innerHTML = '<tr><td colspan="7" style="text-align:center; padding:15px;">در حال دریافت سوابق...</td></tr>';
  
  try {
    const data = await safeFetchJson('/api/dt/announcements/history');
    
    if (!data.success || !data.history || data.history.length === 0) {
      tbody.innerHTML = '<tr><td colspan="7" style="text-align:center; padding:15px; color:#64748b;">هیچ سابقه اطلاعیه فریز ثبت نشده است.</td></tr>';
      return;
    }

    tbody.innerHTML = '';
    data.history.forEach(item => {
      const tr = document.createElement('tr');
      if (item.isActive) tr.className = 'active-row';

      const freezePeriodStr = (item.startDate && item.endDate)
        ? `${item.startDate} تا ${item.endDate}`
        : (item.startDate || item.endDate || '---');

      const statusHtml = item.isActive
        ? '<span class="freeze-status-badge active" style="font-size:11px; padding:2px 8px;">فعال</span>'
        : '<span class="freeze-status-badge inactive" style="font-size:11px; padding:2px 8px;">غیرفعال</span>';

      const actionsHtml = item.isActive
        ? `<button class="btn-mini-action delete" onclick="deleteFreezeItem(${item.id})">حذف</button>`
        : `<button class="btn-mini-action activate" onclick="activateFreezeItem(${item.id})">فعال‌سازی</button>
           <button class="btn-mini-action delete" onclick="deleteFreezeItem(${item.id})">حذف</button>`;

      tr.innerHTML = `
        <td style="font-weight:700;">#${item.id}</td>
        <td style="white-space:pre-line; line-height:1.4;">${item.messageText}</td>
        <td style="font-weight:600; color:#0369a1;">${freezePeriodStr}</td>
        <td>${item.createdDate || ''} <span style="font-size:10px; color:#64748b;">${item.createdTime || ''}</span></td>
        <td>${item.createdBy || 'مدیر'}</td>
        <td>${statusHtml}</td>
        <td style="text-align:center;">${actionsHtml}</td>
      `;
      tbody.appendChild(tr);
    });
  } catch (err) {
    console.error('Error loading freeze history:', err);
    tbody.innerHTML = `<tr><td colspan="7" style="text-align:center; padding:15px; color:#ef4444;">${err.message}</td></tr>`;
  }
}

async function submitNewFreezeAnnouncement(setAsActive) {
  const input = document.getElementById('txtFreezeMessageInput');
  if (!input) return;

  const messageText = input.value.trim();
  if (!messageText) {
    alert('لطفاً متن پیام فریز را وارد کنید.');
    return;
  }

  const startDate = getFreezePickerDate('Start');
  const endDate = getFreezePickerDate('End');

  try {
    const data = await safeFetchJson('/api/dt/announcement', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ messageText, startDate, endDate, setAsActive })
    });
    if (data.success) {
      alert(setAsActive ? 'پیام جدید ثبت و فعال گردید.' : 'پیام جدید در سوابق ذخیره شد.');
      await checkActiveFreezeAnnouncement();
      await loadFreezeHistoryUI();
    } else {
      alert('خطا: ' + (data.error || 'عملیات ناموفق بود'));
    }
  } catch (err) {
    alert(err.message);
  }
}

async function activateFreezeItem(id) {
  try {
    const data = await safeFetchJson(`/api/dt/announcement/${id}/activate`, { method: 'PUT' });
    if (data.success) {
      await checkActiveFreezeAnnouncement();
      await loadFreezeHistoryUI();
    } else {
      alert('خطا: ' + data.error);
    }
  } catch (err) {
    alert(err.message);
  }
}

async function handleFreezeGlobalToggle() {
  if (!confirm('آیا اطمینان دارید که می‌خواهید تمامی اعلان‌های فریز غیرفعال شوند؟')) return;
  try {
    const data = await safeFetchJson('/api/dt/announcement/deactivate-all', { method: 'PUT' });
    if (data.success) {
      await checkActiveFreezeAnnouncement();
      await loadFreezeHistoryUI();
    } else {
      alert('خطا: ' + data.error);
    }
  } catch (err) {
    alert(err.message);
  }
}

async function deleteFreezeItem(id) {
  if (!confirm('آیا از حذف این سابقه اطمینان دارید؟')) return;
  try {
    const data = await safeFetchJson(`/api/dt/announcement/${id}`, { method: 'DELETE' });
    if (data.success) {
      await checkActiveFreezeAnnouncement();
      await loadFreezeHistoryUI();
    } else {
      alert('خطا: ' + data.error);
    }
  } catch (err) {
    alert(err.message);
  }
}

// Auto-check active announcement on script load
document.addEventListener('DOMContentLoaded', () => {
  setTimeout(checkActiveFreezeAnnouncement, 1000);
});

/* ==========================================================================
   INTERACTIVE JALALI / SHAMSI CALENDAR PICKER POPUP
   ========================================================================== */
let activeCalendarTargetInputId = null;
let activeCalendarTargetChbId = null;
let calendarCurrentYear = 1403;
let calendarCurrentMonth = 5;

const shamsiMonthNames = [
  'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
  'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'
];

function openShamsiCalendarPicker(targetInputId, chbId, event) {
  if (event) {
    event.preventDefault();
    event.stopPropagation();
  }

  activeCalendarTargetInputId = targetInputId;
  activeCalendarTargetChbId = chbId;

  const targetInput = document.getElementById(targetInputId);
  if (targetInput && targetInput.value.trim()) {
    const parts = targetInput.value.trim().split('/');
    if (parts.length === 3) {
      calendarCurrentYear = parseInt(parts[0]) || 1403;
      calendarCurrentMonth = parseInt(parts[1]) || 5;
    }
  } else {
    try {
      const todayStr = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
        year: 'numeric', month: '2-digit', day: '2-digit'
      }).format(new Date());
      const parts = todayStr.split('/');
      calendarCurrentYear = parseInt(parts[0]) || 1403;
      calendarCurrentMonth = parseInt(parts[1]) || 5;
    } catch(e) {}
  }

  renderShamsiCalendarPopup(event);
}

function renderShamsiCalendarPopup(event) {
  let popup = document.getElementById('shamsiCalendarPopup');
  if (!popup) {
    popup = document.createElement('div');
    popup.id = 'shamsiCalendarPopup';
    popup.className = 'shamsi-calendar-popup';
    document.body.appendChild(popup);
  }

  let daysInMonth = 30;
  if (calendarCurrentMonth <= 6) daysInMonth = 31;
  else if (calendarCurrentMonth < 12) daysInMonth = 30;
  else {
    const isKabise = ((calendarCurrentYear - 1399) % 4 === 0);
    daysInMonth = isKabise ? 30 : 29;
  }

  let daysHtml = '';
  for (let d = 1; d <= daysInMonth; d++) {
    const dStr = String(d).padStart(2, '0');
    daysHtml += `<button type="button" class="cal-day-btn" onclick="selectShamsiCalendarDay(${d})">${dStr}</button>`;
  }

  let yearOptionsHtml = '';
  for (let y = 1400; y <= 1408; y++) {
    const sel = y === calendarCurrentYear ? 'selected' : '';
    yearOptionsHtml += `<option value="${y}" ${sel}>${y}</option>`;
  }

  let monthOptionsHtml = '';
  shamsiMonthNames.forEach((mName, idx) => {
    const mNum = idx + 1;
    const sel = mNum === calendarCurrentMonth ? 'selected' : '';
    monthOptionsHtml += `<option value="${mNum}" ${sel}>${mName}</option>`;
  });

  popup.innerHTML = `
    <div class="cal-header">
      <button type="button" class="cal-nav-btn" onclick="changeShamsiCalendarMonth(-1)">◀</button>
      <div class="cal-title-selects">
        <select onchange="calendarCurrentMonth=parseInt(this.value); renderShamsiCalendarPopup();">${monthOptionsHtml}</select>
        <select onchange="calendarCurrentYear=parseInt(this.value); renderShamsiCalendarPopup();">${yearOptionsHtml}</select>
      </div>
      <button type="button" class="cal-nav-btn" onclick="changeShamsiCalendarMonth(1)">▶</button>
    </div>
    <div class="cal-weekdays">
      <span>ش</span><span>ی</span><span>د</span><span>س</span><span>چ</span><span>پ</span><span>ج</span>
    </div>
    <div class="cal-days-grid">
      ${daysHtml}
    </div>
    <div class="cal-footer">
      <button type="button" class="cal-btn-today" onclick="selectShamsiCalendarToday()">تاریخ امروز</button>
      <button type="button" class="cal-btn-close" onclick="closeShamsiCalendarPopup()">بستن</button>
    </div>
  `;

  popup.style.display = 'block';

  if (event) {
    let x = event.clientX || (event.target ? event.target.getBoundingClientRect().left : 200);
    let y = event.clientY || (event.target ? event.target.getBoundingClientRect().bottom : 200);

    const popupWidth = 260;
    const popupHeight = 310;

    if (x + popupWidth > window.innerWidth) x = window.innerWidth - popupWidth - 10;
    if (y + popupHeight > window.innerHeight) y = y - popupHeight - 10;

    popup.style.left = `${x}px`;
    popup.style.top = `${y}px`;
  }
}

function changeShamsiCalendarMonth(delta) {
  calendarCurrentMonth += delta;
  if (calendarCurrentMonth > 12) {
    calendarCurrentMonth = 1;
    calendarCurrentYear++;
  } else if (calendarCurrentMonth < 1) {
    calendarCurrentMonth = 12;
    calendarCurrentYear--;
  }
  renderShamsiCalendarPopup();
}

function selectShamsiCalendarDay(dayNum) {
  if (!activeCalendarTargetInputId) return;

  const yStr = String(calendarCurrentYear);
  const mStr = String(calendarCurrentMonth).padStart(2, '0');
  const dStr = String(dayNum).padStart(2, '0');
  const fullDateStr = `${yStr}/${mStr}/${dStr}`;

  const targetInput = document.getElementById(activeCalendarTargetInputId);
  if (targetInput) targetInput.value = fullDateStr;

  if (activeCalendarTargetChbId) {
    const chb = document.getElementById(activeCalendarTargetChbId);
    if (chb) chb.checked = true;
  }

  autoCalculateDtMultiOutageMin();
  closeShamsiCalendarPopup();
}

function selectShamsiCalendarToday() {
  try {
    const todayStr = new Intl.DateTimeFormat('fa-IR-u-nu-latn', {
      year: 'numeric', month: '2-digit', day: '2-digit'
    }).format(new Date()).replace(/\//g, '/');

    const targetInput = document.getElementById(activeCalendarTargetInputId);
    if (targetInput) targetInput.value = todayStr;

    if (activeCalendarTargetChbId) {
      const chb = document.getElementById(activeCalendarTargetChbId);
      if (chb) chb.checked = true;
    }
  } catch(e) {}
  closeShamsiCalendarPopup();
}

function closeShamsiCalendarPopup() {
  const popup = document.getElementById('shamsiCalendarPopup');
  if (popup) popup.style.display = 'none';
}

document.addEventListener('click', function(e) {
  const popup = document.getElementById('shamsiCalendarPopup');
  if (popup && popup.style.display === 'block') {
    if (!popup.contains(e.target) && !e.target.closest('.btn-calendar-trigger')) {
      closeShamsiCalendarPopup();
    }
  }
});




