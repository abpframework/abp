using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public interface ITenantResolveResultAccessor
    {
        TenantResolveResult Result { get; set; }
    }
}
