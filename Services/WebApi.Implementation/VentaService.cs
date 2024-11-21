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
                    detalleTable.Rows.Add(detalle.IdProducto, detalle.Cantidad, detalle.PrecioUnitario);
                }
                SqlParameter detalleParameter = new SqlParameter("@Detalles", SqlDbType.Structured)
                {
                    TypeName = "dbo.TDetalleVenta",
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
    public Venta GetByID(int id)
    {
        var venta = new Venta();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("Ventas.Sp_MostrarVentaPorId", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        venta.IdVenta = reader.GetInt32(0);
                        venta.IdCliente = reader.GetInt32(1);
                        venta.IdUsuario = reader.GetInt32(2);
                        venta.FechaVenta = reader.GetDateTime(3);
                        venta.Total = reader.GetDecimal(4);
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
                            IdProducto = reader.GetInt32(2),
                            Cantidad = reader.GetInt32(3),
                            PrecioUnitario = reader.GetDecimal(4),
                            Subtotal = reader.GetDecimal(5)
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
                        IdCliente = (int)reader["IdCliente"],
                        FechaVenta = (DateTime)reader["FechaVenta"],
                        Total = (decimal)reader["Total"],
                        VentaDetalle = new List<DetalleVenta>()
                    });
                }
            }

            using (var command = new SqlCommand("Ventas.Sp_MostrarDetalleVenta", connection))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var detalle = new DetalleVenta
                    {
                        IdDetalleVenta = (int)reader["IdDetalleVenta"],
                        IdVenta = (int)reader["IdVenta"],
                        IdProducto = (int)reader["IdProducto"],
                        Cantidad = (int)reader["Cantidad"],
                        PrecioUnitario = (decimal)reader["PrecioUnitario"],
                        Subtotal = (decimal)reader["Subtotal"]
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
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
    public void Update(Venta factura)
    {
        throw new NotImplementedException();
    }
}