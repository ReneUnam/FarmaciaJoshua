using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;
        public EstudianteController(IEstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpGet]
        public ActionResult<List<EstudianteEntities>> GetAll()
        {
            return _estudianteService.GetALL();
        }

        [HttpGet("{id}")]
        public ActionResult<EstudianteEntities> GetByID(int id)
        {
            var test = _estudianteService.GetByID(id);
            if (test == null) return NotFound();
            return Ok(test);
        }

        [HttpPost]
        public ActionResult Add(EstudianteEntities producto)
        {
            _estudianteService.Add(producto);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, EstudianteEntities estudiante)
        {
            var find = _estudianteService.GetByID(id);
            if (find == null) return NotFound();
            estudiante.Estudiante_Id = id;
            _estudianteService.Update(estudiante);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var find = _estudianteService.GetByID(id);
            if (find == null) return NotFound();
            _estudianteService.Delete(id);
            return Ok();
        }
    }
}
