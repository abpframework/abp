using System.Collections.Generic;
using IdentityModel;

namespace Volo.Abp.Http.Client.IdentityModel
{
    public class IdentityClientConfiguration : Dictionary<string, string>
    {
        /// <summary>
        /// Possible values: "client_credentials" or "password".
        /// Default value: "client_credentials".
        /// </summary>
        public string GrantType
        {
            get => this.GetOrDefault(nameof(GrantType));
            set => this[GrantType] = value;
        }

        /// <summary>
        /// Client Id.
        /// </summary>
        public string ClientId
        {
            get => this.GetOrDefault(nameof(ClientId));
            set => this[ClientId] = value;
        }

        /// <summary>
        /// Client secret (as plain text - without hashed).
        /// </summary>
        public string ClientSecret
        {
            get => this.GetOrDefault(nameof(ClientSecret));
            set => this[ClientSecret] = value;
        }

        /// <summary>
        /// User name.
        /// Valid only if <see cref="GrantType"/> is "password".
        /// </summary>
        public string UserName
        {
            get => this.GetOrDefault(nameof(UserName));
            set => this[UserName] = value;
        }

        /// <summary>
        /// Password of the <see cref="UserName"/>.
        /// Valid only if <see cref="GrantType"/> is "password".
        /// </summary>
        public string UserPassword
        {
            get => this.GetOrDefault(nameof(UserPassword));
            set => this[UserPassword] = value;
        }

        /// <summary>
        /// Authority.
        /// </summary>
        public string Authority
        {
            get => this.GetOrDefault(nameof(Authority));
            set => this[Authority] = value;
        }

        /// <summary>
        /// Scope.
        /// </summary>
        public string Scope
        {
            get => this.GetOrDefault(nameof(Scope));
            set => this[Scope] = value;
        }

        public IdentityClientConfiguration()
        {
            
        }

        public IdentityClientConfiguration(
            string clientId, 
            string clientSecret, 
            string grantType = OidcConstants.GrantTypes.ClientCredentials,
            string userName = null,
            string userPassword = null)
        {
            this[nameof(ClientId)] = clientId;
            this[nameof(ClientSecret)] = clientSecret;
            this[nameof(GrantType)] = grantType;
            this[nameof(UserName)] = userName;
            this[nameof(UserPassword)] = userPassword;
        }
    }
}