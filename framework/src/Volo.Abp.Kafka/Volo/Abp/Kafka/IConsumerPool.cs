using System;
using Confluent.Kafka;

namespace Volo.Abp.Kafka;

public interface IConsumerPool : IDisposable
{
    IConsumer<string, byte[]> Get(string groupId, string connectionName = null);
}
