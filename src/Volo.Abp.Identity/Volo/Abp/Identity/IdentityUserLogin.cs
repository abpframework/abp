using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    //TODO: Make constructors internal to ensure that it's called by only from IdentityUser?

    /// <summary>
    /// Represents a login and its associated provider for a user.
    /// </summary>
    public class IdentityUserLogin : Entity
    {
        public const int MaxLoginProviderLength = 64;
        public const int MaxProviderKeyLength = 256;
        public const int MaxProviderDisplayNameLength = 128;

        /// <summary>
        /// Gets or sets the of the primary key of the user associated with this login.
        /// </summary>
        public virtual Guid UserId { get; protected set; }

        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google)
        /// </summary>
        public virtual string LoginProvider { get; protected set; }

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        public virtual string ProviderKey { get; protected set; }

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
        /// </summary>
        public virtual string ProviderDisplayName { get; protected set; }

        protected IdentityUserLogin()
        {
            
        }

        public IdentityUserLogin(Guid userId, [NotNull] string loginProvider, [NotNull] string providerKey, string providerDisplayName)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));

            //TODO: Where to set Id?

            UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = providerDisplayName;
        }

        public IdentityUserLogin(Guid userId, UserLoginInfo login)
        {
            Check.NotNull(login, nameof(login));

            UserId = userId;
            LoginProvider = login.LoginProvider;
            ProviderKey = login.ProviderKey;
            ProviderDisplayName = login.ProviderDisplayName;
        }

        public UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }
    }
}