using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.PermissionManagement
{
    public class GetPermissionListResultDto : ExtensibleObject
    {
        public string EntityDisplayName { get; set; }

        public List<PermissionGroupDto> Groups { get; set; }
    }
}