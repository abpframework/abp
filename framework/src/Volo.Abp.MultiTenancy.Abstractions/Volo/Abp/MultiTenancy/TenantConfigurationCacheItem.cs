using System;

namespace Volo.Abp.MultiTenancy;

[Serializable]
[IgnoreMultiTenancy]
public class TenantConfigurationCacheItem
{
    private const string CacheKeyFormat = "i:{0},n:{1}";

    public TenantConfiguration? Value { get; set; }

    public TenantConfigurationCacheItem()
    {

    }

    public TenantConfigurationCacheItem(TenantConfiguration? value)
    {
        Value = value;
    }

    public static string CalculateCacheKey(Guid? id, string? name)
    {
        if (id == null && name.IsNullOrWhiteSpace())
        {
            throw new AbpException("Both id and name can't be invalid.");
        }
        return string.Format(CacheKeyFormat,
            id?.ToString() ?? "null",
            (name.IsNullOrWhiteSpace() ? "null" : name));
    }

    public static string CalculateCacheKey(Guid id)
    {
        return string.Format(CacheKeyFormat, id.ToString(), "null" );
    }

    public static string CalculateCacheKey(string name)
    {
        return string.Format(CacheKeyFormat, "null", name);
    }
}
