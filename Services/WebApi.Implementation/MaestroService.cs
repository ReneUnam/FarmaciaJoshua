using WebApi.Interface;
using WebApi.Model;

using System.Text.Json;

namespace WebApi.Implementation;

public class MaestroService : IMaestroService
{
    private readonly string JSONPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Services", "WebApi.Implementation", "Data", "Maestro.json");

    private List<MaestroEntities> ReadJSON()
    {
        if (!File.Exists(JSONPath))
        {
            Console.WriteLine("Archivo no encontrado: " + JSONPath);
            return new List<MaestroEntities>();
        }
        var data = File.ReadAllText(JSONPath);
        Console.WriteLine("Informacion:" + data);
        return JsonSerializer.Deserialize<List<MaestroEntities>>(data);
    }

    private void SaveJson(List<MaestroEntities> maestros)
    {
        var data = JsonSerializer.Serialize(maestros, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(JSONPath, data);
    }
    public MaestroEntities Add(MaestroEntities maestro)
    {
        var maestros = ReadJSON();
        maestro.Id = maestros.Any() ? maestros.Max(m => m.Id) + 1 : 1;
        maestros.Add(maestro);
        SaveJson(maestros);
        return maestro;
    }

    public void Delete(int id)
    {
        var maestros = ReadJSON();
        var maestro = maestros.FirstOrDefault(m => m.Id == id);
        if (maestro != null)
        {
            maestros.Remove(maestro);
            SaveJson(maestros);
        }
    }

    public List<MaestroEntities> GetALL()
    {
        return ReadJSON();
    }

    public MaestroEntities GetByID(int id)
    {
        var Maestros = ReadJSON();
        return Maestros.FirstOrDefault(f => f.Id == id);
    }

    public void Update(MaestroEntities maestro)
    {
        var maestros = ReadJSON();
        var find = maestros.FirstOrDefault(m => m.Id == maestro.Id);
        if (find != null)
        {
            find.Name = maestro.Name;
            find.DNI = maestro.DNI;
            find.State = maestro.State;
            SaveJson(maestros);
        }
    }
}
