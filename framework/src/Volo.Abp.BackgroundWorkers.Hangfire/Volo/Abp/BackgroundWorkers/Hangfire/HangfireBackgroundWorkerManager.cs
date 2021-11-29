﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    [Dependency(ReplaceServices = true)]
    public class HangfireBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
    {
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Add(IBackgroundWorker worker)
        {
            if (worker is IHangfireBackgroundWorker hangfireBackgroundWorker)
            {
                if (hangfireBackgroundWorker.RecurringJobId.IsNullOrWhiteSpace())
                {
                    RecurringJob.AddOrUpdate(() => hangfireBackgroundWorker.DoWorkAsync(),
                        hangfireBackgroundWorker.CronExpression);
                }
                else
                {
                    RecurringJob.AddOrUpdate(hangfireBackgroundWorker.RecurringJobId,() => hangfireBackgroundWorker.DoWorkAsync(),
                        hangfireBackgroundWorker.CronExpression);
                }
            }
            else
            {
                int? period;

                if (worker is AsyncPeriodicBackgroundWorkerBase or PeriodicBackgroundWorkerBase)
                {
                    var timer = (AbpTimer) worker.GetType()
                        .GetProperty("Timer", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(worker);
                    period = timer?.Period;
                }
                else
                {
                    return;
                }

                if (period == null)
                {
                    return;
                }

                var adapterType = typeof(HangfirePeriodicBackgroundWorkerAdapter<>).MakeGenericType(worker.GetType());
                var workerAdapter = Activator.CreateInstance(adapterType) as IHangfireBackgroundWorker;

                RecurringJob.AddOrUpdate(() => workerAdapter.DoWorkAsync(), GetCron(period.Value));
            }
        }

        protected virtual string GetCron(int period)
        {
            var time = TimeSpan.FromMilliseconds(period);
            string cron;

            if (time.TotalSeconds <= 59)
            {
                cron = $"*/{time.TotalSeconds} * * * * *";
            }
            else if (time.TotalMinutes <= 59)
            {
                cron = $"*/{time.TotalMinutes} * * * *";
            }
            else if (time.TotalHours <= 23)
            {
                cron = $"0 */{time.TotalHours} * * *";
            }
            else
            {
                cron = $"0 0 */{time.TotalDays} * *";
            }

            return cron;
        }
    }
}
