using JetBrains.Annotations;

namespace Volo.Abp.Auditing;

public interface IAuditingManager
{
    [CanBeNull]
    IAuditLogScope Current { get; }

    IAuditLogSaveHandle BeginScope();
}
