using System;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Volo.Abp.AzureServiceBus;

public interface IConnectionPool : IAsyncDisposable
{
    ServiceBusClient GetClient(string connectionName);

    ServiceBusAdministrationClient GetAdministrationClient(string connectionName);
}
