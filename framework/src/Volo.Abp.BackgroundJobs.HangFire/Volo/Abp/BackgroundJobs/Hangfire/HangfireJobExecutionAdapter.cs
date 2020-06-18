using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
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

        public void Execute(TArgs args)
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
                var jobType = Options.GetJob(typeof(TArgs)).JobType;
                var context = new JobExecutionContext(scope.ServiceProvider, jobType, args);
                AsyncHelper.RunSync(() => JobExecuter.ExecuteAsync(context));
            }
        }
    }
}