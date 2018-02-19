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
    }
}