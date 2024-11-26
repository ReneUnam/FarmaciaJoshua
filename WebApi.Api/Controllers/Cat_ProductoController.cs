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
    public class Cat_ProductoController : ControllerBase
    {
        private readonly ICat_ProductoService _ICat_ProductoService;

        public Cat_ProductoController(ICat_ProductoService cat_producto)
        {
            _ICat_ProductoService = cat_producto;
        }

        [HttpPost]
        public ActionResult Add(Cat_ProductoEntities cat_producto)
        {
            try
            {
                _ICat_ProductoService.Add(cat_producto);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult<IEnumerable<Cat_ProductoEntities>> GetAll()
        {
            try
            {
                var cat_producto = _ICat_ProductoService.GetAll();
                if (cat_producto == null)
                {
                    return NotFound(new { mensaje = "No hay Productos que mostrar" });
                }
                return Ok(cat_producto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Cat_ProductoEntities> GetByID(int id)
        {
            try
            {
                var found = _ICat_ProductoService.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El producto no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Cat_ProductoEntities cat_producto)
        {
            try
            {
                var find = _ICat_ProductoService.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se encontro el producto" });
                cat_producto.IdProducto = id;
                _ICat_ProductoService.Update(cat_producto);
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
                var lolo = _ICat_ProductoService.GetById(id);
                if (lolo == null) return NotFound();
                _ICat_ProductoService.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
