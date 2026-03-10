using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobExecuter_Tests : BackgroundJobsTestBase
{
    private readonly IBackgroundJobExecuter _backgroundJobExecuter;

    public BackgroundJobExecuter_Tests()
    {
        _backgroundJobExecuter = GetRequiredService<IBackgroundJobExecuter>();
    }

    [Fact]
    public async Task Should_Execute_Tasks()
    {
        //Arrange

        var jobObject = GetRequiredService<MyJob>();
        jobObject.ExecutedValues.ShouldBeEmpty();

        //Act

        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(MyJob),
                new MyJobArgs("42")
            )
        );

        //Assert

        jobObject.ExecutedValues.ShouldContain("42");
    }

    [Fact]
    public async Task Should_Execute_Async_Tasks()
    {
        //Arrange

        var jobObject = GetRequiredService<MyAsyncJob>();
        jobObject.ExecutedValues.ShouldBeEmpty();

        //Act

        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(MyAsyncJob),
                new MyAsyncJobArgs("42")
            )
        );

        //Assert

        jobObject.ExecutedValues.ShouldContain("42");
    }

    [Fact]
    public async Task Should_Change_TenantId_If_EventData_Is_MultiTenant()
    {
        //Arrange
        var tenantId = Guid.NewGuid();
        var jobObject = GetRequiredService<MyJob>();
        var asyncJobObject = GetRequiredService<MyAsyncJob>();

        //Act

        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(MyJob),
                new MyJobArgs("42", tenantId)
            )
        );

        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(MyAsyncJob),
                new MyAsyncJobArgs("42", tenantId)
            )
        );

        //Assert

        jobObject.TenantId.ShouldBe(tenantId);
        asyncJobObject.TenantId.ShouldBe(tenantId);
    }

    [Fact]
    public async Task Should_Cancel_Job()
    {
        //Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var jobObject = GetRequiredService<MyJob>();
        jobObject.ExecutedValues.ShouldBeEmpty();

        //Act
        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(MyJob),
                new MyJobArgs("42"),
                cts.Token
            )
        );

        //Assert
        jobObject.Canceled.ShouldBeTrue();

        //Arrange
        var asyncCts = new CancellationTokenSource();
        asyncCts.Cancel();

        var asyncJobObject = GetRequiredService<MyAsyncJob>();
        asyncJobObject.ExecutedValues.ShouldBeEmpty();

        //Act
        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(MyAsyncJob),
                new MyAsyncJobArgs("42"),
                asyncCts.Token
            )
        );

        //Assert
        asyncJobObject.Canceled.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Execute_Anonymous_Job_Handler()
    {
        var tracker = GetRequiredService<AnonymousJobExecutionTracker>();
        tracker.ExecutedJsonData.ShouldBeEmpty();

        var args = new AnonymousJobArgs("TestAnonymousJob", "{\"OrderId\":\"ORD-001\"}");

        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(AnonymousJobExecutorAsyncBackgroundJob),
                args
            )
        );

        tracker.ExecutedJsonData.Count.ShouldBe(1);
        tracker.ExecutedJsonData[0].ShouldContain("ORD-001");
    }

    [Fact]
    public async Task Should_Execute_Anonymous_Job_Handler_Registered_At_Runtime()
    {
        var handlerRegistry = GetRequiredService<IAnonymousJobHandlerRegistry>();
        var executedValues = new List<string>();

        handlerRegistry.Register("RuntimeAnonymousJob", (jsonData, sp, ct) =>
        {
            executedValues.Add(jsonData);
            return Task.CompletedTask;
        });

        var args = new AnonymousJobArgs("RuntimeAnonymousJob", "{\"Message\":\"hello-runtime\"}");

        await _backgroundJobExecuter.ExecuteAsync(
            new JobExecutionContext(
                ServiceProvider,
                typeof(AnonymousJobExecutorAsyncBackgroundJob),
                args
            )
        );

        executedValues.Count.ShouldBe(1);
        executedValues[0].ShouldContain("hello-runtime");

        handlerRegistry.IsRegistered("RuntimeAnonymousJob").ShouldBeTrue();
        handlerRegistry.Unregister("RuntimeAnonymousJob").ShouldBeTrue();
        handlerRegistry.IsRegistered("RuntimeAnonymousJob").ShouldBeFalse();
    }
}
