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

public class AbpAntiForgeryClaimsPrincipalNormalizer_Tests
{
    private const string CookieIssuer = "LOCAL AUTHORITY";
    private const string BearerIssuer = "https://localhost:44361/";
    private const string UserId = "3a0e6f1c-1111-2222-3333-444455556666";
    private const string AntiForgeryHeaderName = "RequestVerificationToken";
    private const string AntiForgeryCookieName = "AF";

    [Fact]
    public void Normalize_should_set_a_constant_issuer_on_user_identifier_claims_only()
    {
        var usernameClaim = new Claim("preferred_username", "admin", ClaimValueTypes.String, CookieIssuer);
        usernameClaim.Properties["test-property"] = "test-value";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
            new[]
            {
                new Claim("sub", UserId, ClaimValueTypes.String, CookieIssuer),
                new Claim(ClaimTypes.NameIdentifier, UserId, ClaimValueTypes.String, CookieIssuer),
                usernameClaim
            },
            "Identity.Application"));

        var normalized = new AbpAntiForgeryClaimsPrincipalNormalizer().Normalize(principal);

        normalized.FindFirst("sub")!.Issuer.ShouldBe(AbpAntiForgeryClaimsPrincipalNormalizer.UserIdClaimIssuer);
        normalized.FindFirst(ClaimTypes.NameIdentifier)!.Issuer.ShouldBe(AbpAntiForgeryClaimsPrincipalNormalizer.UserIdClaimIssuer);

        // value and OriginalIssuer are kept; only Issuer changes
        normalized.FindFirst("sub")!.Value.ShouldBe(UserId);
        normalized.FindFirst("sub")!.OriginalIssuer.ShouldBe(CookieIssuer);

        // non-identifier claims and their properties are untouched
        var normalizedUsername = normalized.FindFirst("preferred_username")!;
        normalizedUsername.Issuer.ShouldBe(CookieIssuer);
        normalizedUsername.Properties["test-property"].ShouldBe("test-value");

        // the original principal is not mutated
        principal.FindFirst("sub")!.Issuer.ShouldBe(CookieIssuer);
    }

    [Fact]
    public void Normalize_should_preserve_identity_metadata()
    {
        var actor = new ClaimsIdentity(new[] { new Claim(AbpClaimTypes.UserId, "actor-id") }, "Actor");
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim("sub", UserId, ClaimValueTypes.String, BearerIssuer) },
            "Identity.Application")
        {
            Actor = actor,
            BootstrapContext = "raw-token",
            Label = "my-label"
        });

        var normalized = new AbpAntiForgeryClaimsPrincipalNormalizer().Normalize(principal);
        var normalizedIdentity = (ClaimsIdentity)normalized.Identity!;

        // identity metadata that the antiforgery claim uid does not use is still preserved on the copy
        normalizedIdentity.Actor.ShouldBeSameAs(actor);
        normalizedIdentity.BootstrapContext.ShouldBe("raw-token");
        normalizedIdentity.Label.ShouldBe("my-label");
        // and the user id claim issuer was still normalized
        normalized.FindFirst("sub")!.Issuer.ShouldBe(AbpAntiForgeryClaimsPrincipalNormalizer.UserIdClaimIssuer);
    }

    [Fact]
    public async Task Token_should_validate_for_the_same_cookie_principal_through_the_decorator()
    {
        // The common server-rendered case: a token generated and validated for the same cookie principal.
        // Guards that wrapping IAntiforgery does not break the basic flow every page POST relies on.
        var (antiforgery, serviceProvider) = CreateDecoratedAntiforgery(normalize: true);

        var cookiePrincipal = CreatePrincipal("Identity.Application", CookieIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, cookiePrincipal);

        var isValid = await ValidateAsync(antiforgery, serviceProvider, cookiePrincipal, cookieToken, requestToken);

        isValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Token_issued_under_one_scheme_should_validate_under_another_when_normalization_is_enabled()
    {
        var (antiforgery, serviceProvider) = CreateDecoratedAntiforgery(normalize: true);

        var cookiePrincipal = CreatePrincipal("Identity.Application", CookieIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, cookiePrincipal);

        var bearerPrincipal = CreatePrincipal("AuthenticationTypes.Federation", BearerIssuer);
        var isValid = await ValidateAsync(antiforgery, serviceProvider, bearerPrincipal, cookieToken, requestToken);

        isValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Token_issued_under_one_scheme_should_fail_under_another_when_normalization_is_disabled()
    {
        var (antiforgery, serviceProvider) = CreateDecoratedAntiforgery(normalize: false);

        var cookiePrincipal = CreatePrincipal("Identity.Application", CookieIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, cookiePrincipal);

        var bearerPrincipal = CreatePrincipal("AuthenticationTypes.Federation", BearerIssuer);
        var isValid = await ValidateAsync(antiforgery, serviceProvider, bearerPrincipal, cookieToken, requestToken);

        isValid.ShouldBeFalse();
    }

    [Fact]
    public async Task Token_should_validate_when_the_cookie_principal_user_id_issuer_is_not_local_authority()
    {
        // Tiered/OIDC templates back the cookie with an OIDC principal whose user id issuer is the token
        // authority. Because the decorator normalizes both generation and validation (Razor Pages validate
        // through the same decorated IAntiforgery), the per-user identifier still matches.
        var (antiforgery, serviceProvider) = CreateDecoratedAntiforgery(normalize: true);

        var oidcCookiePrincipal = CreatePrincipal("Identity.Application", BearerIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, oidcCookiePrincipal);

        var isValid = await ValidateAsync(antiforgery, serviceProvider, oidcCookiePrincipal, cookieToken, requestToken);

        isValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Token_should_validate_across_schemes_when_principal_has_both_sub_and_name_identifier()
    {
        // The extractor picks "sub" before NameIdentifier and a principal can carry both, so the
        // normalization must cover the claim actually picked.
        var (antiforgery, serviceProvider) = CreateDecoratedAntiforgery(normalize: true);

        var cookiePrincipal = CreatePrincipalWithSubAndNameIdentifier("Identity.Application", CookieIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, cookiePrincipal);

        var bearerPrincipal = CreatePrincipalWithSubAndNameIdentifier("AuthenticationTypes.Federation", BearerIssuer);
        var isValid = await ValidateAsync(antiforgery, serviceProvider, bearerPrincipal, cookieToken, requestToken);

        isValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Decorator_should_restore_the_original_principal_after_each_call()
    {
        var (antiforgery, serviceProvider) = CreateDecoratedAntiforgery(normalize: true);

        var originalPrincipal = CreatePrincipal("AuthenticationTypes.Federation", BearerIssuer);
        var httpContext = new DefaultHttpContext { User = originalPrincipal, RequestServices = serviceProvider };

        antiforgery.GetAndStoreTokens(httpContext);

        httpContext.User.ShouldBeSameAs(originalPrincipal);
        httpContext.User.FindFirst(AbpClaimTypes.UserId)!.Issuer.ShouldBe(BearerIssuer);

        httpContext.Request.Headers["Cookie"] = $"{AntiForgeryCookieName}=invalid";
        await antiforgery.IsRequestValidAsync(httpContext);

        httpContext.User.ShouldBeSameAs(originalPrincipal);
        httpContext.User.FindFirst(AbpClaimTypes.UserId)!.Issuer.ShouldBe(BearerIssuer);
    }

    [Fact]
    public async Task Decorator_should_not_normalize_when_disabled()
    {
        var (antiforgery, serviceProvider) = CreateDecoratedAntiforgery(normalize: false);

        var principal = CreatePrincipal("Identity.Application", CookieIssuer);
        var httpContext = new DefaultHttpContext { User = principal, RequestServices = serviceProvider };

        antiforgery.GetAndStoreTokens(httpContext);
        httpContext.User.ShouldBeSameAs(principal);

        httpContext.Request.Headers["Cookie"] = $"{AntiForgeryCookieName}=invalid";
        await antiforgery.IsRequestValidAsync(httpContext);
        httpContext.User.ShouldBeSameAs(principal);
    }

    private static ClaimsPrincipal CreatePrincipal(string authenticationType, string userIdClaimIssuer)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
            new[]
            {
                new Claim(AbpClaimTypes.UserId, UserId, ClaimValueTypes.String, userIdClaimIssuer),
                new Claim("preferred_username", "admin", ClaimValueTypes.String, userIdClaimIssuer)
            },
            authenticationType,
            "preferred_username",
            AbpClaimTypes.Role));
    }

    private static ClaimsPrincipal CreatePrincipalWithSubAndNameIdentifier(string authenticationType, string issuer)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
            new[]
            {
                new Claim("sub", UserId, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.NameIdentifier, UserId, ClaimValueTypes.String, issuer),
                new Claim("preferred_username", "admin", ClaimValueTypes.String, issuer)
            },
            authenticationType,
            "preferred_username",
            AbpClaimTypes.Role));
    }

    private static (IAntiforgery antiforgery, IServiceProvider serviceProvider) CreateDecoratedAntiforgery(bool normalize)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDataProtection();
        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = AntiForgeryCookieName;
            options.HeaderName = AntiForgeryHeaderName;
        });
        services.AddTransient<IAbpAntiForgeryClaimsPrincipalNormalizer, AbpAntiForgeryClaimsPrincipalNormalizer>();

        var serviceProvider = services.BuildServiceProvider();

        var antiforgery = new AbpAntiforgery(
            serviceProvider.GetRequiredService<IAntiforgery>(),
            Microsoft.Extensions.Options.Options.Create(new AbpAntiForgeryOptions { NormalizeUserIdClaimIssuer = normalize }));

        return (antiforgery, serviceProvider);
    }

    private static (string cookieToken, string requestToken) GenerateToken(
        IAntiforgery antiforgery, IServiceProvider serviceProvider, ClaimsPrincipal user)
    {
        var httpContext = new DefaultHttpContext { User = user, RequestServices = serviceProvider };
        var tokenSet = antiforgery.GetAndStoreTokens(httpContext);
        return (ExtractCookieToken(httpContext), tokenSet.RequestToken!);
    }

    private static async Task<bool> ValidateAsync(
        IAntiforgery antiforgery, IServiceProvider serviceProvider, ClaimsPrincipal user, string cookieToken, string requestToken)
    {
        var httpContext = new DefaultHttpContext { User = user, RequestServices = serviceProvider };
        httpContext.Request.Headers["Cookie"] = $"{AntiForgeryCookieName}={cookieToken}";
        httpContext.Request.Headers[AntiForgeryHeaderName] = requestToken;

        return await antiforgery.IsRequestValidAsync(httpContext);
    }

    private static string ExtractCookieToken(HttpContext httpContext)
    {
        var setCookie = httpContext.Response.Headers.SetCookie.ToString();
        var prefix = AntiForgeryCookieName + "=";
        var start = setCookie.IndexOf(prefix, StringComparison.Ordinal) + prefix.Length;
        var end = setCookie.IndexOf(';', start);
        return end < 0 ? setCookie.Substring(start) : setCookie.Substring(start, end - start);
    }
}
