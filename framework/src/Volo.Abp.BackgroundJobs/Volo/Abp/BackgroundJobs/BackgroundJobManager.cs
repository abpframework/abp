using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
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

        protected IServiceProvider ServiceProvider { get; }
        protected IClock Clock { get; }
        protected IBackgroundJobSerializer Serializer { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IBackgroundJobStore Store { get; }

        static BackgroundJobManager()
        {
            JobPollPeriod = 5000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobManager"/> class.
        /// </summary>
        public BackgroundJobManager(
            IServiceProvider serviceProvider,
            IClock clock,
            IBackgroundJobSerializer serializer,
            IBackgroundJobStore store,
            IGuidGenerator guidGenerator,
            AbpTimer timer)
            : base(timer)
        {
            ServiceProvider = serviceProvider;
            Clock = clock;
            Serializer = serializer;
            GuidGenerator = guidGenerator;
            Store = store;

            Timer.Period = JobPollPeriod;
        }

        public async Task<Guid> EnqueueAsync<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
            where TJob : IBackgroundJob<TArgs>
        {
            var jobInfo = new BackgroundJobInfo
            {
                Id = GuidGenerator.Create(),
                JobType = typeof(TJob).AssemblyQualifiedName,
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

        public async Task<bool> DeleteAsync(Guid jobId)
        {
            var jobInfo = await Store.FindAsync(jobId);
            if (jobInfo == null)
            {
                return false;
            }

            await Store.DeleteAsync(jobInfo);
            return true;
        }

        protected override void DoWork()
        {
            var waitingJobs = AsyncHelper.RunSync(() => Store.GetWaitingJobsAsync(1000));

            foreach (var job in waitingJobs)
            {
                TryProcessJob(job);
            }
        }

        private void TryProcessJob(BackgroundJobInfo jobInfo)
        {
            try
            {
                jobInfo.TryCount++;
                jobInfo.LastTryTime = Clock.Now;

                var jobType = Type.GetType(jobInfo.JobType);
                using (var scope = ServiceProvider.CreateScope())
                {
                    var job = scope.ServiceProvider.GetService(jobType);
                    if (job == null)
                    {
                        throw new AbpException("JobType is not registered: " + jobType);
                    }

                    //TODO: Type check for the job object

                    var jobExecuteMethod = job.GetType().GetMethod("Execute");
                    Debug.Assert(jobExecuteMethod != null, nameof(jobExecuteMethod) + " != null");
                    var argsType = jobExecuteMethod.GetParameters()[0].ParameterType;
                    var argsObj = Serializer.Deserialize(jobInfo.JobArgs, argsType);

                    try
                    {
                        jobExecuteMethod.Invoke(job, new[] { argsObj });
                        AsyncHelper.RunSync(() => Store.DeleteAsync(jobInfo));
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);

                        var nextTryTime = jobInfo.CalculateNextTryTime(Clock);
                        if (nextTryTime.HasValue)
                        {
                            jobInfo.NextTryTime = nextTryTime.Value;
                        }
                        else
                        {
                            jobInfo.IsAbandoned = true;
                        }

                        TryUpdate(jobInfo);

                        var backgroundJobException = new BackgroundJobException(
                            "A background job execution is failed. See inner exception for details. See BackgroundJob property to get information on the background job.",
                            ex
                        )
                        {
                            BackgroundJob = jobInfo,
                            JobObject = job
                        };

                        //TODO: Somehow trigger an event for the exception (may create an Volo.Abp.ExceptionHandling package)!
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                jobInfo.IsAbandoned = true;

                TryUpdate(jobInfo);
            }
        }

        private void TryUpdate(BackgroundJobInfo jobInfo)
        {
            try
            {
                Store.UpdateAsync(jobInfo);
            }
            catch (Exception updateEx)
            {
                Logger.LogException(updateEx);
            }
        }
    }
}
