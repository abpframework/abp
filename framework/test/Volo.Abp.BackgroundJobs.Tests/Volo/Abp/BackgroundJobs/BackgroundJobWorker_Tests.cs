using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobWorker_Tests : BackgroundJobWorkerTestBase
{
    private readonly IBackgroundJobStore _store;
    private readonly IClock _clock;
    private readonly AbpBackgroundJobWorkerOptions _workerOptions;

    public BackgroundJobWorker_Tests()
    {
        _store = GetRequiredService<IBackgroundJobStore>();
        _clock = GetRequiredService<IClock>();
        _workerOptions = GetRequiredService<IOptions<AbpBackgroundJobWorkerOptions>>().Value;
    }

    private TestableBackgroundJobWorker CreateWorker()
    {
        return new TestableBackgroundJobWorker(
            GetRequiredService<AbpAsyncTimer>(),
            GetRequiredService<IOptions<AbpBackgroundJobOptions>>(),
            GetRequiredService<IOptions<AbpBackgroundJobWorkerOptions>>(),
            GetRequiredService<IServiceScopeFactory>(),
            GetRequiredService<IAbpDistributedLock>());
    }

    private BackgroundJobInfo NewJob(string jobName)
    {
        return new BackgroundJobInfo
        {
            Id = Guid.NewGuid(),
            JobName = jobName,
            JobArgs = "{}",
            CreationTime = _clock.Now,
            NextTryTime = _clock.Now.AddMinutes(-1)
        };
    }

    private PeriodicBackgroundWorkerContext Context()
    {
        return new PeriodicBackgroundWorkerContext(ServiceProvider);
    }

    // Storing successful jobs

    [Fact]
    public async Task Should_Keep_Successful_Job_As_History_When_Enabled()
    {
        _workerOptions.StoreSuccessfulJobs = true;

        var jobInfo = NewJob("job-a");
        await _store.InsertAsync(jobInfo);

        await CreateWorker().HandleJobSuccessPublicAsync(_store, jobInfo, _clock);

        // The job is kept (marked completed), not deleted.
        var kept = await _store.FindAsync(jobInfo.Id);
        kept.ShouldNotBeNull();
        kept.CompletionTime.ShouldNotBeNull();

        // ...but it is excluded from the waiting jobs.
        (await _store.GetWaitingJobsAsync(null, 1000)).ShouldNotContain(j => j.Id == jobInfo.Id);
    }

    [Fact]
    public async Task Should_Delete_Successful_Job_When_Disabled()
    {
        // StoreSuccessfulJobs is false by default.
        var jobInfo = NewJob("job-a");
        await _store.InsertAsync(jobInfo);

        await CreateWorker().HandleJobSuccessPublicAsync(_store, jobInfo, _clock);

        (await _store.FindAsync(jobInfo.Id)).ShouldBeNull();
    }

    // Dedicated workers by job name

    [Fact]
    public async Task Should_Return_Only_Included_Jobs()
    {
        await _store.InsertAsync(NewJob("job-a"));
        await _store.InsertAsync(NewJob("job-a"));
        await _store.InsertAsync(NewJob("job-b"));

        var worker = CreateWorker();
        worker.ConfigureTest(BackgroundJobNameFilter.Include(new[] { "job-a" }));

        var jobs = await worker.GetWaitingJobsPublicAsync(Context(), _store);

        jobs.Count.ShouldBe(2);
        jobs.ShouldAllBe(j => j.JobName == "job-a");
    }

    [Fact]
    public async Task Should_Exclude_Given_Jobs()
    {
        await _store.InsertAsync(NewJob("job-a"));
        await _store.InsertAsync(NewJob("job-b"));

        var worker = CreateWorker();
        worker.ConfigureTest(BackgroundJobNameFilter.Exclude(new[] { "job-a" }));

        var jobs = await worker.GetWaitingJobsPublicAsync(Context(), _store);

        jobs.Count.ShouldBe(1);
        jobs.Single().JobName.ShouldBe("job-b");
    }

    [Fact]
    public void Should_Match_Job_Names_By_Filter()
    {
        BackgroundJobNameFilter.None.IsMatch("any").ShouldBeTrue();

        var include = BackgroundJobNameFilter.Include(new[] { "job-a" });
        include.IsMatch("job-a").ShouldBeTrue();
        include.IsMatch("job-b").ShouldBeFalse();

        var exclude = BackgroundJobNameFilter.Exclude(new[] { "job-a" });
        exclude.IsMatch("job-a").ShouldBeFalse();
        exclude.IsMatch("job-b").ShouldBeTrue();
    }

    [Fact]
    public void BackgroundJobNameFilter_Should_Reject_Invalid_Mode_And_Names_Combinations()
    {
        Should.Throw<ArgumentException>(() => new BackgroundJobNameFilter(BackgroundJobNameFilterMode.Include));
        Should.Throw<ArgumentException>(() => new BackgroundJobNameFilter(BackgroundJobNameFilterMode.None, new[] { "job-a" }));
        Should.Throw<ArgumentException>(() => new BackgroundJobNameFilter((BackgroundJobNameFilterMode)99, new[] { "job-a" }));
    }

    // Parallel execution / eligibility

    [Fact]
    public void Should_Evaluate_Job_Eligibility()
    {
        var worker = CreateWorker();

        worker.IsJobEligiblePublic(null, _clock).ShouldBeFalse();

        var future = NewJob("job-a");
        future.NextTryTime = _clock.Now.AddMinutes(5);
        worker.IsJobEligiblePublic(future, _clock).ShouldBeFalse();

        var abandoned = NewJob("job-a");
        abandoned.IsAbandoned = true;
        worker.IsJobEligiblePublic(abandoned, _clock).ShouldBeFalse();

        var completed = NewJob("job-a");
        completed.CompletionTime = _clock.Now;
        worker.IsJobEligiblePublic(completed, _clock).ShouldBeFalse();

        var eligible = NewJob("job-a");
        worker.IsJobEligiblePublic(eligible, _clock).ShouldBeTrue();
    }

    [Fact]
    public void Should_Not_Be_Eligible_When_Job_Name_Filtered_Out()
    {
        var worker = CreateWorker();
        worker.ConfigureTest(BackgroundJobNameFilter.Include(new[] { "job-a" }));

        var otherJob = NewJob("job-b");
        worker.IsJobEligiblePublic(otherJob, _clock).ShouldBeFalse();
    }

    [Fact]
    public void Should_Require_Job_Args_Types_For_A_Dedicated_Worker()
    {
        Should.Throw<ArgumentException>(() => new BackgroundJobWorkerConfiguration("lock-a"));
    }

    [Fact]
    public void AddDedicatedWorker_Should_Throw_At_Registration_When_A_Job_Type_Is_Added_Twice()
    {
        var options = new AbpBackgroundJobWorkerOptions();
        options.AddDedicatedWorker<ParallelTestJobArgs>("lock-a");

        Should.Throw<AbpException>(() => options.AddDedicatedWorker<ParallelTestJobArgs>("lock-b"));
    }

    [Fact]
    public void AddDedicatedWorker_Should_Throw_At_Registration_When_A_Lock_Name_Is_Reused()
    {
        var options = new AbpBackgroundJobWorkerOptions();
        options.AddDedicatedWorker<WorkerJobAArgs>("dup-lock");

        Should.Throw<AbpException>(() => options.AddDedicatedWorker<WorkerJobBArgs>("dup-lock"));
    }

    [Fact]
    public void AddDedicatedWorker_Should_Throw_At_Registration_When_Lock_Name_Equals_The_Default()
    {
        var options = new AbpBackgroundJobWorkerOptions();

        Should.Throw<AbpException>(() => options.AddDedicatedWorker<WorkerJobAArgs>(options.DistributedLockName));
    }

    // Parallel execution

    [Fact]
    public async Task Should_Execute_Multiple_Jobs_In_Parallel()
    {
        _workerOptions.MaxParallelJobExecutionCount = 3;

        var jobManager = GetRequiredService<IBackgroundJobManager>();
        await jobManager.EnqueueAsync(new ParallelTestJobArgs { Value = "1" });
        await jobManager.EnqueueAsync(new ParallelTestJobArgs { Value = "2" });
        await jobManager.EnqueueAsync(new ParallelTestJobArgs { Value = "3" });

        await CreateWorker().ExecuteJobsInParallelPublicAsync(Context());

        var tracker = GetRequiredService<ParallelJobTracker>();
        tracker.Executed.Count.ShouldBe(3);
        tracker.Executed.ShouldContain("1");
        tracker.Executed.ShouldContain("2");
        tracker.Executed.ShouldContain("3");

        // Each job must run in its own service scope (isolated DbContext/UOW).
        tracker.ScopeIds.Distinct().Count().ShouldBe(3);

        (await _store.GetWaitingJobsAsync(null, 1000)).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Execute_At_Most_MaxParallel_Jobs_Per_Cycle()
    {
        _workerOptions.MaxParallelJobExecutionCount = 2;

        var jobManager = GetRequiredService<IBackgroundJobManager>();
        for (var i = 0; i < 5; i++)
        {
            await jobManager.EnqueueAsync(new ParallelTestJobArgs { Value = i.ToString() });
        }

        await CreateWorker().ExecuteJobsInParallelPublicAsync(Context());

        var tracker = GetRequiredService<ParallelJobTracker>();
        tracker.Executed.Count.ShouldBe(2);
        (await _store.GetWaitingJobsAsync(null, 1000)).Count.ShouldBe(3);
    }

    [Fact]
    public async Task Should_Skip_Job_Already_Claimed_By_Another_Instance()
    {
        _workerOptions.MaxParallelJobExecutionCount = 5;

        var jobManager = GetRequiredService<IBackgroundJobManager>();
        var lockedJobId = Guid.Parse(await jobManager.EnqueueAsync(new ParallelTestJobArgs { Value = "locked" }));
        await jobManager.EnqueueAsync(new ParallelTestJobArgs { Value = "free" });

        var distributedLock = GetRequiredService<IAbpDistributedLock>();
        var lockName = _workerOptions.PerJobDistributedLockPrefix + lockedJobId;

        await using (await distributedLock.TryAcquireAsync(lockName))
        {
            await CreateWorker().ExecuteJobsInParallelPublicAsync(Context());
        }

        var tracker = GetRequiredService<ParallelJobTracker>();
        tracker.Executed.ShouldContain("free");
        tracker.Executed.ShouldNotContain("locked");
        (await _store.FindAsync(lockedJobId)).ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Mark_Job_Completed_On_Successful_Execution_When_Storing_Enabled()
    {
        _workerOptions.StoreSuccessfulJobs = true;
        _workerOptions.MaxParallelJobExecutionCount = 2;

        var jobManager = GetRequiredService<IBackgroundJobManager>();
        var jobId = Guid.Parse(await jobManager.EnqueueAsync(new ParallelTestJobArgs { Value = "1" }));

        await CreateWorker().ExecuteJobsInParallelPublicAsync(Context());

        GetRequiredService<ParallelJobTracker>().Executed.ShouldContain("1");

        // The job is kept (marked completed), not deleted, and excluded from the waiting list.
        var job = await _store.FindAsync(jobId);
        job.ShouldNotBeNull();
        job.CompletionTime.ShouldNotBeNull();
        (await _store.GetWaitingJobsAsync(null, 1000)).ShouldNotContain(j => j.Id == jobId);
    }
}
