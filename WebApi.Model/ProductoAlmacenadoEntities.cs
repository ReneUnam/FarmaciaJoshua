using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class ProductoAlmacenadoEntities
    {
        public int Almc_Id { get; set; }
        public string Almc_Detalle_Id { get; set; }
        public string Almc_Proveedor_Id { get; set; }
        public string Almc_Lote { get; set; }
        public string Almc_Existencia { get; set; }
        public string Almc_PrecioCompra { get; set; }
        public string Almc_PrecioVenta { get; set; }
        public string Almc_Estado { get; set; }
    }
}


