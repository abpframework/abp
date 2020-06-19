using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionGrantInfoDto : ExtensibleObject
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ParentName { get; set; }

        public bool IsGranted { get; set; }

        public List<string> AllowedProviders { get; set; }

        public List<ProviderInfoDto> GrantedProviders { get; set; }
    }
}