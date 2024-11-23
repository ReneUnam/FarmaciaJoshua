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

        [HttpGet]
        public ActionResult<IEnumerable<ClientesEntities>> GetAll()
        {
            try
            {
                var clientes = _IClientesservice.GetAll();
                if (clientes == null)
                {
                    return NotFound(new { mensaje = "No hay clientes que mostrar" });
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
        public ActionResult Delete(int id)
        {
            try
            {
                var lolo = _IClientesservice.GetById(id);
                if (lolo == null) return NotFound();
                _IClientesservice.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
