using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement.Integration;

public class PermissionGrantOutput
{
    public Dictionary<string, bool> Permissions { get; set; }
}
