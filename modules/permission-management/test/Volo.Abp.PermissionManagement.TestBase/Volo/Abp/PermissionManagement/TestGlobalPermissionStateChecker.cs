using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.PermissionManagement;

public class TestGlobalPermissionStateCheckerCounter : ISingletonDependency
{
    public int BatchCheckCount { get; set; }

    public int SingleCheckCount { get; set; }

    public HashSet<string> CheckedPermissionNames { get; } = new HashSet<string>();

    public void Reset()
    {
        BatchCheckCount = 0;
        SingleCheckCount = 0;
        CheckedPermissionNames.Clear();
    }
}

public class TestGlobalPermissionStateChecker : ISimpleBatchStateChecker<PermissionDefinition>, ITransientDependency
{
    public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<PermissionDefinition> context)
    {
        var counter = GetCounter(context.ServiceProvider);
        counter.SingleCheckCount++;
        counter.CheckedPermissionNames.Add(context.State.Name);
        return Task.FromResult(true);
    }

    public Task<SimpleStateCheckerResult<PermissionDefinition>> IsEnabledAsync(SimpleBatchStateCheckerContext<PermissionDefinition> context)
    {
        var counter = GetCounter(context.ServiceProvider);
        counter.BatchCheckCount++;
        foreach (var state in context.States)
        {
            counter.CheckedPermissionNames.Add(state.Name);
        }

        return Task.FromResult(new SimpleStateCheckerResult<PermissionDefinition>(context.States));
    }

    private static TestGlobalPermissionStateCheckerCounter GetCounter(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<TestGlobalPermissionStateCheckerCounter>();
    }
}
