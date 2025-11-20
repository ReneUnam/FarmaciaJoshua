using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using WebApi.Model;

public class MetricService : IMetricService
{
    private readonly IMongoCollection<LogEntry> _logs;
    private readonly IMongoCollection<Metric> _metrics;

    public MetricService(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoAtlas"));
        var db = client.GetDatabase("farmacia_monitoring");

        _logs = db.GetCollection<LogEntry>("logs");
        _metrics = db.GetCollection<Metric>("metrics");
    }

    public async Task SaveLog(LogEntry log)
    {
        // Si Id viene inválido o no viene, generar uno nuevo
        if (string.IsNullOrWhiteSpace(log.Id) || !MongoDB.Bson.ObjectId.TryParse(log.Id, out _))
            log.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

        if (log.Timestamp == default) log.Timestamp = DateTime.Now;
        await _logs.InsertOneAsync(log);
    }

    public async Task SaveMetric(Metric metric)
    {
        metric.Id = null;
        if (metric.Timestamp == default) metric.Timestamp = DateTime.Now;
        metric.Tags ??= new Dictionary<string, string>();
        await _metrics.InsertOneAsync(metric);
    }

    public async Task SaveLoginLog(string username, string result, long durationMs, int? roleId, string message, object requestObj, object responseObj, Exception ex = null)
    {
        var log = new LogEntry
        {
            Level = ex == null && result == "success" ? "info" : (result == "failure" ? "warning" : "error"),
            Category = "auth",
            Event = "login",
            Result = result,
            Username = username,
            RoleId = roleId,
            Message = message,
            RequestData = System.Text.Json.JsonSerializer.Serialize(requestObj),
            ResponseData = System.Text.Json.JsonSerializer.Serialize(responseObj),
            DurationMs = durationMs,
            ErrorMessage = ex?.Message,
            StackTrace = ex?.StackTrace
        };
        await _logs.InsertOneAsync(log);
    }
}
