using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.AgentTools.Mcp;
using Volo.Abp.Identity;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Real tokens of a user (password, code and refresh token grants of ABP's server), validated by the app's OpenIddict validation
/// on the app's endpoints (<see cref="AgentToolsOpenIddictTestModule.BearerProbePaths"/>) and by the MCP endpoint's guard on
/// <c>/mcp</c>.
/// </summary>
public abstract class AgentToolsScopeConfinementTestBase : AgentToolsOpenIddictTestBase
{
    protected const string OldMcpUrl = "https://old.test/mcp";
    protected const string AppAudience = AgentToolsOpenIddictTestModule.AppAudience;

    private readonly string _secret = Guid.NewGuid().ToString();
    private readonly string _password = "Aa1!" + Guid.NewGuid();

    /// <summary>The answer to a token whose audiences only a host that accepts any audience accepts.</summary>
    protected HttpStatusCode OkIfAnyAudienceIsAccepted => AcceptsAnyAudience ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;

    private const string RedirectUri = "https://client.test/callback";

    /// <summary>The user "bob" and a client that may request exactly these (space-separated) scopes.</summary>
    protected Task CreateUserAndClientAsync(string scopes)
    {
        return WithUnitOfWorkAsync(async sp =>
        {
            (await sp.GetRequiredService<IdentityUserManager>()
                .CreateAsync(new IdentityUser(Guid.NewGuid(), "bob", "bob@app.test"), _password)).CheckErrors();

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = "agent",
                DisplayName = "agent",
                ClientSecret = _secret,
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                RedirectUris = { new Uri(RedirectUri) },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Code
                }
            };
            descriptor.Permissions.UnionWith(scopes.Split(' ').Select(s => OpenIddictConstants.Permissions.Prefixes.Scope + s));
            await sp.GetRequiredService<IOpenIddictApplicationManager>().CreateAsync(descriptor);
        });
    }

    /// <summary>The access and the refresh token of a user, from a client that may request exactly these (space-separated) scopes.</summary>
    protected async Task<(string Access, string Refresh)> GetUserTokensAsync(string scopes)
    {
        await CreateUserAndClientAsync(scopes);

        var body = await RequestTokenAsync(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = "agent",
            ["client_secret"] = _secret,
            ["username"] = "bob",
            ["password"] = _password,
            ["scope"] = scopes + " offline_access"
        });
        return (body.GetProperty("access_token").GetString()!, body.GetProperty("refresh_token").GetString()!);
    }

    protected async Task<string> GetUserTokenAsync(string scopes) => (await GetUserTokensAsync(scopes)).Access;

    /// <summary>An authorization code of the user of <see cref="CreateUserAndClientAsync"/> for the scopes.</summary>
    protected async Task<string> GetCodeAsync(string scopes)
    {
        (await Client.PostAsync("/test/login?userName=bob", null)).EnsureSuccessStatusCode();
        var response = await Client.GetAsync("/connect/authorize?response_type=code&client_id=agent" +
                                             $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}&scope={Uri.EscapeDataString(scopes)}");
        response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        return QueryHelpers.ParseQuery(response.Headers.Location!.Query)["code"].ToString();
    }

    /// <summary>A new access token; <paramref name="scopes"/> narrows its scopes, null keeps them.</summary>
    protected async Task<string> RefreshAsync(string refreshToken, string? scopes = null) =>
        (await RequestTokenAsync(RefreshParameters(refreshToken, scopes))).GetProperty("access_token").GetString()!;

    protected Dictionary<string, string> RefreshParameters(string refreshToken, string? scopes) =>
        TokenParameters("refresh_token", "refresh_token", refreshToken, scopes);

    protected Dictionary<string, string> CodeParameters(string code, string? scopes) =>
        new(TokenParameters("authorization_code", "code", code, scopes)) { ["redirect_uri"] = RedirectUri };

    private Dictionary<string, string> TokenParameters(string grantType, string name, string value, string? scopes)
    {
        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = grantType,
            ["client_id"] = "agent",
            ["client_secret"] = _secret,
            [name] = value
        };
        if (scopes != null)
        {
            parameters["scope"] = scopes;
        }

        return parameters;
    }

    protected async Task ShouldBeInvalidScopeAsync(Dictionary<string, string> parameters)
    {
        var response = await Client.PostAsync("/connect/token", new FormUrlEncodedContent(parameters));
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest, parameters["scope"]);
        (await response.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("error").GetString().ShouldBe("invalid_scope");
    }

    protected Task SetScopeResourcesAsync(string name, params string[] resources) => WithUnitOfWorkAsync(async sp =>
    {
        var manager = sp.GetRequiredService<IOpenIddictScopeManager>();
        var scope = (await manager.FindByNameAsync(name))!;
        var descriptor = new OpenIddictScopeDescriptor();
        await manager.PopulateAsync(descriptor, scope);
        descriptor.Resources.Clear();
        descriptor.Resources.UnionWith(resources);
        await manager.UpdateAsync(scope, descriptor);
    });

    /// <summary>The app's endpoints answer <paramref name="app"/>, an MCP initialize <paramref name="mcp"/> (200 = past the guard).</summary>
    protected async Task ShouldBeAnsweredAsync(string token, HttpStatusCode app, HttpStatusCode mcp)
    {
        foreach (var path in AgentToolsOpenIddictTestModule.BearerProbePaths)
        {
            (await GetWithBearerAsync(path, token)).StatusCode.ShouldBe(app, path);
        }

        (await PostMcpAsync(token)).StatusCode.ShouldBe(mcp);
    }

    protected Task ShouldBeValidAtTheMcpEndpointOnlyAsync(string token) =>
        ShouldBeAnsweredAsync(token, HttpStatusCode.Unauthorized, HttpStatusCode.OK);

    protected Task ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(string token) =>
        ShouldBeAnsweredAsync(token, HttpStatusCode.OK, HttpStatusCode.OK);

    /// <summary>The MCP endpoint answers insufficient_scope.</summary>
    protected Task ShouldBeValidInTheAppOnlyAsync(string token) =>
        ShouldBeAnsweredAsync(token, HttpStatusCode.OK, HttpStatusCode.Forbidden);
}

