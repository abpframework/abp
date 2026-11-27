using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

public class McpAuthenticationTests : AgentToolsMcpTestBase
{
    private const string Metadata = "resource_metadata=\"http://localhost/.well-known/oauth-protected-resource/mcp\"";

    [Fact]
    public async Task No_Token_Is_401_With_The_Resource_Metadata_Hint()
    {
        var response = await PostInitializeAsync(CreateHttpClient());

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.WwwAuthenticate.ToString().ShouldBe("Bearer " + Metadata);
    }

    [Fact]
    public async Task No_Token_Is_401_With_The_Hint_On_Every_Route_MapMcp_Maps()
    {
        var routes = GetRequiredService<EndpointDataSource>().Endpoints.OfType<RouteEndpoint>()
            .Where(e => e.RoutePattern.RawText?.StartsWith("/mcp", StringComparison.Ordinal) == true)
            .SelectMany(e => (e.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods ?? []).Select(m => (Method: m, Path: e.RoutePattern.RawText!)))
            .ToList();
        routes.ShouldNotBeEmpty();

        foreach (var (method, path) in routes)
        {
            var response = await CreateHttpClient().SendAsync(new HttpRequestMessage(new HttpMethod(method), path));

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized, $"{method} {path}");
            response.Headers.WwwAuthenticate.ToString().ShouldBe("Bearer " + Metadata);
        }

        // Stateless: no GET (SSE stream) or DELETE (session end) is mapped, so nothing runs for them.
        (await CreateHttpClient().GetAsync("/mcp")).StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        (await CreateHttpClient().DeleteAsync("/mcp")).StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task Invalid_Token_With_A_Valid_Cookie_Session_Is_401_Invalid_Token()
    {
        var client = CreateHttpClient();
        client.DefaultRequestHeaders.Add("Cookie", await LoginWithCookieAsync(Guid.NewGuid(), TenantA));
        client.DefaultRequestHeaders.Authorization = new("Bearer", "invalid");

        var response = await PostInitializeAsync(client);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.WwwAuthenticate.ToString().ShouldBe("Bearer error=\"invalid_token\", " + Metadata);
    }

    [Fact]
    public async Task Invalid_Token_Is_401_With_Invalid_Token_And_The_Hint()
    {
        var client = CreateHttpClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", "invalid");

        var response = await PostInitializeAsync(client);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.WwwAuthenticate.ToString().ShouldBe("Bearer error=\"invalid_token\", " + Metadata);
    }

    [Fact]
    public async Task Cookie_Session_Alone_Is_401_Although_It_Authenticates_The_Rest_Of_The_App()
    {
        var userId = Guid.NewGuid();
        var cookie = await LoginWithCookieAsync(userId, TenantA);

        var app = CreateHttpClient();
        app.DefaultRequestHeaders.Add("Cookie", cookie);
        var whoami = await app.GetFromJsonAsync<JsonElement>("/test/whoami");
        whoami.GetProperty("userId").GetGuid().ShouldBe(userId);
        whoami.GetProperty("tenantId").GetGuid().ShouldBe(TenantA);

        var response = await PostInitializeAsync(app);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.WwwAuthenticate.ToString().ShouldBe("Bearer " + Metadata);
    }

    [Fact]
    public async Task Token_Without_The_Scope_Is_403_With_Insufficient_Scope()
    {
        var token = NewUser(TenantA) with { Scope = "openid profile AgentToolsX" };

        var response = await PostInitializeAsync(CreateHttpClient(token));

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        response.Headers.WwwAuthenticate.ToString().ShouldBe("Bearer error=\"insufficient_scope\", scope=\"AgentTools\", " + Metadata);
        (await response.Content.ReadAsStringAsync()).ShouldContain("'AgentTools' scope");
    }

    [Fact]
    public async Task Token_Without_A_User_Is_403()
    {
        var response = await PostInitializeAsync(CreateHttpClient(new Token(null, TenantA)));

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        (await response.Content.ReadAsStringAsync()).ShouldContain("client credentials");
    }

    [Fact]
    public async Task Token_With_The_Scope_Among_Others_Connects()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        client.ServerInfo.ShouldNotBeNull();
        (await client.ListToolsAsync()).ShouldNotBeEmpty();
    }
}

/// <summary>ABP's dynamic claims on: the guard refreshes the token's principal as AbpDynamicClaimsMiddleware would.</summary>
public class McpDynamicClaimsTests : AgentToolsMcpTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<SessionContributor>();
        services.Configure<AbpClaimsPrincipalFactoryOptions>(o =>
        {
            o.IsDynamicClaimsEnabled = true;
            o.DynamicContributors.Add(typeof(SessionContributor));
        });
    }

    [Fact]
    public async Task Revoked_Session_Is_401_Invalid_Token()
    {
        var response = await PostInitializeAsync(CreateHttpClient(new Token(SessionContributor.RevokedUserId, TenantA)));

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        response.Headers.WwwAuthenticate.ToString().ShouldStartWith("Bearer error=\"invalid_token\", resource_metadata=");
        (await response.Content.ReadAsStringAsync()).ShouldContain("session is no longer valid");
    }

    [Fact]
    public async Task Live_Session_Works_With_The_Refreshed_Claims()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        (await CallAsync(client, Sample.McpSampleAppService.GetNotes)).IsError.ShouldNotBe(true);

        Observer.Records.ShouldHaveSingleItem().UserName.ShouldBe("refreshed-user");
    }

    /// <summary>Like ABP Identity's dynamic contributor: a revoked session becomes an anonymous principal; others get fresh claims.</summary>
    private class SessionContributor : IAbpDynamicClaimsPrincipalContributor
    {
        public static readonly Guid RevokedUserId = Guid.Parse("0d0d0d0d-0000-0000-0000-00000000000d");

        public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            if (context.ClaimsPrincipal.FindUserId() == RevokedUserId)
            {
                context.ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            }
            else
            {
                context.ClaimsPrincipal.Identities.First().AddOrReplace(new Claim(AbpClaimTypes.UserName, "refreshed-user"));
            }

            return Task.CompletedTask;
        }
    }
}

/// <summary>The app does not start when AbpAgentToolsMcpOptions.AuthenticationScheme is not a bearer scheme.</summary>
public class McpAuthenticationSchemeCheckTests
{
    [Theory]
    [InlineData(CookieAuthenticationDefaults.AuthenticationScheme, nameof(CookieAuthenticationHandler))]
    [InlineData("Forwarding", nameof(PolicySchemeHandler))]
    [InlineData("NoSuchScheme", "not a registered authentication scheme")]
    public void Startup_Fails_For_A_Cookie_A_Policy_Or_A_Missing_Scheme(string scheme, string reason)
    {
        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.ConfigureServices(services =>
        {
            services.AddAuthentication().AddPolicyScheme("Forwarding", null, o => o.ForwardDefault = CookieAuthenticationDefaults.AuthenticationScheme);
            services.PostConfigure<AbpAgentToolsMcpOptions>(o => o.AuthenticationScheme = scheme);
        }));

        var exception = Should.Throw<Exception>(() => factory.CreateClient());

        exception.ToString().ShouldContain($"AbpAgentToolsMcpOptions.AuthenticationScheme is \"{scheme}\"");
        exception.ToString().ShouldContain(reason);
    }
}
