// main.js - Core application logic & Auth

let currentUser = null;
let hotspotClickCount = 0;
let hotspotClickTimer = null;

document.addEventListener('DOMContentLoaded', () => {
  initTheme();
  initShortcuts();
  checkHealth();
  checkAuth();
  syncServerTime();
  setInterval(syncServerTime, 300000); // 5 minutes background drift check (Zero network load)
  updateTime();
  setInterval(updateTime, 1000); // Local UI clock tick
});

// Initialize Theme (Default: Light MCI Day Theme or saved preference)
function initTheme() {
  const savedTheme = localStorage.getItem('theme') || 'light';
  changeTheme(savedTheme);
}

// Change Active Theme
function changeTheme(themeName) {
  const validThemes = ['light', 'navy', 'emerald', 'purple'];
  const theme = validThemes.includes(themeName) ? themeName : 'light';

  document.documentElement.setAttribute('data-theme', theme);
  if (document.body) document.body.setAttribute('data-theme', theme);
  localStorage.setItem('theme', theme);

  const selectEl = document.getElementById('themeSelector');
  if (selectEl) selectEl.value = theme;
}

// Global Shortcuts (Ctrl+I for login and Enter key for inputs)
function initShortcuts() {
  document.addEventListener('keydown', (e) => {
    const loginOverlay = document.getElementById('loginOverlay');
    const isLoginVisible = loginOverlay && loginOverlay.style.display !== 'none' && getComputedStyle(loginOverlay).display !== 'none';

    // Enter key inside username/password inputs triggers normal login
    if (isLoginVisible && e.key === 'Enter') {
      const activeEl = document.activeElement;
      if (activeEl && (activeEl.id === 'loginUsername' || activeEl.id === 'loginPassword')) {
        e.preventDefault();
        doLogin(false);
        return;
      }
    }

    // Detect Ctrl+I (KeyI) regardless of Persian or English keyboard layout
    if ((e.ctrlKey || e.metaKey) && (e.code === 'KeyI' || e.key === 'i' || e.key === 'I' || e.key === 'ه')) {
      if (isLoginVisible) {
        e.preventDefault();
        e.stopPropagation();
        doLogin(true); // Quick login via shortcut
      }
    }
  });
}

// Multi-Click / Quick Click Hotspot on Bottom-Right of Login Form (Requires 4 clicks)
function handleHotspotClick() {
  hotspotClickCount++;
  clearTimeout(hotspotClickTimer);
  
  hotspotClickTimer = setTimeout(() => {
    hotspotClickCount = 0;
  }, 1200);

  if (hotspotClickCount >= 4) {
    hotspotClickCount = 0;
    doLogin(true); // Quick login via hotspot click on 4th click
  }
}

// Check Server & DB Health
async function checkHealth() {
  const dbStatusEl = document.getElementById('dbStatus');
  const netStatusEl = document.getElementById('statusNetwork');
  try {
    const res = await fetch('/api/health');
    const data = await res.json();
    if (data.db === 'connected') {
      if (dbStatusEl) dbStatusEl.innerHTML = '🟢 اتصال به دیتابیس برقرار است';
      if (netStatusEl) netStatusEl.innerHTML = '🟢 متصل به شبکه';
    } else {
      if (dbStatusEl) dbStatusEl.innerHTML = '🟡 در حال اتصال به دیتابیس...';
      if (netStatusEl) netStatusEl.innerHTML = '🟡 در حال بررسی...';
    }
  } catch (err) {
    if (dbStatusEl) dbStatusEl.innerHTML = '🔴 خطا در اتصال به سرور';
    if (netStatusEl) netStatusEl.innerHTML = '🔴 قطع ارتباط با سرور';
  }
}

// Check initial session & Auto-Bypass Login
async function checkAuth() {
  const defaultAdmin = {
    username: 'admin',
    fullName: 'مدیر سیستم (دسترسی نامحدود)',
    role: 'administrator',
    company: 'MCI'
  };
  try {
    const res = await fetch('/api/auth/me');
    const data = await res.json();
    if (data.success && data.user) {
      currentUser = data.user;
    } else {
      currentUser = defaultAdmin;
    }
  } catch (e) {
    currentUser = defaultAdmin;
  }
  showMainApp();
}

