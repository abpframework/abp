using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class JobExecutionContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public Type JobType { get; }

    public object JobArgs { get; }

    public JobExecutionContext(IServiceProvider serviceProvider, Type jobType, object jobArgs)
    {
        ServiceProvider = serviceProvider;
        JobType = jobType;
        JobArgs = jobArgs;
    }
}
