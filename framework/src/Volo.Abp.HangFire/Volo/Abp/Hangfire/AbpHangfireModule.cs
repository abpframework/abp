using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.Abp.Hangfire
{
    [DependsOn(typeof(AbpAuthorizationAbstractionsModule))]
    public class AbpHangfireModule : AbpModule
    {
        private BackgroundJobServer _backgroundJobServer;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var preActions = context.Services.GetPreConfigureActions<IGlobalConfiguration>();
            context.Services.AddHangfire(configuration =>
            {
                preActions.Configure(configuration);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
            _backgroundJobServer = options.BackgroundJobServerFactory.Invoke(context.ServiceProvider);
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            if (_backgroundJobServer != null)
            {
                _backgroundJobServer.SendStop();
                _backgroundJobServer.Dispose();
            }
        }
    }
}
