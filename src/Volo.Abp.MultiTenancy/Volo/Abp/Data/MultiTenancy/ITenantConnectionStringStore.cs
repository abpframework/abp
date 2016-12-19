using JetBrains.Annotations;

namespace Volo.Abp.Data.MultiTenancy
{
    public interface ITenantConnectionStringStore
    {
        [CanBeNull]
        string GetConnectionStringOrNull([NotNull] string tenantId, [CanBeNull] string databaseName);
    }
}