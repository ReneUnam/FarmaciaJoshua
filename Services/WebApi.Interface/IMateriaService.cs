using WebApi.Model;

namespace WebApi.Interface;

public interface IMateriaService
{
    public MateriaEntities Add(MateriaEntities materia);
    public IEnumerable<MateriaEntities> GetALL();
    public MateriaEntities GetByID(int id);
    void Update(MateriaEntities materia);
    void Delete(int id);
}