public class AgentToolsScopeConfinementTests : AgentToolsScopeConfinementTestBase
{
    [Theory]
    [InlineData("openid")]
    [InlineData("profile")]
    [InlineData("email")]
    [InlineData("phone")]
    [InlineData("address")]
    [InlineData("roles")]
    [InlineData("offline_access")]
    [InlineData("openid profile email phone address roles offline_access")]
    public async Task A_Standard_Scope_Next_To_The_Scope_Does_Not_Make_A_Token_Valid_In_The_App(string standardScopes)
    {
        await CreateScopeAsync("AgentTools", AppAudience);

        // Without offline_access, unless it is the standard scope in question.
        var (_, refreshToken) = await GetUserTokensAsync("AgentTools " + standardScopes);
        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await RefreshAsync(refreshToken, "AgentTools " + standardScopes));
    }

    [Theory]
    [InlineData("openid")]
    [InlineData("openid profile email phone address roles offline_access")]
    public async Task Standard_Scopes_Without_The_Scope_Do_Not_Confine_A_Token(string standardScopes)
    {
        await CreateScopeAsync("TestApi", AppAudience);

        await ShouldBeValidInTheAppOnlyAsync(await GetUserTokenAsync("TestApi " + standardScopes));
    }

    [Theory]
    [InlineData("OpenId")]
    [InlineData("Profile")]
    [InlineData("OFFLINE_ACCESS")]
    public async Task A_Scope_That_Differs_From_A_Standard_One_By_Case_Is_An_Api_Scope(string scope)
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync(scope, AppAudience);

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(await GetUserTokenAsync("AgentTools " + scope));
    }

    [Theory]
    [InlineData("agenttools")]
    [InlineData("AgentToolsApi")]
    public async Task A_Token_Whose_Only_Api_Scope_Has_Another_Name_Is_Not_Confined(string scope)
    {
        await CreateScopeAsync(scope, AppAudience);

        await ShouldBeValidInTheAppOnlyAsync(await GetUserTokenAsync(scope));
    }
}

