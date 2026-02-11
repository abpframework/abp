using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.FeatureManagement;

public class EditionFeatureManagementProvider : FeatureManagementProvider, ITransientDependency
{
    public override string Name => EditionFeatureValueProvider.ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor { get; }
    protected ITenantStore TenantStore { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected string CurrentCompatibleProviderName { get; set; }

    public EditionFeatureManagementProvider(
        IFeatureManagementStore store,
        ICurrentPrincipalAccessor principalAccessor,
        ITenantStore tenantStore,
        ICurrentTenant currentTenant)
        : base(store)
    {
        PrincipalAccessor = principalAccessor;
        TenantStore = tenantStore;
        CurrentTenant = currentTenant;
    }

    public override bool Compatible(string providerName)
    {
        CurrentCompatibleProviderName = providerName;
        return providerName == TenantFeatureValueProvider.ProviderName || base.Compatible(providerName);
    }

    protected async override Task<string> NormalizeProviderKeyAsync(string providerKey)
    {
        return (await FindEditionIdAsync(providerKey))?.ToString();
    }

    protected virtual async Task<Guid?> FindEditionIdAsync(string providerKey)
    {
        if (Guid.TryParse(providerKey, out var parsedEditionOrTenantId))
        {
            if (CurrentCompatibleProviderName == TenantFeatureValueProvider.ProviderName)
            {
                var tenant = await TenantStore.FindAsync(parsedEditionOrTenantId);
                if (tenant != null)
                {
                    return tenant?.EditionId;
                }
            }

            return parsedEditionOrTenantId;
        }

        if (CurrentTenant.Id.HasValue)
        {
            var tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
            if (tenant != null)
            {
                return tenant?.EditionId;
            }
        }

        return PrincipalAccessor.Principal?.FindEditionId();
    }
}
