using System;

namespace Volo.Abp.PermissionManagement;

[Serializable]
public class ResourcePermissionGrantCacheItem
{
    private const string CacheKeyFormat = "rn:{0},rk:{1},pn:{2},pk:{3},n:{4}";

    public bool IsGranted { get; set; }

    public ResourcePermissionGrantCacheItem()
    {

    }

    public ResourcePermissionGrantCacheItem(bool isGranted)
    {
        IsGranted = isGranted;
    }

    public static string CalculateCacheKey(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return string.Format(CacheKeyFormat, resourceName, resourceKey, providerName, providerKey, name);
    }

    public static string GetPermissionNameFormCacheKeyOrNull(string cacheKey)
    {
        const string separator = ",n:";
        var index = cacheKey.LastIndexOf(separator, StringComparison.Ordinal);
        return index >= 0 ? cacheKey.Substring(index + separator.Length) : null;
    }
}
