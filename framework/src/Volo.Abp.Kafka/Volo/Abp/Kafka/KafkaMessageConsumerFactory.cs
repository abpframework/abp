using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Kafka
{
    public class KafkaMessageConsumerFactory : IKafkaMessageConsumerFactory, ISingletonDependency, IDisposable
    {
        protected IServiceScope ServiceScope { get; }

        public KafkaMessageConsumerFactory(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScope = serviceScopeFactory.CreateScope();
        }

        public IKafkaMessageConsumer Create(
            string topicName,
            string deadLetterTopicName,
            string groupId,
            string connectionName = null)
        {
            var consumer = ServiceScope.ServiceProvider.GetRequiredService<KafkaMessageConsumer>();
            consumer.Initialize(topicName, deadLetterTopicName, groupId, connectionName);
            return consumer;
        }

        public void Dispose()
        {
            ServiceScope?.Dispose();
        }
    }
}
