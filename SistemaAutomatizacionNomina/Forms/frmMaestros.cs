using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;


namespace SistemaAutomatizacionNomina
{
    public partial class frmMaestros : Form
    {
        public frmMaestros()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmMaestros_Load);
        }

        private async void frmMaestros_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();

            // ─── Suscribir evento para recibir mensajes desde HTML ───
            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            webView21.NavigateToString(@"
<!DOCTYPE html>
<html lang=""es"">
<head>
<meta http-equiv='X-UA-Compatible' content='IE=edge'>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<link rel='stylesheet' href='https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css'>
<link href=""https://fonts.googleapis.com/css2?family=Playfair+Display:wght@400;600;700&family=DM+Sans:wght@300;400;500&display=swap"" rel=""stylesheet"">
<script src='https://code.jquery.com/jquery-3.7.0.min.js'></script>
<script src='https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js'></script>

<style>
  :root {
    --green-deep:    #1a3d2b;
    --green-mid:     #2c6e49;
    --green-light:   #4a9068;
    --green-muted:   #d4e6dc;
    --beige-base:    #f5f0e8;
    --beige-warm:    #ede6d6;
    --beige-dark:    #c8ba9e;
    --cream:         #faf8f3;
    --text-dark:     #1a1a16;
    --text-mid:      #4a4a3a;
    --text-light:    #8a8a72;
    --white:         #ffffff;
    --shadow-soft:   0 4px 24px rgba(26, 61, 43, 0.08);
    --shadow-card:   0 8px 40px rgba(26, 61, 43, 0.12);
    --radius-sm:     8px;
    --radius-md:     14px;
    --radius-lg:     22px;
  }

  * { margin: 0; padding: 0; box-sizing: border-box; }

  body {
    background-color: var(--beige-base);
    font-family: 'DM Sans', sans-serif;
    color: var(--text-dark);
    min-height: 100vh;
    position: relative;
    overflow-x: hidden;
  }

  body::before {
    content: '';
    position: fixed;
    inset: 0;
    background:
      radial-gradient(ellipse 80% 60% at 10% 0%, rgba(44,110,73,0.07) 0%, transparent 60%),
      radial-gradient(ellipse 60% 40% at 90% 100%, rgba(44,110,73,0.05) 0%, transparent 50%);
    pointer-events: none;
    z-index: 0;
  }

  header {
    background: var(--green-deep);
    color: var(--beige-base);
    padding: 0 48px;
    height: 68px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    position: sticky;
    top: 0;
    z-index: 100;
    box-shadow: 0 2px 20px rgba(0,0,0,0.18);
  }

  .header-brand { display: flex; align-items: center; gap: 12px; }
  .header-icon {
    width: 34px; height: 34px;
    background: var(--green-light);
    border-radius: 8px;
    display: flex; align-items: center; justify-content: center;
    font-size: 17px;
  }
  .header-title {
    font-family: 'Playfair Display', serif;
    font-size: 18px; font-weight: 600;
    letter-spacing: 0.02em; color: var(--beige-base);
  }
  .header-subtitle {
    font-size: 11px; color: var(--green-muted);
    letter-spacing: 0.08em; text-transform: uppercase; font-weight: 300;
  }
  .header-badge {
    background: rgba(255,255,255,0.08);
    border: 1px solid rgba(255,255,255,0.12);
    border-radius: 20px; padding: 5px 14px;
    font-size: 12px; color: var(--beige-dark); letter-spacing: 0.04em;
  }

  main {
    position: relative; z-index: 1;
    max-width: 1180px; margin: 0 auto;
    padding: 48px 32px 64px;
    display: grid;
    grid-template-columns: 320px 1fr;
    gap: 40px; align-items: start;
  }

  .form-card {
    background: var(--white);
    border-radius: var(--radius-lg);
    box-shadow: var(--shadow-card);
    overflow: hidden;
    position: sticky; top: 88px;
    animation: slideInLeft 0.5s cubic-bezier(0.22, 1, 0.36, 1) both;
  }

  @keyframes slideInLeft {
    from { opacity: 0; transform: translateX(-24px); }
    to   { opacity: 1; transform: translateX(0); }
  }

  .form-header {
    background: var(--green-deep);
    padding: 28px 28px 24px;
    position: relative; overflow: hidden;
  }
  .form-header::after {
    content: ''; position: absolute;
    bottom: -20px; right: -20px;
    width: 100px; height: 100px;
    background: rgba(255,255,255,0.04); border-radius: 50%;
  }
  .form-header-eyebrow {
    font-size: 10px; letter-spacing: 0.14em;
    text-transform: uppercase; color: var(--green-muted);
    margin-bottom: 6px; font-weight: 500;
  }
  .form-header h2 {
    font-family: 'Playfair Display', serif;
    font-size: 22px; font-weight: 600;
    color: var(--beige-base); line-height: 1.2;
  }

  .form-body { padding: 28px; display: flex; flex-direction: column; gap: 16px; }

  .field-group { display: flex; flex-direction: column; gap: 6px; }
  .field-group label {
    font-size: 11px; font-weight: 500;
    letter-spacing: 0.08em; text-transform: uppercase; color: var(--text-light);
  }
  .field-group input {
    width: 100%; padding: 11px 14px;
    border: 1.5px solid var(--beige-warm);
    border-radius: var(--radius-sm);
    font-family: 'DM Sans', sans-serif;
    font-size: 14px; color: var(--text-dark);
    background: var(--cream); outline: none;
    transition: border-color 0.2s, background 0.2s, box-shadow 0.2s;
  }
  .field-group input:focus {
    border-color: var(--green-mid);
    background: var(--white);
    box-shadow: 0 0 0 3px rgba(44,110,73,0.1);
  }
  .field-group input::placeholder { color: var(--beige-dark); font-weight: 300; }

  .btn-guardar {
    width: 100%; padding: 13px; border: none;
    border-radius: var(--radius-sm);
    background: var(--green-mid); color: var(--beige-base);
    font-family: 'DM Sans', sans-serif;
    font-size: 14px; font-weight: 500; letter-spacing: 0.04em;
    cursor: pointer;
    transition: background 0.2s, transform 0.15s, box-shadow 0.2s;
    margin-top: 4px;
    display: flex; align-items: center; justify-content: center; gap: 8px;
  }
  .btn-guardar:hover {
    background: var(--green-deep);
    box-shadow: 0 4px 16px rgba(44,110,73,0.3);
    transform: translateY(-1px);
  }
  .btn-guardar:active { transform: translateY(0); box-shadow: none; }
  .btn-guardar svg { width: 15px; height: 15px; }

  .table-section {
    animation: slideInRight 0.5s 0.1s cubic-bezier(0.22, 1, 0.36, 1) both;
  }
  @keyframes slideInRight {
    from { opacity: 0; transform: translateX(24px); }
    to   { opacity: 1; transform: translateX(0); }
  }

  .section-header {
    display: flex; align-items: flex-end;
    justify-content: space-between; margin-bottom: 24px;
  }
  .section-title {
    font-family: 'Playfair Display', serif;
    font-size: 28px; font-weight: 700;
    color: var(--green-deep); line-height: 1.1;
  }
  .section-title span {
    display: block;
    font-family: 'DM Sans', sans-serif;
    font-size: 13px; font-weight: 400;
    color: var(--text-light); letter-spacing: 0.03em; margin-top: 4px;
  }
  .count-pill {
    background: var(--green-deep); color: var(--beige-base);
    font-size: 12px; font-weight: 500;
    padding: 4px 12px; border-radius: 20px; letter-spacing: 0.04em;
  }

  .dataTables_wrapper {
    background: var(--white); border-radius: var(--radius-lg);
    box-shadow: var(--shadow-card); overflow: hidden; padding: 0;
  }
  .dt-controls-bar {
    display: flex; align-items: center; justify-content: space-between;
    padding: 20px 24px 16px; border-bottom: 1px solid var(--beige-warm);
  }
  .dataTables_length { padding: 0 !important; }
  .dataTables_filter { padding: 0 !important; }
  .dataTables_wrapper .dataTables_length label,
  .dataTables_wrapper .dataTables_filter label {
    font-size: 13px; color: var(--text-mid);
    font-family: 'DM Sans', sans-serif;
    display: flex; align-items: center; gap: 8px;
  }
  .dataTables_wrapper .dataTables_length select,
  .dataTables_wrapper .dataTables_filter input {
    border: 1.5px solid var(--beige-warm);
    border-radius: var(--radius-sm);
    padding: 6px 10px; font-size: 13px;
    font-family: 'DM Sans', sans-serif;
    color: var(--text-dark); background: var(--cream); outline: none;
  }
  .dataTables_wrapper .dataTables_filter input:focus {
    border-color: var(--green-mid);
    box-shadow: 0 0 0 3px rgba(44,110,73,0.1);
    background: var(--white);
  }

  table#tablaMaestros { width: 100% !important; border-collapse: collapse; }
  table#tablaMaestros thead tr { background: var(--beige-base); }
  table#tablaMaestros thead th {
    font-family: 'DM Sans', sans-serif;
    font-size: 11px; font-weight: 500;
    letter-spacing: 0.1em; text-transform: uppercase;
    color: var(--text-light); padding: 14px 20px;
    border-bottom: 2px solid var(--beige-warm);
    text-align: left; white-space: nowrap;
  }
  table#tablaMaestros tbody td {
    padding: 14px 20px; font-size: 14px;
    color: var(--text-mid); border-bottom: 1px solid var(--beige-warm);
    text-align: left; vertical-align: middle;
  }
  table#tablaMaestros tbody tr:last-child td { border-bottom: none; }
  table#tablaMaestros tbody tr:hover td { background: var(--cream); }
  table#tablaMaestros tbody td:first-child {
    font-weight: 500; color: var(--text-dark);
    display: flex; align-items: center; gap: 10px;
  }

  .avatar-dot {
    width: 30px; height: 30px; border-radius: 50%;
    background: var(--green-muted); color: var(--green-deep);
    font-size: 11px; font-weight: 600;
    display: inline-flex; align-items: center; justify-content: center;
    flex-shrink: 0; letter-spacing: 0.02em;
  }
  .badge-ocupacion {
    display: inline-block;
    background: var(--beige-warm); color: var(--green-deep);
    font-size: 11.5px; font-weight: 500;
    padding: 3px 10px; border-radius: 20px; letter-spacing: 0.02em;
  }

  .dataTables_info {
    font-size: 12px; color: var(--text-light);
    padding: 16px 24px !important; font-family: 'DM Sans', sans-serif;
  }
  .dataTables_paginate { padding: 12px 24px !important; }
  .dataTables_paginate .paginate_button {
    border-radius: var(--radius-sm) !important;
    font-size: 13px !important; font-family: 'DM Sans', sans-serif !important;
    padding: 5px 11px !important; margin: 0 2px !important;
    color: var(--text-mid) !important; background: transparent !important;
    border: 1px solid transparent !important;
  }
  .dataTables_paginate .paginate_button:hover {
    background: var(--beige-warm) !important;
    border-color: var(--beige-dark) !important; color: var(--text-dark) !important;
  }
  .dataTables_paginate .paginate_button.current,
  .dataTables_paginate .paginate_button.current:hover {
    background: var(--green-deep) !important;
    color: var(--beige-base) !important; border-color: var(--green-deep) !important;
  }
  .dataTables_paginate .paginate_button.disabled,
  .dataTables_paginate .paginate_button.disabled:hover {
    color: var(--beige-dark) !important; cursor: default;
  }
  .dt-bottom-bar {
    display: flex; align-items: center;
    justify-content: space-between; border-top: 1px solid var(--beige-warm);
  }

  .toast {
    position: fixed; bottom: 32px; right: 32px;
    background: var(--green-deep); color: var(--beige-base);
    padding: 14px 22px; border-radius: var(--radius-md);
    font-size: 14px; font-family: 'DM Sans', sans-serif;
    box-shadow: 0 8px 24px rgba(0,0,0,0.2);
    display: flex; align-items: center; gap: 10px;
    z-index: 999; opacity: 0; transform: translateY(12px);
    transition: all 0.3s cubic-bezier(0.22, 1, 0.36, 1);
    pointer-events: none;
  }
  .toast.show { opacity: 1; transform: translateY(0); }
