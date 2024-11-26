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
    public class Cat_ProductoService : ICat_ProductoService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public Cat_ProductoService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("connectionSQL");
        }

        public Cat_ProductoEntities Add(Cat_ProductoEntities cat_producto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("Productos.Sp_AgregarProducto", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nombre", cat_producto.Nombre);
                command.Parameters.AddWithValue("@estado", cat_producto.Estado);
                command.Parameters.AddWithValue("@fecha", cat_producto.Fecha_Vencimiento);

                command.ExecuteNonQuery();
            }

            return cat_producto;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Productos.Sp_ElimnarProductos", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Cat_ProductoEntities> GetAll()
        {
            var cat_producto = new List<Cat_ProductoEntities>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("Productos.Sp_MostrarProductos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cat_producto.Add(new Cat_ProductoEntities()
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            Nombre = reader["Nombre"].ToString(),
                            Estado = reader["Estado"].ToString(),
                            Fecha_Vencimiento = reader["Fecha"].ToString(),

                        });
                    }
                }
            }

            return cat_producto;
        }

        public Cat_ProductoEntities GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Productos.Sp_MostrarProductosPorId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                Cat_ProductoEntities cat_producto = null;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cat_producto = new Cat_ProductoEntities
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            Nombre = reader["Nombre"].ToString(),
                            Estado = reader["Estado"].ToString(),
                            Fecha_Vencimiento = reader["Fecha"].ToString(),

                        };
                    }
                }
                return cat_producto;
            }
        }

        public void Update(Cat_ProductoEntities cat_producto)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                var cmd = new SqlCommand("Productos.Sp_EditarProductos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", cat_producto.IdProducto == 0 ? (object)DBNull.Value : cat_producto.IdProducto);
                cmd.Parameters.AddWithValue("@nombre", string.IsNullOrEmpty(cat_producto.Nombre) ? (object)DBNull.Value : cat_producto.Nombre);
                cmd.Parameters.AddWithValue("@estado", string.IsNullOrEmpty(cat_producto.Estado) ? (object)DBNull.Value : cat_producto.Estado);
                cmd.Parameters.AddWithValue("@fecha", string.IsNullOrEmpty(cat_producto.Fecha_Vencimiento) ? (object)DBNull.Value : cat_producto.Fecha_Vencimiento);

                cmd.ExecuteNonQuery();
            }
        }
    }
}