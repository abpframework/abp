using System;

namespace Volo.Abp.BackgroundJobs;

public class AnonymousJobExecutionContext
{
    public string JobName { get; }

    public string JsonData { get; }

    public IServiceProvider ServiceProvider { get; }

    public AnonymousJobExecutionContext(
        string jobName,
        string jsonData,
        IServiceProvider serviceProvider)
    {
        JobName = Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        JsonData = Check.NotNull(jsonData, nameof(jsonData));
        ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
    }
}
