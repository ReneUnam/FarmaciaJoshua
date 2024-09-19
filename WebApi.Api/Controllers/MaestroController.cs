using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interface;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroController : ControllerBase
    {
        private readonly IMaestroService _IMaestroService;

        public MaestroController(IMaestroService maestroService)
        {
            _IMaestroService = maestroService;
        }

        [HttpGet]
        public ActionResult<List<MaestroEntities>> GetAll()
        {
            return _IMaestroService.GetALL();
        }

        [HttpGet("{id}")]
        public ActionResult<MaestroEntities> GetByID(int id)
        {
            var test = _IMaestroService.GetByID(id);
            if (test == null) return NotFound();
            return Ok(test);
        }

        [HttpPost]
        public ActionResult Add(MaestroEntities maestro)
        {
            _IMaestroService.Add(maestro);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, MaestroEntities maestro)
        {
            var find = _IMaestroService.GetByID(id);
            if (find == null) return NotFound();
            maestro.Id = id;
            _IMaestroService.Update(maestro);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var find = _IMaestroService.GetByID(id);
            if (find == null) return NotFound();
            _IMaestroService.Delete(id);
            return Ok();
        }
    }
}
