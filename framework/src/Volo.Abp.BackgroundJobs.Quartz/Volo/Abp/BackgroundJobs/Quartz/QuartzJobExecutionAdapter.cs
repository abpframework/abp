using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Quartz;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs.Quartz;

public class QuartzJobExecutionAdapter<TArgs> : IJob
{
    public ILogger<QuartzJobExecutionAdapter<TArgs>> Logger { get; set; }

    protected AbpBackgroundJobOptions Options { get; }
    protected AbpBackgroundJobQuartzOptions BackgroundJobQuartzOptions { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }
    protected IJsonSerializer JsonSerializer { get; }

    public QuartzJobExecutionAdapter(
        IOptions<AbpBackgroundJobOptions> options,
        IOptions<AbpBackgroundJobQuartzOptions> backgroundJobQuartzOptions,
        IBackgroundJobExecuter jobExecuter,
        IServiceScopeFactory serviceScopeFactory,
        IJsonSerializer jsonSerializer)
    {
        JobExecuter = jobExecuter;
        ServiceScopeFactory = serviceScopeFactory;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
        BackgroundJobQuartzOptions = backgroundJobQuartzOptions.Value;
        Logger = NullLogger<QuartzJobExecutionAdapter<TArgs>>.Instance;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var args = JsonSerializer.Deserialize<TArgs>(context.JobDetail.JobDataMap.GetString(nameof(TArgs))!);
            var jobConfiguration = Options.GetJob(typeof(TArgs));
            var jobContext = new JobExecutionContext(scope.ServiceProvider, jobConfiguration.JobType!, args!, cancellationToken: context.CancellationToken, jobName: jobConfiguration.JobName);
            try
            {
                await JobExecuter.ExecuteAsync(jobContext);
            }
            catch (Exception exception)
            {
                var jobExecutionException = new JobExecutionException(exception);

                var retryIndex = context.JobDetail.JobDataMap.GetString(QuartzBackgroundJobManager.JobDataPrefix + QuartzBackgroundJobManager.RetryIndex)!.To<int>();
                retryIndex++;
                context.JobDetail.JobDataMap.Put(QuartzBackgroundJobManager.JobDataPrefix + QuartzBackgroundJobManager.RetryIndex, retryIndex.ToString());

                await BackgroundJobQuartzOptions.RetryStrategy.Invoke(retryIndex, context, jobExecutionException);

                throw jobExecutionException;
            }
        }
    }
}

public class QuartzJobExecutionAdapter : IJob
{
    public const string JobNameKey = "AbpJobName";
    public const string JobArgsKey = "AbpJobArgs";

    public ILogger<QuartzJobExecutionAdapter> Logger { get; set; }

    protected AbpBackgroundJobOptions Options { get; }
    protected AbpBackgroundJobQuartzOptions BackgroundJobQuartzOptions { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }
    protected IJsonSerializer JsonSerializer { get; }

    public QuartzJobExecutionAdapter(
        IOptions<AbpBackgroundJobOptions> options,
        IOptions<AbpBackgroundJobQuartzOptions> backgroundJobQuartzOptions,
        IBackgroundJobExecuter jobExecuter,
        IServiceScopeFactory serviceScopeFactory,
        IJsonSerializer jsonSerializer)
    {
        JobExecuter = jobExecuter;
        ServiceScopeFactory = serviceScopeFactory;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
        BackgroundJobQuartzOptions = backgroundJobQuartzOptions.Value;
        Logger = NullLogger<QuartzJobExecutionAdapter>.Instance;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var jobName = context.JobDetail.JobDataMap.GetString(JobNameKey)!;
            var serializedArgs = context.JobDetail.JobDataMap.GetString(JobArgsKey)!;
            var jobConfiguration = Options.GetJob(jobName);
            var args = JsonSerializer.Deserialize(jobConfiguration.ArgsType, serializedArgs);
            var jobContext = new JobExecutionContext(scope.ServiceProvider, jobConfiguration.JobType, args, cancellationToken: context.CancellationToken);
            try
            {
                await JobExecuter.ExecuteAsync(jobContext);
            }
            catch (Exception exception)
            {
                var jobExecutionException = new JobExecutionException(exception);

                var retryIndex = context.JobDetail.JobDataMap.GetString(QuartzBackgroundJobManager.JobDataPrefix + QuartzBackgroundJobManager.RetryIndex)!.To<int>();
                retryIndex++;
                context.JobDetail.JobDataMap.Put(QuartzBackgroundJobManager.JobDataPrefix + QuartzBackgroundJobManager.RetryIndex, retryIndex.ToString());

                await BackgroundJobQuartzOptions.RetryStrategy.Invoke(retryIndex, context, jobExecutionException);

                throw jobExecutionException;
            }
        }
    }
}
