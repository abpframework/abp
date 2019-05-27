using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.RabbitMQ
{
    [DependsOn(
        typeof(JsonModule),
        typeof(ThreadingModule)
        )]
    public class RabbitMqModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpRabbitMqOptions>(configuration.GetSection("RabbitMQ"));
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            context.ServiceProvider
                .GetRequiredService<IChannelPool>()
                .Dispose();

            context.ServiceProvider
                .GetRequiredService<IConnectionPool>()
                .Dispose();
        }
    }
}
