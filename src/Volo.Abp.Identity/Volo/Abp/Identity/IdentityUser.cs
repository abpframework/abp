using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.Identity
{
    //Add Name and Surname properties?

    public class IdentityUser : AggregateRoot, IHasConcurrencyStamp
    {
        public const int MaxUserNameLength = 256;
        public const int MaxNormalizedUserNameLength = MaxUserNameLength;
        public const int MaxEmailLength = 256;
        public const int MaxNormalizedEmailLength = MaxEmailLength;

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName { get; protected internal set; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        public virtual string NormalizedUserName { get; protected internal set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email { get; protected internal set; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        public virtual string NormalizedEmail { get; protected internal set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed { get; protected internal set; }

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public virtual string PasswordHash { get; protected internal set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; protected internal set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        public virtual string PhoneNumber { get; protected internal set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        /// <value>True if the telephone number has been confirmed, otherwise false.</value>
        public virtual bool PhoneNumberConfirmed { get; protected internal set; }

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        /// <value>True if 2fa is enabled, otherwise false.</value>
        public virtual bool TwoFactorEnabled { get; protected internal set; }

        /// <summary>
        /// Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        /// <remarks>
        /// A value in the past means the user is not locked out.
        /// </remarks>
        public virtual DateTimeOffset? LockoutEnd { get; protected internal set; }

        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        /// <value>True if the user could be locked out, otherwise false.</value>
        public virtual bool LockoutEnabled { get; protected internal set; }

        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        public virtual int AccessFailedCount { get; protected internal set; }

        //TODO: Can we make collections readonly collection, which will provide encapsulation but can work for all ORMs?

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual IReadOnlyList<IdentityUserRole> Roles => RolesCollection;
        protected virtual List<IdentityUserRole> RolesCollection { get; set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim> Claims { get; protected set; }

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin> Logins { get; protected set; }

        /// <summary>
        /// Navigation property for this users tokens.
        /// </summary>
        public virtual ICollection<IdentityUserToken> Tokens { get; protected set; }

        protected IdentityUser()
        {

        }

        public IdentityUser(Guid id, [NotNull] string userName)
        {
            Check.NotNull(userName, nameof(userName));

            Id = id;
            UserName = userName;
            ConcurrencyStamp = Guid.NewGuid().ToString();

            RolesCollection = new List<IdentityUserRole>();
            Claims = new Collection<IdentityUserClaim>();
            Logins = new Collection<IdentityUserLogin>();
            Tokens = new Collection<IdentityUserToken>();
        }

        public void AddRole(IGuidGenerator guidGenerator, Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (IsInRole(roleId))
            {
                return;
            }

            RolesCollection.Add(new IdentityUserRole(guidGenerator.Create(), Id, roleId));
        }

        public void RemoveRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (!IsInRole(roleId))
            {
                return;
            }

            RolesCollection.RemoveAll(r => r.RoleId == roleId);
        }

        public bool IsInRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            return Roles.Any(r => r.RoleId == roleId);
        }

        public void AddClaim([NotNull] IGuidGenerator guidGenerator, [NotNull] Claim claim)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(claim, nameof(claim));

            Claims.Add(new IdentityUserClaim(guidGenerator.Create(), Id, claim));
        }

        public void AddClaims([NotNull] IGuidGenerator guidGenerator, [NotNull] IEnumerable<Claim> claims)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                AddClaim(guidGenerator, claim);
            }
        }

        public void ReplaceClaim([NotNull] Claim claim, [NotNull] Claim newClaim)
        {
            Check.NotNull(claim, nameof(claim));
            Check.NotNull(newClaim, nameof(newClaim));

            var userClaims = Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
            foreach (var userClaim in userClaims)
            {
                userClaim.SetClaim(newClaim);
            }
        }

        public void RemoveClaims([NotNull] IEnumerable<Claim> claims)
        {
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                RemoveClaim(claim);
            }
        }

        public void RemoveClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.RemoveAll(c => c.ClaimValue == claim.Value && c.ClaimType == claim.Type);
        }

        public void AddLogin([NotNull] IGuidGenerator guidGenerator, [NotNull] UserLoginInfo login)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(login, nameof(login));

            Logins.Add(new IdentityUserLogin(guidGenerator.Create(), Id, login));
        }

        public void RemoveLogin([NotNull] string loginProvider, [NotNull] string providerKey)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));

            Logins.RemoveAll(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
        }

        [CanBeNull]
        public IdentityUserToken FindToken(string loginProvider, string name)
        {
            return Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        public void SetToken(IGuidGenerator guidGenerator, string loginProvider, string name, string value)
        {
            var token = FindToken(loginProvider, name);
            if (token == null)
            {
                Tokens.Add(new IdentityUserToken(guidGenerator.Create(), Id, loginProvider, name, value));
            }
            else
            {
                token.Value = value;
            }
        }

        public void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        /// <summary>
        /// Returns the username for this user.
        /// </summary>
        public override string ToString()
        {
            return $"{base.ToString()} UserName = {UserName}";
        }
    }
}
