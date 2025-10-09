using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Implementation
{
    public class ProveedoresService : IProveedoresService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public ProveedoresService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("ConnectionSQL");
        }

        public ProveedoresEntities Add(ProveedoresEntities proveedores)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("Compras.Sp_AgregarProveedor", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nombre", proveedores.Nombre);
                command.Parameters.AddWithValue("@telefono", proveedores.Telefono);

                command.ExecuteNonQuery();
            }

            return proveedores;
        }

        public void Delete(int id, int estado)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Compras.Sp_ElimnarProveedores", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@Estado", estado);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ProveedoresEntities> GetByEstado(int estado)
        {
            var proveedores = new List<ProveedoresEntities>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("Compras.Sp_MostrarProveedores", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@estado", estado);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        proveedores.Add(new ProveedoresEntities()
                        {
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            Nombre = reader["Nombre"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                        });
                    }
                }
            }

            return proveedores;
        }

        public ProveedoresEntities GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Compras.Sp_MostrarProveedorPorId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                ProveedoresEntities proveedores = null;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        proveedores = new ProveedoresEntities
                        {
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            Nombre = reader["Nombre"].ToString(),
                            Telefono = reader["Telefono"].ToString(),

                        };
                    }
                }
                return proveedores;
            }
        }

        public void Update(ProveedoresEntities proveedores)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                var cmd = new SqlCommand("Compras.Sp_EditarProveedor", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", proveedores.IdProveedor == 0 ? (object)DBNull.Value : proveedores.IdProveedor);
                cmd.Parameters.AddWithValue("@nombre", string.IsNullOrEmpty(proveedores.Nombre) ? (object)DBNull.Value : proveedores.Nombre);
                cmd.Parameters.AddWithValue("@telefono", string.IsNullOrEmpty(proveedores.Telefono) ? (object)DBNull.Value : proveedores.Telefono);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
