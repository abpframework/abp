using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingChecker : IOperationRateLimitingChecker, ITransientDependency
{
    protected AbpOperationRateLimitingOptions Options { get; }
    protected IOperationRateLimitingPolicyProvider PolicyProvider { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected IOperationRateLimitingStore Store { get; }
    protected ICurrentUser CurrentUser { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IWebClientInfoProvider WebClientInfoProvider { get; }

    public OperationRateLimitingChecker(
        IOptions<AbpOperationRateLimitingOptions> options,
        IOperationRateLimitingPolicyProvider policyProvider,
        IServiceProvider serviceProvider,
        IOperationRateLimitingStore store,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        IWebClientInfoProvider webClientInfoProvider)
    {
        Options = options.Value;
        PolicyProvider = policyProvider;
        ServiceProvider = serviceProvider;
        Store = store;
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
        WebClientInfoProvider = webClientInfoProvider;
    }

    public virtual async Task CheckAsync(string policyName, OperationRateLimitingContext? context = null)
    {
        if (!Options.IsEnabled)
        {
            return;
        }

        context = EnsureContext(context);
        var policy = await PolicyProvider.GetAsync(policyName);
        var rules = CreateRules(policy);

        // Phase 1: Check ALL rules without incrementing to get complete status.
        // Do not exit early: a later rule may have a larger RetryAfter that the caller needs to know about.
        var checkResults = new List<OperationRateLimitingRuleResult>();
        foreach (var rule in rules)
        {
            checkResults.Add(await rule.CheckAsync(context));
        }

        if (checkResults.Any(r => !r.IsAllowed))
        {
            // Throw without incrementing any counter; RetryAfter is the max across all blocking rules.
            var aggregatedResult = AggregateResults(checkResults, policy);
            ThrowRateLimitException(policy, aggregatedResult, context);
        }

        // Phase 2: All rules pass - now increment all counters.
        // Also guard against a concurrent race where another request consumed the last quota
        // between Phase 1 and Phase 2.
        var incrementResults = new List<OperationRateLimitingRuleResult>();
        foreach (var rule in rules)
        {
            incrementResults.Add(await rule.AcquireAsync(context));
        }

        if (incrementResults.Any(r => !r.IsAllowed))
        {
            var aggregatedResult = AggregateResults(incrementResults, policy);
            ThrowRateLimitException(policy, aggregatedResult, context);
        }
    }

    public virtual async Task<bool> IsAllowedAsync(string policyName, OperationRateLimitingContext? context = null)
    {
        if (!Options.IsEnabled)
        {
            return true;
        }

        context = EnsureContext(context);
        var policy = await PolicyProvider.GetAsync(policyName);
        var rules = CreateRules(policy);

        foreach (var rule in rules)
        {
            var result = await rule.CheckAsync(context);
            if (!result.IsAllowed)
            {
                return false;
            }
        }

        return true;
    }

    public virtual async Task<OperationRateLimitingResult> GetStatusAsync(string policyName, OperationRateLimitingContext? context = null)
    {
        if (!Options.IsEnabled)
        {
            return new OperationRateLimitingResult
            {
                IsAllowed = true,
                RemainingCount = int.MaxValue,
                MaxCount = int.MaxValue,
                CurrentCount = 0
            };
        }

        context = EnsureContext(context);
        var policy = await PolicyProvider.GetAsync(policyName);
        var rules = CreateRules(policy);
        var ruleResults = new List<OperationRateLimitingRuleResult>();

        foreach (var rule in rules)
        {
            ruleResults.Add(await rule.CheckAsync(context));
        }

        return AggregateResults(ruleResults, policy);
    }

    public virtual async Task ResetAsync(string policyName, OperationRateLimitingContext? context = null)
    {
        context = EnsureContext(context);
        var policy = await PolicyProvider.GetAsync(policyName);
        var rules = CreateRules(policy);

        foreach (var rule in rules)
        {
            await rule.ResetAsync(context);
        }
    }

    protected virtual OperationRateLimitingContext EnsureContext(OperationRateLimitingContext? context)
    {
        context ??= new OperationRateLimitingContext();
        context.ServiceProvider = ServiceProvider;
        return context;
    }

    protected virtual List<IOperationRateLimitingRule> CreateRules(OperationRateLimitingPolicy policy)
    {
        var rules = new List<IOperationRateLimitingRule>();

        for (var i = 0; i < policy.Rules.Count; i++)
        {
            rules.Add(new FixedWindowOperationRateLimitingRule(
                policy.Name,
                i,
                policy.Rules[i],
                Store,
                CurrentUser,
                CurrentTenant,
                WebClientInfoProvider));
        }

        foreach (var customRuleType in policy.CustomRuleTypes)
        {
            rules.Add((IOperationRateLimitingRule)ServiceProvider.GetRequiredService(customRuleType));
        }

        return rules;
    }

    protected virtual OperationRateLimitingResult AggregateResults(
        List<OperationRateLimitingRuleResult> ruleResults,
        OperationRateLimitingPolicy policy)
    {
        var isAllowed = ruleResults.All(r => r.IsAllowed);
        var mostRestrictive = ruleResults
            .OrderBy(r => r.RemainingCount)
            .ThenByDescending(r => r.RetryAfter ?? TimeSpan.Zero)
            .First();

        return new OperationRateLimitingResult
        {
            IsAllowed = isAllowed,
            RemainingCount = mostRestrictive.RemainingCount,
            MaxCount = mostRestrictive.MaxCount,
            CurrentCount = mostRestrictive.MaxCount - mostRestrictive.RemainingCount,
            RetryAfter = ruleResults.Any(r => !r.IsAllowed && r.RetryAfter.HasValue)
                ? ruleResults
                    .Where(r => !r.IsAllowed && r.RetryAfter.HasValue)
                    .Select(r => r.RetryAfter!.Value)
                    .Max()
                : null,
            WindowDuration = mostRestrictive.WindowDuration,
            RuleResults = ruleResults
        };
    }

    protected virtual void ThrowRateLimitException(
        OperationRateLimitingPolicy policy,
        OperationRateLimitingResult result,
        OperationRateLimitingContext context)
    {
        var formatter = context.ServiceProvider.GetRequiredService<IOperationRateLimitingFormatter>();

        var exception = new AbpOperationRateLimitingException(
            policy.Name,
            result,
            policy.ErrorCode);

        if (result.RetryAfter.HasValue)
        {
            exception.SetRetryAfterFormatted(formatter.Format(result.RetryAfter.Value));
        }

        if (result.WindowDuration > TimeSpan.Zero)
        {
            exception.SetWindowDescriptionFormatted(formatter.Format(result.WindowDuration));
        }

        if (result.RuleResults != null)
        {
            var ruleDetails = new List<Dictionary<string, object>>();
            foreach (var ruleResult in result.RuleResults)
            {
                ruleDetails.Add(new Dictionary<string, object>
                {
                    ["RuleName"] = ruleResult.RuleName,
                    ["IsAllowed"] = ruleResult.IsAllowed,
                    ["MaxCount"] = ruleResult.MaxCount,
                    ["RemainingCount"] = ruleResult.RemainingCount,
                    ["CurrentCount"] = ruleResult.MaxCount - ruleResult.RemainingCount,
                    ["WindowDurationSeconds"] = (int)ruleResult.WindowDuration.TotalSeconds,
                    ["WindowDescription"] = ruleResult.WindowDuration > TimeSpan.Zero
                        ? formatter.Format(ruleResult.WindowDuration)
                        : string.Empty,
                    ["RetryAfterSeconds"] = (int)(ruleResult.RetryAfter?.TotalSeconds ?? 0),
                    ["RetryAfter"] = ruleResult.RetryAfter.HasValue
                        ? formatter.Format(ruleResult.RetryAfter.Value)
                        : string.Empty
                });
            }

            exception.WithData("RuleDetails", ruleDetails);
        }

        foreach (var kvp in context.ExtraProperties)
        {
            exception.WithData(kvp.Key, kvp.Value!);
        }

        throw exception;
    }
}
