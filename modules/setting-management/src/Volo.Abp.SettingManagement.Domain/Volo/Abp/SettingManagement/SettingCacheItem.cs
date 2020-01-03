using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.SettingManagement
{
    [Serializable]
    [IgnoreMultiTenancy]
    public class SettingCacheItem
    {
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
            return "pn:" + providerName + ",pk:" + providerKey + ",n:" + name;
        }
    }
}
