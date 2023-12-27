namespace Volo.Abp.Auditing;

public interface IAuditingManager
{
    IAuditLogScope? Current { get; }

    IAuditLogSaveHandle BeginScope();
}
