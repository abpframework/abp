using System;
using System.Threading;

namespace Volo.Abp.BackgroundWorkers;

public class PeriodicBackgroundWorkerContext
{
    public IServiceProvider ServiceProvider { get; }

    public CancellationToken CancellationToken { get; }

    public PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CancellationToken = default;
    }

    public PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        ServiceProvider = serviceProvider;
        CancellationToken = cancellationToken;
    }
}
