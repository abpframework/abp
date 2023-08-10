using System;

namespace Volo.Abp.PermissionManagement.Integration;

public class PermissionGrantInput
{
    public Guid UserId { get; set; }

    public string[] PermissionNames { get; set; }
}
