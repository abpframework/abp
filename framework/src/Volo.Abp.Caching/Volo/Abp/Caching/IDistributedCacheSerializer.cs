namespace Volo.Abp.Caching;

public interface IDistributedCacheSerializer
{
    byte[] Serialize<T>(T obj);

    T Deserialize<T>(byte[] bytes);
}
