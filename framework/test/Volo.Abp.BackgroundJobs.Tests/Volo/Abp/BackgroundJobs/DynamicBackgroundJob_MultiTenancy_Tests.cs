using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class DynamicBackgroundJob_MultiTenancy_Tests : BackgroundJobsTestBase
{
    private readonly IDynamicBackgroundJobManager _dynamicBackgroundJobManager;
    private readonly IBackgroundJobStore _backgroundJobStore;
    private readonly IBackgroundJobExecuter _backgroundJobExecuter;
    private readonly IBackgroundJobSerializer _backgroundJobSerializer;
    private readonly ICurrentTenant _currentTenant;

    public DynamicBackgroundJob_MultiTenancy_Tests()
    {
        _dynamicBackgroundJobManager = GetRequiredService<IDynamicBackgroundJobManager>();
        _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
        _backgroundJobExecuter = GetRequiredService<IBackgroundJobExecuter>();
        _backgroundJobSerializer = GetRequiredService<IBackgroundJobSerializer>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task Dynamic_Handler_Job_Should_Run_Under_The_Enqueue_Tenant()
    {
        var tenantId = Guid.NewGuid();
        Guid? observedTenantId = null;
        var executed = false;

        _dynamicBackgroundJobManager.RegisterHandler("TenantProbeJob", (ctx, _) =>
        {
            executed = true;
            observedTenantId = ctx.ServiceProvider.GetRequiredService<ICurrentTenant>().Id;
            return Task.CompletedTask;
        });

        try
        {
            string jobIdAsString;
            using (_currentTenant.Change(tenantId))
            {
                jobIdAsString = await _dynamicBackgroundJobManager.EnqueueAsync(
                    "TenantProbeJob",
                    new { CustomerId = "C-1" });
            }

            var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
            jobInfo.ShouldNotBeNull();

            var jobArgs = _backgroundJobSerializer.Deserialize(jobInfo.JobArgs, typeof(DynamicBackgroundJobArgs));

            _currentTenant.Id.ShouldBeNull();

            await _backgroundJobExecuter.ExecuteAsync(
                new JobExecutionContext(
                    ServiceProvider,
                    typeof(DynamicBackgroundJobExecutorJob),
                    jobArgs));

            executed.ShouldBeTrue();
            observedTenantId.ShouldBe(tenantId);
        }
        finally
        {
            _dynamicBackgroundJobManager.UnregisterHandler("TenantProbeJob");
        }
    }
}
