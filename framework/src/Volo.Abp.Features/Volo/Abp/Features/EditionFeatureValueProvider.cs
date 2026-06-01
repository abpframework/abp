using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Features;

public class EditionFeatureValueProvider : FeatureValueProvider
{
    public const string ProviderName = "E";

    public override string Name => ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor;

    protected ITenantStore TenantStore { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public EditionFeatureValueProvider(
        IFeatureStore featureStore,
        ICurrentPrincipalAccessor principalAccessor,
        ITenantStore tenantStore,
        ICurrentTenant currentTenant)
        : base(featureStore)
    {
        PrincipalAccessor = principalAccessor;
        TenantStore = tenantStore;
        CurrentTenant = currentTenant;
    }

    public async override Task<string?> GetOrNullAsync(FeatureDefinition feature)
    {
        var editionId = await FindEditionIdAsync();
        if (editionId == null)
        {
            return null;
        }

        return await FeatureStore.GetOrNullAsync(feature.Name, Name, editionId.Value.ToString());
    }

    protected virtual async Task<Guid?> FindEditionIdAsync()
    {
        var editionId = PrincipalAccessor.Principal?.FindEditionId();
        if (editionId != null)
        {
            return editionId;
        }

        if (CurrentTenant.Id == null)
        {
            return null;
        }

        var tenant = await TenantStore.FindAsync(CurrentTenant.Id.Value);
        return tenant?.EditionId;
    }
}
