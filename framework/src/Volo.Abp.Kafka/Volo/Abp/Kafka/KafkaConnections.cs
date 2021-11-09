using System;
using System.Collections.Generic;
using Confluent.Kafka;
using JetBrains.Annotations;

namespace Volo.Abp.Kafka;

[Serializable]
public class KafkaConnections : Dictionary<string, ClientConfig>
{
    public const string DefaultConnectionName = "Default";

    [NotNull]
    public ClientConfig Default
    {
        get => this[DefaultConnectionName];
        set => this[DefaultConnectionName] = Check.NotNull(value, nameof(value));
    }

    public KafkaConnections()
    {
        Default = new ClientConfig();
    }

    public ClientConfig GetOrDefault(string connectionName)
    {
        if (TryGetValue(connectionName, out var connectionFactory))
        {
            return connectionFactory;
        }

        return Default;
    }
}
