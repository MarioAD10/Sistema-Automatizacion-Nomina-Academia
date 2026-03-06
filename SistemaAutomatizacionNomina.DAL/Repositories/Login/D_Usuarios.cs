using SistemaAutomatizacionNomina.Entities.Entities.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAutomatizacionNomina.DAL.Repositories.Login
{
    public class D_Usuarios
    {
        string conexion = ConfigurationManager.ConnectionStrings["Conectar"].ConnectionString;

        public E_Usuarios Login(string nombre, string password)
        {
            using (SqlConnection cnx = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginUsuario", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Password", password);

                cnx.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return new E_Usuarios
                    {
                        Nombre = nombre,
                        Password = password,
                        Rol = dr["Rol"].ToString()
                    };
                }
                return null;
            }
        }
    }
}
