using System.Threading.Tasks;

namespace Volo.Abp.OpenIddict;

public interface IAbpOpenIddictClaimDestinationsProvider
{
    Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context);
}
