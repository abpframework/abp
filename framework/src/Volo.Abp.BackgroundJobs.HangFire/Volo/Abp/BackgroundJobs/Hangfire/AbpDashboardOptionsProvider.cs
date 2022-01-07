using System.Linq;
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
            DisplayNameFunc = (dashboardContext, job) =>
                AbpBackgroundJobOptions.GetJob(job.Args.First().GetType()).JobName
        };
    }
}
