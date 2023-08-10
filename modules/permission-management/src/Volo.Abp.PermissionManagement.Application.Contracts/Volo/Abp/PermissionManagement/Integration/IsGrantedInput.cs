using System;

namespace Volo.Abp.PermissionManagement.Integration;

public class IsGrantedInput
{
    public Guid UserId { get; set; }

    public string[] PermissionNames { get; set; }
}
