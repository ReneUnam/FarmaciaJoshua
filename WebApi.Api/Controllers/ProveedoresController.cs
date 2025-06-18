using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Implementation;
using WebApi.Interface;
using WebApi.Model;


namespace WebApi.Api.Controllers
{
    
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

        
        [HttpGet("estado/{estado}")]
        public ActionResult<IEnumerable<ProveedoresEntities>> GetByEstado(int estado)
        {
            try
            {
                var proveedores = _IProveedoresservice.GetByEstado(estado);
                if (proveedores == null || !proveedores.Any())
                {
                    return NotFound(new { mensaje = "No hay proveedores con el estado especificado" });
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
        public ActionResult Delete(int id, [FromQuery] int estado = 0)
        {
            try
            {
                var proveedores = _IProveedoresservice.GetById(id);
                if (proveedores == null) return NotFound();
                _IProveedoresservice.Delete(id, estado);
                return Ok(new { mensaje = estado == 0 ? "Proveedor desactivado correctamente" : "Proveedor activado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
