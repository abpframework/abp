using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    public interface IAuditingStore
    {
        /// <summary>
        /// Should save audits to a persistent store.
        /// </summary>
        /// <param name="auditInfo">Audit informations</param>
        Task SaveAsync(AuditInfo auditInfo);
    }
}