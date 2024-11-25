namespace WebApi.Model;

public class Compra
{
    public int IdCompra { get; set; }
    public int IdProveedor { get; set; }
    public int IdUsuario { get; set; }
    public DateTime? FechaCompra { get; set; }
    public decimal Total { get; set; }
    public List<DetalleCompra> CompraDetalle { get; set; } = new List<DetalleCompra>();
}

public class DetalleCompra
{
    public int IdDetalleCompra { get; set; }
    public int IdCompra { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
