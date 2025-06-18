using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _IVentaService;

        public VentaController(IVentaService ventaService)
        {
            _IVentaService = ventaService;
        }
        
        [HttpGet("proximo-numero")]
        public IActionResult ObtenerProximoNumeroFactura()
        {
            try
            {
                var numero = _IVentaService.ObtenerProximoNumeroFactura();
                return Ok(new { proximoNumeroFactura = numero });
            }
            catch (Exception error)
            {
                // Log error (no mostrado aquí)
                return StatusCode(500, new { error = "Error al obtener el número de factura" });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Venta>> GetAll()
        {
            var ventas = _IVentaService.GetALL();
            if (ventas == null) { return NotFound(); }
            return Ok(ventas);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var venta = _IVentaService.GetByID(id);
            if (venta == null) { return NotFound(); }
            return Ok(venta);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Venta venta)
        {
            if (venta == null)
            {
                return BadRequest("Datos no encontrados");
            }
            _IVentaService.Add(venta);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Venta venta)
        {
            var find = _IVentaService.GetByID(id);
            if (find == null) return NotFound();
            venta.IdVenta = id;
            _IVentaService.Update(venta);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var find = _IVentaService.GetByID(id);
                if (find == null) return NotFound();
                _IVentaService.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    }
}
