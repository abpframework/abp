using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Volo.Abp.AzureServiceBus;

public class ClientConfig
{
    public string ConnectionString { get; set; }

    public ServiceBusAdministrationClientOptions Admin { get; set; } = new();

    public ServiceBusClientOptions Client { get; set; } = new();

    public ServiceBusProcessorOptions Processor { get; set; } = new();
}
