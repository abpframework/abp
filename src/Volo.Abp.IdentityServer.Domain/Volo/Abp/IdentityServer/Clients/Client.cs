using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    //TODO: Move property initializations to ctor.

    public class Client : AggregateRoot
    {
        public virtual string ClientId { get; set; }

        public virtual string ClientName { get; set; }

        public virtual string Description { get; set; }

        public virtual string ClientUri { get; set; }

        public virtual string LogoUri { get; set; }

        public virtual bool Enabled { get; set; } = true;

        public virtual string ProtocolType { get; set; } = IdentityServerConstants.ProtocolTypes.OpenIdConnect;

        public virtual List<ClientSecret> ClientSecrets { get; set; }

        public virtual bool RequireClientSecret { get; set; } = true;

        public virtual bool RequireConsent { get; set; } = true;

        public virtual bool AllowRememberConsent { get; set; } = true;

        public virtual bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public virtual List<ClientGrantType> AllowedGrantTypes { get; set; }

        public virtual bool RequirePkce { get; set; }

        public virtual bool AllowPlainTextPkce { get; set; }

        public virtual bool AllowAccessTokensViaBrowser { get; set; }

        public virtual List<ClientRedirectUri> RedirectUris { get; set; }

        public virtual List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }

        public virtual string FrontChannelLogoutUri { get; set; }

        public virtual bool FrontChannelLogoutSessionRequired { get; set; } = true;

        public virtual string BackChannelLogoutUri { get; set; }

        public virtual bool BackChannelLogoutSessionRequired { get; set; } = true;

        public virtual bool AllowOfflineAccess { get; set; }

        public virtual List<ClientScope> AllowedScopes { get; set; }

        public virtual int IdentityTokenLifetime { get; set; } = 300;

        public virtual int AccessTokenLifetime { get; set; } = 3600;

        public virtual int AuthorizationCodeLifetime { get; set; } = 300;

        public virtual int? ConsentLifetime { get; set; } = null;

        public virtual int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        public virtual int SlidingRefreshTokenLifetime { get; set; } = 1296000;

        public virtual int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;

        public virtual bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        public virtual int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;

        public virtual int AccessTokenType { get; set; } = (int)0; // AccessTokenType.Jwt;

        public virtual bool EnableLocalLogin { get; set; } = true;

        public virtual List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }

        public virtual bool IncludeJwtId { get; set; }

        public virtual List<ClientClaim> Claims { get; set; }

        public virtual bool AlwaysSendClientClaims { get; set; }

        public virtual string ClientClaimsPrefix { get; set; } = "client_";

        public virtual string PairWiseSubjectSalt { get; set; }

        public virtual List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }

        public virtual List<ClientProperty> Properties { get; set; }

        protected Client()
        {
            
        }

        public Client(Guid id)
        {
            Id = id;
        }
    }
}