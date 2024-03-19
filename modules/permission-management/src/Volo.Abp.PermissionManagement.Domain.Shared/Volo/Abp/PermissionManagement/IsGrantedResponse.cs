using System;
using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement;

public class IsGrantedResponse
{
    public Guid UserId { get; set; }

    public Dictionary<string, bool> Permissions { get; set; }
}
