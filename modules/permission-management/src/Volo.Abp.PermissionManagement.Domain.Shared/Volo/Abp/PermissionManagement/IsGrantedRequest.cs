using System;

namespace Volo.Abp.PermissionManagement;

public class IsGrantedRequest
{
    public Guid UserId { get; set; }

    public string[] PermissionNames { get; set; }
}
