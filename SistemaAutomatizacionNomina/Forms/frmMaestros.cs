using Microsoft.Web.WebView2.Core;
using SistemaAutomatizacionNomina.BLL.Services.Maestros;
using SistemaAutomatizacionNomina.Entities.Entities.Maestros;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaAutomatizacionNomina
{
    public partial class frmMaestros : Form
    {
        B_Maestros objBLL = new B_Maestros();
        private readonly SemaphoreSlim _semaforo = new SemaphoreSlim(1, 1);

        public frmMaestros()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmMaestros_Load);
            this.Size = new System.Drawing.Size(1200, 700);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.MaximumSize = new System.Drawing.Size(1200, 700);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private async void frmMaestros_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();

            webView21.CoreWebView2.Settings.IsWebMessageEnabled = true;

            // Desuscribir primero para evitar doble registro si Load se ejecuta más de una vez
            webView21.CoreWebView2.WebMessageReceived -= CoreWebView2_WebMessageReceived;
            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = null;
            handler = async (s, args) =>
            {
                webView21.NavigationCompleted -= handler;
                await Task.Delay(300);
                await CargarMaestrosEnTabla();
            };
            webView21.NavigationCompleted += handler;

            webView21.NavigateToString(@"
<!DOCTYPE html>
<html lang=""es"">
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<style>
  :root {
    --green-deep:  #1a3d2b;
    --green-mid:   #2c6e49;
    --green-light: #4a9068;
    --green-muted: #d4e6dc;
    --beige-base:  #f5f0e8;
    --beige-warm:  #ede6d6;
    --beige-dark:  #c8ba9e;
    --cream:       #faf8f3;
    --text-dark:   #1a1a16;
    --text-mid:    #4a4a3a;
    --text-light:  #8a8a72;
    --white:       #ffffff;
    --radius-sm:   8px;
    --radius-md:   14px;
    --radius-lg:   22px;
    --shadow:      0 8px 40px rgba(26,61,43,0.12);
  }

  * { margin:0; padding:0; box-sizing:border-box; }

  html, body {
    width: 100%;
    height: 100%;
    overflow-x: hidden;
  }

  body {
    background: var(--beige-base);
    font-family: Arial, sans-serif;
    color: var(--text-dark);
  }

  header {
    background: var(--green-deep);
    color: var(--beige-base);
    padding: 0 40px;
    height: 64px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    box-shadow: 0 2px 16px rgba(0,0,0,0.2);
  }
  .header-left { display:flex; align-items:center; gap:12px; }
  .header-icon {
    width:34px; height:34px; background:var(--green-light);
    border-radius:8px; display:flex; align-items:center;
    justify-content:center; font-size:18px;
  }
  .header-title { font-size:17px; font-weight:700; }
  .header-sub { font-size:11px; color:var(--green-muted); text-transform:uppercase; letter-spacing:.08em; }
  .header-badge {
    background:rgba(255,255,255,.1); border:1px solid rgba(255,255,255,.15);
    border-radius:20px; padding:5px 14px; font-size:12px; color:var(--beige-dark);
  }

  main {
    padding: 32px 28px 40px;
    display: grid;
    grid-template-columns: 300px 1fr;
    gap: 32px;
    align-items: start;
  }

  /* ── Formulario ── */
  .form-card {
    background:var(--white); border-radius:var(--radius-lg);
    box-shadow:var(--shadow); overflow:hidden;
  }
  .form-head { background:var(--green-deep); padding:24px 24px 20px; }
  .form-eyebrow {
    font-size:10px; letter-spacing:.14em; text-transform:uppercase;
    color:var(--green-muted); margin-bottom:4px;
  }
  .form-head h2 { font-size:20px; font-weight:700; color:var(--beige-base); }
  .form-body { padding:24px; display:flex; flex-direction:column; gap:14px; }

  .field label {
    display:block; font-size:11px; font-weight:600;
    letter-spacing:.08em; text-transform:uppercase;
    color:var(--text-light); margin-bottom:5px;
  }
  .field input {
    width:100%; padding:10px 13px;
    border:1.5px solid var(--beige-warm);
    border-radius:var(--radius-sm);
    font-size:14px; color:var(--text-dark);
    background:var(--cream); outline:none;
    transition:border-color .2s, box-shadow .2s;
  }
  .field input:focus {
    border-color:var(--green-mid);
    box-shadow:0 0 0 3px rgba(44,110,73,.1);
    background:var(--white);
  }
  .field input::placeholder { color:var(--beige-dark); }
  .field input.input-error { border-color:#b94040; box-shadow:0 0 0 3px rgba(185,64,64,.1); }

  .btn-guardar {
    width:100%; padding:12px; border:none;
    border-radius:var(--radius-sm);
    background:var(--green-mid); color:var(--white);
    font-size:14px; font-weight:600; cursor:pointer;
    transition:background .2s, transform .15s;
    display:flex; align-items:center; justify-content:center; gap:8px;
  }
  .btn-guardar:hover { background:var(--green-deep); transform:translateY(-1px); }
  .btn-guardar:active { transform:translateY(0); }
  .btn-guardar:disabled { background:var(--beige-dark); cursor:not-allowed; transform:none; }

  /* ── Tabla ── */
  .section-top {
    display:flex; align-items:flex-end;
    justify-content:space-between; margin-bottom:20px;
  }
  .section-title { font-size:26px; font-weight:700; color:var(--green-deep); }
  .section-sub { font-size:13px; color:var(--text-light); margin-top:3px; }
  .count-pill {
    background:var(--green-deep); color:var(--white);
    font-size:12px; font-weight:500;
    padding:4px 12px; border-radius:20px;
  }

  .table-card {
    background:var(--white); border-radius:var(--radius-lg);
    box-shadow:var(--shadow); overflow:hidden;
  }
  .table-controls {
    display:flex; align-items:center; justify-content:space-between;
    padding:16px 20px; border-bottom:1px solid var(--beige-warm);
  }
  .search-box {
    padding:7px 12px; border:1.5px solid var(--beige-warm);
    border-radius:var(--radius-sm); font-size:13px;
    color:var(--text-dark); background:var(--cream); outline:none; width:220px;
  }
  .search-box:focus { border-color:var(--green-mid); background:var(--white); }

  table { width:100%; border-collapse:collapse; }
  thead tr { background:var(--beige-base); }
  thead th {
    font-size:11px; font-weight:600; letter-spacing:.1em;
    text-transform:uppercase; color:var(--text-light);
    padding:13px 18px; border-bottom:2px solid var(--beige-warm);
    text-align:left; white-space:nowrap;
  }
  tbody td {
    padding:13px 18px; font-size:14px; color:var(--text-mid);
    border-bottom:1px solid var(--beige-warm); vertical-align:middle;
  }
  tbody tr:last-child td { border-bottom:none; }
  tbody tr:hover td { background:var(--cream); }
  thead th:last-child, tbody td:last-child { text-align:center; width:52px; padding:13px 10px; }

  .avatar {
    width:28px; height:28px; border-radius:50%;
    background:var(--green-muted); color:var(--green-deep);
    font-size:11px; font-weight:700;
    display:inline-flex; align-items:center; justify-content:center;
    margin-right:8px; vertical-align:middle; flex-shrink:0;
  }
  .badge {
    display:inline-block; background:var(--beige-warm); color:var(--green-deep);
    font-size:11.5px; font-weight:500; padding:3px 10px; border-radius:20px;
  }

  .btn-x {
    width:26px; height:26px; border-radius:50%;
    border:1.5px solid #e0c8c8; background:#fff5f5;
    color:#b94040; font-size:13px; font-weight:700;
    cursor:pointer; display:inline-flex; align-items:center; justify-content:center;
    transition:background .15s, transform .15s;
  }
  .btn-x:hover { background:#b94040; border-color:#b94040; color:#fff; transform:scale(1.1); }

  td.edit-cell { padding:4px 8px !important; }
  td.edit-cell input {
    width:100%; padding:6px 10px;
    border:1.5px solid var(--green-mid); border-radius:var(--radius-sm);
    font-size:14px; color:var(--text-dark); background:var(--white); outline:none;
    box-shadow:0 0 0 3px rgba(44,110,73,.1);
  }
  tr.editing td { background:#f0f8f3 !important; }

  .no-data { text-align:center; padding:40px; color:var(--text-light); font-size:14px; }

  .table-footer {
    display:flex; align-items:center; justify-content:space-between;
    padding:14px 20px; border-top:1px solid var(--beige-warm);
  }
  .page-info { font-size:12px; color:var(--text-light); }
  .pagination { display:flex; gap:4px; }
  .page-btn {
    padding:5px 10px; border:1px solid transparent;
    border-radius:var(--radius-sm); font-size:13px;
    cursor:pointer; background:transparent; color:var(--text-mid);
    transition:background .15s;
  }
  .page-btn:hover { background:var(--beige-warm); border-color:var(--beige-dark); }
  .page-btn.active { background:var(--green-deep); color:#fff; border-color:var(--green-deep); }
  .page-btn:disabled { color:var(--beige-dark); cursor:default; }

  .toast {
    position:fixed; bottom:28px; right:28px;
    background:var(--green-deep); color:#fff;
    padding:13px 20px; border-radius:var(--radius-md);
    font-size:14px; display:flex; align-items:center; gap:8px;
    box-shadow:0 8px 24px rgba(0,0,0,.2);
    z-index:999; opacity:0; transform:translateY(10px);
    transition:all .3s ease; pointer-events:none;
  }
  .toast.show { opacity:1; transform:translateY(0); }
</style>
</head>
<body>

<header>
  <div class=""header-left"">
    <div class=""header-icon"">&#119070;</div>
    <div>
      <div class=""header-title"">Crecendo Studio</div>
      <div class=""header-sub"">Panel de Administración</div>
    </div>
  </div>
  <div class=""header-badge"">Gestión de Maestros</div>
</header>

<main>
  <div class=""form-card"">
    <div class=""form-head"">
      <div class=""form-eyebrow"">Nuevo registro</div>
      <h2>Agregar Maestro</h2>
    </div>
    <div class=""form-body"">
      <div class=""field"">
        <label>Nombre completo</label>
        <input id=""nombre"" placeholder=""Ej. Juan Pérez"">
      </div>
      <div class=""field"">
        <label>Documento <span style=""font-size:10px;color:var(--text-light);font-weight:400;text-transform:none""></span></label>
        <input id=""documento"" placeholder=""000-0000000-0"" maxlength=""13"">
      </div>
      <div class=""field"">
        <label>Teléfono <span style=""font-size:10px;color:var(--text-light);font-weight:400;text-transform:none""></span></label>
        <input id=""telefono"" placeholder=""000-000-0000"" maxlength=""12"">
      </div>
      <div class=""field"">
        <label>Ocupación</label>
        <input id=""ocupacion"" placeholder=""Ej. Violin"">
      </div>
      <button class=""btn-guardar"" id=""btnGuardar"">
        &#128190; Guardar Maestro
      </button>
    </div>
  </div>

  <div class=""table-section"">
    <div class=""section-top"">
      <div>
        <div class=""section-title"">Directorio de Maestros</div>
        <div class=""section-sub"">gestion</div>
      </div>
      <div class=""count-pill"" id=""countPill"">0 maestros</div>
    </div>

    <div class=""table-card"">
      <div class=""table-controls"">
        <input class=""search-box"" id=""searchBox"" placeholder=""Buscar maestro..."">
        <span class=""page-info"" id=""pageInfo""></span>
      </div>
      <table id=""tablaMaestros"">
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Documento</th>
            <th>Teléfono</th>
            <th>Ocupación</th>
            <th></th>
          </tr>
        </thead>
        <tbody id=""tbodyMaestros""></tbody>
      </table>
      <div id=""noData"" class=""no-data"" style=""display:none"">No se encontraron maestros</div>
      <div class=""table-footer"">
        <div class=""page-info"" id=""infoText""></div>
        <div class=""pagination"" id=""pagination""></div>
      </div>
    </div>
  </div>
</main>

<div class=""toast"" id=""toast""><span>&#10003;</span><span id=""toastMsg""></span></div>

<script>
// ══════════════════════════════════════════════
// FORMATO AUTOMÁTICO - DOCUMENTO (000-0000000-0)
// ══════════════════════════════════════════════
function formatDocumento(input) {
  input.addEventListener('keydown', function(ev) {
    if (ev.ctrlKey || ev.metaKey) return;
    var allow = ['Backspace','Delete','ArrowLeft','ArrowRight','Tab','Home','End'];
    if (allow.indexOf(ev.key) >= 0) return;
    if (!/^\d$/.test(ev.key)) { ev.preventDefault(); return; }
  });

  input.addEventListener('input', function() {
    var pos = this.selectionStart;
    var digitos = this.value.replace(/\D/g, '').slice(0, 11);
    var formateado = '';

    for (var i = 0; i < digitos.length; i++) {
      if (i === 3 || i === 10) formateado += '-';
      formateado += digitos[i];
    }

    this.value = formateado;

    var nuevaPos = pos;
    if (pos === 4) nuevaPos = 5;
    if (pos === 12) nuevaPos = 13;
    try { this.setSelectionRange(nuevaPos, nuevaPos); } catch(e) {}
  });
}

// ══════════════════════════════════════════════
// FORMATO AUTOMÁTICO - TELÉFONO (000-000-0000)
// ══════════════════════════════════════════════
function formatTelefono(input) {
  input.addEventListener('keydown', function(ev) {
    if (ev.ctrlKey || ev.metaKey) return;
    var allow = ['Backspace','Delete','ArrowLeft','ArrowRight','Tab','Home','End'];
    if (allow.indexOf(ev.key) >= 0) return;
    if (!/^\d$/.test(ev.key)) { ev.preventDefault(); return; }
  });

  input.addEventListener('input', function() {
    var pos = this.selectionStart;
    var digitos = this.value.replace(/\D/g, '').slice(0, 10);
    var formateado = '';

    for (var i = 0; i < digitos.length; i++) {
      if (i === 3 || i === 6) formateado += '-';
      formateado += digitos[i];
    }

    this.value = formateado;

    var nuevaPos = pos;
    if (pos === 4) nuevaPos = 5;
    if (pos === 8) nuevaPos = 9;
    try { this.setSelectionRange(nuevaPos, nuevaPos); } catch(e) {}
  });
}

formatDocumento(document.getElementById('documento'));
formatTelefono(document.getElementById('telefono'));

// ══════════════════════════════════════════════
// ESTADO GLOBAL
// ══════════════════════════════════════════════
var todosLosMaestros = [];
var maestrosFiltrados = [];
var paginaActual = 1;
var porPagina = 10;
var datosPendientes = {};
var accionPendiente = '';
var filaEnEdicion = null;

// Flag único de control: bloquea tanto el botón como el envío de mensajes
var operacionEnCurso = false;

function initials(name) {
  var p = name.trim().split(' ');
  return (p[0][0] + (p[1] ? p[1][0] : '')).toUpperCase();
}

function showToast(msg, error) {
  var t = document.getElementById('toast');
  t.style.background = error ? '#8b2c2c' : '#1a3d2b';
  document.getElementById('toastMsg').textContent = msg;
  t.classList.add('show');
  setTimeout(function() { t.classList.remove('show'); }, 3000);
}

function escHtml(str) {
  return String(str)
    .replace(/&/g,'&amp;').replace(/</g,'&lt;')
    .replace(/>/g,'&gt;').replace(/\x22/g,'&quot;');
}

// ══════════════════════════════════════════════
// RENDER TABLA
// ══════════════════════════════════════════════
function renderTabla() {
  var tbody = document.getElementById('tbodyMaestros');
  var noData = document.getElementById('noData');
  var total = maestrosFiltrados.length;
  var inicio = (paginaActual - 1) * porPagina;
  var fin = Math.min(inicio + porPagina, total);
  var pagina = maestrosFiltrados.slice(inicio, fin);

  tbody.innerHTML = '';

  if (total === 0) {
    noData.style.display = 'block';
  } else {
    noData.style.display = 'none';
    pagina.forEach(function(m) {
      var tr = document.createElement('tr');
      tr.setAttribute('data-id', m.id);
      tr.innerHTML =
        '<td><span class=""avatar"">' + initials(m.nombre) + '</span>' + escHtml(m.nombre) + '</td>' +
        '<td>' + escHtml(m.documento) + '</td>' +
        '<td>' + escHtml(m.telefono) + '</td>' +
        '<td><span class=""badge"">' + escHtml(m.ocupacion) + '</span></td>' +
        '<td><button class=""btn-x"" onclick=""eliminar(' + m.id + ')"">&#10005;</button></td>';
      tr.addEventListener('dblclick', function() { iniciarEdicion(tr, m); });
      tbody.appendChild(tr);
    });
  }

  document.getElementById('countPill').textContent =
    todosLosMaestros.length + ' maestro' + (todosLosMaestros.length !== 1 ? 's' : '');
  document.getElementById('infoText').textContent =
    total > 0 ? 'Mostrando ' + (inicio+1) + '-' + fin + ' de ' + total : '';

  renderPaginacion(total);
}

function renderPaginacion(total) {
  var totalPags = Math.ceil(total / porPagina);
  var div = document.getElementById('pagination');
  div.innerHTML = '';
  if (totalPags <= 1) return;

  function crearBtn(texto, pag, activo, disabled) {
    var btn = document.createElement('button');
    btn.className = 'page-btn' + (activo ? ' active' : '');
    btn.textContent = texto;
    btn.disabled = disabled;
    if (!disabled && !activo) {
      btn.onclick = function() { paginaActual = pag; renderTabla(); };
    }
    return btn;
  }

  div.appendChild(crearBtn('‹', paginaActual-1, false, paginaActual===1));
  for (var i = 1; i <= totalPags; i++) {
    div.appendChild(crearBtn(i, i, i===paginaActual, false));
  }
  div.appendChild(crearBtn('›', paginaActual+1, false, paginaActual===totalPags));
}

// ── Búsqueda ──
document.getElementById('searchBox').addEventListener('input', function() {
  var q = this.value.toLowerCase();
  maestrosFiltrados = todosLosMaestros.filter(function(m) {
    return m.nombre.toLowerCase().indexOf(q) >= 0 ||
           m.documento.toLowerCase().indexOf(q) >= 0 ||
           m.ocupacion.toLowerCase().indexOf(q) >= 0;
  });
  paginaActual = 1;
  renderTabla();
});

// ── INSERTAR ──
document.getElementById('btnGuardar').addEventListener('click', function() {
  // Bloqueo estricto: si ya hay operación en curso, no hacer nada
  if (operacionEnCurso) return;

  var nombre    = document.getElementById('nombre').value.trim();
  var documento = document.getElementById('documento').value.trim();
  var telefono  = document.getElementById('telefono').value.trim();
  var ocupacion = document.getElementById('ocupacion').value.trim();

  if (!nombre || !documento || !telefono || !ocupacion) {
    showToast('Por favor completa todos los campos', true);
    return;
  }

  if (documento.replace(/\D/g,'').length < 11) {
    showToast('El documento debe tener 11 dígitos', true);
    document.getElementById('documento').classList.add('input-error');
    return;
  }
  if (telefono.replace(/\D/g,'').length < 10) {
    showToast('El teléfono debe tener 10 dígitos', true);
    document.getElementById('telefono').classList.add('input-error');
    return;
  }

  document.getElementById('documento').classList.remove('input-error');
  document.getElementById('telefono').classList.remove('input-error');

  // Activar bloqueo ANTES de postMessage
  operacionEnCurso = true;
  document.getElementById('btnGuardar').disabled = true;

  datosPendientes = { nombre:nombre, documento:documento, telefono:telefono, ocupacion:ocupacion };
  accionPendiente = 'INSERTAR';
  window.chrome.webview.postMessage('INSERTAR|' + nombre + '|' + documento + '|' + telefono + '|' + ocupacion);
});

// ── ELIMINAR ──
window.eliminar = function(id) {
  if (operacionEnCurso) return;
  if (!confirm('¿Eliminar este maestro?')) return;

  operacionEnCurso = true;
  datosPendientes = { id: id };
  accionPendiente = 'ELIMINAR';
  window.chrome.webview.postMessage('ELIMINAR|' + id);
};

// ── EDICIÓN INLINE (doble clic) ──
function iniciarEdicion(tr, m) {
  if (filaEnEdicion || operacionEnCurso) return;
  filaEnEdicion = tr;
  tr.classList.add('editing');

  var tds = tr.querySelectorAll('td');
  tds[0].className = 'edit-cell';
  tds[0].innerHTML = '<input value=""' + escHtml(m.nombre) + '"" id=""e_nombre"">';
  tds[1].className = 'edit-cell';
  tds[1].innerHTML = '<input value=""' + escHtml(m.documento) + '"" id=""e_documento"" maxlength=""13"">';
  tds[2].className = 'edit-cell';
  tds[2].innerHTML = '<input value=""' + escHtml(m.telefono) + '"" id=""e_telefono"" maxlength=""12"">';
  tds[3].className = 'edit-cell';
  tds[3].innerHTML = '<input value=""' + escHtml(m.ocupacion) + '"" id=""e_ocupacion"">';
  tds[4].innerHTML = '<span style=""font-size:11px;color:#8a8a72"">Enter/Esc</span>';

  formatDocumento(document.getElementById('e_documento'));
  formatTelefono(document.getElementById('e_telefono'));

  tds[0].querySelector('input').focus();

  function onKey(ev) {
    if (ev.key === 'Enter') {
      ev.preventDefault();
      if (operacionEnCurso) return;

      var nuevoNombre = document.getElementById('e_nombre').value.trim();
      var nuevoDoc    = document.getElementById('e_documento').value.trim();
      var nuevoTel    = document.getElementById('e_telefono').value.trim();
      var nuevoOcup   = document.getElementById('e_ocupacion').value.trim();

      if (!nuevoNombre || !nuevoDoc || !nuevoTel || !nuevoOcup) {
        showToast('Todos los campos son requeridos', true); return;
      }
      if (nuevoDoc.replace(/\D/g,'').length < 11) {
        showToast('El documento debe tener 11 dígitos', true); return;
      }
      if (nuevoTel.replace(/\D/g,'').length < 10) {
        showToast('El teléfono debe tener 10 dígitos', true); return;
      }

      // Activar bloqueo ANTES de postMessage
      operacionEnCurso = true;
      tr.removeEventListener('keydown', onKey, true);
      datosPendientes = { id:m.id, nombre:nuevoNombre, documento:nuevoDoc, telefono:nuevoTel, ocupacion:nuevoOcup };
      accionPendiente = 'ACTUALIZAR';
      window.chrome.webview.postMessage('ACTUALIZAR|' + m.id + '|' + nuevoNombre + '|' + nuevoDoc + '|' + nuevoTel + '|' + nuevoOcup);
    }
    if (ev.key === 'Escape') {
      tr.removeEventListener('keydown', onKey, true);
      filaEnEdicion = null;
      tr.classList.remove('editing');
      renderTabla();
    }
  }
  tr.addEventListener('keydown', onKey, true);
}

// ── Respuesta desde C# ──
window.recibirRespuesta = function(resultado) {
  // Liberar bloqueo
  operacionEnCurso = false;
  document.getElementById('btnGuardar').disabled = false;

  if (resultado !== 'OK') {
    showToast('Error en la operación. Intenta de nuevo.', true);
    if (filaEnEdicion) { filaEnEdicion.classList.remove('editing'); filaEnEdicion = null; }
    datosPendientes = {}; accionPendiente = ''; return;
  }

  if (accionPendiente === 'INSERTAR') {
    document.getElementById('nombre').value = '';
    document.getElementById('documento').value = '';
    document.getElementById('telefono').value = '';
    document.getElementById('ocupacion').value = '';
    showToast(datosPendientes.nombre + ' agregado correctamente');
  } else if (accionPendiente === 'ELIMINAR') {
    showToast('Maestro eliminado correctamente');
  } else if (accionPendiente === 'ACTUALIZAR') {
    if (filaEnEdicion) { filaEnEdicion.classList.remove('editing'); filaEnEdicion = null; }
    showToast(datosPendientes.nombre + ' actualizado correctamente');
  }

  datosPendientes = {}; accionPendiente = '';
};

// ── Carga desde C# ──
window.cargarMaestros = function(jsonStr) {
  todosLosMaestros = JSON.parse(jsonStr);
  maestrosFiltrados = todosLosMaestros.slice();
  paginaActual = 1;
  filaEnEdicion = null;
  renderTabla();
};
</script>
</body>
</html>
");
        }

        private async Task CargarMaestrosEnTabla()
        {
            try
            {
                List<E_Maestros> lista = objBLL.ListarMaestro("");
                var sb = new System.Text.StringBuilder("[");
                for (int i = 0; i < lista.Count; i++)
                {
                    var m = lista[i];
                    sb.Append("{");
                    sb.Append($"\"id\":{m.IdMaestro},");
                    sb.Append($"\"nombre\":\"{Escape(m.NombreCompleto)}\",");
                    sb.Append($"\"documento\":\"{Escape(m.DocumentoIdentidad)}\",");
                    sb.Append($"\"telefono\":\"{Escape(m.Telefono)}\",");
                    sb.Append($"\"ocupacion\":\"{Escape(m.Ocupacion)}\"");
                    sb.Append("}");
                    if (i < lista.Count - 1) sb.Append(",");
                }
                sb.Append("]");

                string json = sb.ToString().Replace("'", "\\'");
                await webView21.CoreWebView2.ExecuteScriptAsync($"cargarMaestros('{json}')");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando maestros: " + ex.Message);
            }
        }

        private async void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            // Si ya hay una operación en curso en C#, descartar el mensaje duplicado
            if (!await _semaforo.WaitAsync(0)) return;

            string mensaje = e.TryGetWebMessageAsString();
            string[] datos = mensaje.Split('|');
            string accion = datos[0];
            bool resultado = false;

            try
            {
                if (accion == "INSERTAR" && datos.Length >= 5)
                {
                    E_Maestros m = new E_Maestros
                    {
                        NombreCompleto = datos[1],
                        DocumentoIdentidad = datos[2],
                        Telefono = datos[3],
                        Ocupacion = datos[4]
                    };
                    objBLL.InsertandoMaestro(m);
                    resultado = true;
                }
                else if (accion == "ELIMINAR" && datos.Length >= 2)
                {
                    E_Maestros m = new E_Maestros { IdMaestro = int.Parse(datos[1]) };
                    objBLL.EliminandoMaestro(m);
                    resultado = true;
                }
                else if (accion == "ACTUALIZAR" && datos.Length >= 6)
                {
                    E_Maestros m = new E_Maestros
                    {
                        IdMaestro = int.Parse(datos[1]),
                        NombreCompleto = datos[2],
                        DocumentoIdentidad = datos[3],
                        Telefono = datos[4],
                        Ocupacion = datos[5]
                    };
                    objBLL.ModificandoMaestro(m);
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                resultado = false;
            }
            finally
            {
                // Siempre liberar el semáforo al terminar la operación de BD
                _semaforo.Release();
            }

            string respuesta = resultado ? "'OK'" : "'ERROR'";

            // Primero notificar al JS, luego recargar la tabla
            await webView21.CoreWebView2.ExecuteScriptAsync($"recibirRespuesta({respuesta})");

            if (resultado)
            {
                await CargarMaestrosEnTabla();
            }
        }

        private string Escape(string s)
        {
            if (s == null) return "";
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r");
        }

        private void webView21_Click(object sender, EventArgs e) { }
    }
}