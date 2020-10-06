using System;

namespace Volo.Abp.Identity
{
    public class IdentityLinkUserInfo
    {
        public virtual Guid UserId { get; set; }

        public virtual Guid? TenantId { get; set; }

        public IdentityLinkUserInfo(Guid userId, Guid? tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }
    }
}
