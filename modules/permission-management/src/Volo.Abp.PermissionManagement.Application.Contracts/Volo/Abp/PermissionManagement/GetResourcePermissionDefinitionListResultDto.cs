using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement;

public class GetResourcePermissionDefinitionListResultDto
{
    public List<ResourcePermissionDefinitionDto> Permissions { get; set; }
}
