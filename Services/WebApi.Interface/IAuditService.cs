
public interface IAuditService
{
    Task SaveAsync(AuditEvent audit);
}