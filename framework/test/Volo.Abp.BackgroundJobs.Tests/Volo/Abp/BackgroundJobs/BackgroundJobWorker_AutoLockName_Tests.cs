using System;
using System.Linq;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobWorker_AutoLockName_Tests : AbpIntegratedTest<AbpAutoLockNameWorkerTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public void Should_Derive_Bounded_Lock_Name_From_Job_Args_Types()
    {
        var records = GetRequiredService<WorkerStartRecorder>().Records;

        var dedicated = records.Where(r => r.JobNameFilter?.Mode == BackgroundJobNameFilterMode.Include).ToList();
        dedicated.Count.ShouldBe(2);

        // Lock name is derived (prefix + MD5 of the full type name), so it is stable and length-bounded.
        var expectedA = "AbpBackgroundJobDedicatedWorker:" + typeof(WorkerJobAArgs).FullName!.ToMd5();
        var expectedB = "AbpBackgroundJobDedicatedWorker:" + typeof(WorkerJobBArgs).FullName!.ToMd5();

        dedicated.ShouldContain(r => r.DistributedLockName == expectedA);
        dedicated.ShouldContain(r => r.DistributedLockName == expectedB);

        // Bounded length regardless of how long the type names are.
        dedicated.ShouldAllBe(r => r.DistributedLockName!.Length <= 64);
    }
}
