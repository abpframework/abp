using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs;

public class MyAsyncJob : AsyncBackgroundJob<MyAsyncJobArgs>, ISingletonDependency
{
    public List<string> ExecutedValues { get; } = new List<string>();

    public Guid? TenantId { get; set; }

    public bool Canceled { get; set; }

    public override Task ExecuteAsync(MyAsyncJobArgs args)
    {
        if (CancellationTokenProvider.Token.IsCancellationRequested)
        {
            Canceled = true;
        }

        ExecutedValues.Add(args.Value);
        TenantId = CurrentTenant.Id;
        return Task.CompletedTask;
    }
}
