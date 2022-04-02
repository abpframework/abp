using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictTenantResolveContributor : HttpTenantResolveContributorBase
{
    public const string ContributorName = "AbpOpenIddict";

    public override string Name => ContributorName;

    protected async override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
    {
        if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            return null;
        }

        if (httpContext.GetOpenIddictServerRequest() != null)
        {
            context.Handled = true;
            var principal = (await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            return principal?.FindTenantId().ToString();
        }

        return null;
    }
}
