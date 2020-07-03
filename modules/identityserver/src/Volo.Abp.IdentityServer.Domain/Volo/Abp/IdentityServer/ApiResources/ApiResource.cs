using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.IdentityServer.ApiScopes
{
    public class ApiResource : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual string AllowedAccessTokenSigningAlgorithms { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; } = true;

        public virtual List<ApiResourceSecret> Secrets { get; protected set; }

        public virtual List<ApiResourceScope> Scopes { get; protected set; }

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

            Secrets = new List<ApiResourceSecret>();
            Scopes = new List<ApiResourceScope>();
            UserClaims = new List<ApiResourceClaim>();
            Properties = new Dictionary<string, string>();

            Scopes.Add(new ApiResourceScope(id, name));
        }

        public virtual void AddSecret(
            [NotNull] string value,
            DateTime? expiration = null,
            string type = IdentityServerConstants.SecretTypes.SharedSecret,
            string description = null)
        {
            Secrets.Add(new ApiResourceSecret(Id, value, expiration, type, description));
        }

        public virtual void RemoveSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            Secrets.RemoveAll(s => s.Value == value && s.Type == type);
        }

        public virtual ApiResourceSecret FindSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            return Secrets.FirstOrDefault(s => s.Type == type && s.Value == value);
        }

        public virtual ApiResourceScope AddScope([NotNull] string scope)
        {
            var apiResourceScope = new ApiResourceScope(Id, scope);
            Scopes.Add(apiResourceScope);
            return apiResourceScope;
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
            Scopes.Clear();
        }

        public virtual void RemoveScope(string scope)
        {
            Scopes.RemoveAll(r => r.Scope == scope);
        }

        public virtual ApiResourceScope FindScope(string scope)
        {
            return Scopes.FirstOrDefault(r => r.Scope == scope);
        }
    }
}
