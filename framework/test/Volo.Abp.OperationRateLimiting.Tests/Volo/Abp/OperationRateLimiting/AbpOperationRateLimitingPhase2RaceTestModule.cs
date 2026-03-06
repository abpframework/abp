using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace Volo.Abp.OperationRateLimiting;

/// <summary>
/// A mock store that simulates a concurrent race condition:
/// - GetAsync always says the quota is available (Phase 1 checks pass).
/// - IncrementAsync always says the quota is exhausted (Phase 2 finds another request consumed it).
/// </summary>
internal class RaceConditionSimulatorStore : IOperationRateLimitingStore
{
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
        // Simulate: between Phase 1 and Phase 2 another concurrent request consumed the last slot.
        return Task.FromResult(new OperationRateLimitingStoreResult
        {
            IsAllowed = false,
            CurrentCount = maxCount,
            MaxCount = maxCount,
            RetryAfter = duration
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
public class AbpOperationRateLimitingPhase2RaceTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Transient<IOperationRateLimitingStore, RaceConditionSimulatorStore>());

        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddPolicy("TestRacePolicy", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 3)
                      .PartitionByParameter();
            });
        });
    }
}
