using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApi.Interface;
using WebApi.Model;

public class VentaService : IVentaService
{
    private readonly IConfiguration _configuration;
    private string connectionString;

    public VentaService(IConfiguration configuration){
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
                using (SqlCommand command = new SqlCommand("sp_CrearFactura", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Idcliente", venta.IdVenta);
                    command.Parameters.AddWithValue("@fecha", venta.FechaVenta);

                    DataTable detalleTable = new DataTable();
                    detalleTable.Columns.Add("Id", typeof(string));
                    detalleTable.Columns.Add("Cantidad", typeof(int));
                    detalleTable.Columns.Add("Precio", typeof(decimal));

                    foreach (var detalle in venta.VentaDetalle)
                    {
                        detalleTable.Rows.Add(detalle.IdProducto, detalle.Cantidad, detalle.PrecioUnitario);
                    }

                    SqlParameter detalleParameter = new SqlParameter("@Detalles", SqlDbType.Structured)
                    {
                        TypeName = "dbo.TDetalleFactura",
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

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Venta> GetALL()
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

