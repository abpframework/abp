using Volo.Abp.Threading;

namespace Volo.Abp.Auditing
{
    public static class AuditingStoreExtensions
    {
        public static void Save(this IAuditingStore auditingStore, AuditInfo auditInfo)
        {
            AsyncHelper.RunSync(() => auditingStore.SaveAsync(auditInfo));
        }
    }
}