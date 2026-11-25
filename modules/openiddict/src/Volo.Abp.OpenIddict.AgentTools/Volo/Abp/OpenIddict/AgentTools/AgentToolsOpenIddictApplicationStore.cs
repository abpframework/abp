using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// ABP's OpenIddict application store, plus <see cref="AbpAgentToolsOpenIddictOptions.SyncMcpResourcePermission"/>: right before
/// an application is inserted or updated, an application holding the scope permission (<c>scp:</c> +
/// <see cref="AbpAgentToolsOpenIddictOptions.ScopeName"/>) gets the <c>rsrc:</c> permission for
/// <see cref="AbpAgentToolsOpenIddictOptions.McpResource"/>, and one without it loses that permission (only that one). Every
/// create and update goes through here: data seeders, management UIs and any other use of IOpenIddictApplicationManager
/// (all its CreateAsync and UpdateAsync overloads end in this store's CreateAsync or UpdateAsync).
/// </summary>
[Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
[ExposeServices(typeof(IOpenIddictApplicationStore<OpenIddictApplicationModel>))]
public class AgentToolsOpenIddictApplicationStore : AbpOpenIddictApplicationStore
{
    protected AbpAgentToolsOpenIddictOptions AgentToolsOptions { get; }

    public AgentToolsOpenIddictApplicationStore(
        IOpenIddictApplicationRepository repository,
        IUnitOfWorkManager unitOfWorkManager,
        IOpenIddictTokenRepository tokenRepository,
        IGuidGenerator guidGenerator,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IOpenIddictDbConcurrencyExceptionHandler concurrencyExceptionHandler,
        IOptions<AbpOpenIddictStoreOptions> storeOptions,
        IOptions<AbpAgentToolsOpenIddictOptions> agentToolsOptions)
        : base(repository, unitOfWorkManager, tokenRepository, guidGenerator, identifierConverter, concurrencyExceptionHandler, storeOptions)
    {
        AgentToolsOptions = agentToolsOptions.Value;
    }

    public override async ValueTask CreateAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        await SyncMcpResourcePermissionAsync(application, cancellationToken);
        await base.CreateAsync(application, cancellationToken);
    }

    public override async ValueTask UpdateAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        await SyncMcpResourcePermissionAsync(application, cancellationToken);
        await base.UpdateAsync(application, cancellationToken);
    }

    protected virtual async ValueTask SyncMcpResourcePermissionAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        var synced = SyncMcpResourcePermission(await GetPermissionsAsync(application, cancellationToken), AgentToolsOptions);
        if (synced != null)
        {
            await SetPermissionsAsync(application, synced.Value, cancellationToken);
        }
    }

    /// <summary>
    /// The permissions with the MCP <c>rsrc:</c> permission added (the scope permission is present) or removed (it is not), or
    /// null when nothing changes, the sync is off or the MCP URL is unknown.
    /// </summary>
    public static ImmutableArray<string>? SyncMcpResourcePermission(ImmutableArray<string> permissions, AbpAgentToolsOpenIddictOptions options)
    {
        if (!options.SyncMcpResourcePermission || options.McpResource == null)
        {
            return null;
        }

        var resource = Permissions.Prefixes.Resource + options.McpResource;
        var hasScope = permissions.Contains(Permissions.Prefixes.Scope + options.ScopeName);
        if (hasScope == permissions.Contains(resource))
        {
            return null;
        }

        return hasScope ? permissions.Add(resource) : permissions.RemoveAll(p => p == resource);
    }
}
