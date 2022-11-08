using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.Users;

[Serializable]
[EventName("Volo.Abp.Users.UserPasswordChangeRequested")]
public class UserPasswordChangeRequestedEto
{
    public Guid TenantId { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }
}
