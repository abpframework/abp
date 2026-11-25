using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>
/// <see cref="AbpAgentToolsOpenIddictOptions.SyncMcpResourcePermission"/> for applications saved before it was on (or before the
/// MCP URL changed): adds the MCP <c>rsrc:</c> permission to every application with the scope permission and removes it from every
/// application without one, logging each change by client id. Host data only (OpenIddict applications are not multi-tenant).
/// </summary>
public class AgentToolsMcpResourcePermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public ILogger<AgentToolsMcpResourcePermissionDataSeedContributor> Logger { get; set; } =
        NullLogger<AgentToolsMcpResourcePermissionDataSeedContributor>.Instance;

    protected IOpenIddictApplicationManager ApplicationManager { get; }

    protected AbpAgentToolsOpenIddictOptions Options { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AgentToolsMcpResourcePermissionDataSeedContributor(
        IOpenIddictApplicationManager applicationManager,
        IOptions<AbpAgentToolsOpenIddictOptions> options,
        IUnitOfWorkManager unitOfWorkManager)
    {
        ApplicationManager = applicationManager;
        Options = options.Value;
        UnitOfWorkManager = unitOfWorkManager;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId != null || !Options.SyncMcpResourcePermission)
        {
            return;
        }

        if (Options.McpResource == null)
        {
            Logger.LogWarning(
                "Agent tools: the rsrc: (resource) permission of the MCP endpoint was not synchronized with the OpenIddict applications " +
                "because the MCP endpoint's URL is unknown. Set App:SelfUrl or AbpAgentToolsOpenIddictOptions.McpResource here too " +
                "(e.g. in the DbMigrator) and seed again.");
            return;
        }

        await foreach (var application in ApplicationManager.ListAsync())
        {
            var permissions = await ApplicationManager.GetPermissionsAsync(application);
            var synced = AgentToolsOpenIddictApplicationStore.SyncMcpResourcePermission(permissions, Options);
            if (synced == null)
            {
                continue;
            }

            // AgentToolsOpenIddictApplicationStore applies the same change on update.
            var clientId = await ApplicationManager.GetClientIdAsync(application);
            var added = synced.Value.Length > permissions.Length;
            try
            {
                // Its own unit of work, so a failed save (e.g. a concurrency conflict) does not break the other applications.
                using var uow = UnitOfWorkManager.Begin(requiresNew: true);
                await ApplicationManager.UpdateAsync(application);
                await uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Agent tools: could not update the MCP resource permission of the OpenIddict application {ClientId}.", clientId);
                continue;
            }

            Logger.LogInformation(added
                ? "Agent tools: added the MCP resource permission to the OpenIddict application {ClientId}, which has the MCP scope."
                : "Agent tools: removed the MCP resource permission from the OpenIddict application {ClientId}, which no longer has the MCP scope.",
                clientId);
        }
    }
}
