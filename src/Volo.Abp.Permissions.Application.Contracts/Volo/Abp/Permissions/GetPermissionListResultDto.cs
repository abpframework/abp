using System.Collections.Generic;

namespace Volo.Abp.Permissions
{
    public class GetPermissionListResultDto
    {
        public List<PermissionGroupDto> Groups { get; set; }
    }
}