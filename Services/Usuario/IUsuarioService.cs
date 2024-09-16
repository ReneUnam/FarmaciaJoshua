using FARMACIA_JOSHUA_RESTFUL.Models;

namespace FARMACIA_JOSHUA_RESTFUL.Services.Interface
{
    public interface IUsuarioService
    {
        public Usuario Add(Usuario producto);
        public IEnumerable<Usuario> GetAll();
        public Usuario GetById(int id);
        public void Update(Usuario producto);
        public void Delete(int id);
    }
}