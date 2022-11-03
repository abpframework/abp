using System;

namespace Volo.Abp.Dapr;

public interface IDaprSerializer
{
    byte[] Serialize(object obj);

    object Deserialize(byte[] value, Type type);

    object Deserialize(string value, Type type);
}
