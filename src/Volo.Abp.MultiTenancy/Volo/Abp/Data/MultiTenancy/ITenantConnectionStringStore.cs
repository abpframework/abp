using JetBrains.Annotations;

namespace Volo.Abp.Data.MultiTenancy
{
    public interface ITenantConnectionStringStore
    {
        [CanBeNull]
        string GetDefaultConnectionStringOrNull([NotNull] string tenantId);

        [CanBeNull]
        string GetConnectionStringOrNull([NotNull] string tenantId, [NotNull] string connStringName);
    }
}