/// <summary>The cases that run in a host that accepts the audiences "TestApp" and the MCP URL, and in one that accepts any.</summary>
public abstract class AgentToolsScopeConfinementByHostTestBase : AgentToolsScopeConfinementTestBase
{
    [Fact]
    public async Task A_Token_With_Only_The_Scope_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await GetUserTokenAsync("AgentTools"));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task A_Token_With_Only_The_Scope_Is_Valid_At_The_Mcp_Endpoint_Only_When_The_Scope_Has_An_Api_Resource(bool withMcpUrl)
    {
        await CreateScopeAsync("AgentTools", withMcpUrl ? [AppAudience, McpUrl] : [AppAudience]);

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await GetUserTokenAsync("AgentTools openid"));
    }

    [Fact]
    public async Task A_Token_Whose_Scope_Still_Has_A_Former_Mcp_Url_Is_Not_Valid_In_The_App()
    {
        await CreateScopeAsync("AgentTools", OldMcpUrl);

        await ShouldBeAnsweredAsync(await GetUserTokenAsync("AgentTools openid"), HttpStatusCode.Unauthorized, OkIfAnyAudienceIsAccepted);
    }

    [Fact]
    public async Task A_Token_Whose_Scope_Has_A_Former_And_The_Current_Mcp_Url_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await CreateScopeAsync("AgentTools", OldMcpUrl, McpUrl);

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await GetUserTokenAsync("AgentTools openid"));
    }

    [Fact]
    public async Task A_Token_Refreshed_After_The_Mcp_Url_Changed_Is_Not_Valid_In_The_App()
    {
        await CreateScopeAsync("AgentTools", OldMcpUrl);
        var (_, refreshToken) = await GetUserTokensAsync("AgentTools");
        await SetScopeResourcesAsync("AgentTools", McpUrl);

        await ShouldBeAnsweredAsync(await RefreshAsync(refreshToken), HttpStatusCode.Unauthorized, OkIfAnyAudienceIsAccepted);
    }

    [Fact]
    public async Task A_Token_Refreshed_For_The_Scope_Only_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("TestApi", AppAudience);
        var (token, refreshToken) = await GetUserTokensAsync("AgentTools TestApi");

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(token);
        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await RefreshAsync(refreshToken, "AgentTools"));
    }

    [Fact]
    public async Task A_Token_With_The_Scope_And_An_Api_Scope_Is_Valid_In_The_App_And_At_The_Mcp_Endpoint()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("TestApi", AppAudience);
        var (token, refreshToken) = await GetUserTokensAsync("AgentTools TestApi openid");

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(token);
        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(await RefreshAsync(refreshToken));
    }

    [Fact]
    public async Task A_Token_With_The_Scope_And_An_Api_Scope_Without_Resources_Is_Judged_By_Its_Audiences()
    {
        await CreateScopeAsync("AgentTools", AppAudience);
        await CreateScopeAsync("TestApi");

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(await GetUserTokenAsync("AgentTools TestApi"));
    }

    [Fact]
    public async Task A_Token_With_Only_An_Api_Scope_Is_Valid_In_The_App_Only()
    {
        await CreateScopeAsync("TestApi", AppAudience);

        await ShouldBeValidInTheAppOnlyAsync(await GetUserTokenAsync("TestApi"));
    }

    [Fact]
    public async Task A_Token_Without_Scopes_Is_Judged_By_Its_Audiences()
    {
        var secret = await CreateClientCredentialsClientAsync("none", "TestApi");
        var token = await GetClientCredentialsTokenAsync("none", secret, "");

        await ShouldBeAnsweredAsync(token, OkIfAnyAudienceIsAccepted, AcceptsAnyAudience ? HttpStatusCode.Forbidden : HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task A_Client_Credentials_Token_With_Only_The_Scope_Reaches_Only_The_Mcp_Endpoints_Guard()
    {
        await CreateScopeAsync("AgentTools", AppAudience);
        var secret = await CreateClientCredentialsClientAsync("machine", "AgentTools");
        var token = await GetClientCredentialsTokenAsync("machine", secret, "AgentTools");

        (await GetWithBearerAsync("/test/bearer", token)).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        await ShouldBeRefusedForHavingNoUserAsync(token);
    }
}

/// <summary>A token request with a scope parameter that narrows the scopes of the grant.</summary>
public abstract class AgentToolsScopeNarrowingTestBase : AgentToolsScopeConfinementTestBase
{
    [Theory]
    [InlineData("")]
    [InlineData(McpUrl)]
    [InlineData(AppAudience)]
    [InlineData(AppAudience + " " + McpUrl)]
    [InlineData(OldMcpUrl)]
    [InlineData(OldMcpUrl + " " + McpUrl)]
    public async Task A_Grant_Without_An_Api_Scope_Cannot_Be_Refreshed_Without_The_Scope(string resources)
    {
        await CreateScopeAsync("AgentTools", resources.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        var (_, refreshToken) = await GetUserTokensAsync("AgentTools openid");

        foreach (var scopes in new[] { "openid", "offline_access", "openid offline_access", "offline_access openid", " " })
        {
            await ShouldBeInvalidScopeAsync(RefreshParameters(refreshToken, scopes));
        }

        // The grant itself stays valid.
        (await GetWithBearerAsync("/test/bearer", await RefreshAsync(refreshToken))).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("AgentTools")]
    [InlineData("openid AgentTools")]
    [InlineData("AgentTools offline_access")]
    public async Task A_Grant_Without_An_Api_Scope_Can_Be_Refreshed_With_The_Scope(string? scopes)
    {
        await CreateScopeAsync("AgentTools", AppAudience, McpUrl);
        var (_, refreshToken) = await GetUserTokensAsync("AgentTools openid");

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await RefreshAsync(refreshToken, scopes));
    }

    [Theory]
    [InlineData("openid")]
    [InlineData("TestApi")]
    [InlineData("offline_access TestApi")]
    public async Task A_Grant_With_An_Api_Scope_Can_Be_Refreshed_Without_The_Scope(string scopes)
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateScopeAsync("TestApi", AppAudience);
        var (_, refreshToken) = await GetUserTokensAsync("AgentTools TestApi openid");

        await ShouldBeValidInTheAppOnlyAsync(await RefreshAsync(refreshToken, scopes));
    }

    [Theory]
    [InlineData("openid")]
    [InlineData("offline_access")]
    public async Task A_Grant_That_Never_Had_The_Scope_Can_Be_Refreshed_Without_It(string scopes)
    {
        await CreateScopeAsync("TestApi", AppAudience);
        var (_, refreshToken) = await GetUserTokensAsync("TestApi openid");

        await ShouldBeValidInTheAppOnlyAsync(await RefreshAsync(refreshToken, scopes));
    }

    [Fact]
    public async Task A_Code_Cannot_Be_Redeemed_For_Other_Scopes()
    {
        await CreateScopeAsync("AgentTools", AppAudience, McpUrl);
        await CreateUserAndClientAsync("AgentTools openid");

        var refused = CodeParameters(await GetCodeAsync("AgentTools openid"), "openid");
        (await Client.PostAsync("/connect/token", new FormUrlEncodedContent(refused))).StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await RequestTokenAsync(CodeParameters(await GetCodeAsync("AgentTools openid"), null));
        await ShouldBeValidAtTheMcpEndpointOnlyAsync(body.GetProperty("access_token").GetString()!);
    }
}

