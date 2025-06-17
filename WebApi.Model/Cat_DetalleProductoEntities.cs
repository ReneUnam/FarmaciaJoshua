using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class Cat_DetalleProductoEntities
    {
        public int Detalle_Id { get; set; }
        public int Detalle_IdProducto { get; set; }
        public string Detalle_Descripcion { get; set; }
        public int Detalle_IdUnidadMedida { get; set; }
        public DateTime? Detalle_FechaVencimiento { get; set; }
        public bool Detalle_Estado { get; set; }
        public List<ProductoAlmacenadoEntities> ProductoAlmacenado { get; set; } = new List<ProductoAlmacenadoEntities>();
    }
}