using SistemaAutomatizacionNomina.BLL.Services.Maestros;
using SistemaAutomatizacionNomina.Entities.Entities.Maestros;
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
    public partial class frmMaestros : Form
    {
        public frmMaestros()
        {
            InitializeComponent();
        }

        int idSeleccionado = 0;

        private void frmMaestros_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            MostrarBuscarTabla("");
            EstiloDGV();
        }

        //-------------
        // BOTONES
        //-------------

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            E_Maestros maestro = new E_Maestros();
            maestro.NombreCompleto = txtNombre.Text;
            maestro.DocumentoIdentidad = txtDocumentoIdentidad.Text;
            maestro.Telefono = txtTelefono.Text;
            maestro.Ocupacion = txtOcupacion.Text;

            try
            {
                B_Maestros objNegocio = new B_Maestros();
                objNegocio.InsertandoMaestro(maestro);

                MessageBox.Show("Maestro registrado correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MostrarBuscarTabla("");
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            E_Maestros maestro = new E_Maestros();
            maestro.IdMaestro = idSeleccionado;
            maestro.NombreCompleto = txtNombre.Text;
            maestro.DocumentoIdentidad = txtDocumentoIdentidad.Text;
            maestro.Telefono = txtTelefono.Text;
            maestro.Ocupacion = txtOcupacion.Text;

            try
            {
                B_Maestros objNegocio = new B_Maestros();
                objNegocio.ModificandoMaestro(maestro);

                MessageBox.Show("Maestro actualizado correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MostrarBuscarTabla("");
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            if (dgvMaestros.SelectedRows.Count > 0)
            {
                DataGridViewRow fila  = dgvMaestros.SelectedRows[0];

                idSeleccionado = Convert.ToInt32(fila.Cells[0].Value);
                txtNombre.Text = fila.Cells[1].Value.ToString();
                txtDocumentoIdentidad.Text = fila.Cells[2].Value.ToString();
                txtTelefono.Text = fila.Cells[3].Value.ToString();
                txtOcupacion.Text = fila.Cells[4].Value.ToString();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            E_Maestros maestro = new E_Maestros();
            maestro.IdMaestro = idSeleccionado;

            B_Maestros objNegocio = new B_Maestros();
            objNegocio.EliminandoMaestro(maestro);

            MessageBox.Show("Maestro eliminado correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            MostrarBuscarTabla("");
            LimpiarFormulario();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }


        //-------------
        // BOTONES
        //-------------


        public void MostrarBuscarTabla(string busqueda)
        {
            B_Maestros objNegocio = new B_Maestros();
            dgvMaestros.DataSource = objNegocio.ListarMaestro(busqueda);
        }

        private void EstiloDGV()
        {
            this.dgvMaestros.ColumnHeadersVisible = false;
            this.dgvMaestros.AllowUserToAddRows = false;
            this.dgvMaestros.AllowUserToDeleteRows = false;
            this.dgvMaestros.RowHeadersVisible = false;

            dgvMaestros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaestros.MultiSelect = false;


            dgvMaestros.DefaultCellStyle.SelectionBackColor = Color.FromArgb(74, 145, 105);
            dgvMaestros.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvMaestros.DefaultCellStyle.Font = new Font("Microsoft YaHei", 11);

            DataGridViewColumn column;
            column = dgvMaestros.Columns[00]; column.Width = 45;
            column = dgvMaestros.Columns[01]; column.Width = 165;
            column = dgvMaestros.Columns[02]; column.Width = 140;
            column = dgvMaestros.Columns[03]; column.Width = 140;
            column = dgvMaestros.Columns[04]; column.Width = 150;

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            MostrarBuscarTabla(txtBuscar.Text);
        }

        private void pbSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtDocumentoIdentidad.Clear();
            txtTelefono.Clear();
            txtOcupacion.Clear();
            txtBuscar.Clear();
        }

        private void dgvMaestros_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvMaestros.Rows[e.RowIndex];
                
                idSeleccionado = Convert.ToInt32(fila.Cells[0].Value);
                txtNombre.Text = fila.Cells[1].Value.ToString();
                txtDocumentoIdentidad.Text = fila.Cells[2].Value.ToString();
                txtTelefono.Text = fila.Cells[3].Value.ToString();
                txtOcupacion.Text = fila.Cells[4].Value.ToString();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmMaestros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar  == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtNombre.Text.Trim() != string.Empty)
                {
                    txtDocumentoIdentidad.Focus();
                }
            }
        }

        private void txtDocumentoIdentidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtDocumentoIdentidad.Text.Trim() != string.Empty)
                {
                    txtTelefono.Focus();
                }
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtTelefono.Text.Trim() != string.Empty)
                {
                    txtOcupacion.Focus();
                }
            }
        }

        private void txtOcupacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                e.Handled = true;
                if (txtOcupacion.Text.Trim() != string.Empty)
                {
                    btnGuardar.Focus();
                }
            }
        }
    }
}
