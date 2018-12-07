using System;
using Microsoft.Extensions.DependencyInjection;
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
        protected BackgroundJobOptions JobOptions { get; }
        protected BackgroundJobWorkerOptions WorkerOptions { get; }
        protected IClock Clock { get; }
        protected IBackgroundJobSerializer Serializer { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public BackgroundJobWorker(
            AbpTimer timer,
            IBackgroundJobExecuter jobExecuter,
            IBackgroundJobSerializer serializer,
            IOptions<BackgroundJobOptions> jobOptions,
            IOptions<BackgroundJobWorkerOptions> workerOptions,
            IClock clock, 
            IServiceScopeFactory serviceScopeFactory)
            : base(timer)
        {
            JobExecuter = jobExecuter;
            Serializer = serializer;
            Clock = clock;
            ServiceScopeFactory = serviceScopeFactory;
            WorkerOptions = workerOptions.Value;
            JobOptions = jobOptions.Value;
            Timer.Period = WorkerOptions.JobPollPeriod;
        }

        protected override void DoWork()
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var store = scope.ServiceProvider.GetRequiredService<IBackgroundJobStore>();

                var waitingJobs = AsyncHelper.RunSync(
                    () => store.GetWaitingJobsAsync(WorkerOptions.MaxJobFetchCount)
                );

                foreach (var jobInfo in waitingJobs)
                {
                    jobInfo.TryCount++;
                    jobInfo.LastTryTime = Clock.Now;

                    try
                    {
                        var jobConfiguration = JobOptions.GetJob(jobInfo.JobName);
                        var jobArgs = Serializer.Deserialize(jobInfo.JobArgs, jobConfiguration.ArgsType);
                        var context = new JobExecutionContext(scope.ServiceProvider, jobConfiguration.JobType, jobArgs);

                        try
                        {
                            JobExecuter.Execute(context);
                            AsyncHelper.RunSync(() => store.DeleteAsync(jobInfo.Id));
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

                            TryUpdate(store, jobInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        jobInfo.IsAbandoned = true;
                        TryUpdate(store, jobInfo);
                    }
                }
            }
        }

        protected virtual void TryUpdate(IBackgroundJobStore store, BackgroundJobInfo jobInfo)
        {
            try
            {
                store.UpdateAsync(jobInfo);
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