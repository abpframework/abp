using System.Threading.Tasks;

namespace Volo.Abp.OpenIddict;

public interface IAbpOpenIddictClaimsPrincipalHandler
{
    Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context);
}
