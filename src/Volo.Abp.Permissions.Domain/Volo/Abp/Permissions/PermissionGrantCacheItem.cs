using System;

namespace Volo.Abp.Permissions
{
    [Serializable]
    public class PermissionGrantCacheItem
    {
        public string Name { get; set; }

        public bool IsGranted { get; set; }

        public PermissionGrantCacheItem()
        {

        }

        public PermissionGrantCacheItem(string name, bool isGranted)
        {
            Name = name;
            IsGranted = isGranted;
        }

        public static string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return "pn:" + providerName + ",pk:" + providerKey + ",n:" + name;
        }
    }
}