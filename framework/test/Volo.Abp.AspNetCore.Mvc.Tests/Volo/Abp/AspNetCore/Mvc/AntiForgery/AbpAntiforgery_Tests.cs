using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public class AbpAntiforgery_Tests
{
    private const string BearerIssuer = "https://localhost:44361/";
    private const string UserId = "3a0e6f1c-1111-2222-3333-444455556666";
    private static readonly DateTimeOffset ExpiresUtc = new(2030, 1, 2, 3, 4, 5, TimeSpan.Zero);

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
    public Task GetAndStoreTokens_should_preserve_the_authenticate_result() =>
        Should_preserve_the_authenticate_result(
            (antiforgery, httpContext) => { antiforgery.GetAndStoreTokens(httpContext); return Task.CompletedTask; });

    [Fact]
    public Task GetTokens_should_preserve_the_authenticate_result() =>
        Should_preserve_the_authenticate_result(
            (antiforgery, httpContext) => { antiforgery.GetTokens(httpContext); return Task.CompletedTask; });

    [Fact]
    public Task IsRequestValidAsync_should_preserve_the_authenticate_result() =>
        Should_preserve_the_authenticate_result(
            (antiforgery, httpContext) => antiforgery.IsRequestValidAsync(httpContext));

    [Fact]
    public Task ValidateRequestAsync_should_preserve_the_authenticate_result() =>
        Should_preserve_the_authenticate_result(
            (antiforgery, httpContext) => antiforgery.ValidateRequestAsync(httpContext));

    [Fact]
    public Task SetCookieTokenAndHeader_should_preserve_the_authenticate_result() =>
        Should_preserve_the_authenticate_result(
            (antiforgery, httpContext) => { antiforgery.SetCookieTokenAndHeader(httpContext); return Task.CompletedTask; });

    [Fact]
    public async Task Should_preserve_the_authenticate_result_when_the_inner_antiforgery_throws()
    {
        var inner = new RecordingAntiforgery { ThrowOnValidateRequest = true };
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var original = CreatePrincipal(BearerIssuer);
        var httpContext = CreateHttpContext(original, withNormalizer: true, CreateSuccessResult(original));

        await Should.ThrowAsync<AntiforgeryValidationException>(
            () => antiforgery.ValidateRequestAsync(httpContext));

        AssertResultPreserved(httpContext, original);
    }

    [Fact]
    public void Should_restore_both_when_the_feature_does_not_sync_the_user()
    {
        // IAuthenticateResultFeature does not require an implementation to keep the principal and the result
        // in sync; ASP.NET Core itself reuses such a feature. Restoring only the result would leave the
        // normalized principal on HttpContext.User.
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var original = CreatePrincipal(BearerIssuer);
        var originalResult = CreateSuccessResult(original);
        var httpContext = CreateHttpContext(original, withNormalizer: true);
        httpContext.Features.Set<IAuthenticateResultFeature>(
            new DecoupledAuthenticateResultFeature { AuthenticateResult = originalResult });

        antiforgery.GetAndStoreTokens(httpContext);

        httpContext.User.ShouldBeSameAs(original);
        httpContext.Features.Get<IAuthenticateResultFeature>()!.AuthenticateResult.ShouldBeSameAs(originalResult);
    }

    [Fact]
    public void Should_restore_the_original_authenticate_result_instance()
    {
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var original = CreatePrincipal(BearerIssuer);
        var originalResult = CreateSuccessResult(original);
        var httpContext = CreateHttpContext(original, withNormalizer: true, originalResult);

        antiforgery.GetAndStoreTokens(httpContext);

        httpContext.Features.Get<IAuthenticateResultFeature>()!.AuthenticateResult.ShouldBeSameAs(originalResult);
    }

    [Fact]
    public void Should_restore_the_user_when_there_is_no_authenticate_result_feature()
    {
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var original = CreatePrincipal(BearerIssuer);
        var httpContext = CreateHttpContext(original, withNormalizer: true);

        antiforgery.GetAndStoreTokens(httpContext);

        httpContext.Features.Get<IAuthenticateResultFeature>().ShouldBeNull();
        httpContext.User.ShouldBeSameAs(original);
    }

    [Fact]
    public void Should_restore_the_user_when_the_authenticate_result_is_null()
    {
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var original = CreatePrincipal(BearerIssuer);
        var httpContext = CreateHttpContext(original, withNormalizer: true, CreateSuccessResult(original));
        httpContext.Features.Get<IAuthenticateResultFeature>()!.AuthenticateResult = null;
        httpContext.User = original;

        antiforgery.GetAndStoreTokens(httpContext);

        httpContext.User.ShouldBeSameAs(original);
        httpContext.Features.Get<IAuthenticateResultFeature>()!.AuthenticateResult.ShouldBeNull();
    }

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

    private static async Task Should_preserve_the_authenticate_result(Func<IAntiforgery, HttpContext, Task> invoke)
    {
        var inner = new RecordingAntiforgery();
        var antiforgery = new AbpAntiforgery(inner, CreateOptions(normalize: true));
        var original = CreatePrincipal(BearerIssuer);
        var httpContext = CreateHttpContext(original, withNormalizer: true, CreateSuccessResult(original));

        await invoke(antiforgery, httpContext);

        AssertResultPreserved(httpContext, original);
    }

    private static void AssertResultPreserved(HttpContext httpContext, ClaimsPrincipal original)
    {
        var result = httpContext.Features.Get<IAuthenticateResultFeature>()!.AuthenticateResult;
        result.ShouldNotBeNull();
        result.Properties!.IsPersistent.ShouldBeTrue();
        result.Properties.ExpiresUtc.ShouldBe(ExpiresUtc);
        httpContext.User.ShouldBeSameAs(original);
    }

    private static AuthenticateResult CreateSuccessResult(ClaimsPrincipal principal)
    {
        var properties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = ExpiresUtc
        };

        return AuthenticateResult.Success(new AuthenticationTicket(principal, properties, "TestScheme"));
    }

    private static Microsoft.Extensions.Options.IOptions<AbpAntiForgeryOptions> CreateOptions(bool normalize)
    {
        return Microsoft.Extensions.Options.Options.Create(
            new AbpAntiForgeryOptions { NormalizeUserIdClaimIssuer = normalize });
    }

    private static HttpContext CreateHttpContext(
        ClaimsPrincipal user,
        bool withNormalizer,
        AuthenticateResult? authenticateResult = null)
    {
        var services = new ServiceCollection();
        if (withNormalizer)
        {
            services.AddTransient<IAbpAntiForgeryClaimsPrincipalNormalizer, AbpAntiForgeryClaimsPrincipalNormalizer>();
        }

        var httpContext = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider()
        };

        if (authenticateResult != null)
        {
            var features = new TestAuthenticationFeatures(authenticateResult);
            httpContext.Features.Set<IHttpAuthenticationFeature>(features);
            httpContext.Features.Set<IAuthenticateResultFeature>(features);
        }
        else
        {
            httpContext.User = user;
        }

        return httpContext;
    }

    // An IAuthenticateResultFeature that does not touch HttpContext.User, like the one ASP.NET Core
    // reuses in AuthorizationMiddlewareTests.
    private sealed class DecoupledAuthenticateResultFeature : IAuthenticateResultFeature
    {
        public AuthenticateResult? AuthenticateResult { get; set; }
    }

    // Mirrors the framework's internal AuthenticationFeatures: assigning User drops the AuthenticateResult,
    // which is the behaviour AbpAntiforgery has to compensate for when it restores the principal.
    private sealed class TestAuthenticationFeatures : IAuthenticateResultFeature, IHttpAuthenticationFeature
    {
        private ClaimsPrincipal? _user;
        private AuthenticateResult? _result;

        public TestAuthenticationFeatures(AuthenticateResult result)
        {
            AuthenticateResult = result;
        }

        public AuthenticateResult? AuthenticateResult
        {
            get => _result;
            set
            {
                _result = value;
                _user = _result?.Principal;
            }
        }

        public ClaimsPrincipal? User
        {
            get => _user;
            set
            {
                _user = value;
                _result = null;
            }
        }
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

        public bool ThrowOnValidateRequest { get; set; }

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
            if (ThrowOnValidateRequest)
            {
                throw new AntiforgeryValidationException("test");
            }

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
