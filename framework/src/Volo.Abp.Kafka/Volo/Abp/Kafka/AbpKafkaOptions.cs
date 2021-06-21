using System;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Volo.Abp.Kafka
{
    public class AbpKafkaOptions
    {
        public KafkaConnections Connections { get; }

        public Action<ProducerConfig> ConfigureProducer { get; set; }

        public Action<ConsumerConfig> ConfigureConsumer { get; set; }

        public Action<TopicSpecification> ConfigureTopic { get; set; }

        public AbpKafkaOptions()
        {
            Connections = new KafkaConnections();
        }
    }
}
