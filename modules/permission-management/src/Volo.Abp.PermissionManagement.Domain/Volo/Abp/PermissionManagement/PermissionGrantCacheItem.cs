using System;

namespace Volo.Abp.PermissionManagement;

[Serializable]
public class PermissionGrantCacheItem
{
    private const string CacheKeyFormat = "pn:{0},pk:{1},n:{2}";

    public bool IsGranted { get; set; }

    public PermissionGrantCacheItem()
    {

    }

    public PermissionGrantCacheItem(bool isGranted)
    {
        IsGranted = isGranted;
    }

    public static string CalculateCacheKey(string name, string providerName, string providerKey)
    {
        return string.Format(CacheKeyFormat, providerName, providerKey, name);
    }

    public static string GetPermissionNameFormCacheKeyOrNull(string cacheKey)
    {
        const string separator = ",n:";
        var index = cacheKey.LastIndexOf(separator, StringComparison.Ordinal);
        return index >= 0 ? cacheKey.Substring(index + separator.Length) : null;
    }
}
