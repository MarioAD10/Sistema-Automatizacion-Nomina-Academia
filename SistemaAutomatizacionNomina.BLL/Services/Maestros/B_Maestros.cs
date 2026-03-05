using SistemaAutomatizacionNomina.DAL.Repositories.Maestros;
using SistemaAutomatizacionNomina.Entities.Entities.Maestros;
using System.Collections.Generic;

namespace SistemaAutomatizacionNomina.BLL.Services.Maestros
{
    public class B_Maestros
    {
        D_Maestros objDato = new D_Maestros();

        public List<E_Maestros> ListarMaestro(string busqueda)
        {
            return objDato.ListarMaestro(busqueda);
        }

        public void InsertandoMaestro(E_Maestros m)
        {
            objDato.InsertarMaestro(m);
        }

        public void ModificandoMaestro(E_Maestros m)
        {
            objDato.ModificarMaestro(m);
        }

        public void EliminandoMaestro(E_Maestros m)
        {
            objDato.EliminarMaestro(m);
        }
    }
}
