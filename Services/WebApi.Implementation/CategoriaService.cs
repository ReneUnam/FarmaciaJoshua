using WebApi.Interface;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApi.Model;

namespace WebApi.Implementation;

public class CategoriaService : ICategoriaService
{
    private readonly IConfiguration _configuration;
    private string connectionString;

    public CategoriaService(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("connectionSQL");
    }

    public CategoriaEntities Add(CategoriaEntities categoria)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("Sp_AgregarCategoria", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@nombre", categoria.Nombre);
            command.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);

            command.ExecuteNonQuery();
        }

        return categoria;
    }

    public IEnumerable<CategoriaEntities> GetAll()
    {
        var categoria = new List<CategoriaEntities>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new SqlCommand("Sp_MostrarCategoria", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    categoria.Add(new CategoriaEntities()
                    {
                        IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                    });
                }
            }
        }
        return categoria;
    }

    public CategoriaEntities GetById(int id)
    {
        using(var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("Sp_MostrarCategoriaPorId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            CategoriaEntities categoria = null;
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    categoria = new CategoriaEntities
                    {
                        IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),

                    };
                }
            }
            return categoria;
        }
    }

    public void Update(CategoriaEntities categoria)
    {
        using (var conexion = new SqlConnection(connectionString))
        {
            conexion.Open();
            var cmd = new SqlCommand("Sp_EditarCategoria", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", categoria.IdCategoria == 0 ? (object)DBNull.Value : categoria.IdCategoria);
            cmd.Parameters.AddWithValue("@nombre", string.IsNullOrEmpty(categoria.Nombre) ? (object)DBNull.Value : categoria.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrEmpty(categoria.Descripcion) ? (object)DBNull.Value : categoria.Descripcion);
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("Sp_ElimnarCategoria", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
