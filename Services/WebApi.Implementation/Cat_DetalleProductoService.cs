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
using System.Diagnostics.Tracing;

namespace WebApi.Implementation
{
    public class Cat_DetalleProductoService : ICat_DetalleProductoService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public Cat_DetalleProductoService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("connectionSQL");
        }

        public Cat_DetalleProductoEntities Add(Cat_DetalleProductoEntities cat_detalleproducto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("Productos.Sp_AgregarDetalleProducto", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@descripcion", cat_detalleproducto.Detalle_Descripcion);
                command.Parameters.AddWithValue("@idproducto", cat_detalleproducto.Detalle_IdProducto);
                command.Parameters.AddWithValue("@estado", cat_detalleproducto.Detalle_Estado);
                command.Parameters.AddWithValue("@fecha", cat_detalleproducto.Detalle_FechaVencimiento);

                command.ExecuteNonQuery();
            }

            return cat_detalleproducto;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Productos.Sp_ElimnarDetalleProductos", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Cat_DetalleProductoEntities> GetAll()
        {
            var cat_detalleproducto = new List<Cat_DetalleProductoEntities>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("Productos.Sp_MostrarDetalleProductos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cat_detalleproducto.Add(new Cat_DetalleProductoEntities()
                        {
                            Detalle_Id = Convert.ToInt32(reader["Detalle_Id"]),
                            Detalle_Descripcion = reader["Detalle_Descripcion"].ToString(),
                            Detalle_IdProducto = Convert.ToInt32(reader["Detalle_IdProducto"]),
                            Detalle_Estado = reader["Detalle_Estado"].ToString(),
                            Detalle_FechaVencimiento = (DateTime)reader["Detalle_FechaVencimiento"]

                        });
                    }
                }
            }

            return cat_detalleproducto;
        }

        public Cat_DetalleProductoEntities GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Productos.Sp_MostrarDetalleProductosPorId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                Cat_DetalleProductoEntities cat_detalleproducto = null;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cat_detalleproducto = new Cat_DetalleProductoEntities
                        {
                            Detalle_Id = Convert.ToInt32(reader["Detalle_Id"]),
                            Detalle_Descripcion = reader["Detalle_Descripcion"].ToString(),
                            Detalle_IdProducto = Convert.ToInt32(reader["Detalle_IdProducto"]),
                            Detalle_Estado = reader["Detalle_Estado"].ToString(),
                            Detalle_FechaVencimiento = (DateTime)reader["Detalle_FechaVencimiento"]
                        };
                    }
                }
                return cat_detalleproducto;
            }
        }

        public void Update(Cat_DetalleProductoEntities cat_detalleproducto)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                var cmd = new SqlCommand("Productos.Sp_EditarDetalleProductos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@Detalle_Id", cat_detalleproducto.Detalle_Id);
                cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(cat_detalleproducto.Detalle_Descripcion) ? (object)DBNull.Value : cat_detalleproducto.Detalle_Descripcion);
                cmd.Parameters.AddWithValue("@IdProducto", cat_detalleproducto.Detalle_IdProducto == 0 ? (object)DBNull.Value : cat_detalleproducto.Detalle_IdProducto);
                cmd.Parameters.AddWithValue("@Estado", string.IsNullOrEmpty(cat_detalleproducto.Detalle_Estado) ? (object)DBNull.Value : cat_detalleproducto.Detalle_Estado);

                cmd.ExecuteNonQuery();
            }
        }
    }
}