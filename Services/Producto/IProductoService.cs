using FARMACIA_JOSHUA_RESTFUL.Models;

namespace FARMACIA_JOSHUA_RESTFUL.Services.Interface
{
    public interface IProductoService
    {
        public List<Producto> GetAll();
        public Producto GetById(int id);
        public void Update(Producto producto);
        public void Add(Producto producto);
        public void Delete(int id);
    }
}