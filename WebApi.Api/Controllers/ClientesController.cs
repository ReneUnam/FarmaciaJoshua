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
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _IClientesservice;

        public ClientesController(IClientesService clientesService)
        {
            _IClientesservice = clientesService;
        }

        [HttpPost]
        public ActionResult Add(ClientesEntities clientes)
        {
            try
            {
                _IClientesservice.Add(clientes);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet("estado/{estado}")]
        public ActionResult<IEnumerable<ClientesEntities>> GetByEstado(int estado)
        {
            try
            {
                var clientes = _IClientesservice.GetByEstado(estado);
                if (clientes == null || !clientes.Any())
                {
                    return NotFound(new { mensaje = "No hay clientes con el estado especificado" });
                }
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        

        [HttpGet("{id}")]
        public ActionResult<ClientesEntities> GetByID(int id)
        {
            try
            {
                var found = _IClientesservice.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El cliente no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, ClientesEntities clientes)
        {
            try
            {
                var find = _IClientesservice.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se encontro el cliente" });
                clientes.IdCliente = id;
                _IClientesservice.Update(clientes);
                return Ok(new { mensaje = "Actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id, int estado )
        {
            try
            {
                var lolo = _IClientesservice.GetById(id);
                if (lolo == null) return NotFound();
                _IClientesservice.Delete(id, estado);
                return Ok(new { mensaje = estado == 0 ? "Cliente desactivado correctamente" : "Cliente activado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
