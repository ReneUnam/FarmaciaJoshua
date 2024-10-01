using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Implementation;

public class RolService : IRolService
{
    private readonly IConfiguration _configuration;
    private string connectionString;

    public RolService(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("connectionSQL");
    }
    public RolesEntities Add(RolesEntities rol)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("Sp_AgregarRoles", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@nombre", rol.Nombre);
            command.Parameters.AddWithValue("@descripcion", rol.Descripcion);

            command.ExecuteNonQuery();
        }
        return rol;
    }

    public IEnumerable<RolesEntities> GetAll()
    {
        var rol = new List<RolesEntities>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new SqlCommand("Sp_MostrarRoles", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    rol.Add(new RolesEntities()
                    {
                        IdRol = Convert.ToInt32(reader["IdRol"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                    });
                }
            }
        }
        return rol;
    }

    public RolesEntities GetById(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("Sp_MostrarPorIdRol", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            RolesEntities rol = null;
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    rol = new RolesEntities
                    {
                        IdRol = Convert.ToInt32(reader["IdRol"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                    };
                }
            }
            return rol;
        }
    }

    public void Update(RolesEntities rol)
    {
        using (var conexion = new SqlConnection(connectionString))
        {
            conexion.Open();
            var cmd = new SqlCommand("Sp_EditarRol", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", rol.IdRol == 0 ? (object)DBNull.Value : rol.IdRol);
            cmd.Parameters.AddWithValue("@nombre", string.IsNullOrEmpty(rol.Nombre) ? (object)DBNull.Value : rol.Nombre);
            cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrEmpty(rol.Descripcion) ? (object)DBNull.Value : rol.Descripcion);

            cmd.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("Sp_EliminarRol", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

}
