using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Implementation;

public class MateriaService : IMateriaService
{
    private readonly IConfiguration _configuration;
    private string connectionString;
    public MateriaService(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("DatabaseConnection");
    }
    public MateriaEntities Add(MateriaEntities materia)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("INSERT INTO CatMateria (Materia_Nombre) VALUES (@Name)", connection);
            command.Parameters.AddWithValue("@Name", materia.Name);
            connection.Open();
            command.ExecuteNonQuery();
        }
        return materia;
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("DELETE FROM CatMateria WHERE Materia_Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public IEnumerable<MateriaEntities> GetALL()
    {
        var materias = new List<MateriaEntities>();
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("Select * From CatMateria", connection);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    materias.Add(new MateriaEntities
                    {
                        Id = (int)reader["Materia_Id"],
                        Name = reader["Materia_Nombre"].ToString(),
                        State = (bool)reader["Materia_State"]
                    });
                }
            }
        }
        return materias;
    }

    public MateriaEntities GetByID(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("SELECT * FROM CatMateria WHERE Materia_Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            MateriaEntities materia = null;
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    materia = new MateriaEntities
                    {
                        Id = (int)reader["Materia_Id"],
                        Name = reader["Materia_Nombre"].ToString(),
                        State = (bool)reader["Materia_State"]
                    };
                }
            }

            return materia;
        }
    }

    public void Update(MateriaEntities materia)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("UPDATE CatMateria SET Materia_Nombre = @Name, Materia_State = @State WHERE Materia_Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", materia.Id);
            command.Parameters.AddWithValue("@Name", materia.Name);
            command.Parameters.AddWithValue("@State", materia.State);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
