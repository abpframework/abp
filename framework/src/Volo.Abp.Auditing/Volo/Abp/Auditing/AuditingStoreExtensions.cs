using Volo.Abp.Threading;

namespace Volo.Abp.Auditing
{
    public static class AuditingStoreExtensions
    {
        public static void Save(this IAuditingStore auditingStore, AuditLogInfo auditInfo)
        {
            AsyncHelper.RunSync(() => auditingStore.SaveAsync(auditInfo));
        }
    }
}