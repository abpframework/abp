using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Shouldly;
using Volo.Abp.Data;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Applications created and updated through IOpenIddictApplicationManager.
/// </summary>
public abstract class AgentToolsMcpResourcePermissionTestBase : AgentToolsOpenIddictTestBase
{
    protected const string ClaudeCodeRedirect = "http://localhost:33418/callback";
    protected const string OtherResource = "rsrc:https://other.test/api";
    protected const string McpPermission = "rsrc:" + McpUrl; // Permissions.Prefixes.Resource

    /// <summary>A public MCP client (authorization code and refresh token, PKCE required) with <paramref name="scopes"/>.</summary>
    protected Task CreateMcpClientAsync(string clientId, params string[] scopes) => WithUnitOfWorkAsync(async sp =>
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ApplicationType = ApplicationTypes.Web,
            ClientId = clientId,
            DisplayName = clientId,
            ClientType = ClientTypes.Public,
            ConsentType = ConsentTypes.Explicit,
            RedirectUris = { new Uri(ClaudeCodeRedirect) },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code
            },
            Requirements = { Requirements.Features.ProofKeyForCodeExchange }
        };
        descriptor.Permissions.UnionWith(scopes.Select(s => Permissions.Prefixes.Scope + s));
        await sp.GetRequiredService<IOpenIddictApplicationManager>().CreateAsync(descriptor);
    });

    /// <summary>Replaces the scopes of the application.</summary>
    protected Task SetScopesAsync(string clientId, params string[] scopes) => ManagerUpdateAsync(clientId, permissions =>
    {
        permissions.RemoveWhere(p => p.StartsWith(Permissions.Prefixes.Scope));
        permissions.UnionWith(scopes.Select(s => Permissions.Prefixes.Scope + s));
    });

    protected Task ManagerCreateAsync(string clientId, params string[] permissions) => WithUnitOfWorkAsync(async sp =>
    {
        var descriptor = new OpenIddictApplicationDescriptor { ClientId = clientId, ClientType = ClientTypes.Public };
        descriptor.Permissions.UnionWith(permissions);
        await sp.GetRequiredService<IOpenIddictApplicationManager>().CreateAsync(descriptor);
    });

    /// <summary>PopulateAsync, then UpdateAsync(application, descriptor), the way a management UI updates.</summary>
    protected Task ManagerUpdateAsync(string clientId, Action<HashSet<string>> changePermissions) => WithUnitOfWorkAsync(async sp =>
    {
        var manager = sp.GetRequiredService<IOpenIddictApplicationManager>();
        var application = (await manager.FindByClientIdAsync(clientId))!;
        var descriptor = new OpenIddictApplicationDescriptor();
        await manager.PopulateAsync(descriptor, application);
        changePermissions(descriptor.Permissions);
        await manager.UpdateAsync(application, descriptor);
    });

    protected Task<string[]> PermissionsOfAsync(string clientId) => WithUnitOfWorkAsync(async sp =>
    {
        var manager = sp.GetRequiredService<IOpenIddictApplicationManager>();
        var application = await manager.FindByClientIdAsync(clientId);
        application.ShouldNotBeNull();
        return (await manager.GetPermissionsAsync(application)).ToArray();
    });
}

public class AgentToolsMcpResourcePermissionSyncTests : AgentToolsMcpResourcePermissionTestBase
{
    [Fact]
    public async Task Create_With_The_Mcp_Scope_Adds_The_Mcp_Resource_Permission_Once()
    {
        await CreateMcpClientAsync("claude-code", "AgentTools", OpenIddictConstants.Scopes.OfflineAccess);

        var permissions = await PermissionsOfAsync("claude-code");
        permissions.Count(p => p == McpPermission).ShouldBe(1);
        permissions.ShouldContain("scp:AgentTools");
        permissions.ShouldContain("scp:offline_access");
    }

    [Fact]
    public async Task Create_Without_The_Mcp_Scope_Adds_No_Resource_Permission()
    {
        await CreateMcpClientAsync("web-app", OpenIddictConstants.Scopes.OfflineAccess);

        (await PermissionsOfAsync("web-app")).ShouldNotContain(p => p.StartsWith(Permissions.Prefixes.Resource));
    }

