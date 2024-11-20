using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Interface
{
    public interface IAuthService
    {
        Task<UsuarioEntities> Autenticar(String nombreUsuario, string Contraseña);
        string GenerateJwtToken(UsuarioEntities usuario);
    }
}
