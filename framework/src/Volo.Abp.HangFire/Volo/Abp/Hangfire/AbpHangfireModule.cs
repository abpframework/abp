using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;

namespace Volo.Abp.Hangfire
{
    public class AbpHangfireModule : AbpModule
    {
        private BackgroundJobServer _backgroundJobServer;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHangfire(configuration =>
            {
                context.Services.ExecutePreConfiguredActions(configuration);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            if (context.ServiceProvider.GetService<JobStorage>() == null)
            {
                return;
            }
            
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
