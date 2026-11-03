using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Quartz;

public class AbpQuartzModule_Shutdown_Tests
{
    [Fact]
    public async Task Should_Shutdown_Started_Scheduler()
    {
        using var application = await AbpApplicationFactory.CreateAsync<AbpQuartzModule>();
        await application.InitializeAsync();

        var scheduler = application.ServiceProvider.GetRequiredService<IScheduler>();
        scheduler.IsStarted.ShouldBeTrue();

        await application.ShutdownAsync();

        scheduler.IsShutdown.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Shutdown_Scheduler_That_Was_Never_Started()
    {
        using var application = await AbpApplicationFactory.CreateAsync<AbpQuartzNotStartedTestModule>();
        await application.InitializeAsync();

        var scheduler = application.ServiceProvider.GetRequiredService<IScheduler>();
        scheduler.IsStarted.ShouldBeFalse();

        await application.ShutdownAsync();

        scheduler.IsShutdown.ShouldBeTrue();
    }

    [Fact]
    public void Should_Shutdown_Scheduler_That_Was_Never_Started_On_Sync_Shutdown()
    {
        using var application = AbpApplicationFactory.Create<AbpQuartzNotStartedTestModule>();
        application.Initialize();

        var scheduler = application.ServiceProvider.GetRequiredService<IScheduler>();
        scheduler.IsStarted.ShouldBeFalse();

        application.Shutdown();

        scheduler.IsShutdown.ShouldBeTrue();
    }
}

[DependsOn(typeof(AbpQuartzModule))]
public class AbpQuartzNotStartedTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Same as disabling background workers or jobs: the scheduler is created but not started.
        Configure<AbpQuartzOptions>(options =>
        {
            options.StartSchedulerFactory = _ => Task.CompletedTask;
        });
    }
}