    [Fact]
    public async Task Update_Removing_The_Mcp_Scope_Removes_Only_The_Mcp_Resource_Permission_And_Adding_It_Back_Restores_It()
    {
        await CreateMcpClientAsync("claude-code", "AgentTools", OpenIddictConstants.Scopes.OfflineAccess);
        var before = await PermissionsOfAsync("claude-code");

        await SetScopesAsync("claude-code", OpenIddictConstants.Scopes.OfflineAccess);
        (await PermissionsOfAsync("claude-code")).ShouldBe(before.Except(["scp:AgentTools", McpPermission]), ignoreOrder: true);

        await SetScopesAsync("claude-code", "AgentTools", OpenIddictConstants.Scopes.OfflineAccess);
        (await PermissionsOfAsync("claude-code")).ShouldBe(before, ignoreOrder: true);
    }

    [Fact]
    public async Task An_Unrelated_Resource_Permission_Survives_An_Update_That_Removes_The_Mcp_Scope()
    {
        await CreateMcpClientAsync("claude-code", "AgentTools", OpenIddictConstants.Scopes.OfflineAccess);
        await ManagerUpdateAsync("claude-code", permissions => permissions.Add(OtherResource));

        await SetScopesAsync("claude-code", OpenIddictConstants.Scopes.OfflineAccess);

        var permissions = await PermissionsOfAsync("claude-code");
        permissions.ShouldContain(OtherResource);
        permissions.ShouldNotContain(McpPermission);
    }

    [Fact]
    public async Task Direct_Manager_Create_And_Update_Keep_The_Mcp_Resource_Permission_In_Step_And_Leave_Other_Permissions_Alone()
    {
        await ManagerCreateAsync("agent", Permissions.Endpoints.Token, "scp:AgentTools", OtherResource);
        (await PermissionsOfAsync("agent")).ShouldBe([Permissions.Endpoints.Token, "scp:AgentTools", OtherResource, McpPermission], ignoreOrder: true);

        await ManagerUpdateAsync("agent", permissions => permissions.Remove("scp:AgentTools"));
        (await PermissionsOfAsync("agent")).ShouldBe([Permissions.Endpoints.Token, OtherResource], ignoreOrder: true);

        await ManagerUpdateAsync("agent", permissions => permissions.Add("scp:AgentTools"));
        (await PermissionsOfAsync("agent")).ShouldBe([Permissions.Endpoints.Token, "scp:AgentTools", OtherResource, McpPermission], ignoreOrder: true);
    }

    [Fact]
    public async Task Direct_Manager_Create_Of_A_Confidential_Client_With_A_Secret_Gets_The_Mcp_Resource_Permission()
    {
        // CreateAsync(descriptor) with a secret takes the CreateAsync(application, secret) overload.
        await WithUnitOfWorkAsync(async sp =>
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = "server-agent",
                ClientType = ClientTypes.Confidential,
                ClientSecret = Guid.NewGuid().ToString(),
                Permissions = { Permissions.Endpoints.Token, "scp:AgentTools" }
            };
            await sp.GetRequiredService<IOpenIddictApplicationManager>().CreateAsync(descriptor);
        });

        (await PermissionsOfAsync("server-agent")).ShouldContain(McpPermission);
    }

    [Fact]
    public async Task The_Manager_Ignores_Applications_With_Only_A_Different_Resource()
    {
        await ManagerCreateAsync("api", Permissions.Endpoints.Token, OtherResource);

        (await PermissionsOfAsync("api")).ShouldBe([Permissions.Endpoints.Token, OtherResource], ignoreOrder: true);
    }
}

public class AgentToolsMcpResourcePermissionReconciliationTests : AgentToolsMcpResourcePermissionTestBase
{
    [Fact]
    public async Task Seeding_Adds_The_Missing_And_Removes_The_Orphaned_Permission_Logging_Only_Those_Client_Ids()
    {
        await CreateClientsSavedBeforeTheSyncAsync();
        Logs.Clear();

        await SeedAsync<AgentToolsMcpResourcePermissionDataSeedContributor>();

        (await PermissionsOfAsync("missing")).ShouldBe(["scp:AgentTools", McpPermission], ignoreOrder: true);
        (await PermissionsOfAsync("orphaned")).ShouldBe([Permissions.Endpoints.Token, OtherResource], ignoreOrder: true);
        (await PermissionsOfAsync("unrelated")).ShouldBe([OtherResource]);
        (await PermissionsOfAsync("in-step")).ShouldBe(["scp:AgentTools", McpPermission], ignoreOrder: true);

        var entries = Logs.Entries.Where(e => e.Category == typeof(AgentToolsMcpResourcePermissionDataSeedContributor).FullName).ToList();
        entries.Select(e => e.Message).ShouldBe(
        [
            "Agent tools: added the MCP resource permission to the OpenIddict application missing, which has the MCP scope.",
            "Agent tools: removed the MCP resource permission from the OpenIddict application orphaned, which no longer has the MCP scope."
        ], ignoreOrder: true);
        entries.ShouldAllBe(e => !e.Values.Contains("unrelated") && !e.Values.Contains("in-step"));
    }

