using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class AbpRemoteDynamicClaimsCookieAuthenticationOptionsExtensions_Tests : AbpAspNetCoreMvcClientTestBase
{
    private readonly RemoteRefreshResponseHandler _remoteRefreshResponseHandler = new();

    protected override void AfterAddApplication(IServiceCollection services)
    {
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient(Arg.Any<string>()).Returns(_ => new HttpClient(_remoteRefreshResponseHandler)
        {
            BaseAddress = new Uri("https://localhost/")
        });

        services.Replace(ServiceDescriptor.Singleton(httpClientFactory));
        services.Replace(ServiceDescriptor.Transient(_ => Substitute.For<IRemoteServiceHttpClientAuthenticator>()));
        services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    [Fact]
    public async Task Should_Reject_The_Principal_When_The_Remote_Refresh_Is_Rejected()
    {
        var context = await ValidatePrincipalAsync(Guid.NewGuid());

        context.Principal.ShouldBeNull();
        context.ShouldRenew.ShouldBeFalse();
        _remoteRefreshResponseHandler.ReceivedAccessTokens.ShouldBe(new[] { "test-access-token" });
    }

    [Fact]
    public async Task Should_Keep_The_Principal_When_The_Dynamic_Claims_Are_Cached()
    {
        var userId = Guid.NewGuid();
        await GetRequiredService<IDistributedCache<AbpDynamicClaimCacheItem>>().SetAsync(
            AbpDynamicClaimCacheItem.CalculateCacheKey(userId, null),
            new AbpDynamicClaimCacheItem());

        var context = await ValidatePrincipalAsync(userId);

        context.Principal.ShouldNotBeNull();
        _remoteRefreshResponseHandler.ReceivedAccessTokens.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Keep_The_Principal_When_There_Is_No_Access_Token()
    {
        var context = await ValidatePrincipalAsync(Guid.NewGuid(), accessToken: null);

        context.Principal.ShouldNotBeNull();
        _remoteRefreshResponseHandler.ReceivedAccessTokens.ShouldBeEmpty();
    }

    private async Task<CookieValidatePrincipalContext> ValidatePrincipalAsync(Guid userId, string accessToken = "test-access-token")
    {
        var options = new CookieAuthenticationOptions().ValidateRemoteDynamicClaims();

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, AbpClaimTypes.UserName, AbpClaimTypes.Role);
        identity.AddClaim(new Claim(AbpClaimTypes.UserId, userId.ToString()));
        identity.AddClaim(new Claim(AbpClaimTypes.UserName, "john"));

        var properties = new AuthenticationProperties();
        if (accessToken != null)
        {
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = accessToken } });
        }

        var context = new CookieValidatePrincipalContext(
            new DefaultHttpContext { RequestServices = ServiceProvider },
            new AuthenticationScheme(CookieAuthenticationDefaults.AuthenticationScheme, null, typeof(CookieAuthenticationHandler)),
            options,
            new AuthenticationTicket(new ClaimsPrincipal(identity), properties, CookieAuthenticationDefaults.AuthenticationScheme));

        await options.Events.ValidatePrincipal(context);
        return context;
    }

    private class RemoteRefreshResponseHandler : HttpMessageHandler
    {
        public List<string> ReceivedAccessTokens { get; } = new();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ReceivedAccessTokens.Add(request.Headers.Authorization?.Parameter);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }
    }
}
