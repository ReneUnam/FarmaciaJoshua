using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using Newtonsoft.Json;


[ApiController]
[Route("api/[controller]")]
public class MetricsController : ControllerBase
{
    private readonly IMetricService _metricService;

    public MetricsController(IMetricService metricService)
    {
        _metricService = metricService;
    }

    // ====== 1) GUARDAR LOG INDIVIDUAL ======
    [HttpPost("log")]
    public async Task<IActionResult> SaveLog([FromBody] LogEntry log)
    {
        if (log == null)
            return BadRequest("LogEntry no puede ser null.");

        await _metricService.SaveLog(log);
        return Ok(new { message = "Log guardado correctamente" });
    }

    // ====== 2) GUARDAR METRIC INDIVIDUAL ======
    [HttpPost("metric")]
    public async Task<IActionResult> SaveMetric([FromBody] Metric metric)
    {
        if (metric == null)
            return BadRequest("Metric no puede ser null.");

        await _metricService.SaveMetric(metric);
        return Ok(new { message = "Métrica guardada correctamente" });
    }

    // ====== 3) GUARDAR LOTE (LOGS + MÉTRICAS) ======
    [HttpPost("batch")]
    public async Task<IActionResult> SaveBatch([FromBody] List<object> items)
    {
        if (items == null || items.Count == 0)
            return BadRequest("La lista está vacía.");

        int logsGuardados = 0;
        int metricsGuardadas = 0;

        foreach (var item in items)
        {
            var json = item as Newtonsoft.Json.Linq.JObject;
            if (json == null) continue;

            var type = json["type"]?.ToString()?.Trim()?.ToLower();

            if (type == "log")
            {
                var log = json.ToObject<LogEntry>();
                await _metricService.SaveLog(log);
                logsGuardados++;
            }
            else if (type == "metric")
            {
                var metric = json.ToObject<Metric>();
                await _metricService.SaveMetric(metric);
                metricsGuardadas++;
            }
        }

        return Ok(new
        {
            logs = logsGuardados,
            metrics = metricsGuardadas,
            total = logsGuardados + metricsGuardadas
        });
    }
}
