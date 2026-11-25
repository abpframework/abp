using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using Shouldly;
using Volo.Abp.AgentTools.Mcp;
using Xunit;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Real tokens from ABP's token endpoint, validated by the app's OpenIddict validation (audience "TestApp", as the templates set
/// it) on <c>/test/bearer</c> and by the real MCP endpoint's guard (default scheme) on <c>/mcp</c>. Client credentials stand in
/// for the code flow: ABP builds the audience from the scopes' resources the same way; the guard then refuses them for having
/// no user, which proves they passed authentication and the scope check.
/// </summary>
public class AgentToolsOpenIddictTokenTests : AgentToolsOpenIddictTestBase
{
    [Fact]
    public async Task Token_With_Only_The_AgentTools_Scope_Is_Refused_Outside_The_Mcp_Endpoint_Reaches_Its_Guard_And_Is_Refused_Once_Revoked()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools");
        var token = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        await ShouldBeRefusedForHavingNoUserAsync(token);

        await WithUnitOfWorkAsync(async sp =>
        {
            var tokens = sp.GetRequiredService<IOpenIddictTokenManager>();
            await foreach (var entry in tokens.ListAsync())
            {
                await tokens.TryRevokeAsync(entry);
            }
        });

        (await PostMcpAsync(token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Token_With_The_AgentTools_Scope_And_An_App_Api_Scope_Is_Accepted_By_The_App_And_The_Mcp_Endpoint()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("TestApi", AgentToolsOpenIddictTestModule.AppAudience);
        var secret = await CreateClientCredentialsClientAsync("both", "AgentTools TestApi");
        var token = await GetClientCredentialsTokenAsync("both", secret, "AgentTools TestApi");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.OK);
        await ShouldBeRefusedForHavingNoUserAsync(token);
    }

    [Fact]
    public async Task Api_Token_Without_The_AgentTools_Scope_Is_Accepted_By_The_App_And_Is_403_Insufficient_Scope_At_The_Mcp_Endpoint()
    {
        await CreateScopeAsync("TestApi", AgentToolsOpenIddictTestModule.AppAudience);
        var secret = await CreateClientCredentialsClientAsync("api", "TestApi");
        var token = await GetClientCredentialsTokenAsync("api", secret, "TestApi");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.OK);
        var response = await PostMcpAsync(token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        response.Headers.WwwAuthenticate.ToString().ShouldStartWith("Bearer error=\"insufficient_scope\"");
    }

    [Fact]
    public async Task Token_Whose_Scope_Resources_Are_No_Accepted_Audience_Is_Refused()
    {
        // The pitfall the default scope resource avoids: a scope whose resources the validation does not accept as audience.
        await CreateScopeAsync("OtherApi", "Elsewhere");
        var secret = await CreateClientCredentialsClientAsync("other", "OtherApi");
        var token = await GetClientCredentialsTokenAsync("other", secret, "OtherApi");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        (await PostMcpAsync(token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("/test/bearer")]
    [InlineData("/mcpx")]
    [InlineData("/mcp/other")]
    public async Task Token_Whose_Other_Audience_Is_Not_Accepted_Is_Valid_At_The_Mcp_Endpoint_Only(string path)
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("OtherApi", "Elsewhere");
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools OtherApi");
        var token = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools OtherApi");

        (await GetWithBearerAsync(path, token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        await ShouldBeRefusedForHavingNoUserAsync(token);
    }

    [Fact]
    public async Task Token_With_An_Accepted_And_A_Not_Accepted_Audience_Is_Valid_In_The_App_And_At_The_Mcp_Endpoint()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("OtherApi", "Elsewhere");
        await CreateScopeAsync("TestApi", AgentToolsOpenIddictTestModule.AppAudience);
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools OtherApi TestApi");
        var token = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools OtherApi TestApi");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.OK);
        await ShouldBeRefusedForHavingNoUserAsync(token);
    }

    [Theory]
    [InlineData("testapp")]
    [InlineData("https://app.test/MCP")]
    public async Task An_Audience_That_Differs_By_Case_Is_Not_An_Accepted_One(string resource)
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("OtherApi", resource);
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools OtherApi");
        var token = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools OtherApi");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        await ShouldBeRefusedForHavingNoUserAsync(token);
    }

    [Theory]
    [InlineData("/mcpx", null)]
    [InlineData("/mcp/other", null)]
    [InlineData("/test/bearer", null)]
    [InlineData("/test/bearer", "/mcp")] // PathBase "/mcp" from a forwarded prefix the app trusts
    public async Task Token_With_Only_The_AgentTools_Scope_Is_Refused_On_Every_Endpoint_But_The_Mcp_Endpoint(string path, string? forwardedPrefix)
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("TestApi", AgentToolsOpenIddictTestModule.AppAudience);
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools TestApi");
        var mcpOnly = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools");
        var withApi = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools TestApi");

        (await GetWithBearerAsync(path, withApi, forwardedPrefix)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await GetWithBearerAsync(path, mcpOnly, forwardedPrefix)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData(AgentToolsOpenIddictTestModule.WithoutEndpointPath)]
    [InlineData(AgentToolsOpenIddictTestModule.BeforeRoutingPath)]
    public async Task Token_With_Only_The_AgentTools_Scope_Is_Refused_On_A_Request_Without_An_Endpoint(string path)
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("TestApi", AgentToolsOpenIddictTestModule.AppAudience);
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools TestApi");

        (await GetWithBearerAsync(path, await GetClientCredentialsTokenAsync("agent", secret, "AgentTools TestApi")))
            .StatusCode.ShouldBe(HttpStatusCode.OK);
        (await GetWithBearerAsync(path, await GetClientCredentialsTokenAsync("agent", secret, "AgentTools")))
            .StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("/mcp")]
    [InlineData("/MCP")]
    public async Task Token_With_Only_The_AgentTools_Scope_Reaches_The_Mcp_Endpoint_Guard_Whatever_The_Case(string path)
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools");

        await ShouldBeRefusedForHavingNoUserAsync(await GetClientCredentialsTokenAsync("agent", secret, "AgentTools"), path);
    }

    [Fact]
    public void The_Endpoint_Marker_Is_The_One_The_Mcp_Package_Sets()
    {
        AgentToolsMcpTokenAudienceHandler.McpEndpointMetadata.ShouldBe(AbpAgentToolsMcpOptions.EndpointMetadata);
    }
}

