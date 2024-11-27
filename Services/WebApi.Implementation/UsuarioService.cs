using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Implementation;

public class UsuarioService : IUsuarioService
{
    private readonly IConfiguration _configuration;
    private string connectionString;

    public UsuarioService(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("connectionSQL");
    }

    /*public async Task<UsuarioEntities> Add(UsuarioEntities usuario)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("Sp_AgregarUsuario", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@nombres", usuario.Nombres);
            command.Parameters.AddWithValue("@apellidos", usuario.Apellidos);
            command.Parameters.AddWithValue("@nombreDeUsuario", usuario.NombreUsuario);
            command.Parameters.AddWithValue("@pwd", usuario.Contraseña);
            command.Parameters.AddWithValue("@idRol", usuario.IdRol);

            command.ExecuteNonQuery();
        }
        return usuario;
    }*/

    public IEnumerable<UsuarioEntities> GetAll()
    {
        var usuarios = new List<UsuarioEntities>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new SqlCommand("Sp_MostrarUsuarios", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuarios.Add(new UsuarioEntities()
                    {
                        IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                        Nombres = reader["Nombres"].ToString(),
                        Apellidos = reader["Apellidos"].ToString(),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Contraseña = reader["Contraseña"].ToString(),
                        IdRol = Convert.ToInt32(reader["IdRol"]),
                    });
                }
            }
        }
        return usuarios;
    }

    public UsuarioEntities GetById(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("Sp_MostrarUsuarioPorId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            UsuarioEntities usuario = null;
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new UsuarioEntities
                    {
                        IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                        Nombres = reader["Nombres"].ToString(),
                        Apellidos = reader["Apellidos"].ToString(),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Contraseña = reader["Contraseña"].ToString(),
                        IdRol = Convert.ToInt32(reader["IdRol"]),
                    };
                }
            }
            return usuario;
        }
    }

    public void Update(UsuarioEntities usuario)
    {
        using (var conexion = new SqlConnection(connectionString))
        {
            conexion.Open();
            var cmd = new SqlCommand("Sp_EditarUsuario", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", usuario.IdUsuario == 0 ? (object)DBNull.Value : usuario.IdUsuario);
            cmd.Parameters.AddWithValue("@nombres", string.IsNullOrEmpty(usuario.Nombres) ? (object)DBNull.Value : usuario.Nombres);
            cmd.Parameters.AddWithValue("@apellidos", string.IsNullOrEmpty(usuario.Apellidos) ? (object)DBNull.Value : usuario.Apellidos);
            cmd.Parameters.AddWithValue("@nombreDeUsuario", string.IsNullOrEmpty(usuario.NombreUsuario) ? (object)DBNull.Value : usuario.NombreUsuario);
            cmd.Parameters.AddWithValue("@pwd", string.IsNullOrEmpty(usuario.Contraseña) ? (object)DBNull.Value : usuario.Contraseña);
            cmd.Parameters.AddWithValue("@idrol", usuario.IdRol == 0 ? (object)DBNull.Value : usuario.IdRol);

            cmd.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("Sp_EliminarUsuario", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public async Task<UsuarioEntities> add(UsuarioEntities usuario, string Contraseña)
    {
        byte[] salt;
        usuario.Contraseña = CreatePasswordHash(Contraseña, out salt);

        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("INSERT INTO Usuarios(Nombres, Apellidos, NombreUsuario, Contraseña, IdRol, UsuarioSalt) OUTPUT INSERTED.IdUsuario VALUES (@Nombres, @Apellidos, @NombreUsuario, @PasswordHash, @IdRol, @Salt)", connection);
            command.Parameters.AddWithValue("@Nombres", usuario.Nombres);
            command.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
            command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
            command.Parameters.AddWithValue("@PasswordHash", usuario.Contraseña);
            command.Parameters.AddWithValue("@Salt", salt);
            command.Parameters.AddWithValue("@IdRol", usuario.IdRol);

            await connection.OpenAsync();
            usuario.IdUsuario = (int)await command.ExecuteScalarAsync();
        }
        return usuario;
    }

    private string CreatePasswordHash(string Contraseña, out byte[] salt)
    {
        using var hmac = new HMACSHA256();
        salt = hmac.Key;
        var combinedBytes = Encoding.UTF8.GetBytes(Contraseña).Concat(salt).ToArray();
        var hash = hmac.ComputeHash(combinedBytes);
        return Convert.ToBase64String(hash);
    }
}



