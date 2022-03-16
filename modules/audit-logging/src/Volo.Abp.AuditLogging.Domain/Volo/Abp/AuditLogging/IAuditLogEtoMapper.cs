using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace Volo.Abp.AuditLogging;

public interface IAuditLogEtoMapper
{
    Task<AuditLogInfoEto> MapToAuditLogInfoEtoAsync(AuditLogInfo auditLogInfo);
    Task<AuditLogInfo> MapToAuditLogInfoAsync(AuditLogInfoEto auditLogInfoEto);
}