using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("connectionSQL");
        }

        public async Task<UsuarioEntities> Autenticar(string nombreUsuario, string Contraseña)
        {
            UsuarioEntities usuario = null;
            byte[] storedSalt = null;

            using (var connection = new SqlConnection(connectionString))
            {
               var command = new SqlCommand("select * from Usuarios where NombreUsuario = @NombreUsuario", connection);
                command.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync()) 
                {
                    if (reader.Read())
                    {
                        var passwordHash = reader["Contraseña"].ToString();
                        storedSalt = (byte[])reader["UsuarioSalt"];
                        if (VerifyPasswordHash(Contraseña, passwordHash, storedSalt))
                        {
                            usuario = new UsuarioEntities
                            {
                                IdUsuario = (int) reader["IdUsuario"],
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                Contraseña = passwordHash,
                                IdRol = (int) reader["IdRol"],
                            };
                        }
                    }
                }
            }


            return usuario;
        }

        public string GenerateJwtToken(UsuarioEntities usuario)
        {
            throw new NotImplementedException();
        }

        private bool VerifyPasswordHash(string contraseña, string passwordHash, byte[] usuarioSalt)
        {
            using var hmac = new HMACSHA256(usuarioSalt);
            var combinedBytes = Encoding.UTF8.GetBytes(contraseña).Concat(usuarioSalt).ToArray();
            var computedHash = hmac.ComputeHash(combinedBytes);
            return passwordHash == Convert.ToBase64String(computedHash);
        }
    }
}
