using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class IdentityLinkUser : BasicAggregateRoot<Guid>
    {
        public virtual Guid SourceUserId { get; protected set; }

        public virtual Guid? SourceTenantId { get; protected set; }

        public virtual Guid TargetUserId { get; protected set; }

        public virtual Guid? TargetTenantId { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityLinkUser"/>.
        /// </summary>
        protected IdentityLinkUser()
        {

        }

        public IdentityLinkUser(Guid id, IdentityLinkUserInfo sourceUser, IdentityLinkUserInfo targetUser)
            : base(id)
        {
            SourceUserId = sourceUser.UserId;
            SourceTenantId = sourceUser.TenantId;

            TargetUserId = targetUser.UserId;
            TargetTenantId = targetUser.TenantId;
        }

        public IdentityLinkUser(Guid id, Guid sourceUserId, Guid? sourceTenantId, Guid targetUserId, Guid? targetTenantId)
            : base(id)
        {
            SourceUserId = sourceUserId;
            SourceTenantId = sourceTenantId;

            TargetUserId = targetUserId;
            TargetTenantId = targetTenantId;
        }
    }
}
