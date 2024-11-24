using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Api.DTO;
using WebApi.Interface;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _IAuthService;

        public AuthController(IAuthService authService)
        {
            _IAuthService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthDto usuario)
        {
            var user = await _IAuthService.Autenticar(usuario.NombreUsuario, usuario.Contraseña);

            if (user == null)
                return BadRequest(new { message = "Datos incorrectos" });

            var token = _IAuthService.GenerateJwtToken(user);
            return Ok(new { Token = token });
            
        }

    }
}
