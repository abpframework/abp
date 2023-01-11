using System;
using System.Collections.Generic;
using System.Threading;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs;

public class MyJob : BackgroundJob<MyJobArgs>, ISingletonDependency
{
    public List<string> ExecutedValues { get; } = new List<string>();

    public Guid? TenantId { get; set; }

    private readonly ICurrentTenant _currentTenant;

    public bool Canceled { get; set; }

    public MyJob(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public override void Execute(MyJobArgs args, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            Canceled = true;
        }

        ExecutedValues.Add(args.Value);
        TenantId = _currentTenant.Id;
    }
}