public class AgentToolsScopeNarrowingWithAudiencesTests : AgentToolsScopeNarrowingTestBase;

public class AgentToolsScopeNarrowingWithoutAudiencesTests : AgentToolsScopeNarrowingTestBase
{
    protected override bool AcceptsAnyAudience => true;
}

public class AgentToolsScopeConfinementWithAudiencesTests : AgentToolsScopeConfinementByHostTestBase;

public class AgentToolsScopeConfinementWithoutAudiencesTests : AgentToolsScopeConfinementByHostTestBase
{
    protected override bool AcceptsAnyAudience => true;

    [Fact]
    public async Task A_Token_With_Only_The_Scope_And_Without_An_Audience_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await CreateScopeAsync("AgentTools");

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await GetUserTokenAsync("AgentTools"));
    }

    [Fact]
    public async Task A_Token_With_Only_Standard_Scopes_Is_Valid_In_The_App_Only()
    {
        await ShouldBeValidInTheAppOnlyAsync(await GetUserTokenAsync("openid profile"));
    }
}

/// <summary>No App:SelfUrl and no McpResource: there is no MCP audience to tell a token by, its scopes do.</summary>
public abstract class AgentToolsScopeConfinementWithoutMcpUrlTestBase : AgentToolsScopeConfinementTestBase
{
    protected override Dictionary<string, string?> Configuration => new();

    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.SyncMcpResourcePermission = false;

    [Fact]
    public async Task A_Token_With_Only_The_Scope_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await CreateScopeAsync("AgentTools", AppAudience);

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await GetUserTokenAsync("AgentTools openid"));
    }

    [Fact]
    public async Task A_Token_With_The_Scope_And_An_Api_Scope_Is_Valid_In_The_App_And_At_The_Mcp_Endpoint()
    {
        await CreateScopeAsync("AgentTools", AppAudience);
        await CreateScopeAsync("TestApi", AppAudience);

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(await GetUserTokenAsync("AgentTools TestApi"));
    }

    [Fact]
    public async Task A_Token_With_Only_An_Api_Scope_Is_Valid_In_The_App_Only()
    {
        await CreateScopeAsync("TestApi", AppAudience);

        await ShouldBeValidInTheAppOnlyAsync(await GetUserTokenAsync("TestApi"));
    }
}

