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
    private readonly IAnonymousJobHandlerRegistry _anonymousJobHandlerRegistry;

    public BackgroundJobManager_Tests()
    {
        _backgroundJobManager = GetRequiredService<IBackgroundJobManager>();
        _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
        _anonymousJobHandlerRegistry = GetRequiredService<IAnonymousJobHandlerRegistry>();
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
    public async Task Should_Store_Anonymous_Jobs()
    {
        var jobIdAsString = await _backgroundJobManager.EnqueueAsync("TestAnonymousJob", new { OrderId = "ORD-001" });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(AnonymousJobArgs.JobNameConstant);
        jobInfo.JobArgs.ShouldContain("TestAnonymousJob");
        jobInfo.JobArgs.ShouldContain("ORD-001");
    }

    [Fact]
    public async Task Should_Not_Wrap_If_Typed_Job_Exists_For_Same_Name()
    {
        var typedJobName = BackgroundJobNameAttribute.GetName<MyJobArgs>();
        _anonymousJobHandlerRegistry.Register(typedJobName, (_, _) => Task.CompletedTask);

        var jobIdAsString = await _backgroundJobManager.EnqueueAsync(typedJobName, new { Value = "42" });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(typedJobName);
        jobInfo.JobName.ShouldNotBe(AnonymousJobArgs.JobNameConstant);
    }
}
