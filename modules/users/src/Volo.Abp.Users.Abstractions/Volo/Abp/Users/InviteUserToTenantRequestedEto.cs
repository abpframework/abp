using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Users;

[Serializable]
[EventName("Volo.Abp.Users.InviteUserToTenantRequested")]
public class InviteUserToTenantRequestedEto : IMultiTenant
{
    public Guid? TenantId { get; set; }

    public string Email { get; set; }

    public bool DirectlyAddToTenant { get; set; }
}
