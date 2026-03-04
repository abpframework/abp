using Dapr;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus;
using Volo.Abp.Autofac;
using Volo.Abp.Dapr;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.Modularity;

namespace DistDemoApp;

#if DISTDEMO_USE_MONGODB
[DependsOn(
    typeof(DistDemoAppMongoDbInfrastructureModule),
    typeof(AbpAspNetCoreMvcDaprEventBusModule),
    typeof(DistDemoAppSharedModule),
    typeof(AbpAutofacModule)
)]
#else
[DependsOn(
    typeof(DistDemoAppEntityFrameworkCoreInfrastructureModule),
    typeof(AbpAspNetCoreMvcDaprEventBusModule),
    typeof(DistDemoAppSharedModule),
    typeof(AbpAutofacModule)
)]
#endif
public class DistDemoAppAspNetCoreDaprModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
#if DISTDEMO_USE_MONGODB
        context.ConfigureDistDemoMongoInfrastructure();
#else
        context.ConfigureDistDemoEntityFrameworkInfrastructure();
#endif

        Configure<AbpDaprOptions>(options =>
        {
            options.HttpEndpoint = "http://localhost:3500";
        });

        Configure<AbpDaprEventBusOptions>(options =>
        {
            options.PubSubName = "pubsub";
        });
    }
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCloudEvents();
        app.UseConfiguredEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSubscribeHandler();
        });
    }
}
