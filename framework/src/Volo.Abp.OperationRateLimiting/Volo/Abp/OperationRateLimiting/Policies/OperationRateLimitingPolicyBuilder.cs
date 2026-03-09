using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingPolicyBuilder
{
    private readonly string _name;
    private string? _errorCode;
    private readonly List<OperationRateLimitingRuleDefinition> _rules = new();
    private readonly List<Type> _customRuleTypes = new();

    public OperationRateLimitingPolicyBuilder(string name)
    {
        _name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }

    /// <summary>
    /// Add a built-in rule. Multiple rules are AND-combined.
    /// </summary>
    public OperationRateLimitingPolicyBuilder AddRule(
        Action<OperationRateLimitingRuleBuilder> configure)
    {
        var builder = new OperationRateLimitingRuleBuilder(this);
        configure(builder);
        if (!builder.IsCommitted)
        {
            _rules.Add(builder.Build());
        }
        return this;
    }

    /// <summary>
    /// Add a custom rule type (resolved from DI).
    /// </summary>
    public OperationRateLimitingPolicyBuilder AddRule<TRule>()
        where TRule : class, IOperationRateLimitingRule
    {
        _customRuleTypes.Add(typeof(TRule));
        return this;
    }

    /// <summary>
    /// Shortcut: single-rule policy with fixed window.
    /// Returns the rule builder for partition configuration.
    /// </summary>
    public OperationRateLimitingRuleBuilder WithFixedWindow(
        TimeSpan duration, int maxCount)
    {
        var builder = new OperationRateLimitingRuleBuilder(this);
        builder.WithFixedWindow(duration, maxCount);
        return builder;
    }

    /// <summary>
    /// Set a custom ErrorCode for this policy's exception.
    /// </summary>
    public OperationRateLimitingPolicyBuilder WithErrorCode(string errorCode)
    {
        _errorCode = Check.NotNullOrWhiteSpace(errorCode, nameof(errorCode));
        return this;
    }

    internal void AddRuleDefinition(OperationRateLimitingRuleDefinition definition)
    {
        _rules.Add(definition);
    }

    internal OperationRateLimitingPolicy Build()
    {
        if (_rules.Count == 0 && _customRuleTypes.Count == 0)
        {
            throw new AbpException(
                $"Operation rate limit policy '{_name}' has no rules. " +
                "Call AddRule() or WithFixedWindow(...).PartitionBy*() to add at least one rule.");
        }

        var duplicate = _rules
            .Where(r => r.PartitionType != OperationRateLimitingPartitionType.Custom)
            .GroupBy(r => (r.Duration, r.MaxCount, r.PartitionType, r.IsMultiTenant))
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicate != null)
        {
            var (duration, maxCount, partitionType, isMultiTenant) = duplicate.Key;
            throw new AbpException(
                $"Operation rate limit policy '{_name}' has duplicate rules with the same " +
                $"Duration ({duration}), MaxCount ({maxCount}), PartitionType ({partitionType}), " +
                $"and IsMultiTenant ({isMultiTenant}). " +
                "Each rule in a policy must have a unique combination of these properties.");
        }

        return new OperationRateLimitingPolicy
        {
            Name = _name,
            ErrorCode = _errorCode,
            Rules = new List<OperationRateLimitingRuleDefinition>(_rules),
            CustomRuleTypes = new List<Type>(_customRuleTypes)
        };
    }
}