public class AgentToolsScopeConfinementWithoutMcpUrlWithAudiencesTests : AgentToolsScopeConfinementWithoutMcpUrlTestBase;

public class AgentToolsScopeConfinementWithoutMcpUrlWithoutAudiencesTests : AgentToolsScopeConfinementWithoutMcpUrlTestBase
{
    protected override bool AcceptsAnyAudience => true;
}

public class AgentToolsScopeConfinementWithACustomScopeNameTests : AgentToolsScopeConfinementTestBase
{
    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.ScopeName = "Agents";

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.Configure<AbpAgentToolsMcpOptions>(options => options.RequiredScope = "Agents");
    }

    [Fact]
    public async Task A_Token_With_Only_The_Custom_Scope_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await CreateScopeAsync("Agents", AppAudience);

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await GetUserTokenAsync("Agents openid"));
    }

    [Fact]
    public async Task A_Grant_With_Only_The_Custom_Scope_Cannot_Be_Refreshed_Without_It()
    {
        await CreateScopeAsync("Agents", AppAudience);
        var (_, refreshToken) = await GetUserTokensAsync("Agents openid");

        await ShouldBeInvalidScopeAsync(RefreshParameters(refreshToken, "openid"));
        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await RefreshAsync(refreshToken, "Agents"));
    }

    [Fact]
    public async Task The_Default_Name_Is_An_Api_Scope()
    {
        await CreateScopeAsync("Agents", AppAudience);
        await CreateScopeAsync("AgentTools", AppAudience);
        var (token, refreshToken) = await GetUserTokensAsync("Agents AgentTools");

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(token);
        await ShouldBeValidInTheAppOnlyAsync(await RefreshAsync(refreshToken, "AgentTools"));
    }
}

/// <summary>An app that gives the scope an API resource on purpose: only the audiences confine a token.</summary>
public abstract class AgentToolsScopeConfinementTurnedOffTestBase : AgentToolsScopeConfinementTestBase
{
    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options)
    {
        options.ConfineScopeToMcpEndpoint = false;
        options.ScopeResources.Add(AppAudience);
        options.ScopeResources.Add(McpUrl);
    }

    [Fact]
    public async Task A_Token_With_Only_The_Scope_Is_Valid_In_The_App_And_At_The_Mcp_Endpoint()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(await GetUserTokenAsync("AgentTools openid"));
    }

    [Fact]
    public async Task A_Token_Refreshed_For_The_Scope_Only_Is_Valid_In_The_App_And_At_The_Mcp_Endpoint()
    {
        await CreateScopeAsync("AgentTools", McpUrl);
        await CreateScopeAsync("TestApi", AppAudience);
        var (_, refreshToken) = await GetUserTokensAsync("AgentTools TestApi");

        await ShouldBeValidInTheAppAndAtTheMcpEndpointAsync(await RefreshAsync(refreshToken, "AgentTools"));
    }

    [Fact]
    public async Task A_Grant_Can_Be_Refreshed_Without_The_Scope()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        var (_, refreshToken) = await GetUserTokensAsync("AgentTools openid");

        await ShouldBeValidInTheAppOnlyAsync(await RefreshAsync(refreshToken, "openid"));
    }

    [Fact]
    public async Task A_Token_With_Only_The_Mcp_Audience_Is_Valid_At_The_Mcp_Endpoint_Only()
    {
        await CreateScopeAsync("AgentTools", McpUrl);

        await ShouldBeValidAtTheMcpEndpointOnlyAsync(await GetUserTokenAsync("AgentTools"));
    }

    [Fact]
    public async Task A_Token_Without_The_Mcp_Audience_Is_Judged_By_Its_Audiences()
    {
        await CreateScopeAsync("AgentTools", OldMcpUrl);

        await ShouldBeAnsweredAsync(await GetUserTokenAsync("AgentTools"), OkIfAnyAudienceIsAccepted, OkIfAnyAudienceIsAccepted);
    }
}

public class AgentToolsScopeConfinementTurnedOffWithAudiencesTests : AgentToolsScopeConfinementTurnedOffTestBase;

public class AgentToolsScopeConfinementTurnedOffWithoutAudiencesTests : AgentToolsScopeConfinementTurnedOffTestBase
{
    protected override bool AcceptsAnyAudience => true;
}
