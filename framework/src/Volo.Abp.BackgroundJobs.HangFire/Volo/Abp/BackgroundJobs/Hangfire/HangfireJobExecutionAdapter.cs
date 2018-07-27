using Microsoft.Extensions.Options;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    public class HangfireJobExecutionAdapter<TArgs>
    {
        protected BackgroundJobOptions Options { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }

        public HangfireJobExecutionAdapter(IOptions<BackgroundJobOptions> options, IBackgroundJobExecuter jobExecuter)
        {
            JobExecuter = jobExecuter;
            Options = options.Value;
        }

        public void Execute(TArgs args)
        {
            var jobType = Options.GetJob(typeof(TArgs)).JobType;
            var context = new JobExecutionContext(jobType, args);
            JobExecuter.Execute(context);
        }
    }
}
