using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace Volo.Abp.OperationRateLimiting;

/// <summary>
/// A mock store that simulates a multi-rule Phase 2 race condition:
/// - GetAsync always reports quota available (Phase 1 passes for all rules).
/// - IncrementAsync succeeds for the first call, fails on the second call
///   (simulating a concurrent race on Rule2), and tracks total increment calls
///   so tests can verify that Rule3 was never incremented (early break).
/// </summary>
internal class MultiRuleRaceConditionSimulatorStore : IOperationRateLimitingStore
{
    private int _incrementCallCount;

    /// <summary>
    /// Total number of IncrementAsync calls made.
    /// </summary>
    public int IncrementCallCount => _incrementCallCount;

    public Task<OperationRateLimitingStoreResult> GetAsync(string key, TimeSpan duration, int maxCount)
    {
        return Task.FromResult(new OperationRateLimitingStoreResult
        {
            IsAllowed = true,
            CurrentCount = 0,
            MaxCount = maxCount
        });
    }

    public Task<OperationRateLimitingStoreResult> IncrementAsync(string key, TimeSpan duration, int maxCount)
    {
        var callIndex = Interlocked.Increment(ref _incrementCallCount);

        if (callIndex == 2)
        {
            // Second rule: simulate concurrent race - another request consumed the last slot.
            return Task.FromResult(new OperationRateLimitingStoreResult
            {
                IsAllowed = false,
                CurrentCount = maxCount,
                MaxCount = maxCount,
                RetryAfter = duration
            });
        }

        // First rule (and any others if early break fails): succeed.
        return Task.FromResult(new OperationRateLimitingStoreResult
        {
            IsAllowed = true,
            CurrentCount = 1,
            MaxCount = maxCount
        });
    }

    public Task ResetAsync(string key)
    {
        return Task.CompletedTask;
    }
}

[DependsOn(
    typeof(AbpOperationRateLimitingModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
)]
public class AbpOperationRateLimitingPhase2EarlyBreakTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Singleton<IOperationRateLimitingStore, MultiRuleRaceConditionSimulatorStore>());

        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            // 3-rule composite policy: all PartitionByParameter with different durations
            // so they generate unique cache keys and don't trigger duplicate rule validation.
            options.AddPolicy("TestMultiRuleRacePolicy", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(1), maxCount: 5)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(2), maxCount: 5)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromHours(3), maxCount: 5)
                    .PartitionByParameter());
            });
        });
    }
}
