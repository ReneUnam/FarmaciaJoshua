using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Controllers
{
    //[Authorize(Roles = "1")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ICompraService _ICompraService;

        public CompraController(ICompraService compraService)
        {
            _ICompraService = compraService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Compra>> GetAll()
        {
            var compra = _ICompraService.GetALL();
            if (compra == null) { return NotFound(); }
            return Ok(compra);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var compra = _ICompraService.GetByID(id);
            if (compra == null) { return NotFound(); }
            return Ok(compra);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Compra compra)
        {
            if (compra == null)
            {
                return BadRequest("Datos no encontrados");
            }
            _ICompraService.Add(compra);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Compra compra)
        {
            var find = _ICompraService.GetByID(id);
            if (find == null) return NotFound();
            compra.IdCompra = id;
            _ICompraService.Update(compra);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var find = _ICompraService.GetByID(id);
                if (find == null) return NotFound();
                _ICompraService.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    }
}
