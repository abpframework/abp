using System;
using System.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.SettingManagement
{
    [Serializable]
    [IgnoreMultiTenancy]
    public class SettingCacheItem
    {
        private const string CacheKeyFormat = "pn:{0},pk:{1},n:{2}";

        public string Value { get; set; }

        public SettingCacheItem()
        {

        }

        public SettingCacheItem(string value)
        {
            Value = value;
        }

        public static string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return string.Format(CacheKeyFormat, providerName, providerKey, name);
        }

        public static string GetSettingNameFormCacheKeyOrNull(string cacheKey)
        {
            var result = FormattedStringValueExtracter.Extract(cacheKey, CacheKeyFormat, true);
            return result.IsMatch ? result.Matches.Last().Value : null;
        }
    }
}
