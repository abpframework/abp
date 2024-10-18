using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy;

public interface ITenantResolveContext : IServiceProviderAccessor
{
    string? TenantIdOrName { get; set; }

    bool Handled { get; set; }
}
