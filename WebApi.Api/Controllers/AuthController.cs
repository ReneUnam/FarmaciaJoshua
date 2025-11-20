using Microsoft.AspNetCore.Mvc;
using WebApi.Api.DTO;
using WebApi.Interface;
using System.Diagnostics;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _IAuthService;
        private readonly IMetricService _metricService;

        public AuthController(IAuthService authService, IMetricService metricService)
        {
            _IAuthService = authService;
            _metricService = metricService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthDto usuario)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var user = await _IAuthService.Autenticar(usuario.NombreUsuario, usuario.Contraseña);
                sw.Stop();

                // Obtener la hora local
                var timestampLocal = DateTime.Now.ToString("o"); // Formato ISO 8601

                if (user == null)
                {
                    await _metricService.SaveLoginLog(
                        usuario.NombreUsuario,
                        "failure",
                        sw.ElapsedMilliseconds,
                        null,
                        "Credenciales inválidas",
                        new { usuario.NombreUsuario },
                        new { message = "Datos incorrectos" }
                    );
                    return BadRequest(new { message = "Datos incorrectos" });
                }

                var token = _IAuthService.GenerateJwtToken(user);

                await _metricService.SaveLoginLog(
                    usuario.NombreUsuario,
                    "success",
                    sw.ElapsedMilliseconds,
                    user.IdRol,
                    "Login exitoso",
                    new { usuario.NombreUsuario },
                    new { idUsuario = user.IdUsuario, idRol = user.IdRol }
                );

                return Ok(new
                {
                    Token = token,
                    idUsuario = user.IdUsuario,
                    nombres = user.Nombres,
                    apellidos = user.Apellidos,
                    nombreUsuario = user.NombreUsuario,
                    idRol = user.IdRol,
                    rol = user.Rol,
                    timestamp = timestampLocal // Agregar el timestamp local a la respuesta
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                await _metricService.SaveLoginLog(
                    usuario.NombreUsuario,
                    "error",
                    sw.ElapsedMilliseconds,
                    null,
                    "Error interno en login",
                    new { usuario.NombreUsuario },
                    new { error = ex.Message },
                    ex
                );
                return StatusCode(500, new { message = "Error interno" });
            }
        }
    }
}