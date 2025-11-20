using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AuditEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Entity { get; set; }          // "venta"
    public string Action { get; set; }          // create | update | delete
    public string Result { get; set; }          // success | failure
    public string? EntityId { get; set; }       // idVenta (string para flexibilidad)
    public string? PerformedBy { get; set; }    // nombre usuario
    public int? PerformedById { get; set; }     // id usuario
    public decimal? Total { get; set; }          // total de la venta
    public int? Items { get; set; }             // # líneas
    public string? Diff { get; set; }           // JSON de cambios (solo update)
    public string? Message { get; set; }        // breve
    public string? RequestData { get; set; }    // JSON request (entrada)
    public string? ResponseData { get; set; }   // JSON respuesta (salida)
    public DateTime Timestamp { get; set; } = DateTime.Now;
}