    [Fact]
    public async Task Seeding_Again_Changes_And_Logs_Nothing()
    {
        await CreateClientsSavedBeforeTheSyncAsync();
        await SeedAsync<AgentToolsMcpResourcePermissionDataSeedContributor>();
        Logs.Clear();

        await SeedAsync<AgentToolsMcpResourcePermissionDataSeedContributor>();

        Logs.Entries.ShouldNotContain(e => e.Category == typeof(AgentToolsMcpResourcePermissionDataSeedContributor).FullName);
    }

    [Fact]
    public async Task Seeding_A_Tenant_Leaves_The_Host_Applications_Alone()
    {
        await CreateClientsSavedBeforeTheSyncAsync();

        await WithUnitOfWorkAsync(sp => sp.GetRequiredService<AgentToolsMcpResourcePermissionDataSeedContributor>()
            .SeedAsync(new DataSeedContext(Guid.NewGuid())));

        (await PermissionsOfAsync("missing")).ShouldBe(["scp:AgentTools"]);
    }

    /// <summary>Applications as they were stored before the sync existed (it is off while they are created).</summary>
    private async Task CreateClientsSavedBeforeTheSyncAsync()
    {
        var options = GetRequiredService<IOptions<AbpAgentToolsOpenIddictOptions>>().Value;
        options.SyncMcpResourcePermission = false;
        await ManagerCreateAsync("missing", "scp:AgentTools");
        await ManagerCreateAsync("orphaned", Permissions.Endpoints.Token, McpPermission, OtherResource);
        await ManagerCreateAsync("unrelated", OtherResource);
        await ManagerCreateAsync("in-step", "scp:AgentTools", McpPermission);
        options.SyncMcpResourcePermission = true;
    }
}

public class AgentToolsMcpResourcePermissionCustomScopeTests : AgentToolsMcpResourcePermissionTestBase
{
    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.ScopeName = "Agents";

    [Fact]
    public async Task The_Configured_Scope_Name_Decides()
    {
        await CreateMcpClientAsync("custom", "Agents", OpenIddictConstants.Scopes.OfflineAccess);
        await CreateMcpClientAsync("default-name", "AgentTools", OpenIddictConstants.Scopes.OfflineAccess);

        (await PermissionsOfAsync("custom")).ShouldContain(McpPermission);
        (await PermissionsOfAsync("default-name")).ShouldNotContain(McpPermission);
    }
}

public class AgentToolsMcpResourcePermissionSyncOffTests : AgentToolsMcpResourcePermissionTestBase
{
    protected override Dictionary<string, string?> Configuration => new()
    {
        ["App:SelfUrl"] = SelfUrl,
        [$"{AbpAgentToolsOpenIddictOptions.ClientsConfigurationSection}:seeded:RedirectUris:0"] = ClaudeCodeRedirect
    };

    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.SyncMcpResourcePermission = false;

    [Fact]
    public async Task Nothing_Is_Added_Or_Removed_Neither_On_Save_Nor_On_Seeding()
    {
        await CreateMcpClientAsync("claude-code", "AgentTools", OpenIddictConstants.Scopes.OfflineAccess);
        await ManagerCreateAsync("orphaned", Permissions.Endpoints.Token, McpPermission);

        await SeedAsync<AgentToolsMcpResourcePermissionDataSeedContributor>();

        (await PermissionsOfAsync("claude-code")).ShouldNotContain(McpPermission);
        (await PermissionsOfAsync("orphaned")).ShouldBe([Permissions.Endpoints.Token, McpPermission], ignoreOrder: true);
        Logs.Entries.ShouldNotContain(e => e.Category == typeof(AgentToolsMcpResourcePermissionDataSeedContributor).FullName);
    }

    [Fact]
    public async Task The_Client_Seed_Still_Grants_The_Mcp_Resource_Permission()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        (await PermissionsOfAsync("seeded")).ShouldContain(McpPermission);
    }
}

