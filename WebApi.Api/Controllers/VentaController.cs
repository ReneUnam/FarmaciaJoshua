using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interface;
using WebApi.Model;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _VentaService;

        public VentaController(IVentaService facturaService)
        {
            _VentaService = facturaService;
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var venta = _VentaService.GetByID(id);
            if (venta == null) { return NotFound(); }
            return Ok(venta);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Venta>> GetAll()
        {
            var ventas = _VentaService.GetALL();
            if (ventas == null) { return NotFound(); }
            return Ok(ventas);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Venta venta)
        {
            if (venta == null)
            {
                return BadRequest("Datos no encontrados");
            }
            _VentaService.Add(venta);
            return Ok(new { mensaje = "Agregado correctamente" });
        }

    }
}
