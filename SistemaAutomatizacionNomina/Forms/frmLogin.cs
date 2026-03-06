using SistemaAutomatizacionNomina.BLL.Services.Login;
using SistemaAutomatizacionNomina.Entities.Entities.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaAutomatizacionNomina
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            SetPlaceholder(txtUsuario, "Nombre de Usuario");
            SetPlaceholder(txtPassword, "Contraseña");

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            B_Usuarios objNegocio = new B_Usuarios();
            E_Usuarios user = objNegocio.Login(txtUsuario.Text, txtPassword.Text);


            if (user != null)
            {
                if (user.Rol == "Admin" || user.Rol == "Administrador")
                {
                    frmMaestros frm = new frmMaestros();
                    frm.Show();

                    this.Hide();
                }

                else
                {
                   // EN PRÓXIMO SPRINT
                }
            }

            else
            {
                MessageBox.Show("Usuario o Contraseña Incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        //-----------------
        // PLACEHOLDER
        //-----------------

        private void SetPlaceholder(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;
            txt.Tag = placeholder;
            txt.Enter += RemovePlaceholder;
            txt.Leave += AddPlaceholder;
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == txt.Tag.ToString())
            {
                txt.Text = "";
                txt.ForeColor = Color.Gray;
            }
        }

        private void AddPlaceholder(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = txt.Tag.ToString();
                txt.ForeColor = Color.Gray;
            }
        }


        //-----------------
        // BASICOS
        //-----------------

        private void pbSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtUsuario.Text.Trim() != string.Empty)
                {
                    txtPassword.Focus();
                }
                else
                {
                    MessageBox.Show("Por favor, escriba el nombre del usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtPassword.Text.Trim() != string.Empty)
                {
                    btnIngresar.Focus();
                }
                else
                {
                    MessageBox.Show("Por favor, escriba la contraseña", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
