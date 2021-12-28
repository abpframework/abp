using System.Threading.Tasks;

namespace Volo.Abp.AzureServiceBus;

public interface IAzureServiceBusMessageConsumerFactory
{
    /// <summary>
    /// Creates a new <see cref="IAzureServiceBusMessageConsumerFactory"/>.
    /// Avoid to create too many consumers since they are
    /// not disposed until end of the application.
    /// </summary>
    /// <param name="topicName"></param>
    /// <param name="subscriptionName"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    IAzureServiceBusMessageConsumer CreateMessageConsumer(
        string topicName,
        string subscriptionName,
        string connectionName);
}