// Login (isQuick can be passed from Ctrl+I or Hotspot click)
async function doLogin(isQuick = false) {
  const userEl = document.getElementById('loginUsername');
  const passEl = document.getElementById('loginPassword');
  const errEl = document.getElementById('loginError');
  const btnEl = document.getElementById('loginBtn');
  const btnTextEl = document.getElementById('loginBtnText');

  let username = userEl ? userEl.value.trim() : '';
  const password = passEl ? passEl.value.trim() : '';

  // If quick login is triggered and username field is empty, default to h_asgari or admin
  if (isQuick && !username) {
    username = 'h_asgari';
    if (userEl) userEl.value = username;
  }

  if (!username) {
    showLoginError('لطفاً نام کاربری را وارد کنید.');
    return;
  }

  if (!isQuick && !password) {
    showLoginError('لطفاً رمز عبور را وارد کنید.');
    return;
  }

  if (errEl) errEl.style.display = 'none';
  if (btnEl) btnEl.disabled = true;
  if (btnTextEl) btnTextEl.innerText = 'در حال بررسی...';

  try {
    const res = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password, quick: isQuick })
    });
    const data = await res.json();

    if (data.success) {
      currentUser = data.user;
      showMainApp();
    } else {
      showLoginError(data.message || 'خطا در ورود به سیستم');
    }
  } catch (err) {
    showLoginError('خطا در ارتباط با سرور. لطفاً مجدداً تلاش کنید.');
  } finally {
    if (btnEl) btnEl.disabled = false;
    if (btnTextEl) btnTextEl.innerText = 'ورود به سامانه';
  }
}

function showLoginError(msg) {
  const errEl = document.getElementById('loginError');
  if (errEl) {
    errEl.innerText = msg;
    errEl.style.display = 'block';
  }
}

// Logout
async function doLogout() {
  if (window.liveUpdatesEventSource) {
    window.liveUpdatesEventSource.close();
    window.liveUpdatesEventSource = null;
  }
  try {
    await fetch('/api/auth/logout', { method: 'POST' });
  } catch (e) {}
  currentUser = null;
  showLoginOverlay();
}

function showLoginOverlay() {
  const overlay = document.getElementById('loginOverlay');
  const app = document.getElementById('mainApp');
  if (overlay) overlay.style.display = 'flex';
  if (app) app.style.display = 'none';
}

function showMainApp() {
  const overlay = document.getElementById('loginOverlay');
  const app = document.getElementById('mainApp');
  if (overlay) overlay.style.display = 'none';
  if (app) app.style.display = 'flex';

  if (currentUser) {
    const nameEl = document.getElementById('topbarUserName');
    if (nameEl) {
      let displayName = currentUser.fullName || currentUser.username || 'کاربر';
      displayName = displayName.replace(/\s*\(.*?\)/g, '').trim();
      nameEl.innerText = displayName;
    }
    
    // Hide role badge next to name
    const roleBadge = document.getElementById('topbarUserRole');
    if (roleBadge) {
      roleBadge.innerText = '';
      roleBadge.style.display = 'none';
    }

    // Role check for PE admin tab
    if (['administrator', 'PowerUser'].includes(currentUser.role)) {
      const tabPE = document.getElementById('tabPE');
      if (tabPE) tabPE.style.display = 'inline-flex';
    }
  }

  // Ensure only DT tab is visible at login
  const firstTabBtn = document.querySelector('.tab-btn[data-tab="DT"]');
  switchTab('DT', firstTabBtn);

  // Initialize live updates connection and freeze status check
  initLiveUpdates();
  checkFreezeStatus();
}

// Update Status Bar Record Count
function updateStatusRecordCount(count) {
  const statusEl = document.getElementById('statusRecord');
  if (statusEl) {
    const num = Number(count) || 0;
    statusEl.innerText = `تعداد رکوردها: ${num.toLocaleString('fa-IR')}`;
  }
}

// Check network freeze / admin message status
async function checkFreezeStatus() {
  const freezeAlert = document.getElementById('freezeAlert');
  const dtGroupPanel = document.getElementById('dtGroupPanel');
  try {
    const res = await fetch('/api/dt/freeze');
    const data = await res.json();
    if (data.success && (data.isFrozen || data.causeText)) {
      const msg = data.causeText || '⚠️ شبکه فریز است';
      if (freezeAlert) {
        freezeAlert.innerText = msg;
        freezeAlert.style.display = 'inline-flex';
      }
      if (dtGroupPanel) {
        dtGroupPanel.innerText = msg;
        dtGroupPanel.style.display = 'block';
      }
    } else {
      if (freezeAlert) freezeAlert.style.display = 'none';
      if (dtGroupPanel) {
        dtGroupPanel.innerText = '';
        dtGroupPanel.style.display = 'none';
      }
    }
  } catch (e) {
    if (freezeAlert) freezeAlert.style.display = 'none';
    if (dtGroupPanel) {
      dtGroupPanel.innerText = '';
      dtGroupPanel.style.display = 'none';
    }
  }
}

