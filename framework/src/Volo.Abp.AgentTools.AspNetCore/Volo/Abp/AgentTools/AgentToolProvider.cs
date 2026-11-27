using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;

namespace Volo.Abp.AgentTools;

/// <summary>
/// No tools while the <see cref="AgentToolsFeatures.Enable"/> feature is off. Otherwise the catalogue tools minus the
/// ones the policy store disables (one bulk policy read per list), the ones the current user may not call and, when the
/// context cannot ask a human, the ones that require approval.
/// </summary>
public class AgentToolProvider : IAgentToolProvider
{
    protected IAgentToolCatalog Catalog { get; }

    protected IAgentToolPolicyStore PolicyStore { get; }

    protected IAgentToolInvoker Invoker { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected ILogger<AgentToolProvider> Logger { get; }

    public AgentToolProvider(
        IAgentToolCatalog catalog,
        IAgentToolPolicyStore policyStore,
        IAgentToolInvoker invoker,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AgentToolProvider> logger)
    {
        Catalog = catalog;
        PolicyStore = policyStore;
        Invoker = invoker;
        ServiceScopeFactory = serviceScopeFactory;
        Logger = logger;
    }

    public virtual async Task<IReadOnlyList<AgentTool>> GetToolsAsync(AgentToolContext context, CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var tools = new List<AgentTool>();
        if (!await scope.ServiceProvider.GetRequiredService<IFeatureChecker>().IsEnabledAsync(AgentToolsFeatures.Enable))
        {
            return tools;
        }

        var policies = await PolicyStore.GetListAsync(Catalog.Tools, cancellationToken);
        foreach (var definition in Catalog.Tools)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var policy = policies[definition.Name];
            if (!policy.Enabled || (policy.RequiresApproval && !context.SupportsApproval) || !await IsPermittedAsync(scope.ServiceProvider, definition))
            {
                continue;
            }

            tools.Add(new AgentTool(definition, new AgentToolFunction(definition, Invoker, context, policy.RequiresApproval), policy.RequiresApproval));
        }

        return tools;
    }

    /// <summary>
    /// The checks ABP runs before a call, without calling: global feature, [Authorize] and [RequiresFeature] of the
    /// implementation method and class (where the interceptors read them), those of a hand-written controller, and the
    /// policy names of ABP's CRUD application services. A call by name still runs the real checks.
    /// Other checks inside method bodies are not seen; the call refuses them.
    /// </summary>
    protected virtual async Task<bool> IsPermittedAsync(IServiceProvider services, AgentToolDefinition definition)
    {
        if (typeof(IGlobalFeatureCheckingEnabled).IsAssignableFrom(definition.ImplementationType)
            && !GlobalFeatureHelper.IsGlobalFeatureEnabled(definition.ImplementationType, out _))
        {
            return false;
        }

        try
        {
            var method = definition.ImplementationMethod;
            await services.GetRequiredService<IMethodInvocationAuthorizationService>().CheckAsync(new MethodInvocationAuthorizationContext(method));
            await services.GetRequiredService<IMethodInvocationFeatureCheckerService>().CheckAsync(new MethodInvocationFeatureCheckerContext(method));
            await AgentToolAuthorizationChecks.CheckControllerAsync(services, definition);
        }
        catch (AbpAuthorizationException)
        {
            return false;
        }

        // CRUD adapter: needs an instance (policy names are set in constructors); resolved only for CRUD services.
        // A service that fails to resolve now costs only its own tool, never the whole list.
        string? crudPolicy;
        try
        {
            crudPolicy = AgentToolAuthorizationChecks.FindCrudPolicyName(definition, () => services.GetRequiredService(definition.ServiceInterface));
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Agent tool {ToolName} left out: its service {ServiceInterface} could not be resolved.",
                definition.Name, definition.ServiceInterface.FullName);
            return false;
        }

        return crudPolicy == null || await services.GetRequiredService<IAuthorizationService>().IsGrantedAsync(crudPolicy);
    }
}
