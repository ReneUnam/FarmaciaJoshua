using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Interface
{
    public interface IClientesService
    {
        public ClientesEntities Add(ClientesEntities clientes);
        public IEnumerable<ClientesEntities> GetAll();
        public ClientesEntities GetById(int id);
        public void Update(ClientesEntities clientes);
        public void Delete(int id);
    }
}
