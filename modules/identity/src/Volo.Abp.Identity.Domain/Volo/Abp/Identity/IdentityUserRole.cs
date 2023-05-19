using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents the link between a user and a role.
/// </summary>
public class IdentityUserRole : Entity, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the primary key of the user that is linked to a role.
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the primary key of the role that is linked to the user.
    /// </summary>
    public virtual Guid RoleId { get; protected set; }

    protected IdentityUserRole()
    {

    }

    protected internal IdentityUserRole(Guid userId, Guid roleId, Guid? tenantId)
    {
        UserId = userId;
        RoleId = roleId;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, RoleId };
    }
}
