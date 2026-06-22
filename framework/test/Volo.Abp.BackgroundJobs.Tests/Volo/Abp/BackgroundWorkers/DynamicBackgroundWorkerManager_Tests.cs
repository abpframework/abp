using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Xunit;

namespace Volo.Abp.BackgroundWorkers;

public class DynamicBackgroundWorkerManager_Tests : BackgroundJobsTestBase
{
    private readonly IDynamicBackgroundWorkerManager _dynamicWorkerManager;

    public DynamicBackgroundWorkerManager_Tests()
    {
        _dynamicWorkerManager = GetRequiredService<IDynamicBackgroundWorkerManager>();
    }

    [Fact]
    public void Should_Report_Provider_Capabilities_Using_Marker_Interfaces()
    {
        (_dynamicWorkerManager is ISupportsRuntimeRegistration).ShouldBeTrue();
        (_dynamicWorkerManager is ISupportsCronScheduling).ShouldBeFalse();
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
    public async Task Should_Throw_When_CronExpression_Is_Set()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _dynamicWorkerManager.AddAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule
                {
                    Period = 1000,
                    CronExpression = "0 */5 * * * *"
                },
                (_, _) => Task.CompletedTask
            );
        });
    }

    [Fact]
    public async Task Should_Throw_When_CronExpression_Is_Set_On_UpdateSchedule()
    {
        var workerName = "dynamic-worker-" + Guid.NewGuid();

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule { Period = 1000 },
            (_, _) => Task.CompletedTask
        );

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _dynamicWorkerManager.UpdateScheduleAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule
                {
                    Period = 1000,
                    CronExpression = "0 */5 * * * *"
                }
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

    [Fact]
    public async Task Should_Handle_Concurrent_Add_With_Same_Name()
    {
        var workerName = "concurrent-worker-" + Guid.NewGuid();
        var executedHandlerIds = new ConcurrentBag<int>();

        var tasks = Enumerable.Range(0, 10).Select(i =>
            _dynamicWorkerManager.AddAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule { Period = 60000 },
                (_, _) =>
                {
                    executedHandlerIds.Add(i);
                    return Task.CompletedTask;
                }
            )
        ).ToList();

        await Task.WhenAll(tasks);

        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeTrue();

        var removed = await _dynamicWorkerManager.RemoveAsync(workerName);
        removed.ShouldBeTrue();
        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Handle_Concurrent_Add_And_Remove()
    {
        var workerNames = Enumerable.Range(0, 10)
            .Select(i => $"concurrent-worker-{i}-" + Guid.NewGuid())
            .ToList();

        var addTasks = workerNames.Select(name =>
            _dynamicWorkerManager.AddAsync(
                name,
                new DynamicBackgroundWorkerSchedule { Period = 60000 },
                (_, _) => Task.CompletedTask
            )
        ).ToList();

        await Task.WhenAll(addTasks);

        foreach (var name in workerNames)
        {
            _dynamicWorkerManager.IsRegistered(name).ShouldBeTrue();
        }

        var removeTasks = workerNames.Select(name =>
            _dynamicWorkerManager.RemoveAsync(name)
        ).ToList();

        var results = await Task.WhenAll(removeTasks);

        results.ShouldAllBe(r => r);

        foreach (var name in workerNames)
        {
            _dynamicWorkerManager.IsRegistered(name).ShouldBeFalse();
        }
    }

    [Fact]
    public async Task Should_Handle_Concurrent_Add_Remove_Update()
    {
        var workerName = "concurrent-mixed-" + Guid.NewGuid();

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule { Period = 60000 },
            (_, _) => Task.CompletedTask
        );

        var tasks = new List<Task>
        {
            _dynamicWorkerManager.AddAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule { Period = 30000 },
                (_, _) => Task.CompletedTask
            ),
            _dynamicWorkerManager.UpdateScheduleAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule { Period = 20000 }
            ),
            _dynamicWorkerManager.AddAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule { Period = 10000 },
                (_, _) => Task.CompletedTask
            )
        };

        await Task.WhenAll(tasks);

        // After all concurrent operations, worker should still be in a consistent state
        var isRegistered = _dynamicWorkerManager.IsRegistered(workerName);
        isRegistered.ShouldBeTrue();

        var removed = await _dynamicWorkerManager.RemoveAsync(workerName);
        removed.ShouldBeTrue();
        _dynamicWorkerManager.IsRegistered(workerName).ShouldBeFalse();
    }
}
