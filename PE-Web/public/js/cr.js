// cr.js - Network CRs (Change Requests) Management, Comparison & Import Logic

let crAllRecords = [];
let crFilteredRecords = [];

/**
 * Main entrance when user clicks CR Tab
 */
async function loadCR() {
  const loading = document.getElementById('crLoading');
  const container = document.getElementById('crTableContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  const q = document.getElementById('crSearchInput')?.value || '';
  const status = document.getElementById('crFilterStatus')?.value || 'all';
  const workflow = document.getElementById('crFilterWorkflow')?.value || 'all';

  const queryParams = new URLSearchParams({
    q: q.trim(),
    status: status,
    workflowType: workflow
  });

  try {
    const res = await fetch(`/api/cr/list?${queryParams.toString()}`);
    const data = await res.json();

    if (data.success) {
      crAllRecords = data.data || [];
      renderCrTable(crAllRecords);
      updateCrRecordCount(crAllRecords.length);
      populateCrFilters(data.availableStatuses || [], data.availableWorkflows || []);
    }
  } catch (e) {
    console.error('CR load error:', e);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

function updateCrRecordCount(count) {
  const countEl = document.getElementById('crRecordCount');
  if (countEl) {
    countEl.innerText = `${(count || 0).toLocaleString('fa-IR')} رکورد سی‌آر`;
  }
}

function populateCrFilters(statuses, workflows) {
  const statusSelect = document.getElementById('crFilterStatus');
  const wfSelect = document.getElementById('crFilterWorkflow');

  if (statusSelect && statusSelect.dataset.populated !== 'true') {
    const cur = statusSelect.value;
    statusSelect.innerHTML = '<option value="all">تمام وضعیت‌ها</option>' +
      statuses.map(s => `<option value="${s}">${s}</option>`).join('');
    statusSelect.value = cur || 'all';
    statusSelect.dataset.populated = 'true';
  }

  if (wfSelect && wfSelect.dataset.populated !== 'true') {
    const curWf = wfSelect.value;
    wfSelect.innerHTML = '<option value="all">تمام فرآیندها</option>' +
      workflows.map(w => `<option value="${w}">${w}</option>`).join('');
    wfSelect.value = curWf || 'all';
    wfSelect.dataset.populated = 'true';
  }
}

function renderCrTable(records) {
  const headEl = document.getElementById('crTableHead');
  const bodyEl = document.getElementById('crTableBody');
  if (!headEl || !bodyEl) return;

  headEl.innerHTML = `
    <tr>
      <th style="width: 50px; text-align: center;">ردیف</th>
      <th style="width: 170px;">شناسه تیکت (Ticket ID)</th>
      <th>عنوان درخواست (Title)</th>
      <th style="width: 160px;">نوع فرآیند (Workflow)</th>
      <th style="width: 130px; text-align: center;">وضعیت (Status)</th>
      <th style="width: 100px; text-align: center;">حالت (State)</th>
      <th style="width: 90px; text-align: center;">تاخیر (Overtime)</th>
      <th style="width: 130px;">ارجاع‌دهنده</th>
      <th style="width: 130px; text-align: center;">زمان ارجاع</th>
      <th style="width: 140px; text-align: center;">آخرین ویرایش</th>
    </tr>
  `;

  if (records.length === 0) {
    bodyEl.innerHTML = `<tr><td colspan="10" style="text-align:center; padding: 28px; color: var(--text-muted);">هیچ رکوردی از سی‌آرها یافت نشد. فایل اکسل جدید را بارگذاری کنید.</td></tr>`;
    return;
  }

  let html = '';
  records.forEach((r, idx) => {
    let statusBadge = 'status-default';
    if (r.status?.includes('completed') || r.status?.includes('Success') || r.status?.includes('تکمیل')) {
      statusBadge = 'status-success';
    } else if (r.status?.includes('Reject') || r.status?.includes('Cancel') || r.status?.includes('رد')) {
      statusBadge = 'status-danger';
    } else if (r.status?.includes('running') || r.status?.includes('Processing')) {
      statusBadge = 'status-warning';
    }

    const isOt = String(r.isOvertime || '').trim();
    const otBadge = isOt === '1' || isOt.toLowerCase() === 'yes' || isOt.toLowerCase() === 'true'
      ? `<span class="cr-badge-ot-yes">⚠️ تاخیر</span>`
      : `<span class="cr-badge-ot-no">عادی</span>`;

    html += `
      <tr class="cr-row">
        <td style="text-align: center; color: var(--text-muted); font-weight: bold;">${(idx + 1).toLocaleString('fa-IR')}</td>
        <td>
          <strong style="font-family: monospace; color: var(--accent-cyan); font-size: 13px; direction: ltr; display: inline-block;">
            ${r.ticketId || '-'}
          </strong>
        </td>
        <td style="font-weight: 500; color: var(--text-primary); max-width: 320px;" title="${r.title || ''}">${r.title || '-'}</td>
        <td style="color: var(--text-secondary); font-size: 12px;">${r.workflowType || '-'}</td>
        <td style="text-align: center;">
          <span class="cr-status-badge ${statusBadge}">${r.status || '-'}</span>
        </td>
        <td style="text-align: center; font-size: 12px; color: var(--text-secondary);">${r.state || '-'}</td>
        <td style="text-align: center;">${otBadge}</td>
        <td style="color: var(--text-primary); font-size: 12px;">${r.dispatchedBy || '-'}</td>
        <td style="text-align: center; font-family: monospace; font-size: 11px; direction: ltr; color: var(--text-muted);">${r.dispatchTime || '-'}</td>
        <td style="text-align: center; font-family: monospace; font-size: 11px; direction: ltr; color: var(--text-secondary);">${r.lastModTime || '-'}</td>
      </tr>
    `;
  });

  bodyEl.innerHTML = html;
}

function filterCrTableGlobal(query) {
  loadCR();
}

// ==========================================
// EXCEL UPLOAD & MERGE OPERATIONS
// ==========================================
function openCrUploadModal() {
  document.getElementById('crUploadFileInput').value = '';
  document.getElementById('crSheetNameInput').value = 'Table 1';
  document.getElementById('crUploadFileNameDisplay').innerText = 'هنوز فایلی انتخاب نشده است';
  document.getElementById('crUploadResultBanner').style.display = 'none';

  const modal = document.getElementById('crUploadModal');
  if (modal) modal.style.display = 'flex';
}

function closeCrUploadModal() {
  const modal = document.getElementById('crUploadModal');
  if (modal) modal.style.display = 'none';
}

function handleCrFileSelected(input) {
  const file = input.files?.[0];
  const display = document.getElementById('crUploadFileNameDisplay');
  if (file && display) {
    display.innerText = `📄 ${file.name} (${(file.size / 1024).toFixed(1)} KB)`;
    display.style.color = '#38bdf8';
  }
}

async function executeCrExcelUpload() {
  const fileInput = document.getElementById('crUploadFileInput');
  const sheetInput = document.getElementById('crSheetNameInput');
  const btn = document.getElementById('btnSubmitCrUpload');
  const banner = document.getElementById('crUploadResultBanner');

  const file = fileInput?.files?.[0];
  if (!file) {
    alert('لطفاً یک فایل اکسل انتخاب نمایید.');
    return;
  }

  const formData = new FormData();
  formData.append('excelFile', file);
  formData.append('sheetName', (sheetInput?.value || 'Table 1').trim());

  if (btn) {
    btn.disabled = true;
    btn.innerText = '⏳ در حال پردازش و مقایسه...';
  }

  try {
    const res = await fetch('/api/cr/upload', {
      method: 'POST',
      body: formData
    });

    const data = await res.json();
    if (data.success) {
      if (banner) {
        banner.innerHTML = `
          <div style="background: rgba(16, 185, 129, 0.15); border: 1px solid #10b981; border-radius: 8px; padding: 12px; color: #34d399; margin-top: 10px;">
            <strong>✅ ${data.message}</strong>
            <div style="font-size: 12px; margin-top: 4px; color: #e2e8f0;">
              کل سطرهای پردازش‌شده: ${data.stats?.totalProcessed || 0} | شیت: ${data.stats?.sheetNameUsed || ''}
            </div>
          </div>
        `;
        banner.style.display = 'block';
      }
      await loadCR();
      setTimeout(() => {
        closeCrUploadModal();
      }, 1600);
    } else {
      alert(data.message || data.error || 'خطا در پردازش فایل اکسل');
    }
  } catch (err) {
    alert('خطا در ارتباط با سرور: ' + err.message);
  } finally {
    if (btn) {
      btn.disabled = false;
      btn.innerText = '🚀 بارگذاری و ادغام با سوابق';
    }
  }
}

// ==========================================
// DELETE ALL CR RECORDS (Two-Step Safety Check)
// ==========================================
async function deleteAllCrDataPrompt() {
  if (!confirm('⚠️ اخطار مهم: آیا مطمئن هستید که می‌خواهید تمام سوابق جدول سی‌آرها (tblTotalCr) را پاک‌سازی کنید؟')) {
    return;
  }

  if (!confirm('🚨 تایید نهایی: این عملیات غیرقابل بازگشت است. آیا از حذف قطعی همه داده‌ها اطمینان دارید؟')) {
    return;
  }

  try {
    const res = await fetch('/api/cr/all', {
      method: 'DELETE'
    });
    const data = await res.json();

    if (data.success) {
      await loadCR();
      alert(data.message || 'تمام داده‌های سی‌آر با موفقیت حذف گردید.');
    } else {
      alert(data.message || data.error || 'خطا در حذف داده‌ها');
    }
  } catch (err) {
    alert('خطا در ارتباط با سرور: ' + err.message);
  }
}

// ==========================================
// EXCEL EXPORT
// ==========================================
function exportCrToExcel() {
  if (!crAllRecords || crAllRecords.length === 0) {
    alert('رکوردی جهت خروجی اکسل وجود ندارد.');
    return;
  }

  let csvContent = '\uFEFF'; // UTF-8 BOM
  csvContent += 'ردیف,شناسه تیکت,عنوان,نوع فرآیند,وضعیت,حالت,تاخیر,ارجاع دهنده,زمان ارجاع,آخرین زمان ویرایش\n';

  crAllRecords.forEach((r, idx) => {
    const clean = (str) => `"${(str || '').replace(/"/g, '""')}"`;
    csvContent += [
      idx + 1,
      clean(r.ticketId),
      clean(r.title),
      clean(r.workflowType),
      clean(r.status),
      clean(r.state),
      clean(r.isOvertime),
      clean(r.dispatchedBy),
      clean(r.dispatchTime),
      clean(r.lastModTime)
    ].join(',') + '\n';
  });

  const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.setAttribute('href', url);
  link.setAttribute('download', `CR_Export_${new Date().toISOString().slice(0, 10)}.csv`);
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}

function openCrManagerModal() {
  const modal = document.getElementById('crManagerModal');
  if (modal) {
    modal.style.display = 'flex';
    loadCR();
  }
}

function closeCrManagerModal(e) {
  if (e && e.target && e.target.id !== 'crManagerModal' && !e.target.classList.contains('modal-close') && !e.target.classList.contains('btn-cancel-action')) {
    return;
  }
  const modal = document.getElementById('crManagerModal');
  if (modal) modal.style.display = 'none';
}
