using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Metric
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }         // "api_response_time", "ventas_creadas", etc.
    public double Value { get; set; }        // números
    public Dictionary<string, string> Tags { get; set; } // user, endpoint, tipo
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
