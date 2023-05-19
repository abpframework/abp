using JetBrains.Annotations;

namespace Volo.Abp.Auditing;

public interface IAuditLogScope
{
    [NotNull]
    AuditLogInfo Log { get; }
}
