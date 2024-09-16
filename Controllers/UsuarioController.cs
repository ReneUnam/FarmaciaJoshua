using FARMACIA_JOSHUA_RESTFUL.Models;
using FARMACIA_JOSHUA_RESTFUL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _IUsuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _IUsuarioService = usuarioService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetAll()
        {
            try
            {
                var usuarios = _IUsuarioService.GetAll();
                if (usuarios == null)
                {
                    return NotFound(new { mensaje = "No hay usuarios que mostrar" });
                }
                return Ok(usuarios);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> GetByID(int id)
        {
            try{
                var found = _IUsuarioService.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El usuario no existe"} );

                return Ok(found);
            }
            catch (Exception ex){
                return  StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
            
        }

        [HttpPost]
        public ActionResult Add(Usuario usuario)
        {
            try
            {
                _IUsuarioService.Add(usuario);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Usuario usuario)
        {
            try{
                var find = _IUsuarioService.GetById(id);
                if (find == null) return NotFound(new {mensaje ="No se pudo encontrar el usuario"} );
                usuario.IdUsuario = id;
                _IUsuarioService.Update(usuario);
                return Ok(new {mensaje ="Actualizado correctamente"} );
            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, new {mensaje = ex.Message});
            }

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try{
                var find = _IUsuarioService.GetById(id);
                if (find == null) return NotFound();
                _IUsuarioService.Delete(id);
                return Ok(new {mensaje ="Borrado correctamente"});
            }catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, new {mensaje = ex.Message});
            }
        }
    }
}
