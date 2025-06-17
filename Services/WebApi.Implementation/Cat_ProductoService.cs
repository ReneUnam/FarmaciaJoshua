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
                command.Parameters.AddWithValue("@idcategoria", cat_producto.IdCategoria);

                command.ExecuteNonQuery();
            }

            return cat_producto;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Productos.Sp_EliminarProductos", connection);
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
                        int idProducto = Convert.ToInt32(reader["IdProducto"]);
                        var producto = cat_producto.FirstOrDefault(p => p.IdProducto == idProducto);

                        if (producto == null)
                        {
                            producto = new Cat_ProductoEntities
                            {
                                IdProducto = idProducto,
                                Nombre = reader["Nombre"].ToString(),
                                IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                                IdLaboratorio = Convert.ToInt32(reader["IdLaboratorio"]),
                                Estado = Convert.ToBoolean(reader["Estado"]),
                                DetalleProducto = new List<Cat_DetalleProductoEntities>()
                            };
                            cat_producto.Add(producto);
                        }

                        int detalleId = Convert.ToInt32(reader["Detalle_Id"]);
                        var detalle = producto.DetalleProducto.FirstOrDefault(d => d.Detalle_Id == detalleId);
                        if (detalle == null)
                        {
                            detalle = new Cat_DetalleProductoEntities
                            {
                                Detalle_Id = detalleId,
                                Detalle_IdProducto = reader["Detalle_IdProducto"]!= DBNull.Value ? Convert.ToInt32(reader["Detalle_IdProducto"]) : 0,
                                Detalle_Descripcion = reader["Detalle_Descripcion"].ToString(),
                                Detalle_IdUnidadMedida = Convert.ToInt32(reader["Detalle_IdUnidadMedida"]),
                                Detalle_FechaVencimiento = reader["Detalle_FechaVencimiento"] as DateTime?,
                                Detalle_Estado = Convert.ToBoolean(reader["Detalle_Estado"]),
                                ProductoAlmacenado = new List<ProductoAlmacenadoEntities>()
                            };
                            producto.DetalleProducto.Add(detalle);
                        }
                        if (reader["Almc_Id"] != DBNull.Value)
                        {
                            detalle.ProductoAlmacenado.Add(new ProductoAlmacenadoEntities
                            {
                                Almc_Id = Convert.ToInt32(reader["Almc_Id"]),
                                Almc_Detalle_Id = reader["Almc_Detalle_Id"]!= DBNull.Value ? Convert.ToInt32(reader["Almc_Detalle_Id"]) : 0,
                                Almc_Proveedor_Id = Convert.ToInt32(reader["Almc_Proveedor_Id"]),
                                Almc_Lote = reader["Almc_Lote"].ToString(),
                                Almc_Existencia = Convert.ToInt32(reader["Almc_Existencia"]),
                                Almc_PrecioCompra = Convert.ToDecimal(reader["Almc_PrecioCompra"]),
                                Almc_PrecioVenta = Convert.ToDecimal(reader["Almc_PrecioVenta"]),
                                Almc_Estado = Convert.ToBoolean(reader["Almc_Estado"])

                            });
                        }
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
                            // Estado = reader["Estado"].ToString(),
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"])
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
                // cmd.Parameters.AddWithValue("@estado", string.IsNullOrEmpty(cat_producto.Estado) ? (object)DBNull.Value : cat_producto.Estado);
                cmd.Parameters.AddWithValue("@idcategoria", cat_producto.IdCategoria == 0 ? (object)DBNull.Value : cat_producto.IdCategoria);

                cmd.ExecuteNonQuery();
            }
        }
    }
}