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
        protected BackgroundJobOptions Options { get; }
        protected IClock Clock { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobManager"/> class.
        /// </summary>
        public BackgroundJobWorker(
            IBackgroundJobStore store,
            AbpTimer timer,
            IBackgroundJobExecuter jobExecuter,
            IOptions<BackgroundJobOptions> options,
            IClock clock)
            : base(timer)
        {
            JobExecuter = jobExecuter;
            Clock = clock;
            Store = store;
            Options = options.Value;
            Timer.Period = Options.JobPollPeriod;
        }

        protected override void DoWork()
        {
            var waitingJobs = AsyncHelper.RunSync(() => Store.GetWaitingJobsAsync(Options.MaxJobFetchCount));

            foreach (var jobInfo in waitingJobs)
            {
                jobInfo.TryCount++;
                jobInfo.LastTryTime = Clock.Now;

                var context = new JobExecutionContext(jobInfo.JobName, jobInfo.JobArgs);

                try
                {
                    JobExecuter.Execute(context);

                    if (context.Result == JobExecutionResult.Success)
                    {
                        AsyncHelper.RunSync(() => Store.DeleteAsync(jobInfo.Id));
                    }
                    else if (context.Result == JobExecutionResult.Failed)
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
            var nextWaitDuration = Options.DefaultFirstWaitDuration * (Math.Pow(Options.DefaultWaitFactor, jobInfo.TryCount - 1));
            var nextTryDate = jobInfo.LastTryTime.HasValue
                ? jobInfo.LastTryTime.Value.AddSeconds(nextWaitDuration)
                : Clock.Now.AddSeconds(nextWaitDuration);

            if (nextTryDate.Subtract(jobInfo.CreationTime).TotalSeconds > Options.DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }
    }
}