using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public abstract class BackgroundJobRepository_Tests<TStartupModule> : BackgroundJobsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IBackgroundJobRepository _backgroundJobRepository;
    private readonly IClock _clock;

    protected BackgroundJobRepository_Tests()
    {
        _backgroundJobRepository = GetRequiredService<IBackgroundJobRepository>();
        _clock = GetRequiredService<IClock>();
    }

    [Theory]
    [InlineData(2)]
    [InlineData(5)]
    public async Task GetWaitingListAsync(int maxResultCount)
    {
        var backgroundJobs = await _backgroundJobRepository.GetWaitingListAsync("App1", maxResultCount);

        backgroundJobs.Count.ShouldBeGreaterThan(0);
        backgroundJobs.Count.ShouldBeLessThanOrEqualTo(maxResultCount);

        backgroundJobs.ForEach(j => j.IsAbandoned.ShouldBeFalse());
        backgroundJobs.ForEach(j => j.NextTryTime.ShouldBeLessThanOrEqualTo(_clock.Now.AddSeconds(1))); //1 second tolerance

        backgroundJobs.All(j => j.ApplicationName == "App1").ShouldBeTrue();
        backgroundJobs.Any(j => j.ApplicationName == "App2").ShouldBeFalse();
        backgroundJobs.Any(j => j.ApplicationName == null).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Filter_Waiting_List_By_Included_Job_Names()
    {
        // App1 waiting jobs: two "TestJobName" + one "OtherJobName".
        var testJobs = await _backgroundJobRepository.GetWaitingListAsync("App1", 10, BackgroundJobNameFilter.Include(new[] { "TestJobName" }));
        testJobs.Count.ShouldBe(2);
        testJobs.ShouldAllBe(j => j.JobName == "TestJobName");

        var otherJobs = await _backgroundJobRepository.GetWaitingListAsync("App1", 10, BackgroundJobNameFilter.Include(new[] { "OtherJobName" }));
        otherJobs.Count.ShouldBe(1);
        otherJobs.Single().JobName.ShouldBe("OtherJobName");

        var none = await _backgroundJobRepository.GetWaitingListAsync("App1", 10, BackgroundJobNameFilter.Include(new[] { "NonExistentJob" }));
        none.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Filter_Waiting_List_By_Excluded_Job_Names()
    {
        var withoutTestJob = await _backgroundJobRepository.GetWaitingListAsync("App1", 10, BackgroundJobNameFilter.Exclude(new[] { "TestJobName" }));
        withoutTestJob.Count.ShouldBe(1);
        withoutTestJob.Single().JobName.ShouldBe("OtherJobName");

        var withoutNonExistent = await _backgroundJobRepository.GetWaitingListAsync("App1", 10, BackgroundJobNameFilter.Exclude(new[] { "NonExistentJob" }));
        withoutNonExistent.Count.ShouldBe(3);
    }

    [Fact]
    public async Task Should_Return_Null_From_Store_When_Job_Not_Found()
    {
        // The parallel worker re-reads a claimed job under the lock; a job removed by another instance must return null (not throw).
        var store = GetRequiredService<IBackgroundJobStore>();
        var found = await store.FindAsync(Guid.NewGuid());
        found.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Exclude_Completed_Jobs_From_Waiting_List()
    {
        var completedJobId = Guid.NewGuid();
        await _backgroundJobRepository.InsertAsync(
            new BackgroundJobRecord(completedJobId)
            {
                ApplicationName = "App1",
                JobName = "TestJobName",
                JobArgs = "{ value: 1 }",
                NextTryTime = _clock.Now.Subtract(TimeSpan.FromMinutes(1)),
                Priority = BackgroundJobPriority.Normal,
                IsAbandoned = false,
                CompletionTime = _clock.Now,
                CreationTime = _clock.Now.Subtract(TimeSpan.FromMinutes(2)),
                TryCount = 1
            },
            autoSave: true);

        var waitingJobs = await _backgroundJobRepository.GetWaitingListAsync("App1", 100);
        waitingJobs.ShouldNotContain(j => j.Id == completedJobId);
    }

    [Fact]
    public async Task Should_Delete_Old_Successful_Jobs_Of_The_Given_Application_Only()
    {
        var oldApp1Id = Guid.NewGuid();
        await _backgroundJobRepository.InsertAsync(NewCompletedJob(oldApp1Id, "App1", _clock.Now.Subtract(TimeSpan.FromDays(2))), autoSave: true);

        var recentApp1Id = Guid.NewGuid();
        await _backgroundJobRepository.InsertAsync(NewCompletedJob(recentApp1Id, "App1", _clock.Now), autoSave: true);

        var oldApp2Id = Guid.NewGuid();
        await _backgroundJobRepository.InsertAsync(NewCompletedJob(oldApp2Id, "App2", _clock.Now.Subtract(TimeSpan.FromDays(2))), autoSave: true);

        var deleted = await _backgroundJobRepository.DeleteAsync("App1", _clock.Now.Subtract(TimeSpan.FromDays(1)), 100);
        deleted.ShouldBe(1);

        (await _backgroundJobRepository.FindAsync(oldApp1Id)).ShouldBeNull();        // App1 old completed → deleted
        (await _backgroundJobRepository.FindAsync(recentApp1Id)).ShouldNotBeNull();  // App1 recent → kept
        (await _backgroundJobRepository.FindAsync(oldApp2Id)).ShouldNotBeNull();     // App2 old → not touched (isolation)
    }

    private BackgroundJobRecord NewCompletedJob(Guid id, string applicationName, DateTime completionTime)
    {
        return new BackgroundJobRecord(id)
        {
            ApplicationName = applicationName,
            JobName = "TestJobName",
            JobArgs = "{ value: 1 }",
            NextTryTime = _clock.Now.Subtract(TimeSpan.FromMinutes(1)),
            Priority = BackgroundJobPriority.Normal,
            IsAbandoned = false,
            CompletionTime = completionTime,
            CreationTime = _clock.Now.Subtract(TimeSpan.FromMinutes(5)),
            TryCount = 1
        };
    }
}
