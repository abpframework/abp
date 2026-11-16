using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionDefinitionDto
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<string> AllowedProviders { get; set; }
}
