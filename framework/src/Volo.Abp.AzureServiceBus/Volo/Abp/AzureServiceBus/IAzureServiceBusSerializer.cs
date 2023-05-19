using System;

namespace Volo.Abp.AzureServiceBus;

public interface IAzureServiceBusSerializer
{
    byte[] Serialize(object obj);

    object Deserialize(byte[] value, Type type);

    T Deserialize<T>(byte[] value);
}
