using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Reads the <see cref="AbpAgentToolPolicyOptions.ConfigurationSection"/> strictly, over what code has set. Not with the
/// configuration binder: even with ErrorOnUnknownConfiguration it takes "" and [] for "not set", which would leave a tool enabled.
/// </summary>
internal static class AgentToolPolicyConfigurationReader
{
    public static void Apply(IConfiguration configuration, AbpAgentToolPolicyOptions options)
    {
        var section = configuration.GetSection(AbpAgentToolPolicyOptions.ConfigurationSection);
        foreach (var tool in GetChildren(section, "with tool names as keys"))
        {
            // The elements of an array have their indexes as keys.
            if (tool.Key.All(char.IsAsciiDigit))
            {
                throw Error(tool, "its key must be a tool name.");
            }

            // A new instance: code may have set one instance for several tools.
            options.Tools.TryGetValue(tool.Key, out var code);
            var policy = new AgentToolPolicyConfiguration { Enabled = code?.Enabled, RequiresApproval = code?.RequiresApproval };
            foreach (var field in GetChildren(tool, "with the fields Enabled and RequiresApproval"))
            {
                if (field.Key.Equals(nameof(policy.Enabled), StringComparison.OrdinalIgnoreCase))
                {
                    policy.Enabled = GetBoolean(field) ?? policy.Enabled;
                }
                else if (field.Key.Equals(nameof(policy.RequiresApproval), StringComparison.OrdinalIgnoreCase))
                {
                    policy.RequiresApproval = GetBoolean(field) ?? policy.RequiresApproval;
                }
                else
                {
                    throw Error(field, "there is no such field, the fields are Enabled and RequiresApproval.");
                }
            }

            options.Tools[tool.Key] = policy;
        }
    }

    /// <summary>{}, null and a missing section have neither a value nor children. A scalar has a value; [] has the value "".</summary>
    private static IEnumerable<IConfigurationSection> GetChildren(IConfigurationSection section, string expected)
    {
        return section.Value == null ? section.GetChildren() : throw Error(section, $"it must be an object {expected}.");
    }

    /// <summary>Null = not set.</summary>
    private static bool? GetBoolean(IConfigurationSection field)
    {
        if (!field.GetChildren().Any())
        {
            if (field.Value == null)
            {
                return null;
            }

            if (bool.TryParse(field.Value, out var value))
            {
                return value;
            }
        }

        throw Error(field, "it must be true or false.");
    }

    // Without the value: whatever was put there by mistake stays out of the logs.
    private static AbpException Error(IConfigurationSection section, string reason)
    {
        return new AbpException($"Agent tools: the configuration key '{section.Path}' is invalid: {reason}");
    }
}
