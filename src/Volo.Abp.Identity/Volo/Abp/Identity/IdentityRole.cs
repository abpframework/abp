// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.Identity
{
    //TODO: Should set Id to a GUID (where? on repository?)
    //TODO: Properties should not be public!

    /// <summary>
    /// Represents a role in the identity system
    /// </summary>
    public class IdentityRole : AggregateRoot, IHasConcurrencyStamp
    {
        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public virtual ICollection<IdentityRoleClaim> Claims { get; } = new Collection<IdentityRoleClaim>();

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityRole"/>.
        /// </summary>
        protected IdentityRole() { }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityRole"/>.
        /// </summary>
        /// <param name="name">The role name.</param>
        public IdentityRole([NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
        }

        public void AddClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.Add(new IdentityRoleClaim(Id, claim));
        }

        public void AddClaims([NotNull] IEnumerable<Claim> claims)
        {
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                AddClaim(claim);
            }
        }

        /// <summary>
        /// Returns the name of the role.
        /// </summary>
        /// <returns>The name of the role.</returns>
        public override string ToString()
        {
            return Name;
        }

        public void RemoveClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.RemoveAll(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
        }
    }
}
