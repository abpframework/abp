using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    //TODO: Split enqueueing & background worker!

    /// <summary>
    /// Default implementation of <see cref="IBackgroundJobManager"/>.
    /// </summary>
    public class BackgroundJobManager : PeriodicBackgroundWorkerBase, IBackgroundJobManager, ISingletonDependency
    {
        /// <summary>
        /// Interval between polling jobs from <see cref="IBackgroundJobStore"/>.
        /// Default value: 5000 (5 seconds).
        /// </summary>
        public static int JobPollPeriod { get; set; } //TODO: Move to options

        protected IClock Clock { get; }
        protected IBackgroundJobSerializer Serializer { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }
        protected IBackgroundJobStore Store { get; }

        static BackgroundJobManager()
        {
            JobPollPeriod = 5000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobManager"/> class.
        /// </summary>
        public BackgroundJobManager(
            IClock clock,
            IBackgroundJobSerializer serializer,
            IBackgroundJobStore store,
            IGuidGenerator guidGenerator,
            AbpTimer timer, 
            IBackgroundJobExecuter jobExecuter)
            : base(timer)
        {
            Clock = clock;
            Serializer = serializer;
            GuidGenerator = guidGenerator;
            JobExecuter = jobExecuter;
            Store = store;

            Timer.Period = JobPollPeriod;
        }

        public Task<Guid> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
        {
            var jobName = BackgroundJobNameAttribute.GetNameOrNull<TArgs>();
            return EnqueueAsync(jobName, args, priority, delay);
        }

        public async Task<Guid> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
        {
            var jobInfo = new BackgroundJobInfo
            {
                Id = GuidGenerator.Create(),
                JobName = jobName,
                JobArgs = Serializer.Serialize(args),
                Priority = priority,
                CreationTime = Clock.Now,
                NextTryTime = Clock.Now
            };

            if (delay.HasValue)
            {
                jobInfo.NextTryTime = Clock.Now.Add(delay.Value);
            }

            await Store.InsertAsync(jobInfo);

            return jobInfo.Id;
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
