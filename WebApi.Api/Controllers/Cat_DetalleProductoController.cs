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
    public class Cat_DetalleProductoController : ControllerBase
    {
        private readonly ICat_DetalleProductoService _ICat_DetalleProductoService;

        public Cat_DetalleProductoController(ICat_DetalleProductoService cat_detalleproducto)
        {
            ICat_DetalleProductoService = cat_detalleproductoService;
        }

        [HttpPost]
        public ActionResult Add(Cat_DetalleProductoEntities cat_detalleproducto)
        {
            try
            {
                _ICat_DetalleProductoService.Add(cat_detalleproducto);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Cat_DetalleProductoEntities>> GetAll()
        {
            try
            {
                var cat_detalleproducto = ICat_DetalleProductoService.GetAll();
                if (cat_detalleproducto == null)
                {
                    return NotFound(new { mensaje = "No hay Productos que mostrar" });
                }
                return Ok(cat_detalleproducto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Cat_DetalleProductoEntities> GetByID(int id)
        {
            try
            {
                var found = ICat_DetalleProductoService.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El producto no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Cat_DetalleProductoEntities cat_detalleproducto)
        {
            try
            {
                var find = ICat_DetalleProductoService.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se encontro el producto" });
                cat_detalleproducto.Detalle_Id = id;
                _ICat_DetalleProductoService.Update(cat_detalleproducto);
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
                var lolo = _ICat_DetalleProductoService.GetById(id);
                if (lolo == null) return NotFound();
                _ICat_DetalleProductoService.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}