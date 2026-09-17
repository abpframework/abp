using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.Modularity;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BackgroundWorkers.Hangfire;
using Volo.Abp.Hangfire;

namespace Volo.Abp.BackgroundJobs.DemoApp.HangFire;

[DependsOn(
    typeof(DemoAppSharedModule),
    typeof(AbpAutofacModule),
    typeof(AbpBackgroundJobsHangfireModule),
    typeof(AbpBackgroundWorkersHangfireModule)
)]
public class DemoAppHangfireModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<IGlobalConfiguration>(hangfireConfiguration =>
        {
            hangfireConfiguration.UseSqlServerStorage(configuration.GetConnectionString("Default"));
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpHangfireOptions>(options =>
        {
            options.ServerOptions = new BackgroundJobServerOptions
            {
                Queues = new []{ "default", "my-default" }
            };
        });

        Configure<AbpHangfirePeriodicBackgroundWorkerAdapterOptions>(options =>
        {
            options.TimeZone = TimeZoneInfo.Local;
            options.Queue = "my-default";
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var backgroundWorkerManager = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
        await backgroundWorkerManager.AddAsync(context.ServiceProvider.GetRequiredService<TestWorker>());
    }
}
