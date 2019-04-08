using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolveResultAccessor
    {
        [CanBeNull]
        TenantResolveResult Result { get; set; }
    }
}
