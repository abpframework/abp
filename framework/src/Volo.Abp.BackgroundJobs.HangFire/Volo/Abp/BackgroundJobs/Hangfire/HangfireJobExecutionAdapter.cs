using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    public class HangfireJobExecutionAdapter<TArgs>
    {
        public ILogger<HangfireJobExecutionAdapter<TArgs>> Logger { get; set; }
        
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
            Logger = NullLogger<HangfireJobExecutionAdapter<TArgs>>.Instance;
        }

        public void Execute(TArgs args)
        {
            if (!Options.IsJobExecutionEnabled)
            {
                Logger.LogWarning("Background jobs system is disabled");
                return;
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