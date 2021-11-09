using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AzureServiceBus;

[Serializable]
public class AzureServiceBusConnections : Dictionary<string, ClientConfig>
{
    public const string DefaultConnectionName = "Default";

    [NotNull]
    public ClientConfig Default
    {
        get => this[DefaultConnectionName];
        set => this[DefaultConnectionName] = Check.NotNull(value, nameof(value));
    }

    public AzureServiceBusConnections()
    {
        Default = new ClientConfig();
    }

    public ClientConfig GetOrDefault(string connectionName)
    {
        return TryGetValue(connectionName, out var connectionFactory)
            ? connectionFactory
            : Default;
    }
}
