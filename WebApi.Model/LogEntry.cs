using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class LogEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Level { get; set; }        // info, warning, error
    public string Controller { get; set; }
    public string Action { get; set; }
    public string User { get; set; }
    public string IpAddress { get; set; }

    public string RequestData { get; set; }
    public string ResponseData { get; set; }

    public long DurationMs { get; set; }     // tiempo ejecución
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string ErrorMessage { get; set; }
    public string StackTrace { get; set; }
}
