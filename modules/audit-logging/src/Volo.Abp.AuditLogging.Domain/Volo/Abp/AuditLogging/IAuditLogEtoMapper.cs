using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace Volo.Abp.AuditLogging;

public interface IAuditLogEtoMapper
{
    Task<AuditLogInfoEto> CovertToAuditLogInfoEtoAsync(AuditLogInfo auditLogInfo);
    Task<AuditLogInfo> CovertToAuditLogInfoAsync(AuditLogInfoEto auditLogInfoEto);
}