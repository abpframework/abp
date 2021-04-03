using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;

namespace Volo.Abp.AzureServiceBus
{
    public static class ServiceBusAdministrationClientExtensions
    {
        public static async Task SetupTopicAsync(this ServiceBusAdministrationClient client, string topicName)
        {
            if (!await client.TopicExistsAsync(topicName))
            {
                await client.CreateTopicAsync(topicName);   
            }
        }

        public static async Task SetupSubscriptionAsync(this ServiceBusAdministrationClient client, string topicName, string subscriptionName)
        {
            await client.SetupTopicAsync(topicName);
            if (!await client.SubscriptionExistsAsync(topicName, subscriptionName))
            {
                await client.CreateSubscriptionAsync(topicName, subscriptionName);
            }
        }
    }
}