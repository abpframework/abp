using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionGrantInfoDto
{
    public string ProviderName { get; set; }

    public string ProviderKey { get; set; }

    public string ProviderDisplayName { get; set; }

    public string ProviderNameDisplayName { get; set; }

    public List<GrantedResourcePermissionDto> Permissions { get; set; }
}
