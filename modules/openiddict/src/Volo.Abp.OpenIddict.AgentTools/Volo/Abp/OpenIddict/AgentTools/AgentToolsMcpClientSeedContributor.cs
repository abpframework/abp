using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Creates an OpenIddict application per entry of the <c>AgentTools:Mcp:Clients</c> configuration section (keyed by client id):
/// authorization code + refresh token, PKCE required, response type code, the MCP scope and the <c>rsrc:</c> (resource) permission for the MCP
/// endpoint. Does nothing without that section. An existing client id is left unchanged. The secret of a confidential client comes
/// only from configuration (user secrets, environment, a vault provider) and is never logged; a client whose settings are
/// incomplete is skipped with a warning.
/// </summary>
public class AgentToolsMcpClientSeedContributor : IDataSeedContributor, ITransientDependency
{
    public ILogger<AgentToolsMcpClientSeedContributor> Logger { get; set; } = NullLogger<AgentToolsMcpClientSeedContributor>.Instance;

    protected IConfiguration Configuration { get; }

    protected IOpenIddictApplicationManager ApplicationManager { get; }

    protected AbpAgentToolsOpenIddictOptions Options { get; }

    public AgentToolsMcpClientSeedContributor(
        IConfiguration configuration,
        IOpenIddictApplicationManager applicationManager,
        IOptions<AbpAgentToolsOpenIddictOptions> options)
    {
        Configuration = configuration;
        ApplicationManager = applicationManager;
        Options = options.Value;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId != null)
        {
            return;
        }

        foreach (var section in Configuration.GetSection(AbpAgentToolsOpenIddictOptions.ClientsConfigurationSection).GetChildren())
        {
            await SeedClientAsync(section.Key, section.Get<AgentToolsMcpClientConfiguration>() ?? new AgentToolsMcpClientConfiguration());
        }
    }

    protected virtual async Task SeedClientAsync(string clientId, AgentToolsMcpClientConfiguration client)
    {
        if (await ApplicationManager.FindByClientIdAsync(clientId) != null)
        {
            Logger.LogInformation("Agent tools: the MCP client {ClientId} already exists; left unchanged.", clientId);
            return;
        }

        var problem = FindProblem(client, out var type, out var redirectUris);
        if (problem != null)
        {
            Logger.LogWarning("Agent tools: the MCP client {ClientId} from configuration was not created: {Problem}", clientId, problem);
            return;
        }

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientType = type,
            ClientSecret = type == ClientTypes.Confidential ? client.ClientSecret : null,
            ConsentType = client.ConsentType?.ToLowerInvariant() ?? ConsentTypes.Explicit,
            DisplayName = client.DisplayName ?? clientId,
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.Endpoints.Revocation,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Prefixes.Scope + Options.ScopeName
            },
            Requirements = { Requirements.Features.ProofKeyForCodeExchange }
        };
        descriptor.RedirectUris.UnionWith(redirectUris);
        if (Options.McpResource != null)
        {
            descriptor.Permissions.Add(Permissions.Prefixes.Resource + Options.McpResource);
        }

        await ApplicationManager.CreateAsync(descriptor);
        Logger.LogInformation("Agent tools: created the {Type} MCP client {ClientId}.", type, clientId);
    }

    /// <summary>Null when the client can be created. The message never contains the secret.</summary>
    protected virtual string? FindProblem(AgentToolsMcpClientConfiguration client, out string type, out List<Uri> redirectUris)
    {
        type = (client.Type ?? ClientTypes.Public).ToLowerInvariant();
        redirectUris = new List<Uri>();
        var hasSecret = !string.IsNullOrEmpty(client.ClientSecret);

        if (type != ClientTypes.Public && type != ClientTypes.Confidential)
        {
            return "Type must be \"public\" or \"confidential\".";
        }

        if (client.ConsentType != null && client.ConsentType.ToLowerInvariant() is not
                (ConsentTypes.Explicit or ConsentTypes.External or ConsentTypes.Implicit or ConsentTypes.Systematic))
        {
            return "ConsentType must be \"explicit\", \"external\", \"implicit\" or \"systematic\".";
        }

        if (hasSecret && client.ClientSecret!.StartsWith('<') && client.ClientSecret.EndsWith('>'))
        {
            return "ClientSecret is an unreplaced placeholder (\"<...>\"); set the real secret in user secrets or an environment variable.";
        }

        if (type == ClientTypes.Confidential && !hasSecret)
        {
            return "a confidential client needs ClientSecret; set it in user secrets, an environment variable or a vault, never in appsettings.json.";
        }

        if (type == ClientTypes.Public && hasSecret)
        {
            return "a public client must not have a ClientSecret; remove it or set Type to \"confidential\".";
        }

        foreach (var value in client.RedirectUris ?? [])
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            {
                return $"the redirect URI \"{value}\" is not an absolute URI.";
            }

            redirectUris.Add(uri);
        }

        return redirectUris.Count == 0 ? "RedirectUris is empty; the authorization code flow needs at least one (exact match)." : null;
    }
}

/// <summary>One entry of <c>AgentTools:Mcp:Clients</c>; the entry's key is the client id.</summary>
public class AgentToolsMcpClientConfiguration
{
    /// <summary>Shown on the consent page. Default: the client id.</summary>
    public string? DisplayName { get; set; }

    /// <summary>"public" (default; desktop and CLI agents such as Claude Code) or "confidential" (server-side agents).</summary>
    public string? Type { get; set; }

    /// <summary>Confidential clients only. From user secrets, an environment variable or a vault; never in appsettings.json.</summary>
    public string? ClientSecret { get; set; }

    /// <summary>Absolute redirect URIs, matched exactly (port included). At least one.</summary>
    public string[]? RedirectUris { get; set; }

    /// <summary>OpenIddict consent type. Default "explicit": the user approves the client once on the consent page.</summary>
    public string? ConsentType { get; set; }
}
