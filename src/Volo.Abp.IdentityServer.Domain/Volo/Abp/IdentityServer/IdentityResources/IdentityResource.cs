using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityResource : AggregateRoot<Guid>
    {
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Emphasize { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; }

        public virtual List<IdentityClaim> UserClaims { get; set; }

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
            bool showInDiscoveryDocument = true)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
            DisplayName = displayName;
            Description = description;
            Enabled = enabled;
            Required = required;
            Emphasize = emphasize;
            ShowInDiscoveryDocument = showInDiscoveryDocument;
            
            UserClaims = new List<IdentityClaim>();
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new IdentityClaim(Id, type));
        }
    }
}
