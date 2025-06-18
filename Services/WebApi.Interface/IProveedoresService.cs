using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Interface
{
    public interface IProveedoresService
    {
        public ProveedoresEntities Add(ProveedoresEntities proveedores);
        public IEnumerable<ProveedoresEntities> GetByEstado(int estado);
        public ProveedoresEntities GetById(int id);
        public void Update(ProveedoresEntities proveedores);
        public void Delete(int id);
    }
}
