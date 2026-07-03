using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Testing;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobCleanupWorker_Tests : AbpIntegratedTest<AbpBackgroundJobCleanupTestModule>
{
    private readonly IBackgroundJobStore _store;
    private readonly IClock _clock;
    private readonly AbpBackgroundJobWorkerOptions _workerOptions;

    public BackgroundJobCleanupWorker_Tests()
    {
        _store = GetRequiredService<IBackgroundJobStore>();
        _clock = GetRequiredService<IClock>();
        _workerOptions = GetRequiredService<IOptions<AbpBackgroundJobWorkerOptions>>().Value;
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    private TestableBackgroundJobCleanupWorker CreateWorker()
    {
        return new TestableBackgroundJobCleanupWorker(
            GetRequiredService<AbpAsyncTimer>(),
            GetRequiredService<IServiceScopeFactory>(),
            GetRequiredService<IOptions<AbpBackgroundJobOptions>>(),
            GetRequiredService<IOptions<AbpBackgroundJobWorkerOptions>>(),
            GetRequiredService<IAbpDistributedLock>());
    }

    private Task RunCleanupAsync()
    {
        return CreateWorker().DoWorkPublicAsync(new PeriodicBackgroundWorkerContext(ServiceProvider));
    }

    private async Task<Guid> InsertCompletedJobAsync(DateTime completionTime)
    {
        var id = Guid.NewGuid();
        await _store.InsertAsync(new BackgroundJobInfo
        {
            Id = id,
            JobName = "job-a",
            JobArgs = "{}",
            CreationTime = _clock.Now,
            NextTryTime = _clock.Now,
            CompletionTime = completionTime
        });
        return id;
    }

    [Fact]
    public async Task Should_Delete_Completed_Jobs_Older_Than_Retention()
    {
        // Retention is 1 day (configured in the module).
        var oldJobId = await InsertCompletedJobAsync(_clock.Now.Subtract(TimeSpan.FromDays(2)));
        var recentJobId = await InsertCompletedJobAsync(_clock.Now);

        await RunCleanupAsync();

        (await _store.FindAsync(oldJobId)).ShouldBeNull();       // older than retention → deleted
        (await _store.FindAsync(recentJobId)).ShouldNotBeNull(); // within retention → kept
    }

    [Fact]
    public async Task Should_Not_Delete_When_StoreSuccessfulJobs_Disabled()
    {
        _workerOptions.StoreSuccessfulJobs = false;

        var oldJobId = await InsertCompletedJobAsync(_clock.Now.Subtract(TimeSpan.FromDays(2)));

        await RunCleanupAsync();

        (await _store.FindAsync(oldJobId)).ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Not_Delete_When_Retention_Is_Null()
    {
        _workerOptions.SuccessfulJobRetentionTime = null;

        var oldJobId = await InsertCompletedJobAsync(_clock.Now.Subtract(TimeSpan.FromDays(2)));

        await RunCleanupAsync();

        (await _store.FindAsync(oldJobId)).ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Delete_All_Old_Jobs_In_Batches()
    {
        _workerOptions.MaxJobFetchCount = 2;

        var ids = new List<Guid>();
        for (var i = 0; i < 5; i++)
        {
            ids.Add(await InsertCompletedJobAsync(_clock.Now.Subtract(TimeSpan.FromDays(2))));
        }

        await RunCleanupAsync();

        foreach (var id in ids)
        {
            (await _store.FindAsync(id)).ShouldBeNull();
        }
    }

    [Fact]
    public async Task Should_Not_Loop_Forever_When_MaxJobFetchCount_Is_Zero()
    {
        _workerOptions.MaxJobFetchCount = 0;

        var oldJobId = await InsertCompletedJobAsync(_clock.Now.Subtract(TimeSpan.FromDays(2)));

        // Must return (not hang) even though nothing can be fetched/deleted with a zero page size.
        await RunCleanupAsync();

        (await _store.FindAsync(oldJobId)).ShouldNotBeNull();
    }
}
