using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

public class MauiBlazorCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ITransientDependency
{
    private AbpComponentsClaimsCache ClaimsCache { get; }

    public MauiBlazorCurrentPrincipalAccessor(
        IClientScopeServiceProviderAccessor clientScopeServiceProviderAccessor)
    {
        ClaimsCache = clientScopeServiceProviderAccessor.ServiceProvider.GetRequiredService<AbpComponentsClaimsCache>();
    }

    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return ClaimsCache.Principal;
    }
}