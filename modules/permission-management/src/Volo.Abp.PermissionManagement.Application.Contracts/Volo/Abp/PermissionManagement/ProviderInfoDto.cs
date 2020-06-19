using Volo.Abp.ObjectExtending;

namespace Volo.Abp.PermissionManagement
{
    public class ProviderInfoDto : ExtensibleObject
    {
        public string ProviderName { get; set; }

        public string ProviderKey { get; set; }
    }
}