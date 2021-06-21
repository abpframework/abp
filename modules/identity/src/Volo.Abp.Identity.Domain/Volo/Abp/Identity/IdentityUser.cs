using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class IdentityUser : FullAuditedAggregateRoot<Guid>, IUser
    {
        public virtual Guid? TenantId { get; protected set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName { get; protected internal set; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedUserName { get; protected internal set; }

        /// <summary>
        /// Gets or sets the Name for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the Surname for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email { get; protected internal set; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedEmail { get; protected internal set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed { get; protected internal set; }

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string PasswordHash { get; protected internal set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        [DisableAuditing]
        public virtual string SecurityStamp { get; protected internal set; }

        public virtual bool IsExternal { get; set; }

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        [CanBeNull]
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

        //TODO: Can we make collections readonly collection, which will provide encapsulation. But... can work for all ORMs?

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole> Roles { get; protected set; }

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

        /// <summary>
        /// Navigation property for this organization units.
        /// </summary>
        public virtual ICollection<IdentityUserOrganizationUnit> OrganizationUnits { get; protected set; }

        protected IdentityUser()
        {
        }

        public IdentityUser(
            Guid id,
            [NotNull] string userName,
            [NotNull] string email,
            Guid? tenantId = null)
            : base(id)
        {
            Check.NotNull(userName, nameof(userName));
            Check.NotNull(email, nameof(email));

            TenantId = tenantId;
            UserName = userName;
            NormalizedUserName = userName.ToUpperInvariant();
            Email = email;
            NormalizedEmail = email.ToUpperInvariant();
            ConcurrencyStamp = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();

            Roles = new Collection<IdentityUserRole>();
            Claims = new Collection<IdentityUserClaim>();
            Logins = new Collection<IdentityUserLogin>();
            Tokens = new Collection<IdentityUserToken>();
            OrganizationUnits = new Collection<IdentityUserOrganizationUnit>();
        }

        public virtual void AddRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (IsInRole(roleId))
            {
                return;
            }

            Roles.Add(new IdentityUserRole(Id, roleId, TenantId));
        }

        public virtual void RemoveRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (!IsInRole(roleId))
            {
                return;
            }

            Roles.RemoveAll(r => r.RoleId == roleId);
        }

        public virtual bool IsInRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            return Roles.Any(r => r.RoleId == roleId);
        }

        public virtual void AddClaim([NotNull] IGuidGenerator guidGenerator, [NotNull] Claim claim)
        {
            Check.NotNull(guidGenerator, nameof(guidGenerator));
            Check.NotNull(claim, nameof(claim));

            Claims.Add(new IdentityUserClaim(guidGenerator.Create(), Id, claim, TenantId));
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

        public virtual IdentityUserClaim FindClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
        }

        public virtual void ReplaceClaim([NotNull] Claim claim, [NotNull] Claim newClaim)
        {
            Check.NotNull(claim, nameof(claim));
            Check.NotNull(newClaim, nameof(newClaim));

            var userClaims = Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
            foreach (var userClaim in userClaims)
            {
                userClaim.SetClaim(newClaim);
            }
        }

        public virtual void RemoveClaims([NotNull] IEnumerable<Claim> claims)
        {
            Check.NotNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                RemoveClaim(claim);
            }
        }

        public virtual void RemoveClaim([NotNull] Claim claim)
        {
            Check.NotNull(claim, nameof(claim));

            Claims.RemoveAll(c => c.ClaimValue == claim.Value && c.ClaimType == claim.Type);
        }

        public virtual void AddLogin([NotNull] UserLoginInfo login)
        {
            Check.NotNull(login, nameof(login));

            Logins.Add(new IdentityUserLogin(Id, login, TenantId));
        }

        public virtual void RemoveLogin([NotNull] string loginProvider, [NotNull] string providerKey)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));

            Logins.RemoveAll(userLogin =>
                userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
        }

        [CanBeNull]
        public virtual IdentityUserToken FindToken(string loginProvider, string name)
        {
            return Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        public virtual void SetToken(string loginProvider, string name, string value)
        {
            var token = FindToken(loginProvider, name);
            if (token == null)
            {
                Tokens.Add(new IdentityUserToken(Id, loginProvider, name, value, TenantId));
            }
            else
            {
                token.Value = value;
            }
        }

        public virtual void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        public virtual void AddOrganizationUnit(Guid organizationUnitId)
        {
            if (IsInOrganizationUnit(organizationUnitId))
            {
                return;
            }

            OrganizationUnits.Add(
                new IdentityUserOrganizationUnit(
                    Id,
                    organizationUnitId,
                    TenantId
                )
            );
        }

        public virtual void RemoveOrganizationUnit(Guid organizationUnitId)
        {
            if (!IsInOrganizationUnit(organizationUnitId))
            {
                return;
            }

            OrganizationUnits.RemoveAll(
                ou => ou.OrganizationUnitId == organizationUnitId
            );
        }

        public virtual bool IsInOrganizationUnit(Guid organizationUnitId)
        {
            return OrganizationUnits.Any(
                ou => ou.OrganizationUnitId == organizationUnitId
            );
        }

        /// <summary>
        /// Use <see cref="IdentityUserManager.ConfirmEmailAsync"/> for regular email confirmation.
        /// Using this skips the confirmation process and directly sets the <see cref="EmailConfirmed"/>.
        /// </summary>
        public virtual void SetEmailConfirmed(bool confirmed)
        {
            EmailConfirmed = confirmed;
        }

        public virtual void SetPhoneNumberConfirmed(bool confirmed)
        {
            PhoneNumberConfirmed = confirmed;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, UserName = {UserName}";
        }

        /// <summary>
        /// Normally use <see cref="IdentityUserManager.ChangePhoneNumberAsync"/> to change the phone number
        /// in the application code.
        /// This method is to directly set it with a confirmation information.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="confirmed"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetPhoneNumber(string phoneNumber, bool confirmed)
        {
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = !phoneNumber.IsNullOrWhiteSpace() && confirmed;
        }
    }
}
