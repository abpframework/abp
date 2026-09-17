using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement;

public class GetResourcePermissionListResultDto
{
    public List<ResourcePermissionGrantInfoDto> Permissions { get; set; }
}
