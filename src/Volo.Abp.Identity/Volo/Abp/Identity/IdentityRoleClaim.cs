using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Represents a claim that is granted to all users within a role.
    /// </summary>
    public class IdentityRoleClaim : Entity
    {
        /// <summary>
        /// Gets or sets the of the primary key of the role associated with this claim.
        /// </summary>
        public virtual string RoleId { get; protected set; }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string ClaimType { get; protected set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string ClaimValue { get; protected set; }

        protected IdentityRoleClaim()
        {
            
        }

        public IdentityRoleClaim([NotNull] string roleId, [NotNull] Claim claim)
        {
            Check.NotNull(roleId, nameof(roleId));
            Check.NotNull(claim, nameof(claim));

            RoleId = roleId;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public IdentityRoleClaim([NotNull] string roleId, [NotNull] string claimType, string claimValue)
        {
            Check.NotNull(roleId, nameof(roleId));
            Check.NotNull(claimType, nameof(claimType));

            RoleId = roleId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        /// <summary>
        /// Constructs a new claim with the type and value.
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }
}