using System.Linq;
using System.Threading;
using Hangfire;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.Hangfire;

public class AbpDashboardOptionsProvider : ITransientDependency
{
    protected AbpBackgroundJobOptions AbpBackgroundJobOptions { get; }

    public AbpDashboardOptionsProvider(IOptions<AbpBackgroundJobOptions> abpBackgroundJobOptions)
    {
        AbpBackgroundJobOptions = abpBackgroundJobOptions.Value;
    }

    public virtual DashboardOptions Get()
    {
        return new DashboardOptions
        {
            DisplayNameFunc = (_, job) =>
            {
                var jobName = job.ToString();
                if (job.Args.Count == 3 && job.Args.Last() is CancellationToken)
                {
                    jobName = AbpBackgroundJobOptions.GetJob(job.Args[1].GetType()).JobName;
                }

                return jobName;
            }
        };
    }
}
