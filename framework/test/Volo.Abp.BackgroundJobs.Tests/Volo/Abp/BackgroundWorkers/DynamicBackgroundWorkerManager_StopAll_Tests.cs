using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Xunit;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Isolated tests for <see cref="IDynamicBackgroundWorkerManager.StopAllAsync"/>.
/// Kept in a separate class so the singleton manager is fresh per test (xUnit creates
/// a new test class instance — and therefore a new ABP application — for each test method).
/// </summary>
public class DynamicBackgroundWorkerManager_StopAll_Tests : BackgroundJobsTestBase
{
    private readonly IDynamicBackgroundWorkerManager _dynamicWorkerManager;

    public DynamicBackgroundWorkerManager_StopAll_Tests()
    {
        _dynamicWorkerManager = GetRequiredService<IDynamicBackgroundWorkerManager>();
    }

    [Fact]
    public async Task Should_Stop_All_Workers_And_Clear_Registry()
    {
        var workerName1 = "stop-all-worker-1-" + Guid.NewGuid();
        var workerName2 = "stop-all-worker-2-" + Guid.NewGuid();

        await _dynamicWorkerManager.AddAsync(
            workerName1,
            new DynamicBackgroundWorkerSchedule { Period = 60000 },
            (_, _) => Task.CompletedTask
        );
        await _dynamicWorkerManager.AddAsync(
            workerName2,
            new DynamicBackgroundWorkerSchedule { Period = 60000 },
            (_, _) => Task.CompletedTask
        );

        _dynamicWorkerManager.IsRegistered(workerName1).ShouldBeTrue();
        _dynamicWorkerManager.IsRegistered(workerName2).ShouldBeTrue();

        await _dynamicWorkerManager.StopAllAsync();

        _dynamicWorkerManager.IsRegistered(workerName1).ShouldBeFalse();
        _dynamicWorkerManager.IsRegistered(workerName2).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Throw_ObjectDisposedException_When_AddAsync_Called_After_StopAllAsync()
    {
        await _dynamicWorkerManager.StopAllAsync();

        await Assert.ThrowsAsync<ObjectDisposedException>(() =>
            _dynamicWorkerManager.AddAsync(
                "post-stop-worker-" + Guid.NewGuid(),
                new DynamicBackgroundWorkerSchedule { Period = 1000 },
                (_, _) => Task.CompletedTask
            )
        );
    }

    [Fact]
    public async Task Should_Throw_ObjectDisposedException_When_UpdateScheduleAsync_Called_After_StopAllAsync()
    {
        var workerName = "update-after-stop-" + Guid.NewGuid();

        await _dynamicWorkerManager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule { Period = 60000 },
            (_, _) => Task.CompletedTask
        );

        await _dynamicWorkerManager.StopAllAsync();

        await Assert.ThrowsAsync<ObjectDisposedException>(() =>
            _dynamicWorkerManager.UpdateScheduleAsync(
                workerName,
                new DynamicBackgroundWorkerSchedule { Period = 1000 }
            )
        );
    }

    [Fact]
    public async Task Should_Be_Idempotent_When_StopAllAsync_Called_Multiple_Times()
    {
        await _dynamicWorkerManager.AddAsync(
            "idempotent-stop-" + Guid.NewGuid(),
            new DynamicBackgroundWorkerSchedule { Period = 60000 },
            (_, _) => Task.CompletedTask
        );

        // Should not throw when called multiple times
        await _dynamicWorkerManager.StopAllAsync();
        await _dynamicWorkerManager.StopAllAsync();
    }
}
