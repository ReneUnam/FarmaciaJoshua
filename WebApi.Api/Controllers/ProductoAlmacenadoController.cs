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
    public class ProductoAlmacenadoController : ControllerBase
    {
        private readonly IProductoAlmacenadoService _IProductoAlmacenadoService;

        public ProductoAlmacenadoController(IProductoAlmacenadoService productoalmacenado)
        {
            IProductoAlmacenadoService = productoalmacenadoService;
        }

        [HttpPost]
        public ActionResult Add(ProductoAlmacenadoEntities productoalmacenado)
        {
            try
            {
                _IProductoAlmacenadoService.Add(productoalmacenado);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<ProductoAlmacenadoEntities>> GetAll()
        {
            try
            {
                var productoalmacenado = IProductoAlmacenadoService.GetAll();
                if (productoalmacenado == null)
                {
                    return NotFound(new { mensaje = "No hay Productos que mostrar" });
                }
                return Ok(productoalmacenado);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<ProductoAlmacenadoEntities> GetByID(int id)
        {
            try
            {
                var found = IProductoAlmacenadoService.GetById(id);
                if (found == null) return NotFound(new { mensaje = "El producto no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, ProductoAlmacenadoEntities productoalmacenado)
        {
            try
            {
                var find = IProductoAlmacenadoService.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se encontro el producto" });
                productoalmacenado.Almc_Id = id;
                _IProductoAlmacenadoService.Update(productoalmacenado);
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
                var lolo = IProductoAlmacenadoService.GetById(id);
                if (lolo == null) return NotFound();
                _IProductoAlmacenadoService.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}