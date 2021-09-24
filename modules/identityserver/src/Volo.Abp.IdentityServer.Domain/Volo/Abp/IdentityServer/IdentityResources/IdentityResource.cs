using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityResource : FullAuditedAggregateRoot<Guid>
    {
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Emphasize { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; }

        public virtual List<IdentityResourceClaim> UserClaims { get; set; }

        public virtual List<IdentityResourceProperty> Properties { get; set; }

        protected IdentityResource()
        {

        }

        public IdentityResource(
            Guid id,
            [NotNull] string name,
            string displayName = null,
            string description = null,
            bool enabled = true,
            bool required = false,
            bool emphasize = false,
            bool showInDiscoveryDocument = true
        ) : base(id)
        {
            Check.NotNull(name, nameof(name));

            Name = name;
            DisplayName = displayName;
            Description = description;
            Enabled = enabled;
            Required = required;
            Emphasize = emphasize;
            ShowInDiscoveryDocument = showInDiscoveryDocument;

            UserClaims = new List<IdentityResourceClaim>();
            Properties = new List<IdentityResourceProperty>();
        }

        public IdentityResource(Guid id, IdentityServer4.Models.IdentityResource resource) : base(id)
        {
            Name = resource.Name;
            DisplayName = resource.DisplayName;
            Description = resource.Description;
            Enabled = resource.Enabled;
            Required = resource.Required;
            Emphasize = resource.Emphasize;
            ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument;
            UserClaims = resource.UserClaims.Select(claimType => new IdentityResourceClaim(id, claimType)).ToList();
            Properties = resource.Properties.Select(x => new IdentityResourceProperty(Id, x.Key, x.Value)).ToList();
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new IdentityResourceClaim(Id, type));
        }

        public virtual void RemoveAllUserClaims()
        {
            UserClaims.Clear();
        }

        public virtual void RemoveUserClaim(string type)
        {
            UserClaims.RemoveAll(c => c.Type == type);
        }

        public virtual IdentityResourceClaim FindUserClaim(string type)
        {
            return UserClaims.FirstOrDefault(c => c.Type == type);
        }

        public virtual void AddProperty([NotNull] string key, string value)
        {
            Properties.Add(new IdentityResourceProperty(Id, key, value));
        }

        public virtual void RemoveAllProperties()
        {
            Properties.Clear();
        }

        public virtual void RemoveProperty(string key)
        {
            Properties.RemoveAll(r => r.Key == key);
        }

        public virtual IdentityResourceProperty FindProperty(string key)
        {
            return Properties.FirstOrDefault(r => r.Key == key);
        }
    }
}
