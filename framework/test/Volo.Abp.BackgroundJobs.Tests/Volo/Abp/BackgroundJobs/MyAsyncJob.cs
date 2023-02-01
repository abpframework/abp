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

    private readonly ICurrentTenant _currentTenant;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    public bool Canceled { get; set; }

    public MyAsyncJob(
        ICurrentTenant currentTenant,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        _currentTenant = currentTenant;
        _cancellationTokenProvider = cancellationTokenProvider;
    }

    public override Task ExecuteAsync(MyAsyncJobArgs args)
    {
        if (_cancellationTokenProvider.Token.IsCancellationRequested)
        {
            Canceled = true;
        }

        ExecutedValues.Add(args.Value);
        TenantId = _currentTenant.Id;
        return Task.CompletedTask;
    }
}
