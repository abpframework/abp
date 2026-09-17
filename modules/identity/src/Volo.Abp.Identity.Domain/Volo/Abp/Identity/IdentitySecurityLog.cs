using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SecurityLog;

namespace Volo.Abp.Identity;

public class IdentitySecurityLog : AggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual string ApplicationName { get; protected set; }

    public virtual string Identity { get; protected set; }

    public virtual string Action { get; protected set; }

    public virtual Guid? UserId { get; protected set; }

    public virtual string UserName { get; protected set; }

    public virtual string TenantName { get; protected set; }

    public virtual string ClientId { get; protected set; }

    public virtual string CorrelationId { get; protected set; }

    public virtual string ClientIpAddress { get; protected set; }

    public virtual string BrowserInfo { get; protected set; }

    public virtual DateTime CreationTime { get; protected set; }

    protected IdentitySecurityLog()
    {

    }

    public IdentitySecurityLog(IGuidGenerator guidGenerator, SecurityLogInfo securityLogInfo)
        : base(guidGenerator.Create())
    {
        TenantId = securityLogInfo.TenantId;
        TenantName = securityLogInfo.TenantName.Truncate(IdentitySecurityLogConsts.MaxTenantNameLength);

        ApplicationName = securityLogInfo.ApplicationName.Truncate(IdentitySecurityLogConsts.MaxApplicationNameLength);
        Identity = securityLogInfo.Identity.Truncate(IdentitySecurityLogConsts.MaxIdentityLength);
        Action = securityLogInfo.Action.Truncate(IdentitySecurityLogConsts.MaxActionLength);

        UserId = securityLogInfo.UserId;
        UserName = securityLogInfo.UserName.Truncate(IdentitySecurityLogConsts.MaxUserNameLength);

        CreationTime = securityLogInfo.CreationTime;

        ClientIpAddress = securityLogInfo.ClientIpAddress.Truncate(IdentitySecurityLogConsts.MaxClientIpAddressLength);
        ClientId = securityLogInfo.ClientId.Truncate(IdentitySecurityLogConsts.MaxClientIdLength);
        CorrelationId = securityLogInfo.CorrelationId.Truncate(IdentitySecurityLogConsts.MaxCorrelationIdLength);
        BrowserInfo = securityLogInfo.BrowserInfo.Truncate(IdentitySecurityLogConsts.MaxBrowserInfoLength);

        foreach (var property in securityLogInfo.ExtraProperties)
        {
            ExtraProperties.Add(property.Key, property.Value);
        }
    }
}
