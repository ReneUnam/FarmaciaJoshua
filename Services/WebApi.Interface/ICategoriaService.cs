using WebApi.Model;

namespace WebApi.Interface;

public interface ICategoriaService
{
    public CategoriaEntities Add(CategoriaEntities categoria);
    public IEnumerable<CategoriaEntities> GetAll();
    public CategoriaEntities GetById(int id);
    public void Update(CategoriaEntities categoria);
    public void Delete(int id);
}
