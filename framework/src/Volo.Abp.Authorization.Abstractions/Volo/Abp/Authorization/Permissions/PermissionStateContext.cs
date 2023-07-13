using System;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionStateContext
{
    public IServiceProvider ServiceProvider { get; set; } = default!;

    public PermissionDefinition Permission { get; set; } = default!;
}
