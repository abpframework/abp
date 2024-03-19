using System;
using System.Collections.Generic;

namespace Volo.Abp.Identity;

public class IdentityUserIdWithRoleNames
{
    public Guid Id { get; set; }

    public string[] RoleNames { get; set; }
}