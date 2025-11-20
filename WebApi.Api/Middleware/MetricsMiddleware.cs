using WebApi.Model;
using System.Diagnostics;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMetricService _mongo;

    public MetricsMiddleware(RequestDelegate next, IMetricService mongo)
    {
        _next = next;
        _mongo = mongo;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Evitar medir el propio pipeline de ingestión y swagger
        if (path.StartsWith("/api/metrics", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var sw = Stopwatch.StartNew();
        await _next(context);
        sw.Stop();

        try
        {
            var metric = new Metric
            {
                // Nombre de métrica consistente
                Name = "api_response_time_ms",
                Value = sw.ElapsedMilliseconds,
                Tags = new Dictionary<string, string>
                {
                    { "method", context.Request.Method },
                    { "path", path },                 // opcional: podrías normalizar a ruta plantilla
                    { "status", (context.Response?.StatusCode ?? 0).ToString() }
                },
                // Guarda en UTC o en local según prefieras
                Timestamp = DateTime.Now
            };

            await _mongo.SaveMetric(metric);
        }
        catch
        {
            // No romper la request por fallos de métricas
        }
    }
}