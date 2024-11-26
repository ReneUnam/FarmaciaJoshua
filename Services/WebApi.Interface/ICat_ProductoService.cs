using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Interface
{
    public interface ICat_ProductoService
    {
        public Cat_ProductoEntities Add(Cat_ProductoEntities cat_producto);
        public IEnumerable<Cat_ProductoEntities> GetAll();
        public Cat_ProductoEntities GetById(int id);
        public void Update(Cat_ProductoEntities cat_producto);
        public void Delete(int id);
    }
}
