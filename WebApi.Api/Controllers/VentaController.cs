using Microsoft.AspNetCore.Mvc;
using WebApi.Interface;
using WebApi.Model;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly IAuditService _audit;
        private readonly IMetricService _metrics;

        public VentaController(IVentaService ventaService, IAuditService audit, IMetricService metrics)
        {
            _ventaService = ventaService;
            _audit = audit;
            _metrics = metrics;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Venta>> GetAll()
        {
            var ventas = _ventaService.GetALL();
            if (ventas == null) return NotFound();
            return Ok(ventas);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var venta = _ventaService.GetByID(id);
            if (venta == null) return NotFound();
            return Ok(venta);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Venta venta)
        {
            if (venta == null) return BadRequest("Datos no encontrados");

            var sw = Stopwatch.StartNew();
            try
            {
                var created = _ventaService.Add(venta);
                sw.Stop();


                await RegistrarMetricas("create", "success", sw.ElapsedMilliseconds, created);
                await RegistrarAuditoria("create", "success", created,
                    request: created, response: new { created.IdVenta });

                return Ok(created);
            }
            catch (Exception ex)
            {
                sw.Stop();
                await RegistrarMetricas("create", "error", sw.ElapsedMilliseconds, venta);
                await RegistrarAuditoria("create", "failure", venta, request: venta, message: ex.Message);
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Venta venta)
        {
            var anterior = _ventaService.GetByID(id);
            if (anterior == null) return NotFound();

            venta.IdVenta = id;
            var sw = Stopwatch.StartNew();
            try
            {
                _ventaService.Update(venta);
                sw.Stop();

                var diff = CrearDiff(anterior, venta);

                await RegistrarMetricas("update", "success", sw.ElapsedMilliseconds, venta);
                await RegistrarAuditoria("update", "success", venta, request: venta, diff: diff);

                return Ok(venta);
            }
            catch (Exception ex)
            {
                sw.Stop();
                await RegistrarMetricas("update", "error", sw.ElapsedMilliseconds, venta);
                await RegistrarAuditoria("update", "failure", venta, request: venta, message: ex.Message);
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var venta = _ventaService.GetByID(id);
            if (venta == null) return NotFound();

            var sw = Stopwatch.StartNew();
            try
            {
                _ventaService.Delete(id);
                sw.Stop();

                await RegistrarMetricas("delete", "success", sw.ElapsedMilliseconds, venta);
                await RegistrarAuditoria("delete", "success", venta, message: "Venta eliminada");

                return Ok(new { mensaje = "Borrado correctamente" });
            }
            catch (Exception ex)
            {
                sw.Stop();
                await RegistrarMetricas("delete", "error", sw.ElapsedMilliseconds, venta);
                await RegistrarAuditoria("delete", "failure", venta, message: ex.Message);
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        private async Task RegistrarMetricas(string action, string result, long ms, Venta? v)
        {
            try
            {
                await _metrics.SaveMetric(new Metric
                {
                    Name = "sale_process_time_ms",
                    Value = ms,
                    Tags = new Dictionary<string, string>
                    {
                        { "action", action },
                        { "result", result }
                    },
                    Timestamp = DateTime.Now
                });

                if (result == "success" && v != null)
                {
                    await _metrics.SaveMetric(new Metric
                    {
                        Name = "sale_total_amount",
                        Value = (double)v.Total,
                        Tags = new Dictionary<string, string> { { "action", action } },
                        Timestamp = DateTime.Now
                    });

                    var items = v.VentaDetalle?.Count ?? 0;
                    await _metrics.SaveMetric(new Metric
                    {
                        Name = "sale_items_count",
                        Value = items,
                        Tags = new Dictionary<string, string> { { "action", action } },
                        Timestamp = DateTime.Now
                    });
                }
            }
            catch { }
        }

        private async Task RegistrarAuditoria(
            string action,
            string result,
            Venta? venta,
            object? request = null,
            object? response = null,
            string? diff = null,
            string? message = null)
        {
            try
            {
                var performedBy = User.Identity?.Name; // viene de ClaimTypes.Name
                int? performedById = null;
                var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(idClaim, out var uid)) performedById = uid;

                var audit = new AuditEvent
                {
                    Entity = "venta",
                    Action = action,
                    Result = result,
                    EntityId = venta?.IdVenta.ToString(),
                    PerformedBy = performedBy,
                    PerformedById = performedById,       // <- desde el token
                    Total = venta?.Total,
                    Items = venta?.VentaDetalle?.Count,
                    Diff = diff,
                    Message = message,
                    RequestData = request != null ? JsonSerializer.Serialize(request) : null,
                    ResponseData = response != null ? JsonSerializer.Serialize(response) : null,
                    Timestamp = DateTime.Now
                };
                await _audit.SaveAsync(audit);
            }
            catch { }
        }
        private string? CrearDiff(Venta oldV, Venta newV)
        {
            try
            {
                return JsonSerializer.Serialize(new
                {
                    old = new { oldV.Total, Items = oldV.VentaDetalle?.Count },
                    @new = new { newV.Total, Items = newV.VentaDetalle?.Count }
                });
            }
            catch
            {
                return null;
            }
        }
    }
}