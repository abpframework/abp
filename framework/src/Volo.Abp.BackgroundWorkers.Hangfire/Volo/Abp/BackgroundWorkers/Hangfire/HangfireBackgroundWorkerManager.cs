using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Hangfire;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IBackgroundWorkerManager), typeof(HangfireBackgroundWorkerManager))]
public class HangfireBackgroundWorkerManager : BackgroundWorkerManager, ISingletonDependency
{
    protected AbpHangfireBackgroundJobServer BackgroundJobServer { get; set; } = default!;
    protected IServiceProvider ServiceProvider { get; }

    public HangfireBackgroundWorkerManager(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public void Initialize()
    {
        BackgroundJobServer = ServiceProvider.GetRequiredService<AbpHangfireBackgroundJobServer>();
    }

    public async override Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        var abpHangfireOptions = ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
        var defaultQueuePrefix = abpHangfireOptions.DefaultQueuePrefix;
        var defaultQueue = abpHangfireOptions.DefaultQueue;

        switch (worker)
        {
            case IHangfireBackgroundWorker hangfireBackgroundWorker:
            {
                var unProxyWorker = ProxyHelper.UnProxy(hangfireBackgroundWorker);

                RecurringJob.AddOrUpdate(
                    hangfireBackgroundWorker.RecurringJobId,
                    hangfireBackgroundWorker.Queue.IsNullOrWhiteSpace() ? defaultQueue : defaultQueuePrefix + hangfireBackgroundWorker.Queue,
                    () => ((IHangfireBackgroundWorker)unProxyWorker).DoWorkAsync(cancellationToken),
                    hangfireBackgroundWorker.CronExpression,
                    new RecurringJobOptions
                    {
                        TimeZone = hangfireBackgroundWorker.TimeZone
                    });

                break;
            }
            case AsyncPeriodicBackgroundWorkerBase or PeriodicBackgroundWorkerBase:
            {
                int? period = null;
                string? CronExpression = null;

                if (worker is AsyncPeriodicBackgroundWorkerBase asyncPeriodicBackgroundWorkerBase)
                {
                    period = asyncPeriodicBackgroundWorkerBase.Period;
                    CronExpression = asyncPeriodicBackgroundWorkerBase.CronExpression;
                }
                else if (worker is PeriodicBackgroundWorkerBase periodicBackgroundWorkerBase)
                {
                    period = periodicBackgroundWorkerBase.Period;
                    CronExpression = periodicBackgroundWorkerBase.CronExpression;
                }

                if (period == null && CronExpression.IsNullOrWhiteSpace())
                {
                    return;
                }

                var adapterType = typeof(HangfirePeriodicBackgroundWorkerAdapter<>).MakeGenericType(ProxyHelper.GetUnProxiedType(worker));
                var workerAdapter = (Activator.CreateInstance(adapterType) as IHangfireBackgroundWorker)!;

                Expression<Func<Task>> methodCall = () => workerAdapter.DoWorkAsync(cancellationToken);
                var recurringJobId = !workerAdapter.RecurringJobId.IsNullOrWhiteSpace() ? workerAdapter.RecurringJobId : GetRecurringJobId(worker, methodCall);

                RecurringJob.AddOrUpdate(
                    recurringJobId,
                    workerAdapter.Queue.IsNullOrWhiteSpace() ? defaultQueue : defaultQueuePrefix + workerAdapter.Queue,
                    methodCall,
                    CronExpression ?? GetCron(period!.Value),
                    new RecurringJobOptions
                    {
                        TimeZone = workerAdapter.TimeZone
                    });
                break;
            }
            default:
                await base.AddAsync(worker, cancellationToken);
                break;
        }
    }

    private readonly static MethodInfo? GetRecurringJobIdMethodInfo = typeof(RecurringJob).GetMethod("GetRecurringJobId", BindingFlags.NonPublic | BindingFlags.Static);
    protected virtual string? GetRecurringJobId(IBackgroundWorker worker, Expression<Func<Task>> methodCall)
    {
        string? recurringJobId = null;
        if (GetRecurringJobIdMethodInfo != null)
        {
            var job = Job.FromExpression(methodCall);
            recurringJobId = (string)GetRecurringJobIdMethodInfo.Invoke(null, [job])!;
        }

        if (recurringJobId.IsNullOrWhiteSpace())
        {
            recurringJobId = $"HangfirePeriodicBackgroundWorkerAdapter<{worker.GetType().Name}>.DoWorkAsync";
        }

        return recurringJobId;
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
        else if(time.TotalDays <= 31)
        {
            cron = $"0 0 0 1/{time.TotalDays} * *";
        }
        else
        {
            throw new AbpException($"Cannot convert period: {period} to cron expression, use HangfireBackgroundWorkerBase to define worker");
        }

        return cron;
    }
}
