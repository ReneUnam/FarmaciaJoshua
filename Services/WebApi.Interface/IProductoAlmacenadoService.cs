using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Interface
{
    public interface IProductoAlmacenadoService
    {
        public ProductoAlmacenadoEntities Add(ProductoAlmacenadoEntities productoalmacenado);
        public IEnumerable<ProductoAlmacenadoEntities> GetAll();
        public ProductoAlmacenadoEntities GetById(int id);
        public void Update(ProductoAlmacenadoEntities productoalmacenado);
        public void Delete(int id);
    }
}
