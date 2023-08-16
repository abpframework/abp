using System;

namespace Volo.Abp.Identity;

public class RoleWithUserCount
{
    public Guid RoleId { get; set; }

    public long UserCount { get; set; }
}
