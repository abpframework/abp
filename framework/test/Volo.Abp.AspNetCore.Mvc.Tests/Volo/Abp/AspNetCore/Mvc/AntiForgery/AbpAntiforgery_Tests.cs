using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public class AbpAntiforgery_Tests
{
    private const string BearerIssuer = "https://localhost:44361/";
    private const string UserId = "3a0e6f1c-1111-2222-3333-444455556666";

    [Fact]
    public Task GetAndStoreTokens_should_normalize_the_user_and_restore_it() =>
        Should_normalize_then_restore(
            (antiforgery, httpContext) => { antiforgery.GetAndStoreTokens(httpContext); return Task.CompletedTask; },
            inner => inner.UserSeenByGetAndStoreTokens);

    [Fact]
    public Task GetTokens_should_normalize_the_user_and_restore_it() =>
        Should_normalize_then_restore(
            (antiforgery, httpContext) => { antiforgery.GetTokens(httpContext); return Task.CompletedTask; },
            inner => inner.UserSeenByGetTokens);

    [Fact]
    public Task IsRequestValidAsync_should_normalize_the_user_and_restore_it() =>
        Should_normalize_then_restore(
            (antiforgery, httpContext) => antiforgery.IsRequestValidAsync(httpContext),
            inner => inner.UserSeenByIsRequestValid);

    [Fact]
    public Task ValidateRequestAsync_should_normalize_the_user_and_restore_it() =>
        Should_normalize_then_restore(
            (antiforgery, httpContext) => antiforgery.ValidateRequestAsync(httpContext),
            inner => inner.UserSeenByValidateRequest);

    [Fact]
    public Task SetCookieTokenAndHeader_should_normalize_the_user_and_restore_it() =>
        Should_normalize_then_restore(
            (antiforgery, httpContext) => { antiforgery.SetCookieTokenAndHeader(httpContext); return Task.CompletedTask; },
            inner => inner.UserSeenBySetCookieTokenAndHeader);

    [Fact]
    public void Should_delegate_the_result_to_the_inner_antiforgery()
    {
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var httpContext = CreateHttpContext(CreatePrincipal(BearerIssuer), withNormalizer: true);

        var tokenSet = antiforgery.GetAndStoreTokens(httpContext);

        tokenSet.RequestToken.ShouldBe(RecordingAntiforgery.RequestToken);
        tokenSet.CookieToken.ShouldBe(RecordingAntiforgery.CookieToken);
    }

    [Fact]
    public void Should_not_normalize_when_disabled()
    {
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: false));
        var original = CreatePrincipal(BearerIssuer);
        var httpContext = CreateHttpContext(original, withNormalizer: true);

        antiforgery.GetAndStoreTokens(httpContext);

        // the inner saw the original (un-normalized) principal
        inner.UserSeenByGetAndStoreTokens.ShouldBeSameAs(original);
        inner.UserSeenByGetAndStoreTokens!.FindFirst(AbpClaimTypes.UserId)!.Issuer.ShouldBe(BearerIssuer);
        httpContext.User.ShouldBeSameAs(original);
    }

    [Fact]
    public void Should_not_resolve_the_normalizer_service_when_disabled()
    {
        // the normalizer is intentionally not registered; the disabled fast-path must not touch RequestServices
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: false));
        var httpContext = CreateHttpContext(CreatePrincipal(BearerIssuer), withNormalizer: false);

        Should.NotThrow(() => antiforgery.GetAndStoreTokens(httpContext));
    }

    private static async Task Should_normalize_then_restore(
        Func<IAntiforgery, HttpContext, Task> invoke,
        Func<RecordingAntiforgery, ClaimsPrincipal?> userSeenByInner)
    {
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var original = CreatePrincipal(BearerIssuer);
        var httpContext = CreateHttpContext(original, withNormalizer: true);

        await invoke(antiforgery, httpContext);

        // the inner ran against the normalized principal
        userSeenByInner(inner)!.FindFirst(AbpClaimTypes.UserId)!.Issuer
            .ShouldBe(AbpAntiForgeryClaimsPrincipalNormalizer.UserIdClaimIssuer);
        // the original principal is restored after the call
        httpContext.User.ShouldBeSameAs(original);
    }

    private static Microsoft.Extensions.Options.IOptions<AbpAntiForgeryOptions> CreateOptions(bool normalize)
    {
        return Microsoft.Extensions.Options.Options.Create(
            new AbpAntiForgeryOptions { NormalizeUserIdClaimIssuer = normalize });
    }

    private static HttpContext CreateHttpContext(ClaimsPrincipal user, bool withNormalizer)
    {
        var services = new ServiceCollection();
        if (withNormalizer)
        {
            services.AddTransient<IAbpAntiForgeryClaimsPrincipalNormalizer, AbpAntiForgeryClaimsPrincipalNormalizer>();
        }

        return new DefaultHttpContext
        {
            User = user,
            RequestServices = services.BuildServiceProvider()
        };
    }

    private static ClaimsPrincipal CreatePrincipal(string userIdClaimIssuer)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
            new[]
            {
                new Claim(AbpClaimTypes.UserId, UserId, ClaimValueTypes.String, userIdClaimIssuer)
            },
            "AuthenticationTypes.Federation"));
    }

    private sealed class RecordingAntiforgery : IAntiforgery
    {
        public const string RequestToken = "test-request-token";
        public const string CookieToken = "test-cookie-token";

        public ClaimsPrincipal? UserSeenByGetAndStoreTokens { get; private set; }
        public ClaimsPrincipal? UserSeenByGetTokens { get; private set; }
        public ClaimsPrincipal? UserSeenByIsRequestValid { get; private set; }
        public ClaimsPrincipal? UserSeenByValidateRequest { get; private set; }
        public ClaimsPrincipal? UserSeenBySetCookieTokenAndHeader { get; private set; }

        public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext)
        {
            UserSeenByGetAndStoreTokens = httpContext.User;
            return CreateTokenSet();
        }

        public AntiforgeryTokenSet GetTokens(HttpContext httpContext)
        {
            UserSeenByGetTokens = httpContext.User;
            return CreateTokenSet();
        }

        public Task<bool> IsRequestValidAsync(HttpContext httpContext)
        {
            UserSeenByIsRequestValid = httpContext.User;
            return Task.FromResult(true);
        }

        public Task ValidateRequestAsync(HttpContext httpContext)
        {
            UserSeenByValidateRequest = httpContext.User;
            return Task.CompletedTask;
        }

        public void SetCookieTokenAndHeader(HttpContext httpContext)
        {
            UserSeenBySetCookieTokenAndHeader = httpContext.User;
        }

        private static AntiforgeryTokenSet CreateTokenSet()
        {
            return new AntiforgeryTokenSet(RequestToken, CookieToken, "RequestVerificationToken", "RequestVerificationToken");
        }
    }
}
