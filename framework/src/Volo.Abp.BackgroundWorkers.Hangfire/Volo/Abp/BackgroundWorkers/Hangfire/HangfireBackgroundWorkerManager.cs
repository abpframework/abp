using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

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

    public Task AddAsync(IBackgroundWorker worker)
    {
        if (worker is IHangfireBackgroundWorker hangfireBackgroundWorker)
        {
            var unProxyWorker = ProxyHelper.UnProxy(hangfireBackgroundWorker);
            if (hangfireBackgroundWorker.RecurringJobId.IsNullOrWhiteSpace())
            {
                RecurringJob.AddOrUpdate(() => ((IHangfireBackgroundWorker)unProxyWorker).DoWorkAsync(),hangfireBackgroundWorker.CronExpression);
            }
            else
            {
                RecurringJob.AddOrUpdate(hangfireBackgroundWorker.RecurringJobId,() => ((IHangfireBackgroundWorker)unProxyWorker).DoWorkAsync(),
                    hangfireBackgroundWorker.CronExpression);
            }
        }
        else
        {
            int? period;

            if (worker is AsyncPeriodicBackgroundWorkerBase or PeriodicBackgroundWorkerBase)
            {
                var timer = worker.GetType()
                    .GetProperty("Timer", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(worker);

                if (worker is AsyncPeriodicBackgroundWorkerBase)
                {
                    period = ((AbpAsyncTimer)timer)?.Period;
                }
                else
                {
                    period = ((AbpTimer)timer)?.Period;
                }
            }
            else
            {
                return Task.CompletedTask;
            }

            if (period == null)
            {
                return Task.CompletedTask;
            }

            var adapterType = typeof(HangfirePeriodicBackgroundWorkerAdapter<>).MakeGenericType(ProxyHelper.GetUnProxiedType(worker));
            var workerAdapter = Activator.CreateInstance(adapterType) as IHangfireBackgroundWorker;

            RecurringJob.AddOrUpdate(() => workerAdapter.DoWorkAsync(), GetCron(period.Value));
        }

        return Task.CompletedTask;
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
            throw new AbpException($"Cannot convert period: {period} to cron expression, use HangfireBackgroundWorkerBase to define worker");
        }

        return cron;
    }
}
