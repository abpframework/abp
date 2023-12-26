using System;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

[Serializable]
public class IdentitySessionEto : IMultiTenant
{
    public Guid Id { get; set; }

    public virtual string SessionId  { get; set; }

    public virtual string Device  { get; set; }

    public virtual string DeviceInfo  { get; set; }

    public virtual Guid? TenantId  { get; set; }

    public virtual Guid UserId  { get; set; }

    public virtual string ClientId { get; set; }

    public virtual string IpAddresses  { get; set; }

    public virtual DateTime SignedIn  { get; set; }

    public virtual DateTime? LastAccessed  { get; set; }
}
