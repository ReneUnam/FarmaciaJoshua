using System.Diagnostics.CodeAnalysis;

namespace FARMACIA_JOSHUA_RESTFUL.Models{
    public class Producto{
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaDeVencimiento { get; set; } 
        public int IdCategoria { get; set; }
        public int IdProveedor { get; set; }
        public string Estado { get; set; }
    }
}