/// <summary>An app that calls no AddAudiences: its OpenIddict validation accepts any audience.</summary>
public class AgentToolsOpenIddictTokenWithoutAudiencesTests : AgentToolsOpenIddictTestBase
{
    protected override bool AcceptsAnyAudience => true;

    [Fact]
    public void The_Mcp_Url_Is_Not_Added_To_The_Audiences()
    {
        GetRequiredService<IOptions<OpenIddictValidationOptions>>().Value.Audiences.ShouldBeEmpty();
    }

    [Fact]
    public async Task Token_With_Only_The_AgentTools_Scope_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools");
        var token = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        await ShouldBeRefusedForHavingNoUserAsync(token);
    }

    [Fact]
    public async Task Token_With_Any_Other_Audience_Is_Valid_In_The_App_And_At_The_Mcp_Endpoint()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("OtherApi", "Elsewhere");
        var secret = await CreateClientCredentialsClientAsync("agent", "AgentTools OtherApi");
        var token = await GetClientCredentialsTokenAsync("agent", secret, "AgentTools OtherApi");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.OK);
        await ShouldBeRefusedForHavingNoUserAsync(token);
    }

    [Theory]
    [InlineData(null)] // no audience at all
    [InlineData("Elsewhere")]
    public async Task Token_Without_The_Mcp_Audience_Is_Valid_In_The_App(string? resource)
    {
        await CreateScopeAsync("OtherApi", resource == null ? [] : [resource]);
        var secret = await CreateClientCredentialsClientAsync("other", "OtherApi");
        var token = await GetClientCredentialsTokenAsync("other", secret, "OtherApi");

        foreach (var path in AgentToolsOpenIddictTestModule.BearerProbePaths)
        {
            (await GetWithBearerAsync(path, token)).StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