</style>
</head>
<body>

<header>
  <div class=""header-brand"">
    <div class=""header-icon"">𝄞</div>
    <div>
      <div class=""header-title"">Crecendo Studio</div>
      <div class=""header-subtitle"">Panel de Administración</div>
    </div>
  </div>
  <div class=""header-badge"">Gestión de Maestros</div>
</header>

<main>
  <div class=""form-card"">
    <div class=""form-header"">
      <div class=""form-header-eyebrow"">Nuevo registro</div>
      <h2>Agregar Maestro</h2>
    </div>
    <div class=""form-body"">
      <div class=""field-group"">
        <label>Nombre completo</label>
        <input id='nombre' placeholder='Ej. Juan Pérez García'>
      </div>
      <div class=""field-group"">
        <label>Documento</label>
        <input id='documento' placeholder='Número de documento'>
      </div>
      <div class=""field-group"">
        <label>Teléfono</label>
        <input id='telefono' placeholder='555-0000'>
      </div>
      <div class=""field-group"">
        <label>Ocupación </label>
        <input id='ocupacion' placeholder='Ej. Violin'>
      </div>
      <button class=""btn-guardar"" id='btnGuardar'>
        <svg viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2.5"" stroke-linecap=""round"" stroke-linejoin=""round"">
          <path d=""M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z""/>
          <polyline points=""17 21 17 13 7 13 7 21""/>
          <polyline points=""7 3 7 8 15 8""/>
        </svg>
        Guardar Maestro
      </button>
    </div>
  </div>

  <div class=""table-section"">
    <div class=""section-header"">
      <div class=""section-title"">
        Directorio de Maestros
        <span>Registro académico actualizado</span>
      </div>
      <div class=""count-pill"" id=""countPill"">0 maestros</div>
    </div>

    <div class=""dataTables_wrapper"">
      <div class=""dt-controls-bar"" id=""dtControlsTop""></div>
      <table id='tablaMaestros'>
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Documento</th>
            <th>Teléfono</th>
            <th>Ocupación</th>
          </tr>
        </thead>
        <tbody></tbody>
      </table>
      <div class=""dt-bottom-bar"">
        <div id=""dtInfo""></div>
        <div id=""dtPaginate""></div>
      </div>
    </div>
  </div>
