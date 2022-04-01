using Volo.Abp.Collections;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictClaimDestinationsOptions
{
    public ITypeList<IAbpOpenIddictClaimDestinationsProvider> ClaimDestinationsProvider { get; }

    public AbpOpenIddictClaimDestinationsOptions()
    {
        ClaimDestinationsProvider = new TypeList<IAbpOpenIddictClaimDestinationsProvider>();
    }
}
