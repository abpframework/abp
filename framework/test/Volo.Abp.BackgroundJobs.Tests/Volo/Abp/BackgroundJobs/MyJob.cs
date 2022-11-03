using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs;

public class MyJob : BackgroundJob<MyJobArgs>, ISingletonDependency
{
    public List<string> ExecutedValues { get; } = new List<string>();
    
    public Guid? TenantId { get; set; }

    private readonly ICurrentTenant _currentTenant;
    
    public MyJob(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public override void Execute(MyJobArgs args)
    {
        ExecutedValues.Add(args.Value);
        TenantId = _currentTenant.Id;
    }
}
