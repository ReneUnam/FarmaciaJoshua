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
    public class ProductoAlmacenadoService : IProductoAlmacenadoService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public ProductoAlmacenadoService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("connectionSQL");
        }

        public ProductoAlmacenadoEntities Add(ProductoAlmacenadoEntities Tbl_ProductoAlmacenado)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("Productos.Sp_AgregarProductoAlmacenado", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@detalleid", Tbl_ProductoAlmacenado.Almc_Detalle_Id);
                command.Parameters.AddWithValue("@proveedorid", Tbl_ProductoAlmacenado.Almc_Proveedor_Id);
                command.Parameters.AddWithValue("@lote", Tbl_ProductoAlmacenado.Almc_Lote);
                command.Parameters.AddWithValue("@existencia", Tbl_ProductoAlmacenado.Almc_Existencia);
                command.Parameters.AddWithValue("@preciocompra", Tbl_ProductoAlmacenado.Almc_PrecioCompra);
                command.Parameters.AddWithValue("@precioventa", Tbl_ProductoAlmacenado.Almc_PrecioVenta);
                command.Parameters.AddWithValue("@estado", Tbl_ProductoAlmacenado.Almc_Estado);

                command.ExecuteNonQuery();
            }

            return Tbl_ProductoAlmacenado;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Productos.Sp_EliminarProductosAlmacenado", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ProductoAlmacenadoEntities> GetAll()
        {
            var productoalmacenado = new List<ProductoAlmacenadoEntities>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("Productos.Sp_MostrarProductosAlmacenado", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productoalmacenado.Add(new ProductoAlmacenadoEntities()
                        {
                            Almc_Id = Convert.ToInt32(reader["Almc_Id"]),
                            Almc_Detalle_Id = Convert.ToInt32(reader["Almc_Detalle_Id"]),
                            Almc_Proveedor_Id = Convert.ToInt32(reader["Almc_Proveedor_Id"]),
                            Almc_Lote = reader["Almc_Lote"].ToString(),
                            Almc_Existencia = Convert.ToInt32(reader["Almc_Existencia"]),
                            Almc_PrecioCompra = Convert.ToDecimal(reader["Almc_PrecioCompra"]),
                            Almc_PrecioVenta = Convert.ToDecimal(reader["Almc_PrecioVenta"]),
                            Almc_Estado = Convert.ToBoolean(reader["Almc_Estado"].ToString()),

                        });
                    }
                }
            }

            return productoalmacenado;
        }

        public ProductoAlmacenadoEntities GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Productos.Sp_MostrarProductosAlmacenadoPorId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                ProductoAlmacenadoEntities productoalmacenado = null;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productoalmacenado = new ProductoAlmacenadoEntities
                        {
                            Almc_Id = Convert.ToInt32(reader["Almc_Id"]),
                            Almc_Detalle_Id = Convert.ToInt32(reader["Almc_Detalle_Id"]),
                            Almc_Proveedor_Id = Convert.ToInt32(reader["Almc_Proveedor_Id"]),
                            Almc_Lote = reader["Almc_Lote"].ToString(),
                            Almc_Existencia = Convert.ToInt32(reader["Almc_Existencia"]),
                            Almc_PrecioCompra = Convert.ToDecimal(reader["Almc_PrecioCompra"]),
                            Almc_PrecioVenta = Convert.ToDecimal(reader["Almc_PrecioVenta"]),
                            Almc_Estado = Convert.ToBoolean(reader["Almc_Estado"]),
                        };
                    }
                }
                return productoalmacenado;
            }
        }

        public void Update(ProductoAlmacenadoEntities productoalmacenado)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                var cmd = new SqlCommand("Productos.Sp_EditarProductosAlmacenado", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@detalleid", productoalmacenado.Almc_Detalle_Id == 0 ? (object)DBNull.Value : productoalmacenado.Almc_Detalle_Id);
                cmd.Parameters.AddWithValue("@proveedorid", productoalmacenado.Almc_Proveedor_Id == 0 ? (object)DBNull.Value : productoalmacenado.Almc_Proveedor_Id);
                cmd.Parameters.AddWithValue("@lote", string.IsNullOrEmpty(productoalmacenado.Almc_Lote) ? (object)DBNull.Value : productoalmacenado.Almc_Lote);
                cmd.Parameters.AddWithValue("@existencia", productoalmacenado.Almc_Existencia == 0 ? (object)DBNull.Value : productoalmacenado.Almc_Existencia);
                cmd.Parameters.AddWithValue("@preciocompra", productoalmacenado.Almc_PrecioCompra == 0 ? (object)DBNull.Value : productoalmacenado.Almc_PrecioCompra);
                cmd.Parameters.AddWithValue("@precioventa", productoalmacenado.Almc_PrecioVenta == 0 ? (object)DBNull.Value : productoalmacenado.Almc_PrecioVenta);
                cmd.Parameters.AddWithValue("@estado", productoalmacenado.Almc_Estado == null ? (object)DBNull.Value : productoalmacenado.Almc_Estado);

                cmd.ExecuteNonQuery();
            }
        }
    }
}