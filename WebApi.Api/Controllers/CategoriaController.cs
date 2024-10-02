using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Implementation;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _ICategoriaservice;

        public CategoriaController (ICategoriaService categoriaService)
        {
            _ICategoriaservice = categoriaService;
        }

        [HttpPost]
        public ActionResult Add(CategoriaEntities categoria)
        {
            try
            {
                _ICategoriaservice.Add(categoria);
                return Ok(new { mensaje = "Agregado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaEntities>> GetAll()
        {
            try
            {
                var categoria = _ICategoriaservice.GetAll();
                if (categoria == null)
                {
                    return NotFound(new { mensaje = "No hay categorias que mostrar" });
                }
                return Ok(categoria);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public ActionResult<CategoriaEntities> GetByID(int id)
        {
            try
            {
                var found = _ICategoriaservice.GetById(id);
                if (found == null) return NotFound(new { mensaje = "La categoria no existe" });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, CategoriaEntities categoria)
        {
            try
            {
                var find = _ICategoriaservice.GetById(id);
                if (find == null) return NotFound(new { mensaje = "No se encontro la categoria" });
                categoria.IdCategoria = id;
                _ICategoriaservice.Update(categoria);
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
                var lolo = _ICategoriaservice.GetById(id);
                if (lolo == null) return NotFound();
                _ICategoriaservice.Delete(id);
                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

    }
}
