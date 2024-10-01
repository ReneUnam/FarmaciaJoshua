using WebApi.Model;

namespace WebApi.Interface;

public interface IRolService
{
    public RolesEntities Add(RolesEntities roles);
    public IEnumerable<RolesEntities> GetAll();
    public RolesEntities GetById(int id);
    public void Update(RolesEntities roles);
    public void Delete(int id);

}
