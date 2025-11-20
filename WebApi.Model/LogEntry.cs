using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class LogEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // Nivel (info | warning | error)
    public string Level { get; set; }

    // Categoría general del evento (auth, venta, producto, sistema, etc.)
    public string Category { get; set; }

    // Evento específico (login, logout, create, update, etc.)
    public string Event { get; set; }

    // Resultado del evento (success | failure | error)
    public string Result { get; set; }

    // Usuario involucrado (username)
    public string Username { get; set; }

    // Rol (si aplica en login exitoso)
    public int? RoleId { get; set; }

    // Mensaje breve (ej: "Login exitoso", "Credenciales inválidas")
    public string Message { get; set; }

    // Datos de request (JSON serializado)
    public string RequestData { get; set; }

    // Datos de respuesta (JSON serializado)
    public string ResponseData { get; set; }

    // Duración del proceso en milisegundos
    public long DurationMs { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Si hubo error técnico
    public string ErrorMessage { get; set; }
    public string StackTrace { get; set; }

    public LogEntry()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }
}