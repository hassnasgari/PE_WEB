// pe.js - Advanced System Management, Audit Logging & User Access Control (RBAC)

let peCurrentSubTab = 'users';
let peLogsCurrentPage = 1;
let peLogsTotalPages = 1;
let peLogsPageLimit = 100;
let peAllUsersList = [];
let peAllRolesList = [];

/**
 * Main entrance when user clicks PE Tab
 */
async function loadPE() {
  await loadPeRoles();
  if (peCurrentSubTab === 'users') {
    await loadPeUsers();
  } else if (peCurrentSubTab === 'logs') {
    await loadPeLogs(1);
  } else if (peCurrentSubTab === 'backups') {
    await loadPeBackups();
  } else if (peCurrentSubTab === 'stats') {
    await loadPeSystemStats();
  } else if (peCurrentSubTab === 'clients') {
    await loadPeClients();
  } else if (peCurrentSubTab === 'vpns') {
    await loadPeVpns();
  }
}

/**
 * Switch Sub-Tabs inside PE Management Screen
 */
function switchPeSub(subName, btnEl) {
  peCurrentSubTab = subName;
  document.querySelectorAll('.pe-sub-btn').forEach(b => b.classList.remove('active'));
  document.querySelectorAll('.pe-sub-content').forEach(sc => sc.classList.remove('active'));

  if (btnEl) btnEl.classList.add('active');
  const target = document.getElementById(`pe-${subName}`);
  if (target) target.classList.add('active');

  if (subName === 'users') loadPeUsers();
  if (subName === 'logs') loadPeLogs(1);
  if (subName === 'backups') loadPeBackups();
  if (subName === 'stats') loadPeSystemStats();
  if (subName === 'clients') loadPeClients();
  if (subName === 'vpns') loadPeVpns();
}

// ==========================================
// 1. ROLES & PERMISSIONS LOADER
// ==========================================
async function loadPeRoles() {
  try {
    const res = await fetch('/api/pe/roles');
    const data = await res.json();
    if (data.success) {
      peAllRolesList = data.data || [];
      populateRoleSelectOptions();
    }
  } catch (e) {
    console.error('Error loading roles:', e);
  }
}

function populateRoleSelectOptions() {
  const roleSelect = document.getElementById('peModalUserRole');
  const filterRoleSelect = document.getElementById('peUserFilterRole');
  
  if (roleSelect) {
    roleSelect.innerHTML = peAllRolesList.map(r => 
      `<option value="${r.rol}">${r.rol} - (${r.Description || 'بدون توضیح'})</option>`
    ).join('');
  }

  if (filterRoleSelect) {
    filterRoleSelect.innerHTML = '<option value="all">تمام نقش‌ها</option>' + 
      peAllRolesList.map(r => `<option value="${r.rol}">${r.rol}</option>`).join('');
  }
}

