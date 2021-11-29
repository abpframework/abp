using System;
using Confluent.Kafka;

namespace Volo.Abp.Kafka
{
    public interface IProducerPool : IDisposable
    {
        IProducer<string, byte[]> Get(string connectionName = null);
    }
}
