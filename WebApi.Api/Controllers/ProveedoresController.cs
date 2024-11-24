using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Implementation;
using WebApi.Interface;
using WebApi.Model;


namespace WebApi.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedoresService _IProveedoresservice;

        public ProveedoresController(IProveedoresService proveedoresService)
        {
            _IProveedoresservice = proveedoresService;
        }

        [HttpPost]
        public ActionResult Add(ProveedoresEntities proveedores)
        {
            try
            {
                _IProveedoresservice.Add(proveedores);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<ProveedoresEntities>> GetAll()
        {
            try
            {
                var proveedores = _IProveedoresservice.GetAll();
                if (proveedores == null)
                {
                    return NotFound(new { mensaje = "No hay Proveedores que mostrar" });
                }
                return Ok(proveedores);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<ProveedoresEntities> GetByID(int id)
        {
            try
            {
                var found = _IProveedoresservice.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El proveedor no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, ProveedoresEntities proveedores)
        {
            try
            {
                var find = _IProveedoresservice.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se encontro el proveedor" });
                proveedores.IdProveedor = id;
                _IProveedoresservice.Update(proveedores);
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
                var lolo = _IProveedoresservice.GetById(id);
                if (lolo == null) return NotFound();
                _IProveedoresservice.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
