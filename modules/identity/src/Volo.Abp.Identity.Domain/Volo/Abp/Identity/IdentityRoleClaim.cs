using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents a claim that is granted to all users within a role.
/// </summary>
public class IdentityRoleClaim : IdentityClaim
{
    /// <summary>
    /// Gets or sets the of the primary key of the role associated with this claim.
    /// </summary>
    public virtual Guid RoleId { get; protected set; }

    protected IdentityRoleClaim()
    {

    }

    protected internal IdentityRoleClaim(
        Guid id,
        Guid roleId,
        [NotNull] Claim claim,
        Guid? tenantId)
        : base(
              id,
              claim,
              tenantId)
    {
        RoleId = roleId;
    }

    public IdentityRoleClaim(
        Guid id,
        Guid roleId,
        [NotNull] string claimType,
        string claimValue,
        Guid? tenantId)
        : base(
              id,
              claimType,
              claimValue,
              tenantId)
    {
        RoleId = roleId;
    }
}
