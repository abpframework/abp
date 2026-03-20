using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BackgroundWorkers;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class DynamicBackgroundWorkerManager_Tests : BackgroundJobsTestBase
{
    private readonly IDynamicBackgroundWorkerManager _dynamicWorkerManager;

    public DynamicBackgroundWorkerManager_Tests()
    {
        _dynamicWorkerManager = GetRequiredService<IDynamicBackgroundWorkerManager>();
    }

    [Fact]
    public async Task Should_Register_Dynamic_Worker()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = 1000
            },
            (_, _) => Task.CompletedTask
        );

        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Execute_Dynamic_Handler()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        await _dynamicWorkerManager.AddAsync(
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
    public async Task Should_Add_Dynamic_Worker_With_Default_Schedule()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await _dynamicWorkerManager.AddAsync(
            workerName,
            (_, _) => Task.CompletedTask
        );

        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Remove_Dynamic_Worker()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = 1000
            },
            (_, _) => Task.CompletedTask
        );

        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeTrue();

        var result = await _dynamicWorkerManager.RemoveAsync(workerName);
        result.ShouldBeTrue();
        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_When_Removing_NonExistent_Worker()
    {
        var result = await _dynamicWorkerManager.RemoveAsync("non-existent-worker-" + Guid.NewGuid());
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Update_Dynamic_Worker_Schedule()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();
        var executionCount = 0;

        await _dynamicWorkerManager.AddAsync(
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

        var result = await _dynamicWorkerManager.UpdateScheduleAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = 50
            }
        );

        result.ShouldBeTrue();
        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeTrue();

        var timeout = TimeSpan.FromSeconds(5);
        var startTime = DateTime.UtcNow;
        while (executionCount == 0 && DateTime.UtcNow - startTime < timeout)
        {
            await Task.Delay(50);
        }

        executionCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Return_False_When_Updating_NonExistent_Worker()
    {
        var result = await _dynamicWorkerManager.UpdateScheduleAsync(
            "non-existent-worker-" + Guid.NewGuid(),
            new DynamicBackgroundWorkerSchedule { Period = 1000 }
        );

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Replace_Existing_Worker_When_Same_Name_Added()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();
        var secondHandlerTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule { Period = 60000 },
            (_, _) => Task.CompletedTask
        );

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule { Period = 50 },
            (_, _) =>
            {
                secondHandlerTcs.TrySetResult(true);
                return Task.CompletedTask;
            }
        );

        var completedTask = await Task.WhenAny(secondHandlerTcs.Task, Task.Delay(5000));
        completedTask.ShouldBe(secondHandlerTcs.Task);
        (await secondHandlerTcs.Task).ShouldBeTrue();

        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeTrue();

        var removed = await _dynamicWorkerManager.RemoveAsync(workerName);
        removed.ShouldBeTrue();
        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Throw_When_Period_Is_Zero()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _dynamicWorkerManager.AddAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule { Period = 0 },
                (_, _) => Task.CompletedTask
            );
        });
    }

    [Fact]
    public async Task Should_Throw_When_Period_Is_Negative()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _dynamicWorkerManager.AddAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule { Period = -1000 },
                (_, _) => Task.CompletedTask
            );
        });
    }

    [Fact]
    public async Task Should_Throw_When_No_Period_And_No_CronExpression()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _dynamicWorkerManager.AddAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule(),
                (_, _) => Task.CompletedTask
            );
        });
    }

    [Fact]
    public async Task Should_Continue_Running_After_Handler_Throws_Exception()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();
        var callCount = 0;
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule { Period = 50 },
            (_, _) =>
            {
                var count = Interlocked.Increment(ref callCount);
                if (count == 1)
                {
                    throw new InvalidOperationException("Simulated failure");
                }

                tcs.TrySetResult(true);
                return Task.CompletedTask;
            }
        );

        var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(5000));
        completedTask.ShouldBe(tcs.Task);
        callCount.ShouldBeGreaterThan(1);
    }

    [Fact]
    public async Task Should_Not_Be_Registered_After_Remove()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();
        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeFalse();

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule { Period = 1000 },
            (_, _) => Task.CompletedTask
        );

        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeTrue();

        await _dynamicWorkerManager.RemoveAsync(workerName);

        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeFalse();
    }
}
