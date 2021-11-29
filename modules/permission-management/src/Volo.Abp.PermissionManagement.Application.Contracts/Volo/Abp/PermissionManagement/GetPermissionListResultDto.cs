using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement
{
    public class GetPermissionListResultDto
    {
        public string EntityDisplayName { get; set; }

        public List<PermissionGroupDto> Groups { get; set; }
    }
}