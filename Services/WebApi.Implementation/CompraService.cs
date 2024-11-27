using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApi.Interface;
using WebApi.Model;

public class CompraService : ICompraService
{
    private readonly IConfiguration _configuration;
    private string connectionString;
    public CompraService(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("connectionSQL");
    }
    public Compra Add(Compra compra)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                var command = new SqlCommand("Compras.Sp_AgregarCompra", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdProveedor", compra.IdProveedor);
                command.Parameters.AddWithValue("@idusuario", compra.IdUsuario);
                command.Parameters.AddWithValue("@fecha", compra.FechaCompra);

                DataTable detalleTable = new DataTable();
                detalleTable.Columns.Add("Id", typeof(int));
                detalleTable.Columns.Add("Cantidad", typeof(int));
                detalleTable.Columns.Add("Precio", typeof(decimal));

                foreach (var detalle in compra.CompraDetalle)
                {
                    detalleTable.Rows.Add(detalle.IdProducto, detalle.Cantidad, detalle.PrecioUnitario);
                }
                SqlParameter detalleParameter = new SqlParameter("@Detalles", SqlDbType.Structured)
                {
                    TypeName = "Compras.TDetalleCompra",
                    Value = detalleTable
                };

                command.Parameters.Add(detalleParameter);

                command.ExecuteNonQuery();

                return compra;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
    }
    public Compra GetByID(int id)
    {
        var compra = new Compra();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("Compras.Sp_MostrarCompraPorId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        compra.IdCompra = reader.GetInt32(0);
                        compra.IdProveedor = reader.GetInt32(1);
                        compra.IdUsuario = reader.GetInt32(2);
                        compra.FechaCompra = reader.GetDateTime(3);
                        compra.Total = reader.GetDecimal(4);
                    }
                }
            }

            using (var command = new SqlCommand("SELECT * FROM Compras.DetalleCompra WHERE IdCompra = @Id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    compra.CompraDetalle = new List<DetalleCompra>();
                    while (reader.Read())
                    {
                        compra.CompraDetalle.Add(new DetalleCompra
                        {
                            IdDetalleCompra = reader.GetInt32(0),
                            IdCompra = reader.GetInt32(1),
                            IdProducto = reader.GetInt32(2),
                            Cantidad = reader.GetInt32(3),
                            PrecioUnitario = reader.GetDecimal(4),
                            Subtotal = reader.GetDecimal(5)
                        });
                    }
                }
            }
        }
        return compra;
    }
    public IEnumerable<Compra> GetALL()
    {
        var compras = new List<Compra>();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new SqlCommand("Compras.Sp_MostrarCompras", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    compras.Add(new Compra
                    {
                        IdCompra = (int)reader["IdCompra"],
                        IdUsuario = (int)reader["IdUsuario"],
                        IdProveedor = (int)reader["IdProveedor"],
                        FechaCompra = (DateTime)reader["FechaCompra"],
                        Total = (decimal)reader["Total"],
                        CompraDetalle = new List<DetalleCompra>()
                    });
                }
            }

            using (var command = new SqlCommand("Compras.Sp_MostrarDetalleCompra", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var detalle = new DetalleCompra
                    {
                        IdDetalleCompra = (int)reader["IdDetalleCompra"],
                        IdCompra = (int)reader["IdCompra"],
                        IdProducto = (int)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Subtotal = (decimal)reader["Subtotal"]
                    };
                    var compra = compras.FirstOrDefault(find => find.IdCompra == detalle.IdCompra);
                    if (compra != null)
                    {
                        compra.CompraDetalle.Add(detalle);
                    }
                }
            }
        }
        return compras;
    }
    public void Update(Compra compra)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var compraCommand = new SqlCommand("Compras.Sp_EditarCompra", connection, transaction);
                    compraCommand.CommandType = CommandType.StoredProcedure;

                    compraCommand.Parameters.AddWithValue("@id", compra.IdCompra);
                    compraCommand.Parameters.AddWithValue("@idproveedor", compra.IdProveedor == 0 ? DBNull.Value : compra.IdProveedor);
                    compraCommand.Parameters.AddWithValue("@idusuario", compra.IdUsuario == 0 ? DBNull.Value : compra.IdUsuario);
                    compraCommand.Parameters.AddWithValue("@fecha", compra.FechaCompra ?? (object)DBNull.Value);

                    compraCommand.ExecuteNonQuery();

                    foreach (var detail in compra.CompraDetalle)
                    {
                        var detalleCommand = new SqlCommand("Compras.Sp_EditarDetalleCompra", connection, transaction);
                        detalleCommand.CommandType = CommandType.StoredProcedure;

                        detalleCommand.Parameters.AddWithValue("@idcompra", compra.IdCompra);
                        detalleCommand.Parameters.AddWithValue("@iddetalle", detail.IdDetalleCompra);
                        detalleCommand.Parameters.AddWithValue("@idproducto", detail.IdProducto == 0 ? DBNull.Value : detail.IdProducto);
                        detalleCommand.Parameters.AddWithValue("@cantidad", detail.Cantidad == 0 ? DBNull.Value : detail.Cantidad);
                        detalleCommand.Parameters.AddWithValue("@Precio", detail.PrecioUnitario == default(decimal) ? (object)DBNull.Value : detail.PrecioUnitario);

                        detalleCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
    public void Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var detallecompra = new SqlCommand("Compras.Sp_EliminarDetalleCompra", connection, transaction);
                    detallecompra.CommandType = CommandType.StoredProcedure;
                    detallecompra.Parameters.AddWithValue("@id", id);
                    detallecompra.ExecuteNonQuery();

                    var compra = new SqlCommand("Compras.Sp_EliminarCompra", connection, transaction);
                    compra.CommandType = CommandType.StoredProcedure;
                    compra.Parameters.AddWithValue("@id", id);
                    compra.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}