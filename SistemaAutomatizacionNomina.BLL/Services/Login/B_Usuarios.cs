using SistemaAutomatizacionNomina.DAL.Repositories.Login;
using SistemaAutomatizacionNomina.Entities.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAutomatizacionNomina.BLL.Services.Login
{
    public class B_Usuarios
    {
        D_Usuarios objDato = new D_Usuarios();

        public E_Usuarios Login(string nombre, string password)
        {
            return objDato.Login(nombre, password);
        }
    }
}
