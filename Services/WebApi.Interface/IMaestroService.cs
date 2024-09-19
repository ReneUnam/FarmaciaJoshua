using WebApi.Model;

namespace WebApi.Interface;

public interface IMaestroService
{

    public MaestroEntities Add(MaestroEntities maestro);
    public List<MaestroEntities> GetALL();
    public MaestroEntities GetByID(int id);
    void Update(MaestroEntities maestro);
    void Delete(int id);
}
