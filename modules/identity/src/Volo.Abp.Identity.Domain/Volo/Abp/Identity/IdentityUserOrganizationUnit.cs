using System;
using Volo.Abp.Domain.Entities.Auditing;
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
        /// Id of the <see cref="OrganizationUnit"/>.
        /// </summary>
        public virtual Guid OrganizationUnitId { get; set; }

        /// <summary>
        /// Specifies if the organization is soft deleted or not.
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// </summary>
        public IdentityUserOrganizationUnit()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// </summary>
        /// <param name="tenantId">TenantId</param>
        /// <param name="userId">Id of the User.</param>
        /// <param name="organizationUnitId">Id of the <see cref="OrganizationUnit"/>.</param>
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