let isNotUpdatedState = false;

// Update status icon and alert banner state
function setUpdateStatusState(isNotUpdated) {
  isNotUpdatedState = isNotUpdated;
  const alertEl = document.getElementById('dbChangeAlert');
  const updateImg = document.getElementById('imgStatusUpdate');
  const updateIconContainer = document.getElementById('statusUpdateIcon');

  if (isNotUpdated) {
    if (alertEl) alertEl.style.display = 'inline-flex';
    if (updateImg) {
      updateImg.src = '/ico/not_update.svg';
      updateImg.classList.add('animate-pulse');
      updateImg.style.filter = 'drop-shadow(0 0 6px rgba(255, 69, 0, 0.9))';
    }
    if (updateIconContainer) {
      updateIconContainer.title = 'اطلاعات جدید در دیتابیس ثبت شده است. کلیک کنید تا جدول به‌روزرسانی شود.';
    }
  } else {
    if (alertEl) alertEl.style.display = 'none';
    if (updateImg) {
      updateImg.src = '/ico/update.svg';
      updateImg.classList.remove('animate-pulse');
      updateImg.style.filter = 'none';
    }
    if (updateIconContainer) {
      updateIconContainer.title = 'جدول به طور کامل به‌روز است (برای بازخوانی مجدد کلیک کنید)';
    }
  }
}

// Reset table scroll position to top (show newest record at top)
function resetAllTableScrolls() {
  document.querySelectorAll('.table-scroll-container').forEach(container => {
    container.scrollTop = 0;
    container.scrollLeft = 0;
  });
}

// Mouseover Auto Refresh when hovering over update icon
function handleUpdateIconHover() {
  resetAllTableScrolls();
  triggerGlobalRefresh();
}

// Start EventSource for live database updates
function initLiveUpdates() {
  if (window.liveUpdatesEventSource) {
    window.liveUpdatesEventSource.close();
  }
  
  const source = new EventSource('/api/live-updates');
  window.liveUpdatesEventSource = source;
  
  source.onmessage = (event) => {
    try {
      const data = JSON.parse(event.data);
      if (data.type === 'db-changed') {
        setUpdateStatusState(true);
        if (typeof loadREQ === 'function') loadREQ();
      }
    } catch (e) {
      console.error('SSE message parse error:', e);
    }
  };
  
  source.onerror = () => {
    console.warn('SSE connection error. Reconnecting...');
  };
}

// Global Refresh handler
function triggerGlobalRefresh() {
  setUpdateStatusState(false);
  resetAllTableScrolls();
  
  // Refresh current active tab
  const activeTabBtn = document.querySelector('.tab-btn.active');
  if (activeTabBtn) {
    const tabName = activeTabBtn.getAttribute('data-tab');
    if (tabName === 'DT' && typeof loadDT === 'function') loadDT();
    if (tabName === 'DB' && typeof loadDB === 'function') loadDB();
    if (tabName === 'PB' && typeof loadTEL === 'function') loadTEL();
    if (tabName === 'Req' && typeof loadREQ === 'function') loadREQ();
    if (tabName === 'CR' && typeof loadCR === 'function') loadCR();
    if (tabName === 'PE' && typeof loadPE === 'function') loadPE();
  }
}

// Tab switcher
function switchTab(tabName, btnEl) {
  document.body.setAttribute('data-active-tab', tabName);
  document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
  document.querySelectorAll('.tab-content').forEach(tc => {
    tc.classList.remove('active');
    tc.style.setProperty('display', 'none', 'important');
  });

  if (btnEl) {
    btnEl.classList.add('active');
    // Remove pulse flashing once user clicks/views the tab
    btnEl.classList.remove('animate-pulse');
  }

  const targetContent = document.getElementById(`tab-${tabName}`);
  if (targetContent) {
    targetContent.classList.add('active');
    targetContent.style.setProperty('display', 'flex', 'important');
  }

  // Trigger tab data loader
  if (tabName === 'DT' && typeof loadDT === 'function') loadDT();
  if (tabName === 'DB' && typeof loadDB === 'function') loadDB();
  if (tabName === 'PB' && typeof loadTEL === 'function') loadTEL();
  if (tabName === 'Req' && typeof loadREQ === 'function') loadREQ();
  if (tabName === 'CR' && typeof loadCR === 'function') loadCR();
  if (tabName === 'PE' && typeof loadPE === 'function') loadPE();
}

