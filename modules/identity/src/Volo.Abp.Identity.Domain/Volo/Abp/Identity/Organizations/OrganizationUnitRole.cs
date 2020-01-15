using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity.Organizations
{
    /// <summary>
    /// Represents membership of a User to an OU.
    /// </summary>
    public class OrganizationUnitRole : CreationAuditedEntity, IMultiTenant, ISoftDelete
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// Id of the Role.
        /// </summary>
        public virtual Guid RoleId { get; set; }

        /// <summary>
        /// Id of the <see cref="OrganizationUnit"/>.
        /// </summary>
        public virtual Guid OrganizationUnitId { get; set; }

        /// <summary>
        /// Specifies if the organization is soft deleted or not.
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUnitRole"/> class.
        /// </summary>
        public OrganizationUnitRole()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUnitRole"/> class.
        /// </summary>
        /// <param name="tenantId">TenantId</param>
        /// <param name="roleId">Id of the User.</param>
        /// <param name="organizationUnitId">Id of the <see cref="OrganizationUnit"/>.</param>
        public OrganizationUnitRole(Guid? tenantId, Guid roleId, Guid organizationUnitId)
        {
            TenantId = tenantId;
            RoleId = roleId;
            OrganizationUnitId = organizationUnitId;
        }

        public override object[] GetKeys()
        {
            return new object[] { OrganizationUnitId, RoleId };
        }
    }
}
