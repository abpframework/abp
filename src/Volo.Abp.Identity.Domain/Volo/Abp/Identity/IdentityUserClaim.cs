using System;
using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Represents a claim that a user possesses. 
    /// </summary>
    public class IdentityUserClaim : Entity<Guid>
    {
        /// <summary>
        /// Gets or sets the primary key of the user associated with this claim.
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string ClaimType { get; protected set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string ClaimValue { get; protected set; }

        protected IdentityUserClaim()
        {
            
        }

        protected internal IdentityUserClaim(Guid id, Guid userId, [NotNull] Claim claim)
            : this(id, userId, claim.Type, claim.Value)
        {

        }

        protected internal IdentityUserClaim(Guid id, Guid userId, [NotNull] string claimType, string claimValue)
        {
            Check.NotNull(claimType, nameof(claimType));

            Id = id;
            UserId = userId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        /// <summary>
        /// Creates a Claim instance from this entity.
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        public void SetClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}