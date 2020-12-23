using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.IdentityServer.ApiScopes
{
    public class ApiScope :  FullAuditedAggregateRoot<Guid>
    {
        public virtual bool Enabled { get; set; }

        [NotNull]
        public virtual string Name { get; protected set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Emphasize { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; }

        public virtual List<ApiScopeClaim> UserClaims { get; protected set; }

        public virtual List<ApiScopeProperty> Properties { get; protected set; }

        protected ApiScope()
        {

        }

        public ApiScope(
            Guid id,
            [NotNull] string name,
            string displayName = null,
            string description = null,
            bool required = false,
            bool emphasize = false,
            bool showInDiscoveryDocument = true,
            bool enabled = true)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            DisplayName = displayName ?? name;
            Description = description;
            Required = required;
            Emphasize = emphasize;
            ShowInDiscoveryDocument = showInDiscoveryDocument;
            Enabled = enabled;

            UserClaims = new List<ApiScopeClaim>();
            Properties = new List<ApiScopeProperty>();
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new ApiScopeClaim(Id, type));
        }

        public virtual void RemoveAllUserClaims()
        {
            UserClaims.Clear();
        }

        public virtual void RemoveClaim(string type)
        {
            UserClaims.RemoveAll(r => r.Type == type);
        }

        public virtual ApiScopeClaim FindClaim(string type)
        {
            return UserClaims.FirstOrDefault(r => r.Type == type);
        }

        public virtual void AddProperty([NotNull] string key, string value)
        {
            Properties.Add(new ApiScopeProperty(Id, key, value));
        }

        public virtual void RemoveAllProperties()
        {
            Properties.Clear();
        }

        public virtual void RemoveProperty(string key)
        {
            Properties.RemoveAll(r => r.Key == key);
        }

        public virtual ApiScopeProperty FindProperty(string key)
        {
            return Properties.FirstOrDefault(r => r.Key == key);
        }
    }
}
