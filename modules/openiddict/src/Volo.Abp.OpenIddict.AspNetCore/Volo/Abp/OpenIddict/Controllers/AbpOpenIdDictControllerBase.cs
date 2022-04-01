using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.OpenIddict.Controllers;

public abstract class AbpOpenIdDictControllerBase : AbpController
{
    protected SignInManager<IdentityUser> SignInManager => LazyServiceProvider.LazyGetRequiredService<SignInManager<IdentityUser>>();
    protected IdentityUserManager UserManager => LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();
    protected IOpenIddictApplicationManager ApplicationManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictApplicationManager>();
    protected IOpenIddictAuthorizationManager AuthorizationManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictAuthorizationManager>();
    protected IOpenIddictScopeManager ScopeManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictScopeManager>();
    protected IOpenIddictTokenManager TokenManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictTokenManager>();
    protected IOptions<AbpOpenIddictClaimDestinationsOptions> OpenIddictClaimDestinationsOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpOpenIddictClaimDestinationsOptions>>();

    protected AbpOpenIdDictControllerBase()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
    }

    protected virtual Task<OpenIddictRequest> GetOpenIddictServerRequest(HttpContext httpContext)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException(L["TheOpenIDConnectRequestCannotBeRetrieved"]);

        return Task.FromResult(request);
    }

    protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        await foreach (var resource in ScopeManager.ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }

    protected virtual async Task SetClaimsDestinationsAsync(ClaimsPrincipal principal)
    {
        using (var scope = LazyServiceProvider.LazyGetRequiredService<IServiceProvider>().CreateScope())
        {
            foreach (var providerType in OpenIddictClaimDestinationsOptions.Value.ClaimDestinationsProvider)
            {
                var provider = (IAbpOpenIddictClaimDestinationsProvider)scope.ServiceProvider.GetRequiredService(providerType);
                await provider.SetDestinationsAsync(new AbpOpenIddictClaimDestinationsProviderContext(scope.ServiceProvider, principal, principal.Claims.ToArray()));
            }
        }
    }
}