// ==========================================
// 2. USER MANAGEMENT (مدیریت کاربران)
// ==========================================
async function loadPeUsers() {
  const loading = document.getElementById('peUsersLoading');
  const container = document.getElementById('peUsersContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/pe/users');
    const data = await res.json();
    if (data.success) {
      peAllUsersList = data.data || [];
      renderPeUsersTable(peAllUsersList);
      updatePeUserStats(peAllUsersList);
    }
  } catch (e) {
    console.error('Users load error:', e);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

function updatePeUserStats(users) {
  const totalEl = document.getElementById('peUsersTotalCount');
  if (totalEl) {
    totalEl.innerText = `${users.length.toLocaleString('fa-IR')} کاربر`;
  }
}

function renderPeUsersTable(users) {
  const headEl = document.getElementById('peUsersHead');
  const bodyEl = document.getElementById('peUsersBody');
  if (!headEl || !bodyEl) return;

  headEl.innerHTML = `
    <tr>
      <th style="width: 50px; text-align: center;">ردیف</th>
      <th>کاربر</th>
      <th>نام و نام خانوادگی</th>
      <th>سازمان / شرکت</th>
      <th style="text-align: center;">نقش و سطح دسترسی</th>
      <th style="width: 140px; text-align: center;">عملیات</th>
    </tr>
  `;

  if (users.length === 0) {
    bodyEl.innerHTML = `<tr><td colspan="6" style="text-align:center; padding: 24px; color: var(--text-muted);">هیچ کاربری با این مشخصات یافت نشد.</td></tr>`;
    return;
  }

  const roleColors = {
    'administrator': { bg: 'rgba(168, 85, 247, 0.15)', color: '#c084fc', border: '#a855f7', label: 'مدیر ارشد (Administrator)' },
    'PowerUser': { bg: 'rgba(56, 189, 248, 0.15)', color: '#38bdf8', border: '#0284c7', label: 'همکار واحد (PowerUser)' },
    'register': { bg: 'rgba(16, 185, 129, 0.15)', color: '#34d399', border: '#059669', label: 'ثبت‌کننده (Register)' },
    'TS': { bg: 'rgba(245, 158, 11, 0.15)', color: '#fbbf24', border: '#d97706', label: 'کارشناس TS' },
    'PM_VISIT': { bg: 'rgba(236, 72, 153, 0.15)', color: '#f472b6', border: '#db2777', label: 'بازدید و PM' },
    'Others': { bg: 'rgba(148, 163, 184, 0.15)', color: '#94a3b8', border: '#64748b', label: 'سایر دسترسی‌ها' },
    '---': { bg: 'rgba(239, 68, 68, 0.15)', color: '#f87171', border: '#dc2626', label: 'مسدود / غیرفعال' }
  };

  let html = '';
  users.forEach((u, idx) => {
    const roleMeta = roleColors[u.role] || { bg: 'rgba(148, 163, 184, 0.15)', color: '#cbd5e1', border: '#64748b', label: u.role || 'عادی' };
    const fullName = (u.fullName || '').trim() || (u.userName || '');
    const company = (u.company || 'مخابرات اصفهان').trim();
    const initials = fullName.split(' ').map(w => w[0]).slice(0, 2).join('') || u.userName.substring(0, 2);

    html += `
      <tr class="pe-user-row">
        <td style="text-align: center; color: var(--text-muted); font-weight: bold;">${(idx + 1).toLocaleString('fa-IR')}</td>
        <td>
          <div style="display: flex; align-items: center; gap: 10px;">
            <div class="pe-user-avatar">${initials}</div>
            <div>
              <strong style="color: var(--text-primary); font-family: monospace; font-size: 13px;">${u.userName}</strong>
            </div>
          </div>
        </td>
        <td style="font-weight: 500; color: var(--text-primary);">${fullName}</td>
        <td><span class="pe-company-pill">${company}</span></td>
        <td style="text-align: center;">
          <span class="pe-role-badge" style="background: ${roleMeta.bg}; color: ${roleMeta.color}; border: 1px solid ${roleMeta.border};">
            ${roleMeta.label}
          </span>
        </td>
        <td style="text-align: center;">
          <div style="display: inline-flex; gap: 6px;">
            <button class="pe-action-btn edit" onclick="openEditUserModal('${u.userName}')" title="ویرایش اطلاعات و نقش">
              ✏️ ویرایش
            </button>
            <button class="pe-action-btn delete" onclick="deleteUserPrompt('${u.userName}')" title="حذف کاربر">
              🗑️
            </button>
          </div>
        </td>
      </tr>
    `;
  });

  bodyEl.innerHTML = html;
}

function filterPeUsersGlobal() {
  const query = (document.getElementById('peUserSearchInput')?.value || '').toLowerCase().trim();
  const roleFilter = document.getElementById('peUserFilterRole')?.value || 'all';

  const filtered = peAllUsersList.filter(u => {
    const matchQuery = !query || 
      (u.userName && u.userName.toLowerCase().includes(query)) ||
      (u.fullName && u.fullName.toLowerCase().includes(query)) ||
      (u.company && u.company.toLowerCase().includes(query));

    const matchRole = roleFilter === 'all' || u.role === roleFilter;

    return matchQuery && matchRole;
  });

  renderPeUsersTable(filtered);
  updatePeUserStats(filtered);
}

// User Modal Operations
function openAddUserModal() {
  document.getElementById('peUserModalTitle').innerText = '➕ تعریف کاربر جدید';
  document.getElementById('peModalUsername').value = '';
  document.getElementById('peModalUsername').disabled = false;
  document.getElementById('peModalFullName').value = '';
  document.getElementById('peModalCompany').value = 'مخابرات استان اصفهان';
  document.getElementById('peModalPassword').value = '';
  document.getElementById('peModalPasswordGroup').style.display = 'block';
  document.getElementById('peModalPasswordLabel').innerText = 'رمز عبور *';
  document.getElementById('peModalIsEdit').value = 'false';

  const modal = document.getElementById('peUserModal');
  if (modal) modal.style.display = 'flex';
}

function openEditUserModal(username) {
  const user = peAllUsersList.find(u => u.userName === username);
  if (!user) return;

  document.getElementById('peUserModalTitle').innerText = `✏️ ویرایش کاربر: ${username}`;
  document.getElementById('peModalUsername').value = user.userName;
  document.getElementById('peModalUsername').disabled = true;
  document.getElementById('peModalFullName').value = user.fullName || '';
  document.getElementById('peModalCompany').value = user.company || 'مخابرات استان اصفهان';
  document.getElementById('peModalUserRole').value = user.role || 'register';
  document.getElementById('peModalPassword').value = '';
  document.getElementById('peModalPasswordGroup').style.display = 'block';
  document.getElementById('peModalPasswordLabel').innerText = 'کلمه عبور جدید (در صورت نیاز به تغییر):';
  document.getElementById('peModalIsEdit').value = 'true';

  const modal = document.getElementById('peUserModal');
  if (modal) modal.style.display = 'flex';
}

function closePeUserModal() {
  const modal = document.getElementById('peUserModal');
  if (modal) modal.style.display = 'none';
}

async function savePeUser() {
  const isEdit = document.getElementById('peModalIsEdit').value === 'true';
  const userName = document.getElementById('peModalUsername').value.trim();
  const fullName = document.getElementById('peModalFullName').value.trim();
  const company = document.getElementById('peModalCompany').value.trim();
  const role = document.getElementById('peModalUserRole').value;
  const password = document.getElementById('peModalPassword').value.trim();

  if (!userName) {
    alert('لطفاً نام کاربری را وارد فرمایید.');
    return;
  }

  if (!isEdit && !password) {
    alert('وارد کردن رمز عبور برای کاربر جدید الزامی است.');
    return;
  }

  try {
    let res, data;
    if (isEdit) {
      res = await fetch(`/api/pe/users/${encodeURIComponent(userName)}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ fullName, company, role, password })
      });
    } else {
      res = await fetch('/api/pe/users', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userName, fullName, company, role, password })
      });
    }

    data = await res.json();
    if (data.success) {
      closePeUserModal();
      await loadPeUsers();
      if (typeof showTopbarMsg === 'function') {
        showTopbarMsg(data.message || 'عملیات با موفقیت انجام شد', 'success');
      } else {
        alert(data.message || 'عملیات با موفقیت انجام شد');
      }
    } else {
      alert(data.message || data.error || 'خطا در ثبت اطلاعات');
    }
  } catch (err) {
    alert('خطا در برقراری ارتباط با سرور: ' + err.message);
  }
}

async function deleteUserPrompt(username) {
  if (!confirm(`آیا از حذف حساب کاربری "${username}" از سامانه اطمینان کامل دارید؟`)) {
    return;
  }

  try {
    const res = await fetch(`/api/pe/users/${encodeURIComponent(username)}`, {
      method: 'DELETE'
    });
    const data = await res.json();
    if (data.success) {
      await loadPeUsers();
      alert('کاربر با موفقیت حذف شد.');
    } else {
      alert(data.message || data.error || 'خطا در حذف کاربر');
    }
  } catch (e) {
    alert('خطا در ارتباط با سرور: ' + e.message);
  }
}

// ==========================================
// 3. AUDIT LOGS (سوابق و لاگ‌های سیستم)
// ==========================================
async function loadPeLogs(page = 1) {
  peLogsCurrentPage = page;
  const loading = document.getElementById('peLogsLoading');
  const container = document.getElementById('peLogsContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  const q = document.getElementById('peLogSearchInput')?.value || '';
  const user = document.getElementById('peLogUserFilter')?.value || 'all';
  const actionType = document.getElementById('peLogActionFilter')?.value || 'all';
  const period = document.getElementById('peLogPeriodFilter')?.value || 'all';

  const queryParams = new URLSearchParams({
    page: peLogsCurrentPage,
    limit: peLogsPageLimit,
    q: q.trim(),
    user: user,
    actionType: actionType,
    period: period
  });

  try {
    const res = await fetch(`/api/pe/logs?${queryParams.toString()}`);
    const data = await res.json();

    if (data.success) {
      peLogsTotalPages = data.pagination?.totalPages || 1;
      renderPeLogsTable(data.data || []);
      renderPeLogsPagination(data.pagination);
      populatePeLogUsersFilter(data.availableUsers || []);
      
      const countEl = document.getElementById('peLogsTotalCount');
      if (countEl && data.pagination?.total !== undefined) {
        countEl.innerText = `${data.pagination.total.toLocaleString('fa-IR')} رکورد لاگ`;
      }
    }
  } catch (e) {
    console.error('Logs load error:', e);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

function populatePeLogUsersFilter(users) {
  const select = document.getElementById('peLogUserFilter');
  if (!select || select.dataset.populated === 'true') return;

  const currentVal = select.value;
  select.innerHTML = '<option value="all">تمام کاربران</option>' +
    users.map(u => {
      const uName = typeof u === 'string' ? u : u.userName;
      const uFull = typeof u === 'object' && u.userFullName ? ` (${u.userFullName})` : '';
      return `<option value="${uName}">${uName}${uFull}</option>`;
    }).join('');
  select.value = currentVal || 'all';
  select.dataset.populated = 'true';
}

function renderPeLogsTable(logs) {
  const headEl = document.getElementById('peLogsHead');
  const bodyEl = document.getElementById('peLogsBody');
  if (!headEl || !bodyEl) return;

  headEl.innerHTML = `
    <tr>
      <th style="width: 70px; text-align: center;">شناسه</th>
      <th style="width: 120px;">نام کاربری</th>
      <th style="width: 150px;">نام و نام خانوادگی</th>
      <th>شرح فعالیت و رویداد ثبت‌شده</th>
      <th style="width: 150px;">تاریخ شمسی</th>
      <th style="width: 80px; text-align: center;">ساعت</th>
      <th style="width: 110px;">آدرس IP</th>
      <th style="width: 140px;">سیستم / کامپیوتر</th>
    </tr>
  `;

  if (logs.length === 0) {
    bodyEl.innerHTML = `<tr><td colspan="8" style="text-align:center; padding: 24px; color: var(--text-muted);">هیچ لاگ یا رخدادی در این بازه یافت نشد.</td></tr>`;
    return;
  }

  let html = '';
  logs.forEach(log => {
    const action = log.action || '';
    let badgeClass = 'pe-log-default';
    let icon = '📝';

    if (action.includes('ثبت') || action.includes('افزودن') || action.includes('ایجاد')) {
      badgeClass = 'pe-log-create';
      icon = '🟢';
    } else if (action.includes('ویرایش') || action.includes('تغییر') || action.includes('بروزرسانی')) {
      badgeClass = 'pe-log-update';
      icon = '🟡';
    } else if (action.includes('حذف') || action.includes('پاک')) {
      badgeClass = 'pe-log-delete';
      icon = '🔴';
    } else if (action.includes('ورود') || action.includes('لاگین')) {
      badgeClass = 'pe-log-auth';
      icon = '🔵';
    } else if (action.includes('ناموفق') || action.includes('خطا')) {
      badgeClass = 'pe-log-danger';
      icon = '⚠️';
    }

    const fullNameDisplay = (log.userFullName && log.userFullName.trim()) 
      || (log.fullName && log.fullName.trim()) 
      || (log.userName || '-');

    html += `
      <tr class="pe-log-row">
        <td style="text-align: center; font-family: monospace; font-weight: bold; color: var(--text-muted);">${log.id}</td>
        <td>
          <div style="display: flex; align-items: center; gap: 6px;">
            <span style="font-size: 14px;">👤</span>
            <strong style="color: var(--text-primary); font-family: monospace;">${log.userName || '-'}</strong>
          </div>
        </td>
        <td>
          <span style="color: var(--accent-primary, #6366f1); font-weight: 600;">${fullNameDisplay}</span>
        </td>
        <td>
          <div class="pe-log-action-wrapper ${badgeClass}">
            <span class="pe-log-icon">${icon}</span>
            <span class="pe-log-text">${action}</span>
          </div>
        </td>
        <td style="color: var(--text-secondary); font-size: 12px; font-weight: 500;">${log.date || '-'}</td>
        <td style="text-align: center; font-family: monospace; color: var(--text-primary); direction: ltr;">${log.time || '-'}</td>
        <td style="font-family: monospace; color: var(--accent-cyan); font-size: 12px; direction: ltr;">${log.Ip || '-'}</td>
        <td style="color: var(--text-muted); font-size: 12px;">${log.computerName || '-'}</td>
      </tr>
    `;
  });

  bodyEl.innerHTML = html;
}

function renderPeLogsPagination(pagination) {
  const container = document.getElementById('peLogsPaginationContainer');
  if (!container || !pagination) return;

  const { page, totalPages, total } = pagination;
  if (totalPages <= 1) {
    container.innerHTML = `<span style="color: var(--text-muted); font-size: 12px;">مجموع: ${total.toLocaleString('fa-IR')} رکورد</span>`;
    return;
  }

  container.innerHTML = `
    <div class="pe-pagination-wrapper">
      <button class="pe-page-btn" ${page <= 1 ? 'disabled' : ''} onclick="loadPeLogs(1)">« اول</button>
      <button class="pe-page-btn" ${page <= 1 ? 'disabled' : ''} onclick="loadPeLogs(${page - 1})">‹ قبلی</button>
      <span class="pe-page-info">صفحه ${page.toLocaleString('fa-IR')} از ${totalPages.toLocaleString('fa-IR')}</span>
      <button class="pe-page-btn" ${page >= totalPages ? 'disabled' : ''} onclick="loadPeLogs(${page + 1})">بعدی ›</button>
      <button class="pe-page-btn" ${page >= totalPages ? 'disabled' : ''} onclick="loadPeLogs(${totalPages})">آخر »</button>
    </div>
  `;
}

// ==========================================
// 4. SYSTEM STATS & HEALTH (آمار و وضعیت سیستم)
// ==========================================
async function loadPeSystemStats() {
  const container = document.getElementById('peStatsContainer');
  if (!container) return;

  try {
    const res = await fetch('/api/pe/system-stats');
    const data = await res.json();
    if (data.success && data.stats) {
      const s = data.stats;
      document.getElementById('statTotalUsers').innerText = (s.totalUsers || 0).toLocaleString('fa-IR');
      document.getElementById('statTotalLogs').innerText = (s.totalLogs || 0).toLocaleString('fa-IR');
      document.getElementById('statTotalDowntimes').innerText = (s.totalDowntimes || 0).toLocaleString('fa-IR');
      document.getElementById('statTotalSites').innerText = (s.totalSites || 0).toLocaleString('fa-IR');
      document.getElementById('statTotalContacts').innerText = (s.totalContacts || 0).toLocaleString('fa-IR');
      document.getElementById('statDbServer').innerText = s.dbServer || '172.16.3.116';
    }
  } catch (e) {
    console.error('Stats error:', e);
  }
}

// ==========================================
// 5. CLIENTS & VPNS
// ==========================================
async function loadPeClients() {
  const loading = document.getElementById('peClientsLoading');
  const container = document.getElementById('peClientsContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/pe/clients');
    const data = await res.json();
    if (data.success) {
      renderGenericTable(data.data || [], 'peClientsHead', 'peClientsBody');
    }
  } catch (e) {
    console.error('Clients load error:', e);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

async function loadPeVpns() {
  const loading = document.getElementById('peVpnsLoading');
  const container = document.getElementById('peVpnsContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/pe/vpns');
    const data = await res.json();
    if (data.success) {
      renderGenericTable(data.data || [], 'peVpnsHead', 'peVpnsBody');
    }
  } catch (e) {
    console.error('Vpns load error:', e);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

function renderGenericTable(data, headId, bodyId) {
  const headEl = document.getElementById(headId);
  const bodyEl = document.getElementById(bodyId);
  if (!headEl || !bodyEl) return;

  if (data.length === 0) {
    headEl.innerHTML = '<tr><th>پیام</th></tr>';
    bodyEl.innerHTML = '<tr><td style="text-align:center; padding:15px; color: var(--text-muted);">اطلاعاتی یافت نشد</td></tr>';
    return;
  }

  const keys = Object.keys(data[0]);
  let headHtml = '<tr><th style="width:60px; text-align:center;">ردیف</th>';
  keys.forEach(k => headHtml += `<th>${k}</th>`);
  headHtml += '</tr>';
  headEl.innerHTML = headHtml;

  let bodyHtml = '';
  data.forEach((row, idx) => {
    bodyHtml += `<tr><td style="text-align:center; color: var(--text-muted); font-weight:bold;">${(idx + 1).toLocaleString('fa-IR')}</td>`;
    keys.forEach(k => {
      bodyHtml += `<td>${row[k] !== null && row[k] !== undefined ? row[k] : ''}</td>`;
    });
    bodyHtml += '</tr>';
  });
  bodyEl.innerHTML = bodyHtml;
}

// ==========================================
// 6. DATABASE BACKUP MANAGEMENT (مدیریت پشتیبان‌گیری)
// ==========================================
async function loadPeBackups() {
  const loading = document.getElementById('peBackupsLoading');
  const container = document.getElementById('peBackupsContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/backup/backups');
    const data = await res.json();
    if (data.success) {
      renderPeBackupsTable(data.data || []);
    }
  } catch (e) {
    console.error('Backups load error:', e);
  } finally {
    if (loading) loading.style.display = 'none';
    if (container) container.style.display = 'block';
  }
}

function renderPeBackupsTable(backups) {
  const headEl = document.getElementById('peBackupsHead');
  const bodyEl = document.getElementById('peBackupsBody');
  if (!headEl || !bodyEl) return;

  headEl.innerHTML = `
    <tr>
      <th style="width: 50px; text-align: center;">ردیف</th>
      <th>نام فایل پشتیبان</th>
      <th style="width: 140px; text-align: center;">حجم فایل</th>
      <th style="width: 180px; text-align: center;">تاریخ ایجاد</th>
      <th style="width: 120px; text-align: center;">دریافت فایل</th>
    </tr>
  `;

  if (backups.length === 0) {
    bodyEl.innerHTML = `<tr><td colspan="5" style="text-align:center; padding: 24px; color: var(--text-muted);">هنوز هیچ فایل پشتیبانی تهیه نشده است. از دکمه‌های بالا استفاده کنید.</td></tr>`;
    return;
  }

  let html = '';
  backups.forEach((b, idx) => {
    html += `
      <tr>
        <td style="text-align: center; color: var(--text-muted); font-weight: bold;">${(idx + 1).toLocaleString('fa-IR')}</td>
        <td>
          <div style="display: flex; align-items: center; gap: 8px;">
            <span style="font-size: 16px;">💾</span>
            <strong style="font-family: monospace; color: var(--text-primary); font-size: 12px; direction: ltr;">${b.filename}</strong>
          </div>
        </td>
        <td style="text-align: center; font-weight: 600; color: #38bdf8;">${b.sizeFormatted}</td>
        <td style="text-align: center; color: var(--text-secondary); font-size: 12px;">${b.createdDateStr || '-'}</td>
        <td style="text-align: center;">
          <a href="/api/backup/download-backup/${encodeURIComponent(b.filename)}" class="pe-action-btn edit" style="text-decoration: none; display: inline-flex; align-items: center; gap: 4px;" download>
            📥 دانلود
          </a>
        </td>
      </tr>
    `;
  });

  bodyEl.innerHTML = html;
}

async function executeDatabaseBackup(dbName) {
  const btn = document.getElementById(`btnBackup_${dbName}`);
  if (btn) {
    btn.disabled = true;
    btn.innerText = '⏳ در حال بک‌آپ...';
  }

  try {
    const res = await fetch('/api/backup/backup-database', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ dbName })
    });
    const data = await res.json();

    if (data.success) {
      alert(data.message || 'پشتیبان‌گیری با موفقیت انجام شد.');
      await loadPeBackups();
    } else {
      alert(data.message || data.error || 'خطا در پشتیبان‌گیری');
    }
  } catch (e) {
    alert('خطا در ارتباط با سرور: ' + e.message);
  } finally {
    if (btn) {
      btn.disabled = false;
      btn.innerText = `💾 بک‌آپ ${dbName === 'ALL' ? 'کامل (همه)' : dbName}`;
    }
  }
}
