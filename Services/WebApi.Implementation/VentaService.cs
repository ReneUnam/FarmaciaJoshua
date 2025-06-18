using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApi.Interface;
using WebApi.Model;

public class VentaService : IVentaService
{
    private readonly IConfiguration _configuration;
    private string connectionString;
    public VentaService(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("connectionSQL");
    }
    public Venta Add(Venta venta)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                var command = new SqlCommand("Ventas.Sp_AgregarVenta", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Idcliente", venta.IdCliente);
                command.Parameters.AddWithValue("@idusuario", venta.IdUsuario);
                command.Parameters.AddWithValue("@fecha", venta.FechaVenta);

                DataTable detalleTable = new DataTable();
                detalleTable.Columns.Add("Id", typeof(int));
                detalleTable.Columns.Add("Cantidad", typeof(int));
                detalleTable.Columns.Add("Precio", typeof(decimal));

                foreach (var detalle in venta.VentaDetalle)
                {
                    detalleTable.Rows.Add(detalle.IdProductoAlmacenado, detalle.Cantidad, detalle.PrecioUnitario);
                }
                SqlParameter detalleParameter = new SqlParameter("@Detalles", SqlDbType.Structured)
                {
                    TypeName = "Ventas.TDetalleVenta",
                    Value = detalleTable
                };

                command.Parameters.Add(detalleParameter);

                command.ExecuteNonQuery();

                return venta;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
    }

    public string ObtenerProximoNumeroFactura()
    {
        string proximoNumero = null;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new SqlCommand("Ventas.Sp_ObtenerNumeroFacturaVisual", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        proximoNumero = reader["ProximoNumeroFactura"] as string;
                    }
                }
            }
        }

        return proximoNumero;
    }
    public Venta GetByID(int id)
    {
        var venta = new Venta();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("Ventas.Sp_MostrarVentaPorId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        venta.IdVenta = reader.GetInt32(0);
                        venta.NumeroFactura = reader["NumeroFactura"] as string;
                        venta.IdCliente = reader.GetInt32(2);
                        venta.IdUsuario = reader.GetInt32(3);
                        venta.FechaVenta = reader.GetDateTime(4);
                        venta.Descuento = reader.GetDecimal(5);
                        venta.Subtotal = reader.GetDecimal(6);
                        venta.Total = reader.GetDecimal(7);
                        venta.Estado = reader.GetBoolean(8);
                    }
                }
            }

            using (var command = new SqlCommand("SELECT * FROM Ventas.DetalleVenta WHERE IdVenta = @Id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    venta.VentaDetalle = new List<DetalleVenta>();
                    while (reader.Read())
                    {
                        venta.VentaDetalle.Add(new DetalleVenta
                        {
                            IdDetalleVenta = reader.GetInt32(0),
                            IdVenta = reader.GetInt32(1),
                            IdProductoAlmacenado = reader.GetInt32(2),
                            Cantidad = reader.GetInt32(3),
                            PrecioUnitario = reader.GetDecimal(4),
                            Descuento = reader.GetDecimal(5),
                            Subtotal = reader.GetDecimal(6),
                            Total = reader.GetDecimal(7)
                        });
                    }
                }
            }
        }
        return venta;
    }
    public IEnumerable<Venta> GetALL()
    {
        var ventas = new List<Venta>();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new SqlCommand("Ventas.Sp_MostrarVentas", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ventas.Add(new Venta
                    {
                        IdVenta = (int)reader["IdVenta"],
                        NumeroFactura = reader["NumeroFactura"] as string,
                        IdUsuario = (int)reader["IdUsuario"],
                        IdCliente = (int)reader["IdCliente"],
                        FechaVenta = (DateTime)reader["FechaVenta"],
                        Descuento = (decimal)reader["Descuento"],
                        Subtotal = (decimal)reader["Subtotal"],
                        Total = (decimal)reader["Total"],
                        Estado = (bool)reader["Estado"],
                        VentaDetalle = new List<DetalleVenta>()
                    });
                }
            }

            using (var command = new SqlCommand("Ventas.Sp_MostrarDetalleVenta", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var detalle = new DetalleVenta
                    {
                        IdDetalleVenta = (int)reader["IdDetalleVenta"],
                        IdVenta = (int)reader["IdVenta"],
                        IdProductoAlmacenado = (int)reader["IdProductoAlmacenado"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Descuento = (decimal)reader["Descuento"],
                        Subtotal = (decimal)reader["Subtotal"],
                        Total = (decimal)reader["Total"]
                    };
                    var venta = ventas.FirstOrDefault(find => find.IdVenta == detalle.IdVenta);
                    if (venta != null)
                    {
                        venta.VentaDetalle.Add(detalle);
                    }
                }
            }
        }
        return ventas;
    }
    public void Update(Venta venta)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var ventaCommand = new SqlCommand("Ventas.Sp_EditarVenta", connection, transaction);
                    ventaCommand.CommandType = CommandType.StoredProcedure;

                    ventaCommand.Parameters.AddWithValue("@id", venta.IdVenta);
                    ventaCommand.Parameters.AddWithValue("@idcliente", venta.IdCliente == 0 ? DBNull.Value : venta.IdCliente);
                    ventaCommand.Parameters.AddWithValue("@idusuario", venta.IdUsuario == 0 ? DBNull.Value : venta.IdUsuario);
                    ventaCommand.Parameters.AddWithValue("@fecha", venta.FechaVenta ?? (object)DBNull.Value);

                    ventaCommand.ExecuteNonQuery();

                    foreach (var detail in venta.VentaDetalle)
                    {
                        var detalleCommand = new SqlCommand("Ventas.Sp_EditarDetalleVenta", connection, transaction);
                        detalleCommand.CommandType = CommandType.StoredProcedure;

                        detalleCommand.Parameters.AddWithValue("@idventa", venta.IdVenta);
                        detalleCommand.Parameters.AddWithValue("@iddetalle", detail.IdDetalleVenta);
                        detalleCommand.Parameters.AddWithValue("@idproducto", detail.IdProductoAlmacenado == 0 ? DBNull.Value : detail.IdProductoAlmacenado);
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
                    var detalleventa = new SqlCommand("Ventas.Sp_EliminarDetalleVenta", connection, transaction);
                    detalleventa.CommandType = CommandType.StoredProcedure;
                    detalleventa.Parameters.AddWithValue("@id", id);
                    detalleventa.ExecuteNonQuery();

                    var venta = new SqlCommand("Ventas.Sp_EliminarVenta", connection, transaction);
                    venta.CommandType = CommandType.StoredProcedure;
                    venta.Parameters.AddWithValue("@id", id);
                    venta.ExecuteNonQuery();

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