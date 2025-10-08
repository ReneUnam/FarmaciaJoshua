using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interface;
using WebApi.Model;
using WebApi.Api.DTO;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _IUsuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _IUsuarioService = usuarioService;
        }

        [HttpGet("estado/{estado}")]
        public ActionResult<IEnumerable<UsuarioEntities>> GetByEstado(int estado)
        {
            try
            {
                var usuarios = _IUsuarioService.GetByEstado(estado);
                if (usuarios == null || !usuarios.Any())
                {
                    return NotFound(new { mensaje = "No hay usuarios con el estado especificado" });
                }
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UsuarioEntities> GetByID(int id)
        {
            try
            {
                var found = _IUsuarioService.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El usuario no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> add([FromBody] UsuarioDto usuario)
        {
            var user = new UsuarioEntities
            {
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                NombreUsuario = usuario.NombreUsuario,
                IdRol = usuario.IdRol,

            };

            try
            {
                await _IUsuarioService.add(user, usuario.Contraseña);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, UsuarioEntities usuario)
        {
            try
            {
                var find = _IUsuarioService.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se pudo encontrar el usuario" });
                usuario.IdUsuario = id;
                _IUsuarioService.Update(usuario);
                return Ok(new { mensaje = "Actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id, [FromQuery] int estado = 0)
        {
            try
            {
                var find = _IUsuarioService.GetById(id);
                if (find == null) return NotFound();
                _IUsuarioService.Delete(id, estado);
                return Ok(new { mensaje = estado == 0 ? "Usuario desactivado correctamente" : "Usuario activado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
