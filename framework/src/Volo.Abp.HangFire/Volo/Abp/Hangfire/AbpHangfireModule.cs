using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.Abp.Hangfire;

[DependsOn(typeof(AbpAuthorizationAbstractionsModule))]
public class AbpHangfireModule : AbpModule
{
    private AbpHangfireBackgroundJobServer _backgroundJobServer;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var preActions = context.Services.GetPreConfigureActions<IGlobalConfiguration>();
        context.Services.AddHangfire(configuration =>
        {
            preActions.Configure(configuration);
        });

        context.Services.AddSingleton(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
            return new AbpHangfireBackgroundJobServer(options.BackgroundJobServerFactory.Invoke(serviceProvider));
        });
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        _backgroundJobServer = context.ServiceProvider.GetRequiredService<AbpHangfireBackgroundJobServer>();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        if (_backgroundJobServer == null)
        {
            return;
        }

        _backgroundJobServer.HangfireJobServer?.SendStop();
        _backgroundJobServer.HangfireJobServer?.Dispose();
    }
}
