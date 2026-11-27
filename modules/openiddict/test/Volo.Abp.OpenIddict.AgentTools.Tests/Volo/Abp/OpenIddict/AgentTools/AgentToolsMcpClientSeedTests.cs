using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>Clients from configuration; the secrets are generated per test and exist only in this process.</summary>
public class AgentToolsMcpClientSeedTests : AgentToolsOpenIddictTestBase
{
    private const string Section = AbpAgentToolsOpenIddictOptions.ClientsConfigurationSection;
    private const string ClaudeCodeRedirect = "http://localhost:33418/callback";
    private const string ServerAgentRedirect = "https://agent.test/callback";

    private readonly string _serverAgentSecret = Guid.NewGuid().ToString();
    private readonly string _publicClientSecret = Guid.NewGuid().ToString();

    protected override Dictionary<string, string?> Configuration => new()
    {
        ["App:SelfUrl"] = SelfUrl,
        [$"{Section}:claude-code:DisplayName"] = "Claude Code",
        [$"{Section}:claude-code:RedirectUris:0"] = ClaudeCodeRedirect,
        [$"{Section}:server-agent:Type"] = "Confidential",
        [$"{Section}:server-agent:ClientSecret"] = _serverAgentSecret,
        [$"{Section}:server-agent:RedirectUris:0"] = ServerAgentRedirect,
        [$"{Section}:no-secret:Type"] = "confidential",
        [$"{Section}:no-secret:RedirectUris:0"] = ServerAgentRedirect,
        [$"{Section}:public-with-secret:ClientSecret"] = _publicClientSecret,
        [$"{Section}:public-with-secret:RedirectUris:0"] = ClaudeCodeRedirect,
        [$"{Section}:placeholder:Type"] = "confidential",
        [$"{Section}:placeholder:ClientSecret"] = PlaceholderSecret,
        [$"{Section}:placeholder:RedirectUris:0"] = ServerAgentRedirect,
        [$"{Section}:bad-consent:ConsentType"] = "always",
        [$"{Section}:bad-consent:RedirectUris:0"] = ClaudeCodeRedirect,
        [$"{Section}:implicit-consent:ConsentType"] = "Implicit",
        [$"{Section}:implicit-consent:RedirectUris:0"] = ClaudeCodeRedirect
    };

    private const string PlaceholderSecret = "<from user secrets>";

    [Fact]
    public async Task Public_And_Confidential_Clients_Are_Created_With_Pkce_Explicit_Consent_The_Scope_And_The_Resource_Permission()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        string[] expectedPermissions =
        [
            Permissions.Endpoints.Authorization, Permissions.Endpoints.Token, Permissions.Endpoints.Revocation,
            Permissions.GrantTypes.AuthorizationCode, Permissions.GrantTypes.RefreshToken, Permissions.ResponseTypes.Code,
            "scp:AgentTools", "rsrc:" + McpUrl // Permissions.Prefixes.Resource is "rsrc:"; "rst:" is the response type prefix
        ];

        var claudeCode = await ReadClientAsync("claude-code");
        claudeCode.Type.ShouldBe(ClientTypes.Public);
        claudeCode.DisplayName.ShouldBe("Claude Code");
        claudeCode.ConsentType.ShouldBe(ConsentTypes.Explicit);
        claudeCode.RedirectUris.ShouldBe([ClaudeCodeRedirect]);
        claudeCode.Permissions.ShouldBe(expectedPermissions, ignoreOrder: true);
        claudeCode.Requirements.ShouldBe([Requirements.Features.ProofKeyForCodeExchange]);
        claudeCode.SecretValid.ShouldBeFalse();

