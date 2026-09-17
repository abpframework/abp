using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement;

public class GetResourcePermissionWithProviderListResultDto
{
    public List<ResourcePermissionWithProdiverGrantInfoDto> Permissions { get; set; }
}
