using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionGroupDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<PermissionGrantInfoDto> Permissions { get; set; }
    }
}