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
    public class ClientesService : IClientesService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public ClientesService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("connectionSQL");
        }

        public ClientesEntities Add(ClientesEntities clientes)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("Ventas.Sp_AgregarClientes", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nombre", clientes.Nombre);
                command.Parameters.AddWithValue("@Apellido", clientes.Apellido);

                command.ExecuteNonQuery();
            }

            return clientes;
        }

        public void Delete(int id, int estado)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Ventas.Sp_ElimnarClientes", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@estado", estado);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ClientesEntities> GetByEstado(int estado)
        {
            var clientes = new List<ClientesEntities>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("Ventas.Sp_MostrarCliente", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@estado", estado);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new ClientesEntities()
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                        });
                    }
                }
            }

            return clientes;
        }

        public ClientesEntities GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Ventas.Sp_MostrarClientePorId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                ClientesEntities clientes = null;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        clientes = new ClientesEntities
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),

                        };
                    }
                }
                return clientes;
            }
        }

        public void Update(ClientesEntities clientes)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                var cmd = new SqlCommand("Ventas.Sp_EditarCliente", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", clientes.IdCliente == 0 ? (object)DBNull.Value : clientes.IdCliente);
                cmd.Parameters.AddWithValue("@nombre", string.IsNullOrEmpty(clientes.Nombre) ? (object)DBNull.Value : clientes.Nombre);
                cmd.Parameters.AddWithValue("@apellido", string.IsNullOrEmpty(clientes.Apellido) ? (object)DBNull.Value : clientes.Apellido);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
