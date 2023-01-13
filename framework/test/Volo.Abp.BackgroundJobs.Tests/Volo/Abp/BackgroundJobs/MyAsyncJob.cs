using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs;

public class MyAsyncJob : AsyncBackgroundJob<MyAsyncJobArgs>, ISingletonDependency
{
    public List<string> ExecutedValues { get; } = new List<string>();
    
    public Guid? TenantId { get; set; }
    
    private readonly ICurrentTenant _currentTenant;

    public MyAsyncJob(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public override Task ExecuteAsync(MyAsyncJobArgs args)
    {
        ExecutedValues.Add(args.Value);
        TenantId = _currentTenant.Id;
        return Task.CompletedTask;
    }
}
