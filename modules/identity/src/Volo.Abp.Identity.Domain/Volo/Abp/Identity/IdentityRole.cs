using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Represents a role in the identity system
    /// </summary>
    public class IdentityRole : AggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        public virtual string Name { get; protected internal set; }

        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedName { get; protected internal set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public virtual ICollection<IdentityRoleClaim> Claims { get; protected set; }

        /// <summary>
        /// A default role is automatically assigned to a new user
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// A static role can not be deleted/renamed
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// A user can see other user's public roles
        /// </summary>
        public virtual bool IsPublic { get; set; }
        
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityRole"/>.
        /// </summary>
        protected IdentityRole() { }

        public IdentityRole(Guid id, [NotNull] string name, Guid? tenantId = null)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            TenantId = tenantId;
            NormalizedName = name.ToUpperInvariant();
            ConcurrencyStamp = Guid.NewGuid().ToString();

            Claims = new Collection<IdentityRoleClaim>();
        }

        public virtual void AddClaim([NotNull] IGuidGenerator guidGenerator, [NotNull] Claim claim)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(claim, nameof(claim));

            Claims.Add(new IdentityRoleClaim(guidGenerator.Create(), Id, claim, TenantId));
        }

        public virtual void AddClaims([NotNull] IGuidGenerator guidGenerator, [NotNull] IEnumerable<Claim> claims)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                AddClaim(guidGenerator, claim);
            }
        }

        public virtual IdentityRoleClaim FindClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
        }

        public virtual void RemoveClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.RemoveAll(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
        }

        public virtual void ChangeName(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var oldName = Name;
            Name = name;

            AddLocalEvent(
#pragma warning disable 618
                new IdentityRoleNameChangedEvent
#pragma warning restore 618
                {
                    IdentityRole = this,
                    OldName = oldName
                }
            );

            AddDistributedEvent(
                new IdentityRoleNameChangedEto
                {
                    Id = Id,
                    Name = Name,
                    OldName = oldName,
                    TenantId = TenantId
                }
            );
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Name = {Name}";
        }
    }
}
