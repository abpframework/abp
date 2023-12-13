using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

[Serializable]
public class IdentityUserEmailChangedEto : IMultiTenant 
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Email { get; set; }

    public string OldEmail { get; set; }
}