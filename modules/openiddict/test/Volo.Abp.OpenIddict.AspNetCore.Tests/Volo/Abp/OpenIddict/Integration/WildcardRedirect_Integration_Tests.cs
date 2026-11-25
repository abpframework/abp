using System;
using System.Net;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Shouldly;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.WildcardDomains;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.OpenIddict.Integration;

[DependsOn(typeof(OpenIddictTokenIntegrationTestModule))]
public class WildcardRedirectTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpOpenIddictWildcardDomainOptions>(options =>
        {
            options.EnableWildcardDomainSupport = true;
            options.WildcardDomainsFormat.Add("https://{0}.app.test");
            options.WildcardDomainsFormat.Add("https://*.trusted.test");
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.GetSingletonInstance<ApplicationPartManager>().ApplicationParts
            .AddIfNotContains(typeof(WildcardCookieController).Assembly);
        Configure<AbpTenantResolveOptions>(options => options.AddDomainTenantResolver("{0}.app.test"));
        Configure<AbpDefaultTenantStoreOptions>(options => options.Tenants =
        [
            new TenantConfiguration(Guid.NewGuid(), "acme", "ACME"),
            new TenantConfiguration(Guid.NewGuid(), "other", "OTHER"),
            new TenantConfiguration(Guid.NewGuid(), "x@other", "X@OTHER")
        ]);
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        using var scope = context.ServiceProvider.CreateScope();
        using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        foreach (var clientId in new[] { "tenant-client", "unrelated-client", "no-logout-client" })
        {
            var descriptor = new AbpApplicationDescriptor
            {
                ClientId = clientId,
                ClientType = OpenIddictConstants.ClientTypes.Public,
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.ResponseTypes.Code
                }
            };
            var origin = clientId == "tenant-client" ? "app.test" : "unrelated.test";
            descriptor.RedirectUris.Add(new Uri($"https://{origin}/callback?client=one"));
            descriptor.RedirectUris.Add(new Uri("https://trusted.test/callback?client=one"));
            descriptor.PostLogoutRedirectUris.Add(new Uri($"https://{origin}/signed-out"));
            descriptor.PostLogoutRedirectUris.Add(new Uri("https://trusted.test/signed-out"));
            if (clientId == "tenant-client")
            {
                descriptor.PostLogoutRedirectUris.Add(new Uri("https://app.test"));
            }
            if (clientId == "no-logout-client")
            {
                descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.EndSession);
            }

            await manager.CreateAsync(descriptor);
        }
        var tenants = await scope.ServiceProvider.GetRequiredService<ITenantStore>().GetListAsync();
        var users = scope.ServiceProvider.GetRequiredService<Volo.Abp.Identity.IdentityUserManager>();
        var currentTenant = scope.ServiceProvider.GetRequiredService<ICurrentTenant>();
        foreach (var tenantId in new Guid?[] { null }.Concat(tenants.Select(t => (Guid?)t.Id)))
        {
            using (currentTenant.Change(tenantId))
            {
                (await users.CreateAsync(new IdentityUser(Guid.NewGuid(), "callback-user", "callback@example.test", tenantId))).CheckErrors();
            }
        }
        await uow.CompleteAsync();
    }
}

[Route("test/wildcard")]
public class WildcardCookieController : AbpController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(string tenant,
        [FromServices] ICurrentTenant currentTenant, [FromServices] ITenantStore tenants,
        [FromServices] SignInManager<IdentityUser> signIn)
    {
        var configuration = (await tenants.GetListAsync()).FirstOrDefault(t => t.Name == tenant);
        using (currentTenant.Change(configuration?.Id))
        {
            var user = await signIn.UserManager.FindByNameAsync("callback-user");
            await signIn.SignInAsync(user!, false);
            return Ok();
        }
    }
}

public class WildcardRedirect_Integration_Tests : AbpAspNetCoreIntegratedTestBase<WildcardRedirectTestModule>
{
    [Theory]
    [InlineData("tenant-client", false)]
    [InlineData("unrelated-client", true)]
    public async Task Wildcard_Logout_Must_Not_Skip_Identity_Token_Audience_Check(string audience, bool rejected)
    {
        var transaction = await GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
        transaction.Request = new OpenIddictRequest
        {
            ClientId = "tenant-client", PostLogoutRedirectUri = "https://any.trusted.test/signed-out"
        };
        var validation = new OpenIddictServerEvents.ValidateEndSessionRequestContext(transaction)
        {
            IdentityTokenHintPrincipal = new ClaimsPrincipal(new ClaimsIdentity()).SetAudiences(audience)
        };
        await GetRequiredService<AbpValidateAuthorizedParty>().HandleAsync(validation);
        validation.IsRejected.ShouldBe(rejected);
    }

