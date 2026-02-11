namespace Volo.Abp.Auditing;

public interface IAuditLogScope
{
    AuditLogInfo Log { get; }
}