let lastRecordScope = '۱۵ روز اخیر';

// Time display with Persian Day of Week - synced directly from SQL Server
let _serverSecondsTotal = 0;
let _syncLocalPerformanceMs = 0;
let _serverShamsiDate = '';
let _serverDayOfWeek = '';

async function syncServerTime() {
  try {
    const res = await fetch('/api/dt/server-time');
    const data = await res.json();
    if (data.success) {
      const h = parseInt(data.hour) || 0;
      const m = parseInt(data.minute) || 0;
      const s = parseInt(data.second) || 0;

      _serverSecondsTotal = h * 3600 + m * 60 + s;
      _syncLocalPerformanceMs = performance.now();
      _serverShamsiDate = data.shamsiDate || '';
      _serverDayOfWeek = data.dayOfWeek || '';
      updateTime();
    }
  } catch (e) {
    console.warn('syncServerTime error:', e);
  }
}

function updateTime() {
  const timeEl = document.getElementById('statusDateTime');
  if (timeEl) {
    if (_syncLocalPerformanceMs > 0) {
      const elapsedSec = Math.floor((performance.now() - _syncLocalPerformanceMs) / 1000);
      const totalSec = (_serverSecondsTotal + elapsedSec) % 86400;

      const h = String(Math.floor(totalSec / 3600) % 24).padStart(2, '0');
      const m = String(Math.floor((totalSec % 3600) / 60)).padStart(2, '0');
      const s = String(totalSec % 60).padStart(2, '0');

      timeEl.innerText = `${_serverDayOfWeek} ${_serverShamsiDate} - ${h}:${m}:${s}`;
    } else {
      timeEl.innerText = 'در حال دریافت زمان از سرور SQL...';
    }
  }
}

// Update status bar record count with filter scope in parentheses
function updateStatusRecordCount(count, scopeText) {
  const recordEl = document.getElementById('statusRecord');
  if (recordEl) {
    if (scopeText) lastRecordScope = scopeText;
    const formattedCount = (count || 0).toLocaleString('fa-IR');
    recordEl.innerText = `تعداد رکوردها: ${formattedCount} (${lastRecordScope})`;
  }
}

// Table sort helper
function sortTable(tableId, colIdx) {
  const table = document.getElementById(tableId);
  if (!table) return;
  let switching = true, dir = "asc", switchcount = 0;
  while (switching) {
    switching = false;
    const rows = table.rows;
    let i;
    for (i = 1; i < (rows.length - 1); i++) {
      let shouldSwitch = false;
      const x = rows[i].getElementsByTagName("TD")[colIdx];
      const y = rows[i + 1].getElementsByTagName("TD")[colIdx];
      if (x && y) {
        if (dir == "asc") {
          if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
            shouldSwitch = true;
            break;
          }
        } else if (dir == "desc") {
          if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
            shouldSwitch = true;
            break;
          }
        }
      }
    }
    if (shouldSwitch) {
      rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
      switching = true;
      switchcount++;
    } else {
      if (switchcount == 0 && dir == "asc") {
        dir = "desc";
        switching = true;
      }
    }
  }
}

// TOGGLE FOCUS MODE / FULL TABLE VIEW
function toggleFocusMode() {
  const isFocus = document.body.classList.toggle('focus-mode');
  localStorage.setItem('focusMode', isFocus ? 'true' : 'false');
  const btn = document.getElementById('btnFocusMode');
  if (btn) {
    btn.classList.toggle('active', isFocus);
    btn.innerHTML = isFocus
      ? `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg> <span>خروج از تمرکز</span>`
      : `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 8V4h4M20 8V4h-4M4 16v4h4M20 16v4h-4"/></svg> <span>حالت تمرکز</span>`;
  }
}

// RESTORE FOCUS MODE PREFERENCE ON BOOT
document.addEventListener('DOMContentLoaded', () => {
  if (localStorage.getItem('focusMode') === 'true') {
    document.body.classList.add('focus-mode');
    const btn = document.getElementById('btnFocusMode');
    if (btn) {
      btn.classList.add('active');
      btn.innerHTML = `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M18 6L6 18M6 6l12 12"/></svg> <span>خروج از تمرکز</span>`;
    }
  }
});
