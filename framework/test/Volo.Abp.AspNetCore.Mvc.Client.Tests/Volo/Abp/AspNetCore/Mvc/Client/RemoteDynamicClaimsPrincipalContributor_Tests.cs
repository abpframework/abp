using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteDynamicClaimsPrincipalContributor_Tests : AbpAspNetCoreMvcClientTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    [Fact]
    public async Task Should_Clear_The_Principal_When_The_Session_Is_Revoked()
    {
        var userId = await CacheUserAsync();
        await RevokeSessionAsync("session-a");

        var principal = await ContributeAsync(userId, "session-a");

        principal.Identity!.IsAuthenticated.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Keep_The_Principal_Of_Another_Session()
    {
        var userId = await CacheUserAsync();
        await RevokeSessionAsync("session-a");

        var principal = await ContributeAsync(userId, "session-b");

        principal.Identity!.IsAuthenticated.ShouldBeTrue();
        principal.FindFirst(AbpClaimTypes.UserName)!.Value.ShouldBe("john");
    }

    [Fact]
    public async Task Should_Keep_The_Principal_Without_A_Session_Id()
    {
        var userId = await CacheUserAsync();

        var principal = await ContributeAsync(userId, sessionId: null);

        principal.Identity!.IsAuthenticated.ShouldBeTrue();
    }

    private async Task<Guid> CacheUserAsync()
    {
        var userId = Guid.NewGuid();
        await GetRequiredService<IDistributedCache<AbpDynamicClaimCacheItem>>().SetAsync(
            AbpDynamicClaimCacheItem.CalculateCacheKey(userId, null),
            new AbpDynamicClaimCacheItem(new() { new AbpDynamicClaim(AbpClaimTypes.UserName, "john") }));
        return userId;
    }

    private Task RevokeSessionAsync(string sessionId)
    {
        return GetRequiredService<IDistributedCache<AbpDynamicClaimCacheItem>>().SetAsync(
            AbpDynamicClaimCacheItem.CalculateRevokedSessionCacheKey(sessionId),
            new AbpDynamicClaimCacheItem());
    }

    private async Task<ClaimsPrincipal> ContributeAsync(Guid userId, string sessionId)
    {
        var identity = new ClaimsIdentity("Bearer", AbpClaimTypes.UserName, AbpClaimTypes.Role);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, userId.ToString()));
        identity.AddClaim(new Claim(AbpClaimTypes.UserName, "john"));
        if (sessionId != null)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.SessionId, sessionId));
        }

        var context = new AbpClaimsPrincipalContributorContext(new ClaimsPrincipal(identity), ServiceProvider);
        await GetRequiredService<RemoteDynamicClaimsPrincipalContributor>().ContributeAsync(context);
        return context.ClaimsPrincipal;
    }
}
