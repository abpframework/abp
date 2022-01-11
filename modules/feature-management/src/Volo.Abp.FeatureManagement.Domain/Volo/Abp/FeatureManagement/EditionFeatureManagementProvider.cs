using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.FeatureManagement;

public class EditionFeatureManagementProvider : FeatureManagementProvider, ITransientDependency
{
    public override string Name => EditionFeatureValueProvider.ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor { get; }

    public EditionFeatureManagementProvider(
        IFeatureManagementStore store,
        ICurrentPrincipalAccessor principalAccessor)
        : base(store)
    {
        PrincipalAccessor = principalAccessor;
    }

    protected override Task<string> NormalizeProviderKeyAsync(string providerKey)
    {
        if (providerKey != null)
        {
            return Task.FromResult(providerKey);
        }

        return Task.FromResult(PrincipalAccessor.Principal?.FindEditionId()?.ToString("N"));
    }
}
