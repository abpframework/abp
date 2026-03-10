using System;
using System.Threading;
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

    [Fact]
    public async Task Should_Remove_Dynamic_Worker()
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

        var result = await _backgroundWorkerManager.RemoveAsync(workerName);
        result.ShouldBeTrue();
        _handlerRegistry.IsRegistered(workerName).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_When_Removing_NonExistent_Worker()
    {
        var result = await _backgroundWorkerManager.RemoveAsync("non-existent-worker-" + Guid.NewGuid());
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Update_Dynamic_Worker_Schedule()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();
        var executionCount = 0;

        await _backgroundWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = 60000
            },
            (_, _) =>
            {
                Interlocked.Increment(ref executionCount);
                return Task.CompletedTask;
            }
        );

        var result = await _backgroundWorkerManager.UpdateScheduleAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = 50
            }
        );

        result.ShouldBeTrue();
        _handlerRegistry.IsRegistered(workerName).ShouldBeTrue();

        await Task.Delay(500);
        executionCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Return_False_When_Updating_NonExistent_Worker()
    {
        var result = await _backgroundWorkerManager.UpdateScheduleAsync(
            "non-existent-worker-" + Guid.NewGuid(),
            new DynamicBackgroundWorkerSchedule { Period = 1000 }
        );

        result.ShouldBeFalse();
    }
}
