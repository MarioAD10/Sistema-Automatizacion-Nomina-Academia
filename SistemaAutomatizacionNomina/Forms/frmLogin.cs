using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using SistemaAutomatizacionNomina.BLL.Services.Login;
using SistemaAutomatizacionNomina.Entities.Entities.Login;

namespace SistemaAutomatizacionNomina
{
    public partial class frmLogin : Form
    {
        public frmLogin() 
        {
            InitializeComponent();
            this.Load += new EventHandler(frmLogin_Load);
        }

        private async void frmLogin_Load(object sender, EventArgs e)
        {
            // Tamaño y posición del form
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            await webView21.EnsureCoreWebView2Async();
            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            webView21.NavigateToString(@"
<!DOCTYPE html>
<html lang='es'>
<head>
<meta charset='UTF-8'/>
<style>
  *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    height: 100vh; width: 100vw; overflow: hidden;
    font-family: 'Segoe UI', sans-serif;
    display: flex;
  }
  .left {
    width: 46%;
    background: linear-gradient(160deg, #2a5c46 0%, #3c7962 55%, #4c956c 100%);
    display: flex; flex-direction: column;
    justify-content: center; align-items: center;
    padding: 48px 36px; position: relative; overflow: hidden;
  }
  .left::before {
    content:''; position:absolute; width:420px; height:420px;
    border-radius:50%; border:1.5px solid rgba(255,255,255,.12);
    top:-90px; left:-100px;
  }
  .left::after {
    content:''; position:absolute; width:280px; height:280px;
    border-radius:50%; border:1px solid rgba(255,255,255,.08);
    bottom:40px; right:-80px;
  }
  .star { width:72px; height:72px; margin-bottom:28px; }
  .left h1 {
    font-size:2.2rem; font-weight:700; color:#fff;
    letter-spacing:.06em; text-align:center; text-transform:uppercase;
    line-height:1.15; margin-bottom:12px; z-index:1;
  }
  .left p {
    font-size:.92rem; color:rgba(255,255,255,.72);
    text-align:center; max-width:230px; line-height:1.65; z-index:1;
  }
  .dots {
    position:absolute; bottom:32px; left:50%;
    transform:translateX(-50%); display:flex; gap:8px; z-index:1;
  }
  .dot { width:6px; height:6px; border-radius:50%; background:rgba(255,255,255,.35); }
  .dot.active { background:#fff; width:18px; border-radius:3px; }
  .right {
    flex:1; display:flex; flex-direction:column;
    justify-content:center; align-items:center;
    padding:48px 56px; background:#fff; position:relative;
  }
  .right::before {
    content:''; position:absolute; inset:0;
    background-image:
      linear-gradient(#d8ece3 1px, transparent 1px),
      linear-gradient(90deg, #d8ece3 1px, transparent 1px);
    background-size:40px 40px; opacity:.28; pointer-events:none;
  }
  .card { position:relative; z-index:1; width:100%; max-width:340px; }
  .avatar {
    width:72px; height:72px; background:#eef6f1;
    border:2px solid #d8ece3; border-radius:50%;
    display:flex; align-items:center; justify-content:center;
    margin:0 auto 22px;
  }
  .avatar svg { width:36px; height:36px; }
  .card-title { font-size:1.6rem; font-weight:700; color:#1a2e25; text-align:center; margin-bottom:4px; }
  .card-sub   { font-size:.82rem; color:#7a9e8e; text-align:center; margin-bottom:28px; }
  .field { margin-bottom:15px; }
  .field label {
    display:block; font-size:.72rem; font-weight:600; color:#7a9e8e;
    letter-spacing:.07em; text-transform:uppercase; margin-bottom:5px;
  }
  .field-wrap {
    display:flex; align-items:center;
    border:1.5px solid #d8ece3; border-radius:10px; background:#eef6f1;
    transition:border-color .2s, box-shadow .2s;
  }
  .field-wrap:focus-within {
    border-color:#3c7962; background:#fff;
    box-shadow:0 0 0 3px rgba(60,121,98,.12);
  }
  .field-icon { padding:0 10px 0 13px; display:flex; align-items:center; color:#7a9e8e; flex-shrink:0; }
  .field-icon svg { width:17px; height:17px; }
  .field-wrap input {
    flex:1; border:none; outline:none; background:transparent;
    font-family:'Segoe UI',sans-serif; font-size:.95rem; color:#1a2e25;
    padding:12px 12px 12px 0;
  }
  .field-wrap input::placeholder { color:#7a9e8e; }
  .eye-btn {
    background:none; border:none; cursor:pointer;
    padding:0 12px; color:#7a9e8e; display:flex; align-items:center;
  }
  .eye-btn:hover { color:#3c7962; }
  .eye-btn svg { width:17px; height:17px; }
  .btn-login {
    width:100%; padding:13px; margin-top:6px; border:none; cursor:pointer;
    border-radius:10px;
    background:linear-gradient(135deg, #3c7962 0%, #2a5c46 100%);
    color:#fff; font-family:'Segoe UI',sans-serif;
    font-size:.98rem; font-weight:600; letter-spacing:.04em;
    box-shadow:0 6px 18px rgba(42,92,70,.28);
    transition:transform .15s, box-shadow .15s;
  }
  .btn-login:hover { transform:translateY(-1px); box-shadow:0 10px 24px rgba(42,92,70,.34); background:linear-gradient(135deg,#4c956c,#3c7962); }
  .btn-login:active { transform:translateY(0); }
  .forgot { display:block; text-align:center; margin-top:16px; font-size:.83rem; color:#3c7962; text-decoration:none; cursor:pointer; }
  .forgot:hover { color:#2a5c46; text-decoration:underline; }
  .error-msg { display:none; background:#fff0f0; border:1px solid #f5c0c0; border-radius:8px; padding:9px 12px; font-size:.82rem; color:#c0392b; margin-bottom:12px; text-align:center; }
  .error-msg.show { display:block; }
</style>
</head>
<body>
<div class='left'>
  <div class='star'>
    <svg viewBox='0 0 80 80' fill='none'>
      <path d='M40 4 L43 35 L74 40 L43 45 L40 76 L37 45 L6 40 L37 35 Z' fill='rgba(255,255,255,0.88)'/>
      <path d='M40 18 L41.8 36 L58 40 L41.8 44 L40 62 L38.2 44 L22 40 L38.2 36 Z' fill='rgba(255,255,255,0.28)'/>
    </svg>
  </div>
  <h1>¡Bienvenido!</h1>
  <p>Ingresa tus credenciales para acceder al sistema</p>
  <div class='dots'>
    <div class='dot active'></div><div class='dot'></div><div class='dot'></div>
  </div>
</div>
<div class='right'>
  <div class='card'>
    <div class='avatar'>
      <svg viewBox='0 0 36 36' fill='none'>
        <circle cx='13' cy='13' r='5.5' stroke='#3c7962' stroke-width='1.8'/>
        <path d='M4 30c0-5 4-9 9-9s9 4 9 9' stroke='#3c7962' stroke-width='1.8' stroke-linecap='round'/>
        <circle cx='24' cy='11' r='4.5' stroke='#4c956c' stroke-width='1.5'/>
        <path d='M20 29c0-4 2.5-7.5 6-8.5' stroke='#4c956c' stroke-width='1.5' stroke-linecap='round'/>
      </svg>
    </div>
    <h2 class='card-title'>Iniciar Sesión</h2>
    <p class='card-sub'>Sistema de Automatización de Nómina</p>
    <div class='error-msg' id='errorMsg'></div>
    <div class='field'>
      <label>Usuario</label>
      <div class='field-wrap'>
        <div class='field-icon'>
          <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.8' stroke-linecap='round' stroke-linejoin='round'>
            <path d='M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2'/><circle cx='12' cy='7' r='4'/>
          </svg>
        </div>
        <input type='text' id='txtUsuario' placeholder='Ingresa tu usuario' autocomplete='off'/>
      </div>
    </div>
    <div class='field'>
      <label>Contraseña</label>
      <div class='field-wrap'>
        <div class='field-icon'>
          <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.8' stroke-linecap='round' stroke-linejoin='round'>
            <rect x='3' y='11' width='18' height='11' rx='2'/><path d='M7 11V7a5 5 0 0 1 10 0v4'/>
          </svg>
        </div>
        <input type='password' id='txtContrasena' placeholder='Ingresa tu contraseña'/>
        <button class='eye-btn' id='eyeBtn' type='button'>
          <svg id='eyeIcon' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.8' stroke-linecap='round' stroke-linejoin='round'>
            <path d='M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z'/><circle cx='12' cy='12' r='3'/>
          </svg>
        </button>
      </div>
    </div>
    <button class='btn-login' id='btnIngresar'>Ingresar</button>
    <a class='forgot' id='lnkOlvide'>¿Olvidaste tu contraseña?</a>
  </div>
</div>
<script>
  const u   = document.getElementById('txtUsuario');
  const p   = document.getElementById('txtContrasena');
  const btn = document.getElementById('btnIngresar');
  const err = document.getElementById('errorMsg');

  document.getElementById('eyeBtn').addEventListener('click', () => {
    const show = p.type === 'password';
    p.type = show ? 'text' : 'password';
    document.getElementById('eyeIcon').innerHTML = show
      ? `<path d='M17.94 17.94A10 10 0 0 1 12 20c-7 0-11-8-11-8a18 18 0 0 1 5.06-5.94'/><path d='M9.9 4.24A9 9 0 0 1 12 4c7 0 11 8 11 8a18 18 0 0 1-2.16 3.19'/><line x1='1' y1='1' x2='23' y2='23'/>`
      : `<path d='M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z'/><circle cx='12' cy='12' r='3'/>`;
  });

  [u, p].forEach(el => el.addEventListener('keydown', e => { if(e.key==='Enter') btn.click(); }));

  btn.addEventListener('click', () => {
    err.classList.remove('show');
    if (!u.value.trim()) { showErr('El campo Usuario es obligatorio.');    u.focus(); return; }
    if (!p.value.trim()) { showErr('El campo Contraseña es obligatorio.'); p.focus(); return; }
    if (window.chrome && window.chrome.webview)
      window.chrome.webview.postMessage(JSON.stringify({ action:'login', usuario:u.value.trim(), contrasena:p.value.trim() }));
  });

  document.getElementById('lnkOlvide').addEventListener('click', () => {
    if (window.chrome && window.chrome.webview)
      window.chrome.webview.postMessage(JSON.stringify({ action:'forgotPassword' }));
  });

  function showErr(msg) { err.textContent = msg; err.classList.add('show'); }
  function loginFailed(msg) { showErr(msg || 'Usuario o contraseña incorrectos.'); }
</script>
</body>
</html>
");
            await Task.Delay(300);
            webView21.Focus();
        }

        // ── Recibir mensajes desde JavaScript ─────────────────────────────
        private void CoreWebView2_WebMessageReceived(object sender,
            CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => HandleMessage(e.TryGetWebMessageAsString()))); return; }
            HandleMessage(e.TryGetWebMessageAsString());
        }

        private void HandleMessage(string json)
        {
            if (json.Contains("\"action\":\"login\""))
            {
                string usuario = ExtractJson(json, "usuario");
                string contrasena = ExtractJson(json, "contrasena");
                ProcesarLogin(usuario, contrasena);
            }
            else if (json.Contains("\"action\":\"forgotPassword\""))
            {
                MessageBox.Show(
                    "Para recuperar tu contraseña, comunícate con el administrador del sistema.",
                    "¿Olvidaste tu contraseña?", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ── Conexión con la BLL ───────────────────────────────────────────
        private void ProcesarLogin(string usuario, string contrasena)
        {
            try
            {
                B_Usuarios bll = new B_Usuarios();
                E_Usuarios resultado = bll.Login(usuario, contrasena);

                if (resultado == null)
                {
                    // Credenciales incorrectas → mostrar error en la UI
                    webView21.CoreWebView2.ExecuteScriptAsync(
                        "loginFailed('Usuario o contraseña incorrectos.');");
                    return;
                }

                frmMaestros maestros = new frmMaestros();
                maestros.Show();
                this.Hide();  


            }
            catch (Exception ex)
            {
                webView21.CoreWebView2.ExecuteScriptAsync(
                    $"loginFailed('Error de conexión: {ex.Message.Replace("'", "")}');");
            }
        }

        // ── Helper: extrae valor de un JSON simple ────────────────────────
        private static string ExtractJson(string json, string key)
        {
            string search = $"\"{key}\":\"";
            int start = json.IndexOf(search);
            if (start < 0) return "";
            start += search.Length;
            int end = json.IndexOf('"', start);
            return end < 0 ? "" : json.Substring(start, end - start);
        }

        // ── Eventos vacíos requeridos por el Designer ─────────────────────
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void webView21_Click(object sender, EventArgs e) { }
    }
}



