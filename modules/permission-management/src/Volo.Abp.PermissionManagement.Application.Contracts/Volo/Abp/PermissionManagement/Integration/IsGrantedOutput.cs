using System;
using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement.Integration;

public class IsGrantedOutput
{
    public Guid UserId { get; set; }

    public List<string> GrantedPermissionNames { get; set; }
}
