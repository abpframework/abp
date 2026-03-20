using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobManager_Tests : BackgroundJobsTestBase
{
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IDynamicBackgroundJobManager _dynamicBackgroundJobManager;
    private readonly IBackgroundJobStore _backgroundJobStore;

    public BackgroundJobManager_Tests()
    {
        _backgroundJobManager = GetRequiredService<IBackgroundJobManager>();
        _dynamicBackgroundJobManager = GetRequiredService<IDynamicBackgroundJobManager>();
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
        var jobIdAsString = await _dynamicBackgroundJobManager.EnqueueAsync(jobName, new
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
        var jobIdAsString = await _dynamicBackgroundJobManager.EnqueueAsync(jobName, new { Value = "42" });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(jobName);
    }

    [Fact]
    public async Task Should_Store_Anonymous_Jobs()
    {
        var jobIdAsString = await _dynamicBackgroundJobManager.EnqueueAsync("TestAnonymousJob", new { OrderId = "ORD-001" });
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
        _dynamicBackgroundJobManager.RegisterHandler(typedJobName, (_, _) => Task.CompletedTask);

        var jobIdAsString = await _dynamicBackgroundJobManager.EnqueueAsync(typedJobName, new { Value = "42" });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(typedJobName);
        jobInfo.JobName.ShouldNotBe(AnonymousJobArgs.JobNameConstant);
    }

    [Fact]
    public async Task Should_Throw_For_Unknown_Job_Name()
    {
        await Assert.ThrowsAsync<AbpException>(() =>
            _dynamicBackgroundJobManager.EnqueueAsync("NonExistentJob", new { Value = "42" })
        );
    }

    [Fact]
    public void Should_Register_And_Unregister_Handler()
    {
        _dynamicBackgroundJobManager.IsHandlerRegistered("TestDynamic").ShouldBeFalse();

        _dynamicBackgroundJobManager.RegisterHandler("TestDynamic", (_, _) => Task.CompletedTask);
        _dynamicBackgroundJobManager.IsHandlerRegistered("TestDynamic").ShouldBeTrue();

        _dynamicBackgroundJobManager.UnregisterHandler("TestDynamic").ShouldBeTrue();
        _dynamicBackgroundJobManager.IsHandlerRegistered("TestDynamic").ShouldBeFalse();
    }
}
