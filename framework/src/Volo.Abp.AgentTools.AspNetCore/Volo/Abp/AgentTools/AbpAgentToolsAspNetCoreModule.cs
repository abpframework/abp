using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.AgentTools;

[DependsOn(
    typeof(AbpAgentToolsModule),
    typeof(AbpAspNetCoreMvcModule)
    )]
public class AbpAgentToolsAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddSingleton<IAgentToolCatalog, AgentToolCatalog>();
        context.Services.TryAddTransient<IAgentToolInvoker, AgentToolInvoker>();
        context.Services.TryAddTransient<IAgentToolProvider, AgentToolProvider>();
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
        // Reads and validates the policy configuration, also when nothing is included.
        var policies = context.ServiceProvider.GetRequiredService<IOptions<AbpAgentToolPolicyOptions>>().Value.Tools;
        LogMisplacedPolicies(context.ServiceProvider);

        // Fail fast (MaxTools, duplicate or invalid names, FailOnInvalidSchema) and log the schema diagnostics at startup
        // instead of on the first tool list. Post-initialization: MVC application parts are added in
        // AbpAspNetCoreMvcModule.OnApplicationInitialization, and the ApiExplorer descriptions need them.
        if (context.ServiceProvider.GetRequiredService<IOptions<AbpAgentToolOptions>>().Value.HasAnyInclusion)
        {
            _ = context.ServiceProvider.GetRequiredService<IAgentToolCatalog>().Tools;
        }

        if (policies.Count > 0)
        {
            LogUnknownTools(context.ServiceProvider, policies.Keys);
        }
    }

    /// <summary>
    /// Settings that look like a tool policy, but are not in the section that is read: everything named Enabled or RequiresApproval
    /// below a child section of "AgentTools" (a misspelled "Tools", a tool directly under it), and a value in place of the whole section.
    /// Not a failure: an application may keep settings of its own there. Keys only, never values.
    /// </summary>
    private static void LogMisplacedPolicies(IServiceProvider serviceProvider)
    {
        var toolsSection = AbpAgentToolPolicyOptions.ConfigurationSection;
        var section = serviceProvider.GetRequiredService<IConfiguration>().GetSection(ConfigurationPath.GetParentPath(toolsSection)!);
        var keys = section.AsEnumerable(makePathsRelative: true)
            .Where(x => x.Value != null && IsPolicyField(x.Key) && !IsUnder(x.Key, ConfigurationPath.GetSectionKey(toolsSection)) && !IsUnder(x.Key, "Mcp"))
            .Select(x => ConfigurationPath.Combine(section.Path, x.Key))
            .OrderBy(key => key, StringComparer.Ordinal)
            .ToList();
        if (section.Value != null)
        {
            keys.Insert(0, section.Path);
        }

        if (keys.Count > 0)
        {
            serviceProvider.GetRequiredService<ILogger<AbpAgentToolsAspNetCoreModule>>().LogError(
                "Agent tools: settings that look like a tool policy, but have no effect, because they are not in the configuration section {Section}: {Keys}",
                toolsSection, keys.JoinAsString(", "));
        }

        static bool IsPolicyField(string key) =>
            key.EndsWith(ConfigurationPath.KeyDelimiter + nameof(AgentToolPolicyConfiguration.Enabled), StringComparison.OrdinalIgnoreCase) ||
            key.EndsWith(ConfigurationPath.KeyDelimiter + nameof(AgentToolPolicyConfiguration.RequiresApproval), StringComparison.OrdinalIgnoreCase);

        static bool IsUnder(string key, string name) =>
            key.StartsWith(name + ConfigurationPath.KeyDelimiter, StringComparison.OrdinalIgnoreCase);
    }

    // Not a failure: the same configuration may serve several applications. With nothing included the catalogue is empty.
    private static void LogUnknownTools(IServiceProvider serviceProvider, IEnumerable<string> configuredNames)
    {
        var unknown = configuredNames
            .Except(serviceProvider.GetRequiredService<IAgentToolCatalog>().Tools.Select(t => t.Name), StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();
        if (unknown.Count > 0)
        {
            serviceProvider.GetRequiredService<ILogger<AbpAgentToolsAspNetCoreModule>>().LogError(
                "Agent tools: policy settings (configuration section {Section} or AbpAgentToolPolicyOptions) that have no effect, " +
                "because these tools are not in the catalogue: {Tools}",
                AbpAgentToolPolicyOptions.ConfigurationSection, unknown.JoinAsString(", "));
        }
    }
}
