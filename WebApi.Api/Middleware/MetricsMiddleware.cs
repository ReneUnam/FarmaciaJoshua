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
        var sw = Stopwatch.StartNew();

        await _next(context);

        sw.Stop();

        var metric = new Metric
        {
            Name = "api_response_time_ms",
            Value = sw.ElapsedMilliseconds,
            Tags = new Dictionary<string, string>
            {
                {"method", context.Request.Method},
                {"path", context.Request.Path},
                {"status", context.Response.StatusCode.ToString()}
            }
        };

        await _mongo.SaveMetric(metric);
    }
}
