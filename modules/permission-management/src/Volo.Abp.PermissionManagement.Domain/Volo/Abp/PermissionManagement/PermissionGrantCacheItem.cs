using System;
using System.Linq;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.PermissionManagement
{
    [Serializable]
    public class PermissionGrantCacheItem
    {
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
            return "pn:" + providerName + ",pk:" + providerKey + ",n:" + name;
        }

        public static string ParseCacheKeyOrNull(string key)
        {
            var format = "pn:{0},pk:{1},n:{2}";
            var result = FormattedStringValueExtracter.Extract(key, format, true);
            return result.IsMatch ? result.Matches.Last().Value : null;
        }
    }
}