        var serverAgent = await ReadClientAsync("server-agent");
        serverAgent.Type.ShouldBe(ClientTypes.Confidential);
        serverAgent.DisplayName.ShouldBe("server-agent");
        serverAgent.ConsentType.ShouldBe(ConsentTypes.Explicit);
        serverAgent.RedirectUris.ShouldBe([ServerAgentRedirect]);
        serverAgent.Permissions.ShouldBe(expectedPermissions, ignoreOrder: true);
        serverAgent.Requirements.ShouldBe([Requirements.Features.ProofKeyForCodeExchange]);
        serverAgent.SecretValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Confidential_Client_Without_A_Secret_Is_Skipped_And_Reported_Without_Logging_A_Secret()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        (await FindClientAsync("no-secret")).ShouldBeFalse();
        WarningsOf<AgentToolsMcpClientSeedContributor>()
            .ShouldContain(m => m.Contains("no-secret") && m.Contains("confidential client needs ClientSecret"));
        ShouldNotHaveLoggedASecret();
    }

    [Fact]
    public async Task Public_Client_With_A_Secret_Is_Skipped_And_Reported_Without_Logging_The_Secret()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        (await FindClientAsync("public-with-secret")).ShouldBeFalse();
        WarningsOf<AgentToolsMcpClientSeedContributor>()
            .ShouldContain(m => m.Contains("public-with-secret") && m.Contains("public client must not have a ClientSecret"));
        ShouldNotHaveLoggedASecret();
    }

    [Fact]
    public async Task Client_With_An_Unreplaced_Placeholder_Secret_Is_Skipped_And_Reported_Without_Logging_The_Value()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        (await FindClientAsync("placeholder")).ShouldBeFalse();
        WarningsOf<AgentToolsMcpClientSeedContributor>()
            .ShouldContain(m => m.Contains("placeholder") && m.Contains("unreplaced placeholder"));
        Logs.Entries.ShouldAllBe(e => !e.Message.Contains(PlaceholderSecret) && !e.Values.Contains(PlaceholderSecret));
    }

    [Fact]
    public async Task Client_With_An_Unknown_Consent_Type_Is_Skipped_And_A_Known_One_Is_Accepted_Case_Insensitively()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        (await FindClientAsync("bad-consent")).ShouldBeFalse();
        WarningsOf<AgentToolsMcpClientSeedContributor>()
            .ShouldContain(m => m.Contains("bad-consent") && m.Contains("ConsentType must be"));
        (await ReadClientAsync("implicit-consent")).ConsentType.ShouldBe(ConsentTypes.Implicit);
    }

    [Fact]
    public async Task Seeding_Again_Leaves_Existing_Clients_Alone_And_No_Log_Entry_Ever_Contains_A_Secret()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        var messages = Logs.Entries.Where(e => e.Category == typeof(AgentToolsMcpClientSeedContributor).FullName).Select(e => e.Message).ToList();
        messages.Count(m => m.Contains("created the confidential MCP client server-agent")).ShouldBe(1);
        messages.ShouldContain(m => m.Contains("server-agent already exists"));
        (await ReadClientAsync("server-agent")).SecretValid.ShouldBeTrue();
        ShouldNotHaveLoggedASecret();
    }

    private void ShouldNotHaveLoggedASecret()
    {
        Logs.Entries.ShouldNotBeEmpty();
        foreach (var entry in Logs.Entries)
        {
            foreach (var secret in new[] { _serverAgentSecret, _publicClientSecret })
            {
                entry.Message.ShouldNotContain(secret);
                entry.Values.ShouldNotContain(secret);
            }
        }
    }

    private Task<bool> FindClientAsync(string clientId) => WithUnitOfWorkAsync(async sp =>
        await sp.GetRequiredService<IOpenIddictApplicationManager>().FindByClientIdAsync(clientId) != null);

    private record ClientInfo(string? Type, string? DisplayName, string? ConsentType, string[] RedirectUris, string[] Permissions,
        string[] Requirements, bool SecretValid);

    private Task<ClientInfo> ReadClientAsync(string clientId) => WithUnitOfWorkAsync(async sp =>
    {
        var manager = sp.GetRequiredService<IOpenIddictApplicationManager>();
        var app = await manager.FindByClientIdAsync(clientId);
        app.ShouldNotBeNull();
        return new ClientInfo(
            await manager.GetClientTypeAsync(app),
            await manager.GetDisplayNameAsync(app),
            await manager.GetConsentTypeAsync(app),
            (await manager.GetRedirectUrisAsync(app)).ToArray(),
            (await manager.GetPermissionsAsync(app)).ToArray(),
            (await manager.GetRequirementsAsync(app)).ToArray(),
            await manager.HasClientTypeAsync(app, ClientTypes.Confidential) && await manager.ValidateClientSecretAsync(app, _serverAgentSecret));
    });
}

public class AgentToolsMcpClientSeedWithoutClientsTests : AgentToolsOpenIddictTestBase
{
    [Fact]
    public async Task No_Clients_Configured_Creates_Nothing()
    {
        await SeedAsync<AgentToolsMcpClientSeedContributor>();

        (await WithUnitOfWorkAsync(sp => sp.GetRequiredService<IOpenIddictApplicationManager>().CountAsync().AsTask())).ShouldBe(0);
    }
}
