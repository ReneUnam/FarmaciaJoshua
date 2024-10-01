using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _IRolService;

        public RolController(IRolService rolService)
        {
            _IRolService = rolService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RolesEntities>> GetAll()
        {
            try
            {
                var rol = _IRolService.GetAll();
                if (rol == null)
                {
                    return NotFound(new { mensaje = "No hay un rol que mostrar" });
                }
                return Ok(rol);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<RolesEntities> GetByID(int id)
        {
            try
            {
                var found = _IRolService.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El Rol no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult Add(RolesEntities rol)
        {
            try
            {
                _IRolService.Add(rol);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, RolesEntities rol)
        {
            try
            {
                var find = _IRolService.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se pudo encontrar el Rol" });
                rol.IdRol = id;
                _IRolService.Update(rol);
                return Ok(new { mensaje = "Actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var find = _IRolService.GetById(id);
                if (find == null) return NotFound();
                _IRolService.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


    }
}
