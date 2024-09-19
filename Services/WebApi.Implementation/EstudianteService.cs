using WebApi.Implementation;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Implementation;

public class EstudianteService : IEstudianteService
{
    private List<EstudianteEntities> _estudiantes;
    public EstudianteService()
    {
        _estudiantes = new List<EstudianteEntities>
            {
                new EstudianteEntities{Estudiante_Id=1, Estudiante_Nombre= "NOmbre", Estudiante_Estado=true, Estudiante_DNI="1234"},
                new EstudianteEntities{Estudiante_Id=2, Estudiante_Nombre= "NOmbre", Estudiante_Estado=true, Estudiante_DNI="1234"},
                new EstudianteEntities{Estudiante_Id=3, Estudiante_Nombre= "NOmbre", Estudiante_Estado=true, Estudiante_DNI="1234"},
                new EstudianteEntities{Estudiante_Id=4, Estudiante_Nombre= "NOmbre", Estudiante_Estado=true, Estudiante_DNI="1234"},
            };
    }

    public EstudianteEntities Add(EstudianteEntities estudiante)
    {
        estudiante.Estudiante_Id = _estudiantes.Max(p => p.Estudiante_Id) + 1;
        _estudiantes.Add(estudiante);
        return estudiante;
    }

    public void Delete(int id)
    {
        var find = _estudiantes.FirstOrDefault(p => p.Estudiante_Id == id);
        if (find != null)
        {
            Console.WriteLine(find);
            _estudiantes.Remove(find);
        }
    }

    public List<EstudianteEntities> GetALL()
    {
        return _estudiantes;
    }

    public EstudianteEntities GetByID(int id)
    {
        return _estudiantes.FirstOrDefault(p => p.Estudiante_Id == id);
    }

    public void Update(EstudianteEntities estudiante)
    {
        var find = _estudiantes.FirstOrDefault(p => p.Estudiante_Id == estudiante.Estudiante_Id);
        if (find != null)
        {
            find.Estudiante_Nombre = estudiante.Estudiante_Nombre;
            find.Estudiante_DNI = estudiante.Estudiante_DNI;
            find.Estudiante_Estado = estudiante.Estudiante_Estado;
        }
    }
}
