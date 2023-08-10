using System;
using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement.Integration;

public class PermissionGrantOutput
{
    public Guid UserId { get; set; }

    public Dictionary<string, bool> Permissions { get; set; }
}
