using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobWorker : PeriodicBackgroundWorkerBase, IBackgroundJobWorker, ISingletonDependency
    {
        /// <summary>
        /// Interval between polling jobs from <see cref="IBackgroundJobStore"/>.
        /// Default value: 5000 (5 seconds).
        /// </summary>
        public static int JobPollPeriod { get; set; } //TODO: Move to options

        protected IBackgroundJobExecuter JobExecuter { get; }
        protected IBackgroundJobStore Store { get; }

        static BackgroundJobWorker()
        {
            JobPollPeriod = 5000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobManager"/> class.
        /// </summary>
        public BackgroundJobWorker(
            IBackgroundJobStore store,
            AbpTimer timer,
            IBackgroundJobExecuter jobExecuter)
            : base(timer)
        {
            JobExecuter = jobExecuter;
            Store = store;

            Timer.Period = JobPollPeriod;
        }

        protected override void DoWork()
        {
            var waitingJobs = AsyncHelper.RunSync(() => Store.GetWaitingJobsAsync(1000));

            foreach (var job in waitingJobs)
            {
                JobExecuter.Execute(job);
            }
        }
    }
}