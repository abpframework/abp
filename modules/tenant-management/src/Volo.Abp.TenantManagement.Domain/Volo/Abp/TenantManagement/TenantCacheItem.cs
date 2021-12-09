using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement;

[Serializable]
[IgnoreMultiTenancy]
public class TenantCacheItem
{
    private const string CacheKeyFormat = "i:{0},n:{1}";

    public TenantConfiguration Value { get; set; }

    public TenantCacheItem()
    {

    }

    public TenantCacheItem(TenantConfiguration value)
    {
        Value = value;
    }

    public static string CalculateCacheKey(Guid? id, string name)
    {
        if (id == null && name.IsNullOrWhiteSpace())
        {
            throw new AbpException("Both id and name can't be invalid.");
        }

        return string.Format(CacheKeyFormat,
            id?.ToString() ?? "null",
            (name.IsNullOrWhiteSpace() ? "null" : name));
    }
}
