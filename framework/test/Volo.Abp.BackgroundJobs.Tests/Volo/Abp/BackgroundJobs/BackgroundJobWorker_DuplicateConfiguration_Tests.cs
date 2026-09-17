using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Autofac;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobWorker_DuplicateConfiguration_Tests
{
    [Fact]
    public void Should_Throw_Without_Starting_Any_Worker_When_A_Job_Type_Is_Assigned_To_Multiple_Workers()
    {
        using var application = AbpApplicationFactory.Create<AbpDuplicateWorkerTestModule>(options =>
        {
            options.UseAutofac();
        });

        var exception = Record.Exception(() => application.Initialize());

        exception.ShouldNotBeNull();
        exception.ToString().ShouldContain("dedicated worker");

        // Validation must happen before any worker is started.
        var recorder = application.ServiceProvider.GetRequiredService<WorkerStartRecorder>();
        recorder.Records.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Throw_Without_Starting_Any_Worker_When_Two_Workers_Share_A_Lock_Name()
    {
        using var application = AbpApplicationFactory.Create<AbpDuplicateLockNameTestModule>(options =>
        {
            options.UseAutofac();
        });

        var exception = Record.Exception(() => application.Initialize());

        exception.ShouldNotBeNull();
        exception.ToString().ShouldContain("lock name");

        var recorder = application.ServiceProvider.GetRequiredService<WorkerStartRecorder>();
        recorder.Records.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Throw_Without_Starting_Any_Worker_When_Different_Args_Types_Resolve_To_The_Same_Job_Name()
    {
        using var application = AbpApplicationFactory.Create<AbpSameJobNameTestModule>(options =>
        {
            options.UseAutofac();
        });

        // The two args types pass the eager (by-type) check but resolve to the same job name,
        // so only the manager's backstop validation can reject them.
        var exception = Record.Exception(() => application.Initialize());

        exception.ShouldNotBeNull();
        exception.ToString().ShouldContain("more than one dedicated worker");

        var recorder = application.ServiceProvider.GetRequiredService<WorkerStartRecorder>();
        recorder.Records.ShouldBeEmpty();
    }
}
