using WebApi.Model;

namespace WebApi.Interface;

public interface IRolService
{
    public RolesEntities Add(RolesEntities roles);
    public IEnumerable<RolesEntities> GetByEstado(int estado);
    public RolesEntities GetById(int id);
    public void Update(RolesEntities roles);
    public void Delete(int id, int estado);

}
