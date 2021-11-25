using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement;

public class TenantFeatureManagementProvider : FeatureManagementProvider, ITransientDependency
{
    public override string Name => TenantFeatureValueProvider.ProviderName;

    protected ICurrentTenant CurrentTenant { get; }

    public TenantFeatureManagementProvider(
        IFeatureManagementStore store,
        ICurrentTenant currentTenant)
        : base(store)
    {
        CurrentTenant = currentTenant;
    }

    protected override Task<string> NormalizeProviderKeyAsync(string providerKey)
    {
        if (providerKey != null)
        {
            return Task.FromResult(providerKey);
        }

        return Task.FromResult(CurrentTenant.Id?.ToString());
    }
}
