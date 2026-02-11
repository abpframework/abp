using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents the link between a role and an organization unit.
/// </summary>
public class OrganizationUnitRole : CreationAuditedEntity, IMultiTenant
{
    /// <summary>
    /// TenantId of this entity.
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Id of the Role.
    /// </summary>
    public virtual Guid RoleId { get; protected set; }

    /// <summary>
    /// Id of the <see cref="OrganizationUnit"/>.
    /// </summary>
    public virtual Guid OrganizationUnitId { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationUnitRole"/> class.
    /// </summary>
    protected OrganizationUnitRole()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationUnitRole"/> class.
    /// </summary>
    /// <param name="tenantId">TenantId</param>
    /// <param name="roleId">Id of the Role.</param>
    /// <param name="organizationUnitId">Id of the <see cref="OrganizationUnit"/>.</param>
    public OrganizationUnitRole(Guid roleId, Guid organizationUnitId, Guid? tenantId = null)
    {
        RoleId = roleId;
        OrganizationUnitId = organizationUnitId;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return new object[] { OrganizationUnitId, RoleId };
    }
}
