using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobWorker : PeriodicBackgroundWorkerBase, IBackgroundJobWorker
    {
        protected AbpBackgroundJobOptions JobOptions { get; }

        protected AbpBackgroundJobWorkerOptions WorkerOptions { get; }

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public BackgroundJobWorker(
            AbpTimer timer,
            IOptions<AbpBackgroundJobOptions> jobOptions,
            IOptions<AbpBackgroundJobWorkerOptions> workerOptions,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer)
        {
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

                var waitingJobs = store.GetWaitingJobs(WorkerOptions.MaxJobFetchCount);

                if (!waitingJobs.Any())
                {
                    return;
                }

                var jobExecuter = scope.ServiceProvider.GetRequiredService<IBackgroundJobExecuter>();
                var clock = scope.ServiceProvider.GetRequiredService<IClock>();
                var serializer = scope.ServiceProvider.GetRequiredService<IBackgroundJobSerializer>();

                foreach (var jobInfo in waitingJobs)
                {
                    jobInfo.TryCount++;
                    jobInfo.LastTryTime = clock.Now;

                    try
                    {
                        var jobConfiguration = JobOptions.GetJob(jobInfo.JobName);
                        var jobArgs = serializer.Deserialize(jobInfo.JobArgs, jobConfiguration.ArgsType);
                        var context = new JobExecutionContext(scope.ServiceProvider, jobConfiguration.JobType, jobArgs);

                        try
                        {
                            jobExecuter.Execute(context);

                            store.Delete(jobInfo.Id);
                        }
                        catch (BackgroundJobExecutionException)
                        {
                            var nextTryTime = CalculateNextTryTime(jobInfo, clock);

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
                store.Update(jobInfo);
            }
            catch (Exception updateEx)
            {
                Logger.LogException(updateEx);
            }
        }

        protected virtual DateTime? CalculateNextTryTime(BackgroundJobInfo jobInfo, IClock clock)
        {
            var nextWaitDuration = WorkerOptions.DefaultFirstWaitDuration * (Math.Pow(WorkerOptions.DefaultWaitFactor, jobInfo.TryCount - 1));
            var nextTryDate = jobInfo.LastTryTime?.AddSeconds(nextWaitDuration) ??
                              clock.Now.AddSeconds(nextWaitDuration);

            if (nextTryDate.Subtract(jobInfo.CreationTime).TotalSeconds > WorkerOptions.DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }
    }
}