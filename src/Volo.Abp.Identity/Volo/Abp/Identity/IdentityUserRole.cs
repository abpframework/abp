using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Represents the link between a user and a role.
    /// </summary>
    public class IdentityUserRole : Entity
    {
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        public virtual string UserId { get; protected set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual string RoleId { get; protected set; }

        protected IdentityUserRole()
        {
            
        }

        public IdentityUserRole([NotNull] string userId, [NotNull] string roleId)
        {
            Check.NotNull(userId, nameof(userId));
            Check.NotNull(roleId, nameof(roleId));

            UserId = userId;
            RoleId = roleId;
        }
    }
}