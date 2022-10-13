namespace Volo.Abp.Dapr;

public interface IDaprSerializer
{
    byte[] Serialize(object obj);

    object Deserialize(byte[] value, Type type);

    T Deserialize<T>(byte[] value);

    string SerializeToString(object obj);

    object Deserialize(string value, Type type);

    T Deserialize<T>(string value);
}
