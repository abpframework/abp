using System;

namespace Volo.Abp.AzureServiceBus
{
    public interface IAzureServiceBusSerializer
    {
        byte[] Serialize(object obj);

        object Deserialize(BinaryData value, Type type);
    }
}