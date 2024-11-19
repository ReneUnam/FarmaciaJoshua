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

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var venta = _IVentaService.GetByID(id);
            if (venta == null) { return NotFound(); }
            return Ok(venta);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Venta>> GetAll()
        {
            var ventas = _IVentaService.GetALL();
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
            _IVentaService.Add(venta);
            return Ok();
        }

    }
}
