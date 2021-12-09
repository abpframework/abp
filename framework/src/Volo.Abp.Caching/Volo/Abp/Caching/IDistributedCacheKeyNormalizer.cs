namespace Volo.Abp.Caching;

public interface IDistributedCacheKeyNormalizer
{
    string NormalizeKey(DistributedCacheKeyNormalizeArgs args);
}
