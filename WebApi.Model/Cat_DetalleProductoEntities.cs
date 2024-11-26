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
        public string Detalle_Descripcion { get; set; }
        public string Detalle_IdProducto { get; set; }
        public string Detalle_Estado { get; set; }
    }
}