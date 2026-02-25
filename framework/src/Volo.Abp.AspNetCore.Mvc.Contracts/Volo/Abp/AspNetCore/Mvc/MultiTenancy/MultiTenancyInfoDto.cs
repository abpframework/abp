using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Mvc.MultiTenancy;

public class MultiTenancyInfoDto
{
    public bool IsEnabled { get; set; }

    public TenantUserSharingStrategy UserSharingStrategy { get; set; }
}