/// <summary>
/// The authorization code flow of an MCP client, against the real OpenIddict server: the
/// authorization request and the token request both send resource=&lt;MCP URL&gt;, so OpenIddict's resource permission checks
/// (authorization and token endpoint) run. Login and consent are ABP pages backed by Identity, which this host does not have, so
/// a test handler signs a user in at the authorization endpoint (resources taken from the granted scopes, as ABP's
/// AuthorizeController does) and the token endpoint issues tokens for the code's principal (no pass-through to ABP's controllers).
/// </summary>
public abstract class AgentToolsMcpClientCodeFlowTestBase : AgentToolsMcpResourcePermissionTestBase
{
    protected readonly string CodeVerifier = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.PostConfigure<OpenIddictServerAspNetCoreOptions>(options =>
        {
            options.EnableAuthorizationEndpointPassthrough = false;
            options.EnableTokenEndpointPassthrough = false;
        });
        services.AddOpenIddict().AddServer(server => server.AddEventHandler<OpenIddictServerEvents.HandleAuthorizationRequestContext>(handler =>
            handler.UseInlineHandler(async context =>
            {
                var identity = new ClaimsIdentity("Test", Claims.Name, Claims.Role);
                identity.SetClaim(Claims.Subject, "test-user");
                var principal = new ClaimsPrincipal(identity);
                principal.SetScopes(context.Request.GetScopes());
                // As ABP's AuthorizeController does: the resources (token audiences) are those of the granted scopes.
                var scopeManager = context.Transaction.GetHttpRequest()!.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>();
                var resources = new List<string>();
                await foreach (var resource in scopeManager.ListResourcesAsync(principal.GetScopes()))
                {
                    resources.Add(resource);
                }

                principal.SetResources(resources);
                context.SignIn(principal);
            })));
    }

    /// <summary>The client "claude-code" (scopes AgentTools and offline_access), then the authorization request.</summary>
    protected async Task<HttpResponseMessage> AuthorizeAsync()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await CreateMcpClientAsync("claude-code", "AgentTools", OpenIddictConstants.Scopes.OfflineAccess);

        return await Client.GetAsync(QueryHelpers.AddQueryString("/connect/authorize", new Dictionary<string, string?>
        {
            ["client_id"] = "claude-code",
            ["response_type"] = ResponseTypes.Code,
            ["redirect_uri"] = ClaudeCodeRedirect,
            ["scope"] = "AgentTools offline_access",
            ["resource"] = McpUrl,
            ["code_challenge"] = WebEncoders.Base64UrlEncode(SHA256.HashData(Encoding.ASCII.GetBytes(CodeVerifier))),
            ["code_challenge_method"] = CodeChallengeMethods.Sha256,
            ["state"] = "test-state"
        }));
    }
}

public class AgentToolsMcpClientCodeFlowTests : AgentToolsMcpClientCodeFlowTestBase
{
    [Fact]
    public async Task An_Mcp_Client_Gets_A_Code_And_Tokens_For_The_Mcp_Resource()
    {
        var authorization = await AuthorizeAsync();
        authorization.StatusCode.ShouldBe(HttpStatusCode.Redirect, await authorization.Content.ReadAsStringAsync());
        var location = authorization.Headers.Location!;
        location.GetLeftPart(UriPartial.Path).ShouldBe(ClaudeCodeRedirect);
        var query = QueryHelpers.ParseQuery(location.Query);
        query.ContainsKey("error").ShouldBeFalse(query.GetValueOrDefault("error_description").ToString());

        var response = await Client.PostAsync("/connect/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = GrantTypes.AuthorizationCode,
            ["client_id"] = "claude-code",
            ["code"] = query["code"].ToString(),
            ["redirect_uri"] = ClaudeCodeRedirect,
            ["code_verifier"] = CodeVerifier,
            ["resource"] = McpUrl
        }));
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();

        response.IsSuccessStatusCode.ShouldBeTrue(body.ToString());
        body.TryGetProperty("refresh_token", out _).ShouldBeTrue(body.ToString());
        // The audience comes from the seeded AgentTools scope's resources (the module's default: the MCP URL).
        new JsonWebToken(body.GetProperty("access_token").GetString()).Audiences.ShouldBe([McpUrl]);
    }
}

public class AgentToolsMcpClientCodeFlowWithoutTheSyncTests : AgentToolsMcpClientCodeFlowTestBase
{
    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.SyncMcpResourcePermission = false;

    [Fact]
    public async Task Without_The_Resource_Permission_OpenIddict_Refuses_The_Mcp_Resource()
    {
        // The control: the same client without the rsrc: permission is refused by OpenIddict's resource permission check
        // (ID2192), on the server's error page: no code reaches the client.
        var response = await AuthorizeAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.ShouldContain(Errors.InvalidRequest);
        body.ShouldContain("ID2192");
    }
}
