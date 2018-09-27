using System;
using System.Collections.Generic;
using IdentityServer4;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResource : AggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual List<ApiSecret> Secrets { get; protected set; }

        public virtual List<ApiScope> Scopes { get; protected set; }

        public virtual List<ApiResourceClaim> UserClaims { get; protected set; }

        protected ApiResource()
        {

        }

        public ApiResource(Guid id, [NotNull] string name, string displayName = null, string description = null)
        {
            Check.NotNull(name, nameof(name));

            Id = id;

            Name = name;

            DisplayName = displayName;
            Description = description;

            Enabled = true;

            Secrets = new List<ApiSecret>();
            Scopes = new List<ApiScope>();
            UserClaims = new List<ApiResourceClaim>();

            Scopes.Add(new ApiScope(id, name, displayName, description));
        }

        public virtual void AddSecret(
            [NotNull] string value, 
            DateTime? expiration = null,
            string type = IdentityServerConstants.SecretTypes.SharedSecret,
            string description = null)
        {
            Secrets.Add(new ApiSecret(Id, value, expiration, type, description));
        }

        public virtual void AddScope(
            [NotNull] string name,
            string displayName = null,
            string description = null,
            bool required = false,
            bool emphasize = false,
            bool showInDiscoveryDocument = true)
        {
            Scopes.Add(new ApiScope(Id, name, displayName, description, required, emphasize, showInDiscoveryDocument));
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new ApiResourceClaim(Id, type));
        }
    }
}
