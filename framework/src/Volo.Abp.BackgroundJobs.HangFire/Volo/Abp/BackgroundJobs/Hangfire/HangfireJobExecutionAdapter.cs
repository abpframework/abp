using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs.Hangfire;

public class HangfireJobExecutionAdapter<TArgs>
{
    protected AbpBackgroundJobOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }

    public HangfireJobExecutionAdapter(
        IOptions<AbpBackgroundJobOptions> options,
        IBackgroundJobExecuter jobExecuter,
        IServiceScopeFactory serviceScopeFactory)
    {
        JobExecuter = jobExecuter;
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;
    }

    [Queue("{0}")]
    public async Task ExecuteAsync(string queue, TArgs args, CancellationToken cancellationToken = default)
    {
        if (!Options.IsJobExecutionEnabled)
        {
            throw new AbpException(
                "Background job execution is disabled. " +
                "This method should not be called! " +
                "If you want to enable the background job execution, " +
                $"set {nameof(AbpBackgroundJobOptions)}.{nameof(AbpBackgroundJobOptions.IsJobExecutionEnabled)} to true! " +
                "If you've intentionally disabled job execution and this seems a bug, please report it."
            );
        }

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var jobConfiguration = Options.GetJob(typeof(TArgs));
            var context = new JobExecutionContext(scope.ServiceProvider, jobConfiguration.JobType!, args!, cancellationToken: cancellationToken, jobName: jobConfiguration.JobName);
            await JobExecuter.ExecuteAsync(context);
        }
    }
}

public class HangfireJobExecutionAdapter
{
    protected AbpBackgroundJobOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }
    protected IJsonSerializer JsonSerializer { get; }

    public HangfireJobExecutionAdapter(
        IOptions<AbpBackgroundJobOptions> options,
        IBackgroundJobExecuter jobExecuter,
        IServiceScopeFactory serviceScopeFactory,
        IJsonSerializer jsonSerializer)
    {
        JobExecuter = jobExecuter;
        ServiceScopeFactory = serviceScopeFactory;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
    }

    [Queue("{0}")]
    public async Task ExecuteAsync(string queue, string jobName, string serializedArgs, CancellationToken cancellationToken = default)
    {
        if (!Options.IsJobExecutionEnabled)
        {
            throw new AbpException(
                "Background job execution is disabled. " +
                "This method should not be called! " +
                "If you want to enable the background job execution, " +
                $"set {nameof(AbpBackgroundJobOptions)}.{nameof(AbpBackgroundJobOptions.IsJobExecutionEnabled)} to true! " +
                "If you've intentionally disabled job execution and this seems a bug, please report it."
            );
        }

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var jobConfiguration = Options.GetJob(jobName);
            var args = JsonSerializer.Deserialize(jobConfiguration.ArgsType, serializedArgs);
            var context = new JobExecutionContext(scope.ServiceProvider, jobConfiguration.JobType ?? typeof(object), args, cancellationToken: cancellationToken, jobName: jobName);
            await JobExecuter.ExecuteAsync(context);
        }
    }
}
