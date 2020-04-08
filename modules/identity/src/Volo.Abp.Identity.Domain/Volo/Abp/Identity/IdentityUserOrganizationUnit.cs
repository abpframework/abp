using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity.Organizations;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Represents membership of a User to an OU.
    /// </summary>
    public class IdentityUserOrganizationUnit : CreationAuditedEntity, IMultiTenant, ISoftDelete
    {

        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        public virtual Guid? TenantId { get; set; }

        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Id of the related <see cref="OrganizationUnit"/>.
        /// </summary>
        public virtual Guid OrganizationUnitId { get; set; }

        /// <summary>
        /// Specifies if the organization is soft deleted or not.
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        protected IdentityUserOrganizationUnit()
        {

        }

        public IdentityUserOrganizationUnit(Guid? tenantId, Guid userId, Guid organizationUnitId)
        {
            TenantId = tenantId;
            UserId = userId;
            OrganizationUnitId = organizationUnitId;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, OrganizationUnitId };
        }
    }
}
