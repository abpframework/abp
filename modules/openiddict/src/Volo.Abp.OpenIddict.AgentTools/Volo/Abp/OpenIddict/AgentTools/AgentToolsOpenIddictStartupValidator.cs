using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Validation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// Runs at application initialization of a web host. Throws when <see cref="AbpAgentToolsOpenIddictOptions.SyncMcpResourcePermission"/>
/// is on but the MCP URL is unknown; otherwise logs warnings when the MCP endpoint's OpenIddict setup is incomplete: the scope is
/// missing, the endpoint is not a registered resource, or the scope's resources (the token audience) are not accepted by this
/// host's OpenIddict validation; and an error when the scope should have the MCP URL as a resource but does not.
/// </summary>
public class AgentToolsOpenIddictStartupValidator : ITransientDependency
{
    public const string McpResourceRequiredMessage =
        "Agent tools: AbpAgentToolsOpenIddictOptions.SyncMcpResourcePermission is on, but the MCP endpoint's URL is unknown, so " +
        "OpenIddict applications with the MCP scope cannot get the rsrc: (resource) permission that MCP clients sending " +
        "resource=<endpoint URL> (RFC 8707, e.g. Claude Code) need. Set App:SelfUrl or AbpAgentToolsOpenIddictOptions.McpResource " +
        "to the endpoint's absolute URL, or set AbpAgentToolsOpenIddictOptions.SyncMcpResourcePermission to false.";

    public ILogger<AgentToolsOpenIddictStartupValidator> Logger { get; set; } = NullLogger<AgentToolsOpenIddictStartupValidator>.Instance;

    protected IServiceProvider ServiceProvider { get; }

    protected AbpAgentToolsOpenIddictOptions Options { get; }

    public AgentToolsOpenIddictStartupValidator(IServiceProvider serviceProvider, IOptions<AbpAgentToolsOpenIddictOptions> options)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
    }

    public virtual async Task ValidateAsync()
    {
        if (Options.SyncMcpResourcePermission && Options.McpResource == null)
        {
            throw new AbpException(McpResourceRequiredMessage);
        }

        try
        {
            CheckResource();
            await CheckScopeAsync();
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Agent tools: could not check the OpenIddict setup of the MCP endpoint (is the database ready?).");
        }
    }

    protected virtual void CheckResource()
    {
        if (Options.McpResource == null)
        {
            Logger.LogWarning(
                "Agent tools: the MCP endpoint's URL is unknown, so it is not registered as an OpenIddict resource and MCP clients that " +
                "send resource=<endpoint URL> (RFC 8707, e.g. Claude Code) get invalid_target. Set App:SelfUrl or " +
                "AbpAgentToolsOpenIddictOptions.McpResource to the endpoint's absolute URL.");
            return;
        }

        var server = ServiceProvider.GetRequiredService<IOptions<OpenIddictServerOptions>>().Value;
        if (!Uri.TryCreate(Options.McpResource, UriKind.Absolute, out var resource) || !server.Resources.Contains(resource))
        {
            Logger.LogWarning(
                "Agent tools: the MCP endpoint {Resource} is not a registered OpenIddict resource, so MCP clients that send it as " +
                "resource (RFC 8707) get invalid_target. Register it with OpenIddictServerBuilder.RegisterResources or stop removing it " +
                "from OpenIddictServerOptions.Resources.", Options.McpResource);
        }
    }

    protected virtual async Task CheckScopeAsync()
    {
        using var uow = ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true, isTransactional: false);
        var scopeManager = ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        var scope = await scopeManager.FindByNameAsync(Options.ScopeName);
        if (scope == null)
        {
            Logger.LogWarning(
                "Agent tools: the OpenIddict scope {Scope} does not exist, so MCP clients cannot request it and the MCP endpoint refuses " +
                "their tokens. Run the data seeder with AbpOpenIddictAgentToolsModule loaded (e.g. in the DbMigrator), or create the scope " +
                "with the resource(s) {Resources}.", Options.ScopeName, string.Join(", ", Options.GetScopeResources()));
            await uow.CompleteAsync();
            return;
        }

        var audiences = ServiceProvider.GetRequiredService<IOptions<OpenIddictValidationOptions>>().Value.Audiences;
        var resources = await scopeManager.GetResourcesAsync(scope);
        if (Options.McpResource != null && Options.GetScopeResources().Contains(Options.McpResource) && !resources.Contains(Options.McpResource))
        {
            Logger.LogError(
                "Agent tools: the OpenIddict scope {Scope} has the resources [{Resources}], which do not include the MCP endpoint {McpResource}. " +
                "ABP takes a token's audience from its scopes' resources, so the tokens of this scope are refused, or kept at the MCP " +
                "endpoint by their scopes only (ConfineScopeToMcpEndpoint), not by their audience. The data seeder does not change an " +
                "existing scope. Update the scope's resources: add this URL and remove a former URL of the endpoint; or, if the stored " +
                "resources are intended, set McpResource / ScopeResources of AbpAgentToolsOpenIddictOptions to match them. Tokens issued " +
                "before, refresh tokens included, keep their audience: users sign in to their MCP clients again, or revoke the tokens " +
                "individually (e.g. IOpenIddictTokenManager.TryRevokeAsync).",
                Options.ScopeName, string.Join(", ", resources), Options.McpResource);
        }
        else if (audiences.Count > 0 && !resources.Any(audiences.Contains))
        {
            Logger.LogWarning(
                "Agent tools: the OpenIddict scope {Scope} has the resources [{Resources}], none of which is an audience this host's " +
                "OpenIddict validation accepts [{Audiences}]. ABP takes a token's audience from its scopes' resources, so a token that " +
                "carries only this scope is rejected (401) before the MCP endpoint runs. Add one of the accepted audiences to the scope's " +
                "resources.", Options.ScopeName, string.Join(", ", resources), string.Join(", ", audiences));
        }

        await uow.CompleteAsync();
    }
}
