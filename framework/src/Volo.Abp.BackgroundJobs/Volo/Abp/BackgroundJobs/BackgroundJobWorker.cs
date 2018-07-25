using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobWorker : PeriodicBackgroundWorkerBase, IBackgroundJobWorker, ISingletonDependency
    {
        protected IBackgroundJobExecuter JobExecuter { get; }
        protected IBackgroundJobStore Store { get; }
        protected BackgroundJobOptions JobOptions { get; }
        protected BackgroundJobWorkerOptions WorkerOptions { get; }
        protected IClock Clock { get; }
        protected IBackgroundJobSerializer Serializer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBackgroundJobManager"/> class.
        /// </summary>
        public BackgroundJobWorker(
            IBackgroundJobStore store,
            AbpTimer timer,
            IBackgroundJobExecuter jobExecuter,
            IBackgroundJobSerializer serializer,
            IOptions<BackgroundJobOptions> jobOptions,
            IOptions<BackgroundJobWorkerOptions> workerOptions,
            IClock clock)
            : base(timer)
        {
            JobExecuter = jobExecuter;
            Serializer = serializer;
            Clock = clock;
            Store = store;
            WorkerOptions = workerOptions.Value;
            JobOptions = jobOptions.Value;
            Timer.Period = WorkerOptions.JobPollPeriod;
        }

        protected override void DoWork()
        {
            var waitingJobs = AsyncHelper.RunSync(() => Store.GetWaitingJobsAsync(WorkerOptions.MaxJobFetchCount));

            foreach (var jobInfo in waitingJobs)
            {
                jobInfo.TryCount++;
                jobInfo.LastTryTime = Clock.Now;

                try
                {
                    var jobType = JobOptions.GetJobType(jobInfo.JobName);
                    var jobArgsType = BackgroundJobArgsHelper.GetJobArgsType(jobType);
                    var jobArgs = Serializer.Deserialize(jobInfo.JobArgs, jobArgsType);

                    var context = new JobExecutionContext(jobType, jobArgs);

                    try
                    {
                        JobExecuter.Execute(context);
                        AsyncHelper.RunSync(() => Store.DeleteAsync(jobInfo.Id));
                    }
                    catch (BackgroundJobExecutionException)
                    {
                        var nextTryTime = CalculateNextTryTime(jobInfo);
                        if (nextTryTime.HasValue)
                        {
                            jobInfo.NextTryTime = nextTryTime.Value;
                        }
                        else
                        {
                            jobInfo.IsAbandoned = true;
                        }

                        TryUpdate(jobInfo);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    jobInfo.IsAbandoned = true;
                    TryUpdate(jobInfo);
                }
            }
        }

        protected virtual void TryUpdate(BackgroundJobInfo jobInfo)
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

        protected virtual DateTime? CalculateNextTryTime(BackgroundJobInfo jobInfo) //TODO: Move to another place to override easier
        {
            var nextWaitDuration = WorkerOptions.DefaultFirstWaitDuration * (Math.Pow(WorkerOptions.DefaultWaitFactor, jobInfo.TryCount - 1));
            var nextTryDate = jobInfo.LastTryTime.HasValue
                ? jobInfo.LastTryTime.Value.AddSeconds(nextWaitDuration)
                : Clock.Now.AddSeconds(nextWaitDuration);

            if (nextTryDate.Subtract(jobInfo.CreationTime).TotalSeconds > WorkerOptions.DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }
    }
}