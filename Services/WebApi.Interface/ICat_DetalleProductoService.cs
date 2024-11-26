using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Interface
{
    public interface ICat_DetalleProductoEntities
    {
        public Cat_DetalleProductoEntities Add(Cat_DetalleProductoEntities cat_detalleproducto);
        public IEnumerable<Cat_DetalleProductoEntities> GetAll();
        public Cat_DetalleProductoEntities GetById(int id);
        public void Update(Cat_DetalleProductoEntities cat_detalleproducto);
        public void Delete(int id);
    }
}
