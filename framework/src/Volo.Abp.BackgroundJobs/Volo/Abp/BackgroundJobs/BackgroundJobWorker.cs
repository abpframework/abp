using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobWorker : PeriodicBackgroundWorkerBase, IBackgroundJobWorker, ISingletonDependency
    {
        protected IBackgroundJobExecuter JobExecuter { get; }
        protected IBackgroundJobStore Store { get; }
        protected BackgroundJobOptions Options { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobManager"/> class.
        /// </summary>
        public BackgroundJobWorker(
            IBackgroundJobStore store,
            AbpTimer timer,
            IBackgroundJobExecuter jobExecuter,
            IOptions<BackgroundJobOptions> options)
            : base(timer)
        {
            JobExecuter = jobExecuter;
            Store = store;
            Options = options.Value;
            Timer.Period = Options.JobPollPeriod;
        }

        protected override void DoWork()
        {
            var waitingJobs = AsyncHelper.RunSync(() => Store.GetWaitingJobsAsync(Options.MaxJobFetchCount));

            foreach (var job in waitingJobs)
            {
                JobExecuter.Execute(job);
            }
        }
    }
}