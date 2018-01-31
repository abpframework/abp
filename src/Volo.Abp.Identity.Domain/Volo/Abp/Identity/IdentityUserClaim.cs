using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Represents a claim that a user possesses. 
    /// </summary>
    public class IdentityUserClaim : IdentityClaim
    {
        /// <summary>
        /// Gets or sets the primary key of the user associated with this claim.
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        protected IdentityUserClaim()
        {

        }

        protected internal IdentityUserClaim(Guid id, Guid userId, [NotNull] Claim claim)
            : base(id, claim)
        {
            UserId = userId;

        }

        protected internal IdentityUserClaim(Guid id, Guid userId, [NotNull] string claimType, string claimValue)
            : base(id, claimType, claimValue)
        {
            UserId = userId;
        }
    }
}