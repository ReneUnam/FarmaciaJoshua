namespace WebApi.Model;

public class Venta
{
    public int IdVenta { get; set; }
    public int IdCliente { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaVenta {get; set;}
    public decimal Total { get; set; }
    public List<DetalleVenta> VentaDetalle { get; set; } = new List<DetalleVenta>();
}

public class DetalleVenta{
    public int IdDetalleVenta { get; set; }
    public int IdVenta { get; set; }
    public int IdProducto { get; set; } 
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int Subtotal { get; set; } 
}
