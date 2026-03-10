using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BackgroundWorkers;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class DynamicBackgroundWorkerManager_Tests : BackgroundJobsTestBase
{
    private readonly IBackgroundWorkerManager _backgroundWorkerManager;
    private readonly IDynamicBackgroundWorkerHandlerRegistry _handlerRegistry;

    public DynamicBackgroundWorkerManager_Tests()
    {
        _backgroundWorkerManager = GetRequiredService<IBackgroundWorkerManager>();
        _handlerRegistry = GetRequiredService<IDynamicBackgroundWorkerHandlerRegistry>();
    }

    [Fact]
    public async Task Should_Register_Dynamic_Handler_When_Added()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await _backgroundWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = 1000
            },
            (_, _) => Task.CompletedTask
        );

        _handlerRegistry.IsRegistered(workerName).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Execute_Dynamic_Handler()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        await _backgroundWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = 50
            },
            (context, _) =>
            {
                if (context.WorkerName == workerName)
                {
                    tcs.TrySetResult(true);
                }

                return Task.CompletedTask;
            }
        );

        var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(5000));
        completedTask.ShouldBe(tcs.Task);
        (await tcs.Task).ShouldBeTrue();
    }
}
