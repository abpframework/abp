using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Volo.Abp.AzureServiceBus
{
    public interface IAzureServiceBusMessageConsumer
    {
        void OnMessageReceived(Func<ServiceBusReceivedMessage, Task> callback);
    }
}