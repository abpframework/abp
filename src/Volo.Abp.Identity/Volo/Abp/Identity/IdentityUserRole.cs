using System;
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
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual Guid RoleId { get; protected set; }

        protected IdentityUserRole()
        {
            
        }

        public IdentityUserRole(Guid id, Guid userId, Guid roleId)
        {
            Id = id;
            UserId = userId;
            RoleId = roleId;
        }
    }
}