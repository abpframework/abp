using System.Collections.Immutable;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace OpenIddict.Demo.Server.ExtensionGrants;

public class MyTokenExtensionGrant : ITokenExtensionGrant
{
    public const string ExtensionGrantName = "MyTokenExtensionGrant";

    public string Name => ExtensionGrantName;
    public async Task<IActionResult>  HandleAsync(ExtensionGrantContext context)
    {
        var userToken = context.Request.GetParameter("token").ToString();

        if (string.IsNullOrEmpty(userToken))
        {
            return new ForbidResult(
                new[] {OpenIddictServerAspNetCoreDefaults.AuthenticationScheme},
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest
                }!));
        }

        var transaction = await context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
        transaction.EndpointType = OpenIddictServerEndpointType.Introspection;
        transaction.Request = new OpenIddictRequest
        {
            ClientId = context.Request.ClientId,
            ClientSecret = context.Request.ClientSecret,
            Token = userToken
        };

        var notification = new OpenIddictServerEvents.ProcessAuthenticationContext(transaction);
        var dispatcher = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerDispatcher>();
        await dispatcher.DispatchAsync(notification);

        if (notification.IsRejected)
        {
            return new ForbidResult(
                new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                }));
        }

        var principal = notification.GenericTokenPrincipal;
        if (principal == null)
        {
            return new ForbidResult(
                new []{ OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                }));
        }

        var userId = principal.FindUserId();
        var userManager = context.HttpContext.RequestServices.GetRequiredService<IdentityUserManager>();
        var user = await userManager.GetByIdAsync(userId.Value);
        var userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
        var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);
        claimsPrincipal.SetScopes(principal.GetScopes());
        claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
        await context.HttpContext.RequestServices.GetRequiredService<AbpOpenIddictClaimsPrincipalManager>().HandleAsync(context.Request, principal);
        return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
    }

    private async Task<IEnumerable<string>> GetResourcesAsync(ExtensionGrantContext context, ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        await foreach (var resource in context.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>().ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }
}
