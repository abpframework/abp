using System.Linq;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobWorker_MultiWorkerRegistration_Tests : AbpIntegratedTest<AbpMultiWorkerTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public void Should_Start_Dedicated_Workers_And_A_Default_Worker()
    {
        // The workers are resolved from DI (RecordingBackgroundJobWorker replaces the real one),
        // proving the manager honors the registered/replaced IBackgroundJobWorker.
        var records = GetRequiredService<WorkerStartRecorder>().Records;

        records.Count.ShouldBe(3);

        var jobAName = BackgroundJobNameAttribute.GetName<WorkerJobAArgs>();
        var jobBName = BackgroundJobNameAttribute.GetName<WorkerJobBArgs>();

        var dedicated = records.Where(r => r.JobNameFilter?.Mode == BackgroundJobNameFilterMode.Include).ToList();
        dedicated.Count.ShouldBe(2);
        dedicated.ShouldContain(r => r.DistributedLockName == "lock-a" && r.JobNameFilter!.JobNames.Contains(jobAName));
        dedicated.ShouldContain(r => r.DistributedLockName == "lock-b" && r.JobNameFilter!.JobNames.Contains(jobBName));

        var defaultWorker = records.Single(r => r.JobNameFilter?.Mode == BackgroundJobNameFilterMode.Exclude);
        defaultWorker.DistributedLockName.ShouldBeNull();
        defaultWorker.JobNameFilter!.JobNames.ShouldContain(jobAName);
        defaultWorker.JobNameFilter!.JobNames.ShouldContain(jobBName);
    }
}
