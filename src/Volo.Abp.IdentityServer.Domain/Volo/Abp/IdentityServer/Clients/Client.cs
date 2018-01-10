using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

namespace Volo.Abp.IdentityServer.Clients
{
    public class Client : AggregateRoot
    {
        public virtual string ClientId { get; set; }

        public virtual string ClientName { get; set; }

        public virtual string Description { get; set; }

        public virtual string ClientUri { get; set; }

        public virtual string LogoUri { get; set; }

        public virtual bool Enabled { get; set; } = true;

        public virtual string ProtocolType { get; set; }

        public virtual bool RequireClientSecret { get; set; }

        public virtual bool RequireConsent { get; set; } = true;

        public virtual bool AllowRememberConsent { get; set; }

        public virtual bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public virtual bool RequirePkce { get; set; }

        public virtual bool AllowPlainTextPkce { get; set; }

        public virtual bool AllowAccessTokensViaBrowser { get; set; }

        public virtual string FrontChannelLogoutUri { get; set; }

        public virtual bool FrontChannelLogoutSessionRequired { get; set; }

        public virtual string BackChannelLogoutUri { get; set; }

        public virtual bool BackChannelLogoutSessionRequired { get; set; }

        public virtual bool AllowOfflineAccess { get; set; }

        public virtual int IdentityTokenLifetime { get; set; }

        public virtual int AccessTokenLifetime { get; set; }

        public virtual int AuthorizationCodeLifetime { get; set; }

        public virtual int? ConsentLifetime { get; set; }

        public virtual int AbsoluteRefreshTokenLifetime { get; set; }

        public virtual int SlidingRefreshTokenLifetime { get; set; }

        public virtual int RefreshTokenUsage { get; set; }

        public virtual bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        public virtual int RefreshTokenExpiration { get; set; }

        public virtual int AccessTokenType { get; set; }

        public virtual bool EnableLocalLogin { get; set; }

        public virtual bool IncludeJwtId { get; set; }

        public virtual bool AlwaysSendClientClaims { get; set; }

        public virtual string ClientClaimsPrefix { get; set; }

        public virtual string PairWiseSubjectSalt { get; set; }

        public virtual List<ClientScope> AllowedScopes { get; set; }

        public virtual List<ClientSecret> ClientSecrets { get; set; }

        public virtual List<ClientGrantType> AllowedGrantTypes { get; set; }

        public virtual List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }

        public virtual List<ClientRedirectUri> RedirectUris { get; set; }

        public virtual List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }

        public virtual List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }

        public virtual List<ClientClaim> Claims { get; set; }

        public virtual List<ClientProperty> Properties { get; set; }

        protected Client()
        {

        }

        public Client(Guid id, string clientId)
        {
            Id = id;
            ClientId = clientId;

            //TODO: Replace magics with constants?

            ProtocolType = IdentityServerConstants.ProtocolTypes.OpenIdConnect;
            RequireClientSecret = true;
            RequireConsent = true;
            AllowRememberConsent = true;
            FrontChannelLogoutSessionRequired = true;
            BackChannelLogoutSessionRequired = true;
            IdentityTokenLifetime = 300;
            AccessTokenLifetime = 3600;
            AuthorizationCodeLifetime = 300;
            AbsoluteRefreshTokenLifetime = 2592000;
            SlidingRefreshTokenLifetime = 1296000;
            RefreshTokenUsage = (int)TokenUsage.OneTimeOnly;
            RefreshTokenExpiration = (int)TokenExpiration.Absolute;
            AccessTokenType = (int)IdentityServer4.Models.AccessTokenType.Jwt;
            EnableLocalLogin = true;
            ClientClaimsPrefix = "client_";

            AllowedScopes = new List<ClientScope>();
            ClientSecrets = new List<ClientSecret>();
            AllowedGrantTypes = new List<ClientGrantType>();
            AllowedCorsOrigins = new List<ClientCorsOrigin>();
            RedirectUris = new List<ClientRedirectUri>();
            PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>();
            IdentityProviderRestrictions = new List<ClientIdPRestriction>();
            Claims = new List<ClientClaim>();
            Properties = new List<ClientProperty>();
        }

        public virtual void AddGrantType(IGuidGenerator guidGenerator, string grantType)
        {
            AllowedGrantTypes.Add(
                new ClientGrantType(guidGenerator.Create(), Id, grantType)
            );
        }

        public virtual void AddGrantTypes(IGuidGenerator guidGenerator, IEnumerable<string> grantTypes)
        {
            AllowedGrantTypes.AddRange(
                grantTypes.Select(
                    grantType => new ClientGrantType(guidGenerator.Create(), Id, grantType)
                )
            );
        }

        public virtual void AddSecret(IGuidGenerator guidGenerator, string value, DateTime? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string description = null)
        {
            ClientSecrets.Add(
                new ClientSecret(guidGenerator.Create(), Id, value, expiration, type, description)
            );
        }

        public virtual void AddAllowedScope(IGuidGenerator guidGenerator, string scope)
        {
            AllowedScopes.Add(new ClientScope(guidGenerator.Create(), Id, scope));
        }
    }
}