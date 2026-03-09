using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class DynamicBackgroundJobHandlerProvider : IDynamicBackgroundJobHandlerProvider, ISingletonDependency
{
    protected AbpBackgroundJobOptions Options { get; }

    public DynamicBackgroundJobHandlerProvider(IOptions<AbpBackgroundJobOptions> options)
    {
        Options = options.Value;
    }

    public virtual void Register(string jobName, Func<DynamicBackgroundJobContext, Task> handler)
    {
        Options.AddDynamicJob(jobName, handler);
    }

    public virtual void Register(string jobName, Action<DynamicBackgroundJobContext> handler)
    {
        Options.AddDynamicJob(jobName, handler);
    }

    public virtual bool Unregister(string jobName)
    {
        return Options.RemoveDynamicJob(jobName);
    }

    public virtual bool IsRegistered(string jobName)
    {
        return Options.GetJobOrNull(jobName)?.IsDynamic == true;
    }
}
