using WebApi.Model;

namespace WebApi.Interface;

public interface IUsuarioService
{
    //public UsuarioEntities Add(UsuarioEntities usuario);
        Task<UsuarioEntities> add(UsuarioEntities usuario, string password);
        public IEnumerable<UsuarioEntities> GetAll();
        public UsuarioEntities GetById(int id);
        public void Update(UsuarioEntities usuario);
        public void Delete(int id);
}
