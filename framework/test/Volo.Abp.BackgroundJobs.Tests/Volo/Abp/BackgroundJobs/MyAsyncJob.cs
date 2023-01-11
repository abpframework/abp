using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs;

public class MyAsyncJob : AsyncBackgroundJob<MyAsyncJobArgs>, ISingletonDependency
{
    public List<string> ExecutedValues { get; } = new List<string>();

    public Guid? TenantId { get; set; }

    private readonly ICurrentTenant _currentTenant;

    public bool Canceled { get; set; }

    public MyAsyncJob(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public override Task ExecuteAsync(MyAsyncJobArgs args, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            Canceled = true;
        }

        ExecutedValues.Add(args.Value);
        TenantId = _currentTenant.Id;
        return Task.CompletedTask;
    }
}
