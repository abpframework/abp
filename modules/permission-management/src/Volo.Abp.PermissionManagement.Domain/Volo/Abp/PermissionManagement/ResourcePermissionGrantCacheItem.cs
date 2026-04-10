using System;
using System.Linq;
using Volo.Abp.Text.Formatting;

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
        var result = FormattedStringValueExtracter.Extract(cacheKey, CacheKeyFormat, true);
        return result.IsMatch ? result.Matches.Last().Value : null;
    }
}
