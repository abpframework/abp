using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobManager_Tests : BackgroundJobsTestBase
{
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IBackgroundJobStore _backgroundJobStore;

    public BackgroundJobManager_Tests()
    {
        _backgroundJobManager = GetRequiredService<IBackgroundJobManager>();
        _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
    }

    [Fact]
    public async Task Should_Store_Jobs()
    {
        var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyJobArgs("42"));
        jobIdAsString.ShouldNotBe(default);
        (await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString))).ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Store_Async_Jobs()
    {
        var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyAsyncJobArgs("42"));
        jobIdAsString.ShouldNotBe(default);
        (await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString))).ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Store_Jobs_With_JobName()
    {
        var jobName = BackgroundJobNameAttribute.GetName<MyJobArgs>();
        var jobIdAsString = await _backgroundJobManager.EnqueueAsync(jobName, (object)new
        {
            Value = "42"
        });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(jobName);
    }

    [Fact]
    public async Task Should_Store_Async_Jobs_With_JobName()
    {
        var jobName = BackgroundJobNameAttribute.GetName<MyAsyncJobArgs>();
        var jobIdAsString = await _backgroundJobManager.EnqueueAsync(jobName, (object)new Dictionary<string, object>()
        {
            ["Value"] = "42"
        });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(jobName);
    }

    [Fact]
    public async Task Should_Store_Dynamic_Jobs()
    {
        var jobIdAsString = await _backgroundJobManager.EnqueueAsync("TestDynamicJob", (object)new Dictionary<string, object>
        {
            ["OrderId"] = "ORD-001"
        });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe("TestDynamicJob");
        jobInfo.JobArgs.ShouldContain("ORD-001");
    }
}
