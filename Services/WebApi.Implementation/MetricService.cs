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

    public async Task SaveLog(LogEntry log) =>
        await _logs.InsertOneAsync(log);

    public async Task SaveMetric(Metric metric) =>
        await _metrics.InsertOneAsync(metric);
}
