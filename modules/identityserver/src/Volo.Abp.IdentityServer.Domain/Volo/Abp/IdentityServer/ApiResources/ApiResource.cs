using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResource : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual List<ApiSecret> Secrets { get; protected set; }

        public virtual List<ApiScope> Scopes { get; protected set; }

        public virtual List<ApiResourceClaim> UserClaims { get; protected set; }

        public virtual Dictionary<string, string> Properties { get; protected set; }

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
            Properties = new Dictionary<string, string>();

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

        public virtual void RemoveSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            Secrets.RemoveAll(s => s.Value == value && s.Type == type);
        }

        public virtual ApiSecret FindSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            return Secrets.FirstOrDefault(s => s.Type == type && s.Value == value);
        }

        public virtual ApiScope AddScope(
            [NotNull] string name,
            string displayName = null,
            string description = null,
            bool required = false,
            bool emphasize = false,
            bool showInDiscoveryDocument = true)
        {
            var scope = new ApiScope(Id, name, displayName, description, required, emphasize, showInDiscoveryDocument);
            Scopes.Add(scope);
            return scope;
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new ApiResourceClaim(Id, type));
        }

        public virtual void RemoveAllUserClaims()
        {
            UserClaims.Clear();
        }

        public virtual void RemoveClaim(string type)
        {
            UserClaims.RemoveAll(c => c.Type == type);
        }

        public virtual ApiResourceClaim FindClaim(string type)
        {
            return UserClaims.FirstOrDefault(c => c.Type == type);
        }

        public virtual void RemoveAllSecrets()
        {
            Secrets.Clear();
        }

        public virtual void RemoveAllScopes()
        {
            foreach (var scope in Scopes)
            {
                scope.RemoveAllUserClaims();
            }
            Scopes.Clear();
        }

        public virtual void RemoveScope(string name)
        {
            Scopes.RemoveAll(r => r.Name == name);
        }

        public virtual ApiScope FindScope(string name)
        {
            return Scopes.FirstOrDefault(r => r.Name == name);
        }
    }
}
