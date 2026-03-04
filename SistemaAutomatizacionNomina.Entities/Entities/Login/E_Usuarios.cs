using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAutomatizacionNomina.Entities.Entities.Login
{
    public class E_Usuarios
    {
        private int idUsuario;
        private string nombre;
        private string password;
        private string rol;

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
        public string Rol { get => rol; set => rol = value; }
    }
}
