using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs;

public class MyJob : BackgroundJob<MyJobArgs>, ISingletonDependency
{
    public List<string> ExecutedValues { get; } = new List<string>();

    public Guid? TenantId { get; set; }

    private readonly ICurrentTenant _currentTenant;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    public bool Canceled { get; set; }

    public MyJob(
        ICurrentTenant currentTenant,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        _currentTenant = currentTenant;
        _cancellationTokenProvider = cancellationTokenProvider;
    }

    public override void Execute(MyJobArgs args)
    {
        if (_cancellationTokenProvider.Token.IsCancellationRequested)
        {
            Canceled = true;
        }

        ExecutedValues.Add(args.Value);
        TenantId = _currentTenant.Id;
    }
}