</main>

<div class=""toast"" id=""toast"">
  <span>✓</span> <span id=""toastMsg"">Maestro guardado correctamente</span>
</div>

<script>
  // ─── Helpers ──────────────────────────────────────────────
  function getInitials(name) {
    const parts = name.trim().split(' ');
    return (parts[0][0] + (parts[1] ? parts[1][0] : '')).toUpperCase();
  }

  function decorateTable() {
    $('#tablaMaestros tbody tr').each(function() {
      const tdNombre = $(this).find('td:first-child');
      const name = tdNombre.text().trim();
      if (!tdNombre.find('.avatar-dot').length) {
        tdNombre.html(`<span class=""avatar-dot"">${getInitials(name)}</span>${name}`);
      }
      const tdOcupacion = $(this).find('td:last-child');
      const text = tdOcupacion.text().trim();
      if (!tdOcupacion.find('.badge-ocupacion').length) {
        tdOcupacion.html(`<span class=""badge-ocupacion"">${text}</span>`);
      }
    });
  }

  function showToast(msg, esError = false) {
    const t = document.getElementById('toast');
    t.style.background = esError ? '#8b2c2c' : 'var(--green-deep)';
    document.getElementById('toastMsg').textContent = msg;
    t.classList.add('show');
    setTimeout(() => t.classList.remove('show'), 3000);
  }

  // ─── DataTable ────────────────────────────────────────────
  $(document).ready(function() {
    window.tabla = $('#tablaMaestros').DataTable({
      pageLength: 10,
      lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, ""Todos""]],
      dom: '<""dt-controls-bar""lf>t<""dt-bottom-bar""ip>',
      language: {
        search: """",
        searchPlaceholder: ""Buscar maestro…"",
        lengthMenu: ""Mostrar _MENU_"",
        info: ""Mostrando _START_–_END_ de _TOTAL_"",
        infoEmpty: ""Sin registros"",
        infoFiltered: ""(de _MAX_ total)"",
        zeroRecords: ""No se encontraron maestros"",
        paginate: { first: ""«"", previous: ""‹"", next: ""›"", last: ""»"" }
      },
      drawCallback: function() {
        decorateTable();
        const total = this.api().data().length;
        document.getElementById('countPill').textContent = total + ' maestro' + (total !== 1 ? 's' : '');
      }
    });

    // ─── Guardar temporalmente los valores antes de enviar ──
    let datosPendientes = {};

    // ============================================================
    // CONTRATO CON BACKEND
    // ENVÍO:    'nombre|documento|telefono|ocupacion'  (separado por |)
    // RESPUESTA esperada desde C#: recibirRespuesta('OK') 
    //                           o: recibirRespuesta('ERROR')
    // ============================================================
    document.getElementById('btnGuardar').addEventListener('click', function() {

      // 1. Recoger valores
      const nombre    = document.getElementById('nombre').value.trim();
      const documento = document.getElementById('documento').value.trim();
      const telefono  = document.getElementById('telefono').value.trim();
      const ocupacion = document.getElementById('ocupacion').value.trim();

      // 2. Validar campos vacíos
      if (!nombre || !documento || !telefono || !ocupacion) {
        showToast('Por favor completa todos los campos', true);
        return;
      }

      // 3. Guardar valores para usarlos cuando llegue la respuesta
      datosPendientes = { nombre, documento, telefono, ocupacion };

      // 4. Enviar a C#  ← AQUÍ TERMINA TU TRABAJO
      const mensaje = `${nombre}|${documento}|${telefono}|${ocupacion}`;
      window.chrome.webview.postMessage(mensaje);
    });

    // ─── Respuesta que viene de C# ────────────────────────────
    // El backend llama: ExecuteScriptAsync(""recibirRespuesta('OK')"")
    //               o:  ExecuteScriptAsync(""recibirRespuesta('ERROR')"")
    window.recibirRespuesta = function(resultado) {
      if (resultado === 'OK') {

        // Agregar fila a la tabla con los datos guardados
        window.tabla.row.add([
          datosPendientes.nombre,
          datosPendientes.documento,
          datosPendientes.telefono,
          datosPendientes.ocupacion
        ]).draw(false);

        decorateTable();

        // Limpiar inputs
        ['nombre', 'documento', 'telefono', 'ocupacion'].forEach(id => {
          document.getElementById(id).value = '';
        });

        showToast(`""${datosPendientes.nombre}"" agregado correctamente`);
        datosPendientes = {};

      } else {
        showToast('Error al guardar. Intenta de nuevo.', true);
      }
    };
  });
</script>
</body>
</html>
");
        }
        private void webView21_Click(object sender, EventArgs e)
        {

        }

        // ================================================================
        // ESTE MÉTODO LO COMPLETA EL BACKEND
        // Recibe: 'nombre|documento|telefono|ocupacion'
        // Debe llamar: MaestroService.Guardar(...)
        // Debe responder con: recibirRespuesta('OK') o recibirRespuesta('ERROR')
        // ================================================================
        private async void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string mensaje = e.TryGetWebMessageAsString();
            string[] datos = mensaje.Split('|');

            string nombre = datos[0];
            string documento = datos[1];
            string telefono = datos[2];
            string ocupacion = datos[3];

            // TODO (Backend): Crear la entidad Maestro
            // Maestro maestro = new Maestro();
            // maestro.Nombre    = nombre;
            // maestro.Documento = documento;
            // maestro.Telefono  = telefono;
            // maestro.Ocupacion = ocupacion;

            // TODO (Backend): Llamar la capa lógica
            // MaestroService service = new MaestroService();
            // bool resultado = service.Guardar(maestro);

            // TODO (Backend): Reemplazar esta línea con el resultado real
            bool resultado = false;

            // Responder al HTML
            string respuesta = resultado ? "'OK'" : "'ERROR'";
            await webView21.CoreWebView2.ExecuteScriptAsync($"recibirRespuesta({respuesta})");
        }
    }
}


