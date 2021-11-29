using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Volo.Abp.AzureServiceBus
{
    public interface IProcessorPool : IAsyncDisposable
    {
        Task<ServiceBusProcessor> GetAsync(string subscriptionName, string topicName, string connectionName);
    }
}