    [Theory]
    [InlineData("tenant-client", false, true)]
    [InlineData("no-logout-client", false, false)]
    [InlineData("no-logout-client", true, true)]
    public async Task Logout_Without_ClientId_Requires_Hint_Client_Permission(string clientId, bool ignorePermissions, bool valid)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity()).SetPresenters(clientId);
        (await GetRequiredService<AbpOpenIddictWildcardDomainValidator>().IsValidPostLogoutAsync(
            "https://any.trusted.test/signed-out", principal, ignorePermissions)).ShouldBe(valid);
    }

    [Theory]
    [InlineData("acme", "https://other.app.test/callback?client=one", "other", false)]
    [InlineData("host", "https://other.app.test/callback?client=one", "other", false)]
    [InlineData("acme", "https://acme.app.test/callback?client=one", null, true)]
    [InlineData("host", "https://app.test/callback?client=one", null, true)]
    public async Task Issued_Code_Must_Be_Bound_To_The_Authenticated_User_Tenant(string tenant, string redirect, string requestedTenant, bool valid)
    {
        using var login = await Client.PostAsync("/test/wildcard/login?tenant=" + tenant, null);
        login.StatusCode.ShouldBe(HttpStatusCode.OK);
        var cookies = string.Join("; ", login.Headers.GetValues("Set-Cookie").Select(value => value.Split(';')[0]));
        var url = AuthorizationUrl(redirect, "tenant-client");
        if (requestedTenant != null)
        {
            // Exercise supported custom ordering where the early request tenant differs from the later cookie tenant.
            var options = GetRequiredService<Microsoft.Extensions.Options.IOptions<AbpTenantResolveOptions>>().Value;
            options.TenantResolvers.RemoveAll(resolver => resolver is QueryStringTenantResolveContributor);
            options.TenantResolvers.Insert(options.TenantResolvers.FindIndex(
                resolver => resolver is CurrentUserTenantResolveContributor) + 1, new QueryStringTenantResolveContributor());
            url += "&__tenant=" + requestedTenant;
        }
        using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
        request.Headers.Host = tenant == "host" ? "app.test" : tenant + ".app.test";
        request.Headers.Add("Cookie", cookies);
        using var response = await Client.SendAsync(request);
        var location = response.Headers.Location?.OriginalString ?? "";
        location.Contains("code=", StringComparison.Ordinal).ShouldBe(valid);
        if (valid)
        {
            response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
            location.ShouldStartWith(redirect + "&");
        }
        else
        {
            (WebUtility.UrlDecode(location) + await response.Content.ReadAsStringAsync())
                .ShouldContain("The redirect URI is not valid for this tenant.");
        }
    }

    [Fact]
    public async Task Cached_Authorization_Request_Must_Not_Be_Checked_Before_Sign_In()
    {
        var transaction = await GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
        transaction.EndpointType = OpenIddictServerEndpointType.Authorization;
        transaction.Request = new OpenIddictRequest
        {
            ClientId = "tenant-client", RedirectUri = "https://acme.app.test/callback?client=one"
        };
        var signIn = new OpenIddictServerEvents.ProcessSignInContext(transaction)
        {
            Principal = new ClaimsPrincipal(new ClaimsIdentity())
        };
        await GetRequiredService<AbpValidateTenantRedirect>().HandleAsync(signIn);
        signIn.IsRejected.ShouldBeFalse();
    }

    [Theory]
    [InlineData("acme", null, true)]
    [InlineData("x@other", "https://other.app.test/callback?client=one", true)]
    [InlineData("other", "https://other.app.test/callback?client=one", false)]
    public async Task Sign_In_Checks_The_Resolved_Redirect_Against_The_User_Tenant(string tenant, string requestRedirect, bool rejected)
    {
        var tenantId = (await GetRequiredService<ITenantStore>().GetListAsync()).Single(t => t.Name == tenant).Id;
        var transaction = await GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
        transaction.EndpointType = OpenIddictServerEndpointType.Authorization;
        transaction.Request = new OpenIddictRequest { ClientId = "tenant-client", RedirectUri = requestRedirect };
        // A custom handler may resolve the redirect URI when the request omits it; OpenIddict redirects to that value.
        var validation = new OpenIddictServerEvents.ValidateAuthorizationRequestContext(transaction);
        validation.SetRedirectUri("https://other.app.test/callback?client=one");
        transaction.SetProperty(typeof(OpenIddictServerEvents.ValidateAuthorizationRequestContext).FullName!, validation);
        var signIn = new OpenIddictServerEvents.ProcessSignInContext(transaction)
        {
            Principal = new ClaimsPrincipal(new ClaimsIdentity([new Claim(AbpClaimTypes.TenantId, tenantId.ToString())], "test"))
        };
        await GetRequiredService<AbpValidateTenantRedirect>().HandleAsync(signIn);
        signIn.IsRejected.ShouldBe(rejected);
    }

    [Theory]
    [InlineData("https://acme.app.test/signed-out", true)]
    [InlineData("https://other.app.test/signed-out", false)]
    [InlineData("https://acme.app.test/arbitrary", false)]
    [InlineData("https://acme.app.test", true)]
    [InlineData("https://acme.app.test/", true)]
    public async Task Logout_Without_ClientId_Still_Requires_A_Registered_Tenant_Callback(string redirect, bool valid)
    {
        using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get,
            "/connect/endsession?post_logout_redirect_uri=" + Uri.EscapeDataString(redirect));
        request.Headers.Host = "acme.app.test";
        using var response = await Client.SendAsync(request);
        response.StatusCode.ShouldBe(valid ? HttpStatusCode.Redirect : HttpStatusCode.BadRequest);
        if (valid)
        {
            response.Headers.Location.ShouldBe(new Uri(redirect));
        }
    }

    [Theory]
    [InlineData("acme.app.test", "https://other.app.test/callback?client=one", "tenant-client")]
    [InlineData("acme.app.test", "https://acme.app.test/unregistered", "tenant-client")]
    [InlineData("acme.app.test", "https://acme.app.test/callback?client=two", "tenant-client")]
    [InlineData("acme.app.test", "https://acme.app.test/callback?client=one", "unrelated-client")]
    [InlineData("app.test", "https://acme.app.test/callback?client=one", "tenant-client")]
    [InlineData("missing.app.test", "https://missing.app.test/callback?client=one", "tenant-client")]
    [InlineData("acme.app.test", "https://a.acme.app.test/callback?client=one", "tenant-client")]
    [InlineData("acme.app.test", "https://acme.app.test:8443/callback?client=one", "tenant-client")]
    [InlineData("acme.app.test", "https://acme.app.test/callback?client=one#fragment", "tenant-client")]
    public async Task Invalid_Redirect_Must_Be_Rejected_Before_Authentication(string host, string redirect, string clientId)
    {
        using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, AuthorizationUrl(redirect, clientId));
        request.Headers.Host = host;
        using var response = await Client.SendAsync(request);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        response.Headers.Location.ShouldBeNull();
    }

    [Theory]
    [InlineData("acme.app.test", "https://acme.app.test/callback?client=one")]
    [InlineData("app.test", "https://app.test/callback?client=one")]
    [InlineData("app.test", "https://any.trusted.test/callback?client=one")]
    public async Task Registered_Or_Valid_Tenant_Redirect_Reaches_Login(string host, string redirect)
    {
        GetRequiredService<ICurrentTenant>().Id.ShouldBeNull();
        using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, AuthorizationUrl(redirect, "tenant-client"));
        request.Headers.Host = host;
        using var response = await Client.SendAsync(request);
        response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        response.Headers.Location!.OriginalString.ShouldContain("/Account/Login");
    }

    [Theory]
    [InlineData("https://other.app.test/signed-out", false)]
    [InlineData("https://acme.app.test/arbitrary", false)]
    [InlineData("https://acme.app.test/signed-out", true)]
    public async Task Logout_Uses_The_Same_Tenant_And_Registration_Boundary(string redirect, bool valid)
    {
        using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get,
            "/connect/endsession?client_id=tenant-client&post_logout_redirect_uri=" + Uri.EscapeDataString(redirect));
        request.Headers.Host = "acme.app.test";
        using var response = await Client.SendAsync(request);
        if (valid)
        {
            response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
            response.Headers.Location.ShouldBe(new Uri(redirect));
        }
        else
        {
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            response.Headers.Location.ShouldBeNull();
        }
    }

    private static string AuthorizationUrl(string redirect, string clientId) =>
        "/connect/authorize?client_id=" + clientId + "&response_type=code&scope=openid&redirect_uri=" + Uri.EscapeDataString(redirect);
}
