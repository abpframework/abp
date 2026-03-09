using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobConfiguration
{
    public Type ArgsType { get; }

    public Type? JobType { get; }

    public string JobName { get; }

    public bool IsDynamic { get; }

    public Func<DynamicBackgroundJobContext, Task>? DynamicHandler { get; }

    public BackgroundJobConfiguration(Type jobType, string jobName)
    {
        JobType = jobType;
        ArgsType = BackgroundJobArgsHelper.GetJobArgsType(jobType);
        JobName = jobName;
    }

    public BackgroundJobConfiguration(string jobName, Func<DynamicBackgroundJobContext, Task> handler)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        Check.NotNull(handler, nameof(handler));

        JobName = jobName;
        DynamicHandler = handler;
        IsDynamic = true;
        ArgsType = typeof(Dictionary<string, object>);
    }
}
