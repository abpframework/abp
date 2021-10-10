using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.IdentityServer.Clients
{
    public class Client : FullAuditedAggregateRoot<Guid>
    {
        public virtual string ClientId { get; set; }

        public virtual string ClientName { get; set; }

        public virtual string Description { get; set; }

        public virtual string ClientUri { get; set; }

        public virtual string LogoUri { get; set; }

        public virtual bool Enabled { get; set; } = true;

        public virtual string ProtocolType { get; set; }

        public virtual bool RequireClientSecret { get; set; }

        public virtual bool RequireConsent { get; set; }

        public virtual bool AllowRememberConsent { get; set; }

        public virtual bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public virtual bool RequirePkce { get; set; }

        public virtual bool AllowPlainTextPkce { get; set; }

        public virtual bool RequireRequestObject { get; set; }

        public virtual bool AllowAccessTokensViaBrowser { get; set; }

        public virtual string FrontChannelLogoutUri { get; set; }

        public virtual bool FrontChannelLogoutSessionRequired { get; set; }

        public virtual string BackChannelLogoutUri { get; set; }

        public virtual bool BackChannelLogoutSessionRequired { get; set; }

        public virtual bool AllowOfflineAccess { get; set; }

        public virtual int IdentityTokenLifetime { get; set; }

        public virtual string AllowedIdentityTokenSigningAlgorithms { get; set; }

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

        public virtual int? UserSsoLifetime { get; set; }

        public virtual string UserCodeType { get; set; }

        public virtual int DeviceCodeLifetime { get; set; } = 300;

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

        public Client(Guid id, [NotNull] string clientId)
        : base(id)
        {
            Check.NotNull(clientId, nameof(clientId));

            ClientId = clientId;

            //TODO: Replace magics with constants?

            ProtocolType = IdentityServerConstants.ProtocolTypes.OpenIdConnect;
            RequireClientSecret = true;
            RequireConsent = false;
            AllowRememberConsent = true;
            RequirePkce = true;
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

        public virtual void AddGrantType([NotNull] string grantType)
        {
            AllowedGrantTypes.Add(new ClientGrantType(Id, grantType));
        }

        public virtual void RemoveAllAllowedGrantTypes()
        {
            AllowedGrantTypes.Clear();
        }

        public virtual void RemoveGrantType(string grantType)
        {
            AllowedGrantTypes.RemoveAll(r => r.GrantType == grantType);
        }

        public virtual ClientGrantType FindGrantType(string grantType)
        {
            return AllowedGrantTypes.FirstOrDefault(r => r.GrantType == grantType);
        }

        public virtual void AddSecret([NotNull] string value, DateTime? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string description = null)
        {
            ClientSecrets.Add(new ClientSecret(Id, value, expiration, type, description));
        }

        public virtual void RemoveSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            ClientSecrets.RemoveAll(s => s.Value == value && s.Type == type);
        }

        public virtual ClientSecret FindSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            return ClientSecrets.FirstOrDefault(s => s.Type == type && s.Value == value);
        }

        public virtual void AddScope([NotNull] string scope)
        {
            AllowedScopes.Add(new ClientScope(Id, scope));
        }

        public virtual void RemoveAllScopes()
        {
            AllowedScopes.Clear();
        }

        public virtual void RemoveScope(string scope)
        {
            AllowedScopes.RemoveAll(r => r.Scope == scope);
        }

        public virtual ClientScope FindScope(string scope)
        {
            return AllowedScopes.FirstOrDefault(r => r.Scope == scope);
        }

        public virtual void AddCorsOrigin([NotNull] string origin)
        {
            AllowedCorsOrigins.Add(new ClientCorsOrigin(Id, origin));
        }

        public virtual void AddRedirectUri([NotNull] string redirectUri)
        {
            RedirectUris.Add(new ClientRedirectUri(Id, redirectUri));
        }

        public virtual void AddPostLogoutRedirectUri([NotNull] string postLogoutRedirectUri)
        {
            PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri(Id, postLogoutRedirectUri));
        }

        public virtual void RemoveAllCorsOrigins()
        {
            AllowedCorsOrigins.Clear();
        }

        public virtual void RemoveCorsOrigin(string uri)
        {
            AllowedCorsOrigins.RemoveAll(c => c.Origin == uri);
        }

        public virtual void RemoveAllRedirectUris()
        {
            RedirectUris.Clear();
        }

        public virtual void RemoveRedirectUri(string uri)
        {
            RedirectUris.RemoveAll(r => r.RedirectUri == uri);
        }

        public virtual void RemoveAllPostLogoutRedirectUris()
        {
            PostLogoutRedirectUris.Clear();
        }

        public virtual void RemovePostLogoutRedirectUri(string uri)
        {
            PostLogoutRedirectUris.RemoveAll(p => p.PostLogoutRedirectUri == uri);
        }

        public virtual ClientCorsOrigin FindCorsOrigin(string uri)
        {
            return AllowedCorsOrigins.FirstOrDefault(c => c.Origin == uri);
        }

        public virtual ClientRedirectUri FindRedirectUri(string uri)
        {
            return RedirectUris.FirstOrDefault(r => r.RedirectUri == uri);
        }

        public virtual ClientPostLogoutRedirectUri FindPostLogoutRedirectUri(string uri)
        {
            return PostLogoutRedirectUris.FirstOrDefault(p => p.PostLogoutRedirectUri == uri);
        }

        public virtual void AddProperty([NotNull] string key, [NotNull] string value)
        {
            var property = FindProperty(key);
            if (property == null)
            {
                Properties.Add(new ClientProperty(Id, key, value));
            }
            else
            {
                property.Value = value;
            }
        }

        public virtual void RemoveAllProperties()
        {
            Properties.Clear();
        }

        public virtual void RemoveProperty(string key)
        {
            Properties.RemoveAll(c => c.Key == key);
        }

        public virtual ClientProperty FindProperty(string key)
        {
            return Properties.FirstOrDefault(c => c.Key == key);
        }

        public virtual void AddClaim([NotNull] string type, [NotNull] string value)
        {
            Claims.Add(new ClientClaim(Id, type, value));
        }

        public virtual void RemoveAllClaims()
        {
            Claims.Clear();
        }

        public virtual void RemoveClaim(string type)
        {
            Claims.RemoveAll(c => c.Type == type);
        }

        public virtual void RemoveClaim(string type, string value)
        {
            Claims.RemoveAll(c => c.Type == type && c.Value == value);
        }

        public virtual List<ClientClaim> FindClaims(string type)
        {
            return Claims.Where(c => c.Type == type).ToList();
        }

        public virtual ClientClaim FindClaim(string type, string value)
        {
            return Claims.FirstOrDefault(c => c.Type == type && c.Value == value);
        }

        public virtual void AddIdentityProviderRestriction([NotNull] string provider)
        {
            IdentityProviderRestrictions.Add(new ClientIdPRestriction(Id, provider));
        }

        public virtual void RemoveAllIdentityProviderRestrictions()
        {
            IdentityProviderRestrictions.Clear();
        }

        public virtual void RemoveIdentityProviderRestriction(string provider)
        {
            IdentityProviderRestrictions.RemoveAll(r => r.Provider == provider);
        }

        public virtual ClientIdPRestriction FindIdentityProviderRestriction(string provider)
        {
            return IdentityProviderRestrictions.FirstOrDefault(r => r.Provider == provider);
        }
    }
}
