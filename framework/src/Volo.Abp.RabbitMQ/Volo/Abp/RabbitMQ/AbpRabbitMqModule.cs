using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.RabbitMQ
{
    [DependsOn(
        typeof(AbpJsonModule)
        )]
    public class AbpRabbitMqModule : AbpModule
    {
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
