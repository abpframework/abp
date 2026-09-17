using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictSecurityStampValidatorOptionsExtensions_Tests
{
    [Fact]
    public async Task Should_Remove_ClientId_From_Refreshed_Principal()
    {
        var context = new SecurityStampRefreshingPrincipalContext
        {
            NewPrincipal = CreateCookiePrincipal(
                new Claim(AbpClaimTypes.UserId, "user-1"),
                new Claim(AbpClaimTypes.ClientId, "MyClient"))
        };

        await RefreshPrincipalAsync(context);

        context.NewPrincipal.FindAll(AbpClaimTypes.ClientId).ShouldBeEmpty();
        context.NewPrincipal.FindFirst(AbpClaimTypes.UserId).Value.ShouldBe("user-1");
    }

    [Fact]
    public async Task Should_Not_Touch_A_Principal_Without_ClientId()
    {
        var context = new SecurityStampRefreshingPrincipalContext
        {
            NewPrincipal = CreateCookiePrincipal(new Claim(AbpClaimTypes.UserId, "user-1"))
        };

        await RefreshPrincipalAsync(context);

        context.NewPrincipal.Claims.Count().ShouldBe(1);
        context.NewPrincipal.FindFirst(AbpClaimTypes.UserId).Value.ShouldBe("user-1");
    }

    [Fact]
    public async Task Should_Remove_Every_ClientId_Claim_From_Every_Identity()
    {
        var principal = new ClaimsPrincipal();
        principal.AddIdentity(new ClaimsIdentity(
            new[] { new Claim(AbpClaimTypes.UserId, "user-1"), new Claim(AbpClaimTypes.ClientId, "Client-A") },
            IdentityConstants.ApplicationScheme));
        principal.AddIdentity(new ClaimsIdentity(
            new[] { new Claim(AbpClaimTypes.ClientId, "Client-B"), new Claim(AbpClaimTypes.ClientId, "Client-C") },
            IdentityConstants.ApplicationScheme));

        var context = new SecurityStampRefreshingPrincipalContext { NewPrincipal = principal };

        await RefreshPrincipalAsync(context);

        context.NewPrincipal.FindAll(AbpClaimTypes.ClientId).ShouldBeEmpty();
        context.NewPrincipal.FindFirst(AbpClaimTypes.UserId).Value.ShouldBe("user-1");
    }

    [Fact]
    public async Task Should_Not_Throw_When_New_Principal_Is_Null()
    {
        var context = new SecurityStampRefreshingPrincipalContext { NewPrincipal = null };

        await Should.NotThrowAsync(() => RefreshPrincipalAsync(context));
    }

    [Fact]
    public async Task Should_Remove_ClientId_After_Running_The_Previously_Registered_Callback()
    {
        // The real module order: ABP Identity's callback is registered first, the removal after it. Identity's
        // SecurityStampValidatorCallback.UpdatePrincipal copies client_id from an already-corrupted cookie onto
        // the refreshed principal; the removal still strips it, so the cookie self-heals on its next refresh.
        var previousCallbackRan = false;
        Task PreviousCallback(SecurityStampRefreshingPrincipalContext context)
        {
            previousCallbackRan = true;
            CopyClientIdForward(context);
            return Task.CompletedTask;
        }

        var refreshingContext = CreateCorruptedCookieRefreshContext();

        await RefreshPrincipalAsync(refreshingContext, PreviousCallback);

        previousCallbackRan.ShouldBeTrue();
        refreshingContext.NewPrincipal.FindAll(AbpClaimTypes.ClientId).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Remove_ClientId_When_A_Callback_Is_Registered_After_It()
    {
        // The reverse order: the removal is registered first and an Identity-style callback (which runs its own
        // copy-forward before invoking the previous callback) is registered after it. Because the two wrappers
        // chain in opposite directions, the removal still runs last, so the order the modules load does not matter.
        var options = new SecurityStampValidatorOptions();
        options.RemoveClientIdClaim();

        var previousOnRefreshingPrincipal = options.OnRefreshingPrincipal;
        options.OnRefreshingPrincipal = async context =>
        {
            CopyClientIdForward(context);
            if (previousOnRefreshingPrincipal != null)
            {
                await previousOnRefreshingPrincipal.Invoke(context);
            }
        };

        var refreshingContext = CreateCorruptedCookieRefreshContext();

        await options.OnRefreshingPrincipal(refreshingContext);

        refreshingContext.NewPrincipal.FindAll(AbpClaimTypes.ClientId).ShouldBeEmpty();
    }

    private static async Task RefreshPrincipalAsync(
        SecurityStampRefreshingPrincipalContext context,
        Func<SecurityStampRefreshingPrincipalContext, Task> previousCallback = null)
    {
        var options = new SecurityStampValidatorOptions { OnRefreshingPrincipal = previousCallback };

        options.RemoveClientIdClaim().ShouldBeSameAs(options);

        await options.OnRefreshingPrincipal(context);
    }

    private static void CopyClientIdForward(SecurityStampRefreshingPrincipalContext context)
    {
        var clientId = context.CurrentPrincipal.FindFirst(AbpClaimTypes.ClientId);
        if (clientId != null)
        {
            context.NewPrincipal.Identities.First().AddClaim(clientId);
        }
    }

    private static SecurityStampRefreshingPrincipalContext CreateCorruptedCookieRefreshContext()
    {
        return new SecurityStampRefreshingPrincipalContext
        {
            CurrentPrincipal = CreateCookiePrincipal(
                new Claim(AbpClaimTypes.UserId, "user-1"),
                new Claim(AbpClaimTypes.ClientId, "MyClient")),
            NewPrincipal = CreateCookiePrincipal(new Claim(AbpClaimTypes.UserId, "user-1"))
        };
    }

    private static ClaimsPrincipal CreateCookiePrincipal(params Claim[] claims)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme));
    }
}
