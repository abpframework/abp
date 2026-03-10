using System.Linq;
using System.Text.Json;
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
                    if (job.Args[1] is AnonymousJobArgs anonymousJobArgs)
                    {
                        return anonymousJobArgs.JobName;
                    }

                    jobName = AbpBackgroundJobOptions.GetJob(job.Args[1].GetType()).JobName;
                }

                if (job.Args.Count == 4 && job.Args.Last() is CancellationToken)
                {
                    if (job.Args[1] is string transportJobName)
                    {
                        if (transportJobName == AnonymousJobArgs.JobNameConstant &&
                            job.Args[2] is string serializedArgs &&
                            TryGetEffectiveJobName(serializedArgs, out var effectiveJobName))
                        {
                            return effectiveJobName;
                        }

                        return transportJobName;
                    }
                }

                return jobName;
            }
        };
    }

    protected virtual bool TryGetEffectiveJobName(string serializedArgs, out string effectiveJobName)
    {
        effectiveJobName = string.Empty;

        try
        {
            using var document = JsonDocument.Parse(serializedArgs);
            if (document.RootElement.TryGetProperty(nameof(AnonymousJobArgs.JobName), out var jobNameElement))
            {
                var jobName = jobNameElement.GetString();
                if (!string.IsNullOrWhiteSpace(jobName))
                {
                    effectiveJobName = jobName!;
                    return true;
                }
            }
        }
        catch
        {
            // Ignore parse errors and fallback to transport job name.
        }

        return false;
    }
}
