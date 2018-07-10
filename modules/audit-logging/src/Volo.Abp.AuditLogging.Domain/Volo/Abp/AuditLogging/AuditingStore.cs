using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.AuditLogging
{
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IGuidGenerator _guidGenerator;

        public AuditingStore(IAuditLogRepository auditLogRepository, IGuidGenerator guidGenerator)
        {
            _auditLogRepository = auditLogRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task SaveAsync(AuditLogInfo auditInfo)
        {
            await _auditLogRepository.InsertAsync(new AuditLog(_guidGenerator, auditInfo));
        }
    }
}
