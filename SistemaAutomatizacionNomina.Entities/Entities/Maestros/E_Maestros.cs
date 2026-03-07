using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAutomatizacionNomina.Entities.Entities.Maestros
{
    public class E_Maestros
    {
        private int idMaestro;
        private string nombreCompleto;
        private string documentoIdentidad;
        private string telefono;
        private string ocupacion;

        

        public int IdMaestro { get => idMaestro; set => idMaestro = value; }
        public string NombreCompleto { get => nombreCompleto; set => nombreCompleto = value; }
        public string DocumentoIdentidad { get => documentoIdentidad; set => documentoIdentidad = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Ocupacion { get => ocupacion; set => ocupacion = value; }
    }
}
