using System;
using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

public abstract class IdentityClaim : Entity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string ClaimType { get; protected set; }

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string ClaimValue { get; protected set; }

    protected IdentityClaim()
    {

    }

    protected internal IdentityClaim(Guid id, [NotNull] Claim claim, Guid? tenantId)
        : this(id, claim.Type, claim.Value, tenantId)
    {

    }

    protected internal IdentityClaim(Guid id, [NotNull] string claimType, string claimValue, Guid? tenantId)
    {
        Check.NotNull(claimType, nameof(claimType));

        Id = id;
        ClaimType = claimType;
        ClaimValue = claimValue;
        TenantId = tenantId;
    }

    /// <summary>
    /// Creates a Claim instance from this entity.
    /// </summary>
    /// <returns></returns>
    public virtual Claim ToClaim()
    {
        return new Claim(ClaimType, ClaimValue);
    }

    public virtual void SetClaim([NotNull] Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }
}
