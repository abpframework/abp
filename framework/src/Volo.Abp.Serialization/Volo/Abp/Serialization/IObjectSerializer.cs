namespace Volo.Abp.Serialization;

public interface IObjectSerializer
{
    byte[] Serialize<T>(T obj);

    T Deserialize<T>(byte[] bytes);
}

public interface IObjectSerializer<T>
{
    byte[] Serialize(T obj);

    T Deserialize(byte[] bytes);
}
