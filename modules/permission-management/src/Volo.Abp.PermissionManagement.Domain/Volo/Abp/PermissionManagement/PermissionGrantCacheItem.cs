using System;

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
    }
}