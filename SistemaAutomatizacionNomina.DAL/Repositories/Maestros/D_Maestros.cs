using SistemaAutomatizacionNomina.Entities.Entities.Maestros;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAutomatizacionNomina.DAL.Repositories.Maestros
{
    public class D_Maestros
    {
        string conexion = ConfigurationManager.ConnectionStrings["Conectar"].ConnectionString;

        public List<E_Maestros> ListarMaestro(string busqueda)
        {
            SqlDataReader LeerFilas;
            using (SqlConnection cnx = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_BuscarMaestro", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Busqueda", busqueda);

                cnx.Open();
                LeerFilas = cmd.ExecuteReader();

                List<E_Maestros> Listar = new List<E_Maestros>();
                while (LeerFilas.Read())
                {
                    Listar.Add(new E_Maestros()
                    {
                        IdMaestro = LeerFilas.GetInt32(0),
                        NombreCompleto = LeerFilas.GetString(1),
                        DocumentoIdentidad = LeerFilas.GetString(2),
                        Telefono = LeerFilas.GetString(3),
                        Ocupacion = LeerFilas.GetString(4)
                    });
                }

                LeerFilas.Close();
                return Listar;
            }
        }

        public bool InsertarMaestro (E_Maestros m)
        {
            using (SqlConnection cnx = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertarMaestro", cnx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NombreCompleto", m.NombreCompleto);
                cmd.Parameters.AddWithValue("@DocumentoIdentidad", m.DocumentoIdentidad);
                cmd.Parameters.AddWithValue("@Telefono", m.Telefono);
                cmd.Parameters.AddWithValue("@Ocupacion", m.Ocupacion);

                try
                {
                    cnx.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error en la base de datos: " + ex.Message);
                }
            }
        }

        public bool ModificarMaestro(E_Maestros m)
        {
            using (SqlConnection cnx = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_ActualizarMaestro", cnx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdMaestro", m.IdMaestro);
                cmd.Parameters.AddWithValue("@NombreCompleto", m.NombreCompleto);
                cmd.Parameters.AddWithValue("@DocumentoIdentidad", m.DocumentoIdentidad);
                cmd.Parameters.AddWithValue("@Telefono", m.Telefono);
                cmd.Parameters.AddWithValue("@Ocupacion", m.Ocupacion);

                try
                {
                    cnx.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error en la base de datos: " + ex.Message);
                }
            }
        }

        public bool EliminarMaestro(E_Maestros m)
        {
            using (SqlConnection cnx = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_EliminarMaestro", cnx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdMaestro", m.IdMaestro);              

                try
                {
                    cnx.Open();
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error en la base de datos: " + ex.Message);
                }
            }
        }

    }
}
