using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AzureServiceBus;

public class AzureServiceBusMessageConsumerFactory : IAzureServiceBusMessageConsumerFactory, ISingletonDependency, IDisposable
{
    protected IServiceScope ServiceScope { get; }

    public AzureServiceBusMessageConsumerFactory(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScope = serviceScopeFactory.CreateScope();
    }

    public IAzureServiceBusMessageConsumer CreateMessageConsumer(string topicName, string subscriptionName, string connectionName)
    {
        var processor = ServiceScope.ServiceProvider.GetRequiredService<AzureServiceBusMessageConsumer>();
        processor.Initialize(topicName, subscriptionName, connectionName);
        return processor;
    }

    public void Dispose()
    {
        ServiceScope?.Dispose();
    }
}
