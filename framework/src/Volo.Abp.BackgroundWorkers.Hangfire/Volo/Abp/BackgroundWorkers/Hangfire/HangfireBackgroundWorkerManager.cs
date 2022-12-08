using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Hangfire;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

[Dependency(ReplaceServices = true)]
public class HangfireBackgroundWorkerManager : BackgroundWorkerManager, ISingletonDependency
{
    protected AbpHangfireBackgroundJobServer BackgroundJobServer { get; set; }
    protected IServiceProvider ServiceProvider { get; }

    public HangfireBackgroundWorkerManager(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async override Task StartAsync(CancellationToken cancellationToken = default)
    {
        BackgroundJobServer = ServiceProvider.GetRequiredService<AbpHangfireBackgroundJobServer>();
        await base.StartAsync(cancellationToken);
    }

    public async override Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        switch (worker)
        {
            case IHangfireBackgroundWorker hangfireBackgroundWorker:
            {
                var unProxyWorker = ProxyHelper.UnProxy(hangfireBackgroundWorker);
                if (hangfireBackgroundWorker.RecurringJobId.IsNullOrWhiteSpace())
                {
                    RecurringJob.AddOrUpdate(
                        () => ((IHangfireBackgroundWorker)unProxyWorker).DoWorkAsync(cancellationToken),
                        hangfireBackgroundWorker.CronExpression, hangfireBackgroundWorker.TimeZone,
                        hangfireBackgroundWorker.Queue);
                }
                else
                {
                    RecurringJob.AddOrUpdate(hangfireBackgroundWorker.RecurringJobId,
                        () => ((IHangfireBackgroundWorker)unProxyWorker).DoWorkAsync(cancellationToken),
                        hangfireBackgroundWorker.CronExpression, hangfireBackgroundWorker.TimeZone,
                        hangfireBackgroundWorker.Queue);
                }

                break;
            }
            case AsyncPeriodicBackgroundWorkerBase or PeriodicBackgroundWorkerBase:
            {
                var timer = worker.GetType()
                    .GetProperty("Timer", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(worker);

                var period = worker is AsyncPeriodicBackgroundWorkerBase ? ((AbpAsyncTimer)timer)?.Period : ((AbpTimer)timer)?.Period;

                if (period == null)
                {
                    return;
                }

                var adapterType = typeof(HangfirePeriodicBackgroundWorkerAdapter<>).MakeGenericType(ProxyHelper.GetUnProxiedType(worker));
                var workerAdapter = Activator.CreateInstance(adapterType) as IHangfireBackgroundWorker;

                RecurringJob.AddOrUpdate(() => workerAdapter.DoWorkAsync(cancellationToken), GetCron(period.Value), workerAdapter.TimeZone, workerAdapter.Queue);

                break;
            }
            default:
                await base.AddAsync(worker, cancellationToken);
                break;
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
            throw new AbpException(
                $"Cannot convert period: {period} to cron expression, use HangfireBackgroundWorkerBase to define worker");
        }

        return cron;
    }
}