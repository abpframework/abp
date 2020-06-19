using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionGroupDto : ExtensibleObject
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<PermissionGrantInfoDto> Permissions { get; set; }
    }
}