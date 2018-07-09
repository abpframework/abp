using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AuditLogging
{
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditingStore(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task SaveAsync(AuditLogInfo auditInfo)
        {
            await _auditLogRepository.InsertAsync(new AuditLog(auditInfo));
        }
    }
}
