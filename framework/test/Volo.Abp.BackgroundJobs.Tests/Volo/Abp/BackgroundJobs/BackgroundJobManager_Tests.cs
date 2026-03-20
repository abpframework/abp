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
    private readonly IBackgroundJobExecuter _backgroundJobExecuter;
    private readonly DynamicJobExecutionTracker _tracker;

    public BackgroundJobManager_Tests()
    {
        _backgroundJobManager = GetRequiredService<IBackgroundJobManager>();
        _dynamicBackgroundJobManager = GetRequiredService<IDynamicBackgroundJobManager>();
        _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
        _backgroundJobExecuter = GetRequiredService<IBackgroundJobExecuter>();
        _tracker = GetRequiredService<DynamicJobExecutionTracker>();
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
    public async Task Should_Enqueue_Typed_Job_By_Name()
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
    public async Task Should_Enqueue_Async_Typed_Job_By_Name()
    {
        var jobName = BackgroundJobNameAttribute.GetName<MyAsyncJobArgs>();
        var jobIdAsString = await _dynamicBackgroundJobManager.EnqueueAsync(jobName, new { Value = "42" });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(jobName);
    }

    [Fact]
    public async Task Should_Enqueue_Dynamic_Handler_Job()
    {
        var jobIdAsString = await _dynamicBackgroundJobManager.EnqueueAsync("TestDynamicJob", new { OrderId = "ORD-001" });
        jobIdAsString.ShouldNotBe(default);

        var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
        jobInfo.ShouldNotBeNull();
        jobInfo.JobName.ShouldBe(AnonymousJobArgs.JobNameConstant);
        jobInfo.JobArgs.ShouldContain("TestDynamicJob");
        jobInfo.JobArgs.ShouldContain("ORD-001");
    }

    [Fact]
    public async Task Should_Execute_Dynamic_Handler_Job()
    {
        _tracker.ExecutedJsonData.ShouldBeEmpty();

        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(AnonymousJobExecutorAsyncBackgroundJob),
                new AnonymousJobArgs("TestDynamicJob", "{\"OrderId\":\"ORD-001\"}")
            )
        );

        _tracker.ExecutedJsonData.ShouldContain(d => d.Contains("ORD-001"));
    }

    [Fact]
    public async Task Should_Prefer_Typed_Job_Over_Dynamic_Handler()
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
        _dynamicBackgroundJobManager.IsHandlerRegistered("TestRegister").ShouldBeFalse();

        _dynamicBackgroundJobManager.RegisterHandler("TestRegister", (_, _) => Task.CompletedTask);
        _dynamicBackgroundJobManager.IsHandlerRegistered("TestRegister").ShouldBeTrue();

        _dynamicBackgroundJobManager.UnregisterHandler("TestRegister").ShouldBeTrue();
        _dynamicBackgroundJobManager.IsHandlerRegistered("TestRegister").ShouldBeFalse();
    }
}
