using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolveContext : IServiceProviderAccessor
    {
        [CanBeNull]
        string TenantIdOrName { get; set; }

        bool Handled { get; set; }
    }
}