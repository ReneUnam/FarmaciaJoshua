using FARMACIA_JOSHUA_RESTFUL.Models;
using FARMACIA_JOSHUA_RESTFUL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace FARMACIA_JOSHUA_RESTFUL.Services.Implementation
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("connectionSQL");
        }
        public Usuario Add(Usuario usuario)
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
        }

        public IEnumerable<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("Sp_Mostrar", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario()
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

        public Usuario GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("Sp_MostrarPorId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                Usuario usuario = null;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
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

        public void Update(Usuario usuario)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                var cmd = new SqlCommand("Sp_Editar", conexion);
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
                var command = new SqlCommand("Sp_Eliminar", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}