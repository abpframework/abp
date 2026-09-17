using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

[DisableConventionalRegistration]
public class InMemoryDynamicBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    public string WorkerName { get; }

    private readonly DynamicBackgroundWorkerHandler _handler;

    public InMemoryDynamicBackgroundWorker(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        DynamicBackgroundWorkerHandler handler,
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory)
        : base(timer, serviceScopeFactory)
    {
        WorkerName = Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));
        _handler = Check.NotNull(handler, nameof(handler));

        Timer.Period = schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod;
        CronExpression = schedule.CronExpression;
    }

    public virtual void UpdateSchedule(DynamicBackgroundWorkerSchedule schedule)
    {
        Check.NotNull(schedule, nameof(schedule));

        Timer.Stop();
        Timer.Period = schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod;
        CronExpression = schedule.CronExpression;
        Timer.Start(StartCancellationToken);
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await _handler(
            new DynamicBackgroundWorkerExecutionContext(WorkerName, workerContext.ServiceProvider),
            workerContext.CancellationToken);
    }

    public override string ToString()
    {
        return $"DynamicWorker:{WorkerName}";
    }
}
