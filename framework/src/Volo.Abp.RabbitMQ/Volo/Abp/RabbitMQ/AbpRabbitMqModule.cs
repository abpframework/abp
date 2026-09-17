using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.RabbitMQ;

[DependsOn(
    typeof(AbpJsonModule),
    typeof(AbpThreadingModule)
    )]
public class AbpRabbitMqModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpRabbitMqOptions>(configuration.GetSection("RabbitMQ"));
        Configure<AbpRabbitMqOptions>(options =>
        {
            var connectionsSection = configuration.GetSection("RabbitMQ:Connections");
            foreach (var connection in options.Connections)
            {
                var connectionSection = connectionsSection.GetSection(connection.Key);
                connectionSection.GetSection(nameof(ConnectionFactory.Ssl)).Bind(connection.Value.Ssl);

                var maxInboundMessageBodySize = connectionSection.GetValue<uint?>(
                    nameof(ConnectionFactory.MaxInboundMessageBodySize));
                if (maxInboundMessageBodySize.HasValue)
                {
                    connection.Value.MaxInboundMessageBodySize = maxInboundMessageBodySize.Value;
                }

                connection.Value.AutomaticRecoveryEnabled = false;
            }
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationShutdownAsync(context));
    }

    public async override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<IChannelPool>()
            .DisposeAsync();

        await context.ServiceProvider
            .GetRequiredService<IConnectionPool>()
            .DisposeAsync();
    }
}
