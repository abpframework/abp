using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

public class AbpDynamicClaimsOpenIddictClaimsPrincipalHandler: IAbpOpenIddictClaimsPrincipalHandler, ITransientDependency
{
    public virtual async Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)
    {
        if (context.OpenIddictRequest.IsClientCredentialsGrantType())
        {
            return;
        }

        var abpClaimsPrincipalFactory = context
            .ScopeServiceProvider
            .GetRequiredService<IAbpClaimsPrincipalFactory>();

        await abpClaimsPrincipalFactory.CreateDynamicAsync(context.Principal);
    }
}
