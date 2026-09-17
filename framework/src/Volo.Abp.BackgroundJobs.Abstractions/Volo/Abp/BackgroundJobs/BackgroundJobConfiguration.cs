using System;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobConfiguration
{
    public Type ArgsType { get; }

    public Type JobType { get; }

    public string JobName { get; }

    public BackgroundJobConfiguration(Type jobType, string jobName)
    {
        JobType = jobType;
        ArgsType = BackgroundJobArgsHelper.GetJobArgsType(jobType);
        JobName = jobName;
    }
}
