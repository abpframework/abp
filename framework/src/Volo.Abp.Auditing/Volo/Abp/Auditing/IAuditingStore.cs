using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    public interface IAuditingStore
    {
        void Save(AuditLogInfo auditInfo);

        Task SaveAsync(AuditLogInfo auditInfo);
    }
}