using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DebugController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("check-connection")]
        public IActionResult CheckConnection()
        {
            var conn = _config.GetConnectionString("ConnectionSQL");
            return Ok(new
            {
                ConnectionString = string.IsNullOrEmpty(conn) ? "No encontrada" : conn
            });
        }
    }
}