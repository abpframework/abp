using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Represents a claim that a user possesses. 
    /// </summary>
    public class IdentityUserClaim : Entity
    {
        /// <summary>
        /// Gets or sets the primary key of the user associated with this claim.
        /// </summary>
        public virtual string UserId { get; protected set; }

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

        public IdentityUserClaim([NotNull] string userId, [NotNull] Claim claim)
        {
            Check.NotNull(userId, nameof(userId));
            Check.NotNull(claim, nameof(claim));

            UserId = userId;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public IdentityUserClaim([NotNull] string userId, [NotNull] string claimType, string claimValue)
        {
            Check.NotNull(userId, nameof(userId));
            Check.NotNull(claimType, nameof(claimType));

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
    }
}