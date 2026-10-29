using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteDynamicClaimsPrincipalContributorCacheFailure_Tests : AbpAspNetCoreMvcClientTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IDistributedCache, RevokedSessionFailingCache>());
        services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    [Fact]
    public async Task Should_Clear_The_Principal_When_The_Revoked_Sessions_Can_Not_Be_Read()
    {
        var userId = Guid.NewGuid();
        await GetRequiredService<IDistributedCache<AbpDynamicClaimCacheItem>>().SetAsync(
            AbpDynamicClaimCacheItem.CalculateCacheKey(userId, null),
            new AbpDynamicClaimCacheItem());

        var identity = new ClaimsIdentity("Bearer", AbpClaimTypes.UserName, AbpClaimTypes.Role);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, userId.ToString()));
        identity.AddClaim(new Claim(AbpClaimTypes.SessionId, "session-a"));
        var context = new AbpClaimsPrincipalContributorContext(new ClaimsPrincipal(identity), ServiceProvider);

        await GetRequiredService<RemoteDynamicClaimsPrincipalContributor>().ContributeAsync(context);

        context.ClaimsPrincipal.Identity!.IsAuthenticated.ShouldBeFalse();
    }

    private class RevokedSessionFailingCache : IDistributedCache
    {
        private readonly MemoryDistributedCache _cache = new(Microsoft.Extensions.Options.Options.Create(new MemoryDistributedCacheOptions()));

        public byte[] Get(string key)
        {
            ThrowForRevokedSession(key);
            return _cache.Get(key);
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            ThrowForRevokedSession(key);
            return _cache.GetAsync(key, token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => _cache.Set(key, value, options);

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) => _cache.SetAsync(key, value, options, token);

        public void Refresh(string key) => _cache.Refresh(key);

        public Task RefreshAsync(string key, CancellationToken token = default) => _cache.RefreshAsync(key, token);

        public void Remove(string key) => _cache.Remove(key);

        public Task RemoveAsync(string key, CancellationToken token = default) => _cache.RemoveAsync(key, token);

        private static void ThrowForRevokedSession(string key)
        {
            if (key.Contains("revoked-session:"))
            {
                throw new InvalidOperationException("The cache is not available.");
            }
        }
    }
}
