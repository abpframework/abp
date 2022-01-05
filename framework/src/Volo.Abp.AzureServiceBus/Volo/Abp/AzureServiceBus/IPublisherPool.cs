using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Volo.Abp.AzureServiceBus;

public interface IPublisherPool : IAsyncDisposable
{
    Task<ServiceBusSender> GetAsync(string topicName, string connectionName);
}
