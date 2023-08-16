using System;

namespace Volo.Abp.Identity;

public class RoleWithUserCount
{
    public IdentityRole Role { get; set; }

    public long UserCount { get; set; }
    
    public RoleWithUserCount(IdentityRole role, long userCount)
    {
        Role = role;
        UserCount = userCount;
    }
}
