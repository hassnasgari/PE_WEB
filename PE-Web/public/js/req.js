let rawReqData = [];
let filteredReqData = [];
let reqColumnFilters = {};

async function loadREQ() {
  const loading = document.getElementById('reqLoading');
  const container = document.getElementById('reqTableContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/req/list');
    const result = await res.json();

    if (!result.success) {
      alert('خطا در دریافت درخواست‌های تغییر: ' + result.error);
      return;
    }

    rawReqData = result.data || [];
    applyReqFilters();
    renderReqTable();
    updateReqBadgeCount();
  } catch (err) {
    console.error('loadREQ error:', err);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

function updateReqBadgeCount() {
  const unseenCount = rawReqData.filter(r => String(r.status || '').trim() === 'منتظر بررسی').length;
  const reqBadge = document.getElementById('reqBadge');

  if (reqBadge) {
    if (unseenCount > 0) {
      reqBadge.innerText = unseenCount.toLocaleString('fa-IR');
      reqBadge.style.display = 'inline-block';
    } else {
      reqBadge.style.display = 'none';
    }
  }
}

function applyReqFilters() {
  const globalSearch = (document.getElementById('reqSearchInput')?.value || '').trim().toLowerCase();

  filteredReqData = rawReqData.filter(row => {
    if (globalSearch) {
      const matchGlobal = Object.values(row).some(val => 
        val !== null && val !== undefined && String(val).toLowerCase().includes(globalSearch)
      );
      if (!matchGlobal) return false;
    }

    for (const [colKey, filterVal] of Object.entries(reqColumnFilters)) {
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

function filterReqTableGlobal(query) {
  applyReqFilters();
  renderReqTable();
}

function updateReqColumnFilter(colKey, value, inputEl) {
  reqColumnFilters[colKey] = value;

  const cursorStart = inputEl ? inputEl.selectionStart : null;
  const cursorEnd = inputEl ? inputEl.selectionEnd : null;

  applyReqFilters();
  renderReqTable();

  const newInput = document.querySelector(`#tab-Req .column-filter-input[data-col="${colKey}"]`);
  if (newInput) {
    newInput.focus();
    if (cursorStart !== null && cursorEnd !== null) {
      try { newInput.setSelectionRange(cursorStart, cursorEnd); } catch (e) {}
    }
  }
}

function renderReqTable() {
  const head = document.getElementById('reqTableHead');
  const body = document.getElementById('reqTableBody');
  const foot = document.getElementById('reqTableFoot');

  if (!head || !body) return;

  if (filteredReqData.length === 0) {
    head.innerHTML = '';
    body.innerHTML = '<tr><td colspan="10" style="text-align:center;padding:20px;">هیچ درخواستی پیدا نشد</td></tr>';
    if (foot) foot.innerHTML = '';
    return;
  }

  const columns = [
    { key: 'reqNo', label: 'شماره', width: '50px' },
    { key: 'user', label: 'کاربر', width: '75px' },
    { key: 'requester', label: 'درخواست‌کننده', width: '100px' },
    { key: 'tel', label: 'تلفن', width: '80px' },
    { key: 'request', label: 'متن درخواست', width: '290px' },
    { key: 'requestDateTime', label: 'تاریخ و زمان', width: '110px' },
    { key: 'status', label: 'وضعیت', width: '170px' },
    { key: 'comments', label: 'توضیحات / دلیل رد', width: '120px' },
    { key: 'action', label: 'اقدام و تصمیم‌گیری', width: '160px' }
  ];

  // Header
  let headerHtml = '<tr>';
  columns.forEach(col => {
    headerHtml += `<th style="width:${col.width}; min-width:${col.width}; text-align:center; padding:4px 5px; font-size:11px;">${col.label}</th>`;
  });
  headerHtml += '</tr>';

  // Auto filter
  headerHtml += '<tr class="filter-row">';
  columns.forEach(col => {
    const val = reqColumnFilters[col.key] || '';
    headerHtml += `<th class="filter-cell" style="width:${col.width}; min-width:${col.width};"><input class="column-filter-input" data-col="${col.key}" type="text" value="${val}" oninput="updateReqColumnFilter('${col.key}', this.value, this)" placeholder="فیلتر..."></th>`;
  });
  headerHtml += '</tr>';

  head.innerHTML = headerHtml;

  // Body
  let bodyHtml = '';
  filteredReqData.forEach(row => {
    const statusText = String(row.status || '').trim();
    const isPending = statusText === 'منتظر بررسی' || !statusText;
    const isApproved = statusText.includes('موافقت گردید');
    const isRejected = statusText.includes('موافقت نگردید');

    let bgStyle = '';
    let badgeClass = 'status-badge';
    let badgeText = statusText || 'منتظر بررسی';

    if (isPending) {
      bgStyle = 'background-color: rgba(245, 158, 11, 0.12);';
      badgeClass += ' status-pending';
      badgeText = 'منتظر بررسی';
    } else if (isApproved) {
      bgStyle = 'background-color: rgba(16, 185, 129, 0.10);';
      badgeClass += ' status-approved';
      badgeText = '✔ موافقت گردید';
    } else if (isRejected) {
      bgStyle = 'background-color: rgba(239, 68, 68, 0.10);';
      badgeClass += ' status-rejected';
      badgeText = '✖ موافقت نگردید';
    } else {
      badgeClass += ' status-seen';
      badgeText = statusText || 'دیده‌شده و در حال بررسی';
    }

    bodyHtml += `<tr style="${bgStyle}">`;
    columns.forEach(col => {
      if (col.key === 'reqNo') {
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px;">${row.reqNo}</td>`;
      } else if (col.key === 'user') {
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px; white-space:normal; word-break:break-word;">${row.user || ''}</td>`;
      } else if (col.key === 'requester') {
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px; white-space:normal; word-break:break-word;">${row.requester || ''}</td>`;
      } else if (col.key === 'tel') {
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px;">${row.tel || ''}</td>`;
      } else if (col.key === 'request') {
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:right; white-space:normal; word-break:break-word; padding:3px 6px; font-size:11px;">${row.request || ''}</td>`;
      } else if (col.key === 'requestDateTime') {
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px;">${row.requestDateTime || ''}</td>`;
      } else if (col.key === 'status') {
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px; white-space:normal; word-break:break-word;"><span class="${badgeClass}">${badgeText}</span></td>`;
      } else if (col.key === 'comments') {
        const commentStyle = isRejected ? 'color:#dc2626; font-weight:600;' : 'color:inherit;';
        bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:right; white-space:normal; word-break:break-word; padding:3px 5px; font-size:11px; ${commentStyle}">${row.comments || ''}</td>`;
      } else if (col.key === 'action') {
        if (isPending) {
          bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px; white-space:normal;">
            <button class="btn-approve" onclick="approveReq(${row.reqNo})" title="موافقت با درخواست">✔ موافقت</button>
            <button class="btn-reject" onclick="rejectReq(${row.reqNo})" title="عدم موافقت و ورود دلیل">✖ رد درخواست</button>
          </td>`;
        } else if (isApproved) {
          bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px;"><span style="color:#10b981; font-weight:bold;">✔ تایید شد</span></td>`;
        } else if (isRejected) {
          bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px;"><span style="color:#ef4444; font-weight:bold;">✖ رد شد</span></td>`;
        } else {
          bodyHtml += `<td style="width:${col.width}; min-width:${col.width}; text-align:center; padding:3px 5px; font-size:11px;"><span style="color:#10b981; font-weight:bold;">✓ دیده‌شده</span></td>`;
        }
      }
    });
    bodyHtml += '</tr>';
  });

  body.innerHTML = bodyHtml;

  if (foot) foot.innerHTML = '';

  // Update Status Bar Record Count
  if (typeof updateStatusRecordCount === 'function') {
    updateStatusRecordCount(filteredReqData.length);
  }
}

// Open / Close Create Request Modal
function openCreateReqModal() {
  const modal = document.getElementById('createReqModal');
  const requesterEl = document.getElementById('reqInputRequester');
  
  if (currentUser) {
    if (requesterEl) {
      requesterEl.value = currentUser.fullName || currentUser.username || '';
      requesterEl.readOnly = true;
      requesterEl.style.backgroundColor = 'var(--header-bg)';
      requesterEl.style.cursor = 'not-allowed';
    }
  }

  if (modal) modal.style.display = 'flex';
}

function closeCreateReqModal() {
  const modal = document.getElementById('createReqModal');
  if (modal) modal.style.display = 'none';
}

// Submit New Request
async function submitNewReq() {
  const requesterEl = document.getElementById('reqInputRequester');
  const telEl = document.getElementById('reqInputTel');
  const textEl = document.getElementById('reqInputText');

  const requester = requesterEl ? requesterEl.value.trim() : '';
  const tel = telEl ? telEl.value.trim() : '';
  const requestText = textEl ? textEl.value.trim() : '';

  if (!requester) {
    alert('لطفاً نام درخواست‌کننده را وارد کنید.');
    return;
  }
  if (!tel) {
    alert('لطفاً شماره تماس را وارد کنید.');
    return;
  }
  if (!requestText) {
    alert('لطفاً متن شرح درخواست تغییرات را وارد کنید.');
    return;
  }

  try {
    const res = await fetch('/api/req/create', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ requester, tel, requestText })
    });
    const data = await res.json();

    if (data.success) {
      alert('✅ درخواست شما با موفقیت ثبت شد و هشدار چشمک‌زن برای سیستم فعال گردید.');
      closeCreateReqModal();
      if (textEl) textEl.value = '';
      if (telEl) telEl.value = '';
      loadREQ();
    } else {
      alert('خطا در ثبت درخواست: ' + (data.error || data.message));
    }
  } catch (err) {
    console.error('submitNewReq error:', err);
    alert('خطا در ارتباط با سرور.');
  }
}

// Approve Request
async function approveReq(reqNo) {
  if (!confirm(`آیا از تایید و موافقت با درخواست شماره ${reqNo} اطمینان دارید؟`)) return;

  try {
    const res = await fetch('/api/req/update-status', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ reqNo, status: 'با درخواست موافقت گردید', comments: '' })
    });
    const data = await res.json();

    if (data.success) {
      alert(`✅ موافقت با درخواست شماره ${reqNo} ثبت شد.`);
      loadREQ();
    } else {
      alert('خطا در ثبت موافقت: ' + (data.error || data.message));
    }
  } catch (err) {
    console.error('approveReq error:', err);
    alert('خطا در ارتباط با سرور.');
  }
}

// Reject Request with Reason Comment
async function rejectReq(reqNo) {
  const reason = prompt(`لطفاً علت عدم موافقت / رد درخواست شماره ${reqNo} را وارد کنید:`);
  if (reason === null) return; // User cancelled
  if (!reason.trim()) {
    alert('وارد کردن دلیل عدم موافقت الزامی است.');
    return;
  }

  try {
    const res = await fetch('/api/req/update-status', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ reqNo, status: 'با درخواست موافقت نگردید', comments: reason.trim() })
    });
    const data = await res.json();

    if (data.success) {
      alert(`❌ عدم موافقت با درخواست شماره ${reqNo} و دلیل مربوطه با موفقیت ثبت شد.`);
      loadREQ();
    } else {
      alert('خطا در ثبت رد درخواست: ' + (data.error || data.message));
    }
  } catch (err) {
    console.error('rejectReq error:', err);
    alert('خطا در ارتباط با سرور.');
  }
}
