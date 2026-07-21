using System;

namespace Volo.Abp.BackgroundWorkers;

public class DynamicBackgroundWorkerExecutionContext
{
    public string WorkerName { get; }

    public IServiceProvider ServiceProvider { get; }

    public DynamicBackgroundWorkerExecutionContext(string workerName, IServiceProvider serviceProvider)
    {
        WorkerName = Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
    }
}
