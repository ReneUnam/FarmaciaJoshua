using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class Cat_ProductoEntities
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int IdCategoria { get; set; }
        public int IdLaboratorio { get; set; }
        public bool Estado { get; set; }
        public List<Cat_DetalleProductoEntities> DetalleProducto { get; set; } = new List<Cat_DetalleProductoEntities>();
    }
}