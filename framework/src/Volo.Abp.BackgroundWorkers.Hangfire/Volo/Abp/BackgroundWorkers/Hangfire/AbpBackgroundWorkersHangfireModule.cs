using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

[DependsOn(
    typeof(AbpBackgroundWorkersModule),
    typeof(AbpHangfireModule))]
public class AbpBackgroundWorkersHangfireModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(typeof(HangfirePeriodicBackgroundWorkerAdapter<>));
    }
    
    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
        if (!options.IsEnabled)
        {
            var hangfireOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
            context.ServiceProvider.GetRequiredService<JobStorage>();
            hangfireOptions.BackgroundJobServerFactory = _ => null;
        }
        
        context.ServiceProvider
            .GetRequiredService<HangfireBackgroundWorkerManager>()
            .Initialize(); 
    }
}
