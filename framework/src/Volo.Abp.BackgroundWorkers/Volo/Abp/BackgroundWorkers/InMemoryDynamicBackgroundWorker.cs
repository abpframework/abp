using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

public class InMemoryDynamicBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected string WorkerName { get; }
    protected IDynamicBackgroundWorkerHandlerRegistry HandlerRegistry { get; }

    public InMemoryDynamicBackgroundWorker(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IDynamicBackgroundWorkerHandlerRegistry handlerRegistry)
        : base(timer, serviceScopeFactory)
    {
        WorkerName = Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));
        HandlerRegistry = Check.NotNull(handlerRegistry, nameof(handlerRegistry));

        Timer.Period = schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod;
        CronExpression = schedule.CronExpression;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var handler = HandlerRegistry.Get(WorkerName);
        if (handler == null)
        {
            Logger.LogWarning("No dynamic background worker handler registered for: {WorkerName}", WorkerName);
            return;
        }

        await handler(new DynamicBackgroundWorkerExecutionContext(WorkerName, workerContext.ServiceProvider), workerContext.CancellationToken);
    }
}
