using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
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
    public async Task Normalize_should_set_a_constant_issuer_on_user_identifier_claims_only()
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

        var normalized = await new AbpAntiForgeryClaimsPrincipalNormalizer().NormalizeAsync(principal);

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
    public async Task Token_issued_under_one_scheme_should_validate_under_another_when_normalization_is_enabled()
    {
        var (antiforgery, serviceProvider) = CreateAntiforgery();

        var cookiePrincipal = CreatePrincipal("Identity.Application", CookieIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, cookiePrincipal, normalize: true);

        var bearerPrincipal = CreatePrincipal("AuthenticationTypes.Federation", BearerIssuer);
        var isValid = await ValidateAsync(antiforgery, bearerPrincipal, cookieToken, requestToken, normalize: true);

        isValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Token_issued_under_one_scheme_should_fail_under_another_when_normalization_is_disabled()
    {
        var (antiforgery, serviceProvider) = CreateAntiforgery();

        var cookiePrincipal = CreatePrincipal("Identity.Application", CookieIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, cookiePrincipal, normalize: false);

        var bearerPrincipal = CreatePrincipal("AuthenticationTypes.Federation", BearerIssuer);
        var isValid = await ValidateAsync(antiforgery, bearerPrincipal, cookieToken, requestToken, normalize: false);

        isValid.ShouldBeFalse();
    }

    [Fact]
    public async Task Token_should_validate_across_schemes_when_principal_has_both_sub_and_name_identifier()
    {
        // The extractor picks "sub" before NameIdentifier and a principal can carry both, so the
        // normalization must cover the claim actually picked.
        var (antiforgery, serviceProvider) = CreateAntiforgery();

        var cookiePrincipal = CreatePrincipalWithSubAndNameIdentifier("Identity.Application", CookieIssuer);
        var (cookieToken, requestToken) = GenerateToken(antiforgery, serviceProvider, cookiePrincipal, normalize: true);

        var bearerPrincipal = CreatePrincipalWithSubAndNameIdentifier("AuthenticationTypes.Federation", BearerIssuer);
        var isValid = await ValidateAsync(antiforgery, bearerPrincipal, cookieToken, requestToken, normalize: true);

        isValid.ShouldBeTrue();
    }

    [Fact]
    public void Manager_should_not_require_the_normalizer_service_when_normalization_is_disabled()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDataProtection();
        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = AntiForgeryCookieName;
            options.HeaderName = AntiForgeryHeaderName;
        });
        // IAbpAntiForgeryClaimsPrincipalNormalizer is intentionally not registered.
        var serviceProvider = services.BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            User = CreatePrincipal("Identity.Application", CookieIssuer),
            RequestServices = serviceProvider
        };
        var manager = new AspNetCoreAbpAntiForgeryManager(
            serviceProvider.GetRequiredService<IAntiforgery>(),
            new HttpContextAccessor { HttpContext = httpContext },
            Microsoft.Extensions.Options.Options.Create(new AbpAntiForgeryOptions { NormalizeUserIdClaimIssuer = false }));

        var token = manager.GenerateToken();

        token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Filter_should_restore_the_original_principal_after_validation()
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
        services.AddTransient<AbpAntiForgeryCookieNameProvider>();
        services.Configure<AbpAntiForgeryOptions>(options =>
        {
            options.TokenCookie.Name = AntiForgeryCookieName;
            options.AuthCookieSchemaName = null; // let ShouldValidate rely on the antiforgery cookie only
            options.NormalizeUserIdClaimIssuer = true;
        });
        var serviceProvider = services.BuildServiceProvider();

        var originalPrincipal = CreatePrincipal("AuthenticationTypes.Federation", BearerIssuer);
        var httpContext = new DefaultHttpContext
        {
            User = originalPrincipal,
            RequestServices = serviceProvider
        };
        httpContext.Request.Headers["Cookie"] = $"{AntiForgeryCookieName}=invalid";

        var filter = new AbpValidateAntiforgeryTokenAuthorizationFilter(
            serviceProvider.GetRequiredService<IAntiforgery>(),
            serviceProvider.GetRequiredService<AbpAntiForgeryCookieNameProvider>(),
            NullLogger<AbpValidateAntiforgeryTokenAuthorizationFilter>.Instance);

        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata> { filter });

        await filter.OnAuthorizationAsync(filterContext);

        // The validation ran (and failed for the invalid token), but the original principal must be restored.
        httpContext.User.ShouldBeSameAs(originalPrincipal);
        httpContext.User.FindFirst(AbpClaimTypes.UserId)!.Issuer.ShouldBe(BearerIssuer);
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

    private static (IAntiforgery antiforgery, IServiceProvider serviceProvider) CreateAntiforgery()
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
        return (serviceProvider.GetRequiredService<IAntiforgery>(), serviceProvider);
    }

    private static (string cookieToken, string requestToken) GenerateToken(
        IAntiforgery antiforgery, IServiceProvider serviceProvider, ClaimsPrincipal user, bool normalize)
    {
        var httpContext = new DefaultHttpContext { User = user, RequestServices = serviceProvider };
        var manager = new AspNetCoreAbpAntiForgeryManager(
            antiforgery,
            new HttpContextAccessor { HttpContext = httpContext },
            Microsoft.Extensions.Options.Options.Create(new AbpAntiForgeryOptions { NormalizeUserIdClaimIssuer = normalize }));

        var requestToken = manager.GenerateToken();
        return (ExtractCookieToken(httpContext), requestToken);
    }

    private static async Task<bool> ValidateAsync(
        IAntiforgery antiforgery, ClaimsPrincipal user, string cookieToken, string requestToken, bool normalize)
    {
        var httpContext = new DefaultHttpContext { User = user };
        httpContext.Request.Headers["Cookie"] = $"{AntiForgeryCookieName}={cookieToken}";
        httpContext.Request.Headers[AntiForgeryHeaderName] = requestToken;

        if (normalize)
        {
            httpContext.User = await new AbpAntiForgeryClaimsPrincipalNormalizer().NormalizeAsync(httpContext.User);
        }

        try
        {
            await antiforgery.ValidateRequestAsync(httpContext);
            return true;
        }
        catch (AntiforgeryValidationException)
        {
            return false;
        }
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
