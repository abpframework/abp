using System;
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

    public void Reset()
    {
        BatchCheckCount = 0;
        SingleCheckCount = 0;
    }
}

public class TestGlobalPermissionStateChecker : ISimpleBatchStateChecker<PermissionDefinition>, ITransientDependency
{
    public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<PermissionDefinition> context)
    {
        GetCounter(context.ServiceProvider).SingleCheckCount++;
        return Task.FromResult(true);
    }

    public Task<SimpleStateCheckerResult<PermissionDefinition>> IsEnabledAsync(SimpleBatchStateCheckerContext<PermissionDefinition> context)
    {
        GetCounter(context.ServiceProvider).BatchCheckCount++;
        return Task.FromResult(new SimpleStateCheckerResult<PermissionDefinition>(context.States));
    }

    private static TestGlobalPermissionStateCheckerCounter GetCounter(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<TestGlobalPermissionStateCheckerCounter>();
    }
}
