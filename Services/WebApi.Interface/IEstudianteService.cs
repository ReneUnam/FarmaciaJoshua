using WebApi.Model;

namespace WebApi.Interface;

public interface IEstudianteService
{
    public EstudianteEntities Add(EstudianteEntities estudiante);
    public List<EstudianteEntities> GetALL();
    public EstudianteEntities GetByID(int id);
    void Update(EstudianteEntities estudiante);
    void Delete(int id);
}
