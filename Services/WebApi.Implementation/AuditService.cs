using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

public class AuditService : IAuditService
{
    private readonly IMongoCollection<AuditEvent> _audits;

    public AuditService(IConfiguration cfg)
    {
        var client = new MongoClient(cfg.GetConnectionString("MongoAtlas"));
        var db = client.GetDatabase("farmacia_monitoring");
        _audits = db.GetCollection<AuditEvent>("audits");
    }

    public async Task SaveAsync(AuditEvent audit)
    {
        audit.Id = null;
        if (audit.Timestamp == default) audit.Timestamp = DateTime.Now;
        await _audits.InsertOneAsync(audit);
    }
}