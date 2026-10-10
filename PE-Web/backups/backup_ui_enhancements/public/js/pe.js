// pe.js - Admin & PE Settings tab logic

async function loadPE() {
  loadPeClients();
}

function switchPeSub(subName, btnEl) {
  document.querySelectorAll('.pe-sub-btn').forEach(b => b.classList.remove('active'));
  document.querySelectorAll('.pe-sub-content').forEach(sc => sc.classList.remove('active'));

  if (btnEl) btnEl.classList.add('active');
  const target = document.getElementById(`pe-${subName}`);
  if (target) target.classList.add('active');

  if (subName === 'clients') loadPeClients();
  if (subName === 'logs') loadPeLogs();
  if (subName === 'vpns') loadPeVpns();
}

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

async function loadPeLogs() {
  const loading = document.getElementById('peLogsLoading');
  const container = document.getElementById('peLogsContainer');
  if (loading) loading.style.display = 'flex';
  if (container) container.style.display = 'none';

  try {
    const res = await fetch('/api/pe/logs');
    const data = await res.json();
    if (data.success) {
      renderGenericTable(data.data || [], 'peLogsHead', 'peLogsBody');
    }
  } catch (e) {
    console.error('Logs load error:', e);
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
    bodyEl.innerHTML = '<tr><td style="text-align:center; padding:15px;">اطلاعاتی یافت نشد</td></tr>';
    return;
  }

  const keys = Object.keys(data[0]);
  let headHtml = '<tr><th>ردیف</th>';
  keys.forEach(k => headHtml += `<th>${k}</th>`);
  headHtml += '</tr>';
  headEl.innerHTML = headHtml;

  let bodyHtml = '';
  data.forEach((row, idx) => {
    bodyHtml += `<tr><td>${idx + 1}</td>`;
    keys.forEach(k => {
      bodyHtml += `<td>${row[k] !== null && row[k] !== undefined ? row[k] : ''}</td>`;
    });
    bodyHtml += '</tr>';
  });
  bodyEl.innerHTML = bodyHtml;
}
