using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolveContext : IServiceProviderAccessor
    {
        [CanBeNull]
        TenantInfo Tenant { get; set; }

        bool? Handled { get; set; }
    }
}