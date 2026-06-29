using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.OpenIddict;

/// <summary>
/// Tests for the fix that stops the OAuth <c>client_id</c> of a <c>/connect/authorize</c> request
/// from leaking into the interactive authentication cookie.
///
/// Background: when the cookie's security stamp happens to be refreshed during a
/// <c>/connect/authorize</c> request, <c>OpenIddictClaimsPrincipalContributor</c> stamps the ambient
/// request's <c>client_id</c> onto the principal that is written back to the cookie. From then on
/// <c>ICurrentClient.Id</c> resolves to that client for every later cookie-authenticated request
/// (corrupting audit-log client attribution). The fix strips <c>client_id</c> from the principal at
/// the security-stamp <c>OnRefreshingPrincipal</c> callback, which only runs when the cookie is
/// re-issued and never for OpenIddict token issuance.
/// </summary>
public class OpenIddictCookieClientIdLeak_Tests
{
    [Fact]
    public void Should_Remove_ClientId_From_Refreshed_Cookie_Principal()
    {
        // A cookie principal that was wrongly stamped with client_id while being rebuilt
        // by the security-stamp validator during /connect/authorize.
        var context = new SecurityStampRefreshingPrincipalContext
        {
            CurrentPrincipal = CreateCookiePrincipal(new Claim(AbpClaimTypes.UserId, "user-1")),
            NewPrincipal = CreateCookiePrincipal(
                new Claim(AbpClaimTypes.UserId, "user-1"),
                new Claim(AbpClaimTypes.ClientId, "MyClient"))
        };

        AbpOpenIddictAspNetCoreModule.RemoveClientIdClaimsFromRefreshedPrincipal(context);

        context.NewPrincipal.FindAll(AbpClaimTypes.ClientId).ShouldBeEmpty();
        // unrelated claims are preserved
        context.NewPrincipal.FindFirst(AbpClaimTypes.UserId)!.Value.ShouldBe("user-1");
    }

    [Fact]
    public void Should_Not_Touch_A_Principal_That_Has_No_ClientId()
    {
        var context = new SecurityStampRefreshingPrincipalContext
        {
            NewPrincipal = CreateCookiePrincipal(new Claim(AbpClaimTypes.UserId, "user-1"))
        };

        AbpOpenIddictAspNetCoreModule.RemoveClientIdClaimsFromRefreshedPrincipal(context);

        context.NewPrincipal.Claims.Count().ShouldBe(1);
        context.NewPrincipal.FindFirst(AbpClaimTypes.UserId)!.Value.ShouldBe("user-1");
    }

    [Fact]
    public void Should_Not_Throw_When_New_Principal_Is_Null()
    {
        var context = new SecurityStampRefreshingPrincipalContext { NewPrincipal = null };

        Should.NotThrow(() => AbpOpenIddictAspNetCoreModule.RemoveClientIdClaimsFromRefreshedPrincipal(context));
    }

    [Fact]
    public async Task Registered_Callback_Should_Strip_ClientId_After_Running_The_Previous_Callback()
    {
        // Reproduces the real composition order. A previously registered callback - e.g. ABP
        // Identity's SecurityStampValidatorCallback.UpdatePrincipal - re-introduces client_id from
        // an already-corrupted cookie onto the refreshed principal. The fix is chained AFTER it, so
        // the claim is still removed and the cookie self-heals on its next refresh.
        var services = new ServiceCollection();
        services.AddOptions();

        var previousCallbackRan = false;
        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.OnRefreshingPrincipal = context =>
            {
                previousCallbackRan = true;
                var currentClientId = context.CurrentPrincipal!.FindFirst(AbpClaimTypes.ClientId);
                if (currentClientId != null)
                {
                    context.NewPrincipal!.Identities.First().AddClaim(currentClientId);
                }

                return Task.CompletedTask;
            };
        });

        AbpOpenIddictAspNetCoreModule.ConfigureSecurityStampValidator(services);

        var options = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<SecurityStampValidatorOptions>>()
            .Value;

        var context = new SecurityStampRefreshingPrincipalContext
        {
            CurrentPrincipal = CreateCookiePrincipal(
                new Claim(AbpClaimTypes.UserId, "user-1"),
                new Claim(AbpClaimTypes.ClientId, "MyClient")),
            NewPrincipal = CreateCookiePrincipal(new Claim(AbpClaimTypes.UserId, "user-1"))
        };

        await options.OnRefreshingPrincipal!(context);

        previousCallbackRan.ShouldBeTrue();
        context.NewPrincipal.FindAll(AbpClaimTypes.ClientId).ShouldBeEmpty();
    }

    private static ClaimsPrincipal CreateCookiePrincipal(params Claim[] claims)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme));
    }
}
