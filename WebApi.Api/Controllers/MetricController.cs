using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class MetricsController : ControllerBase
{
    private readonly IMetricService _metricService;

    public MetricsController(IMetricService metricService)
    {
        _metricService = metricService;
    }

    [HttpPost("log")]
    public async Task<IActionResult> SaveLog([FromBody] JsonElement body)
    {
        if (body.ValueKind != JsonValueKind.Object) return BadRequest("Body inválido.");

        // Aceptar wrapper opcional {"log": {...}}
        var payload = body;
        if (body.TryGetProperty("log", out var logProp))
            payload = logProp;

        LogEntry? log;
        try { log = JsonSerializer.Deserialize<LogEntry>(payload.GetRawText()); }
        catch { return BadRequest("Estructura inválida."); }

        if (log == null) return BadRequest("Log inválido.");

        // Forzar valores seguros
        log.Id = null;
        if (log.Timestamp == default) log.Timestamp = DateTime.Now;

        await _metricService.SaveLog(log);
        return Ok(new { message = "Log guardado correctamente" });
    }

    [HttpPost("metric")]
    public async Task<IActionResult> SaveMetric([FromBody] Metric metric)
    {
        if (metric == null) return BadRequest("Metric no puede ser null.");
        metric.Id = null;
        if (metric.Timestamp == default) metric.Timestamp = DateTime.Now;
        metric.Tags ??= new Dictionary<string, string>();

        await _metricService.SaveMetric(metric);
        return Ok(new { message = "Métrica guardada correctamente" });
    }

    [HttpPost("batch")]
    public async Task<IActionResult> SaveBatch([FromBody] JsonElement body)
    {
        if (body.ValueKind != JsonValueKind.Array)
            return BadRequest("El body debe ser un array JSON.");

        int logsGuardados = 0, metricsGuardadas = 0, errores = 0;

        foreach (var item in body.EnumerateArray())
        {
            try
            {
                if (item.ValueKind != JsonValueKind.Object) { errores++; continue; }

                var type = item.TryGetProperty("type", out var t) ? t.GetString()?.Trim().ToLower() : null;

                if (type == "log")
                {
                    var log = JsonSerializer.Deserialize<LogEntry>(item.GetRawText());
                    if (log == null) { errores++; continue; }
                    log.Id = null;
                    if (log.Timestamp == default) log.Timestamp = DateTime.Now;
                    await _metricService.SaveLog(log);
                    logsGuardados++;
                }
                else if (type == "metric")
                {
                    var metric = JsonSerializer.Deserialize<Metric>(item.GetRawText());
                    if (metric == null) { errores++; continue; }
                    metric.Id = null;
                    if (metric.Timestamp == default) metric.Timestamp = DateTime.Now;
                    metric.Tags ??= new Dictionary<string, string>();
                    await _metricService.SaveMetric(metric);
                    metricsGuardadas++;
                }
                else
                {
                    errores++;
                }
            }
            catch
            {
                errores++;
            }
        }

        return Ok(new { logs = logsGuardados, metrics = metricsGuardadas, errores, total = logsGuardados + metricsGuardadas });
    }
}