using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement;

public class UpdateResourcePermissionsDto
{
    public string ProviderName { get; set; }

    public string ProviderKey { get; set; }

    public List<string> Permissions { get; set; }
}
