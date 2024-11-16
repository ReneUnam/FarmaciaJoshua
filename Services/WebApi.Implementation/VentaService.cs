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
                using (SqlCommand command = new SqlCommand("sp_generarVenta", connection))
                {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

    }

    public IEnumerable<Venta> GetALL()
    {
        var ventas = new List<Venta>();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new SqlCommand("Sp_MostrarVenta", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ventas.Add(new Venta
                        {
                            IdVenta = (int)reader["IdVenta"],
                            IdCliente = (int)reader["IdCliente"],
                            FechaVenta = (DateTime)reader["FechaVenta"],
                            VentaDetalle = new List<DetalleVenta>()
                        });
                    }
                }
            }
            using (var command = new SqlCommand("Sp_MostrarDetallesVenta", connection))
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
                    };

                    var venta = ventas.FirstOrDefault(find => find.IdVenta == detalle.IdDetalleVenta);
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


    public Venta GetByID(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Venta factura)
    {
        throw new NotImplementedException();
    }
}

