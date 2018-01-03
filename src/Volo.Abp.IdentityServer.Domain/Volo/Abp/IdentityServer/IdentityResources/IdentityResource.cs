using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityResource : AggregateRoot
    {
        public virtual bool Enabled { get; set; } = true;

        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Emphasize { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; } = true;

        public virtual List<IdentityClaim> UserClaims { get; set; } //TODO: Rename to Claims..?

        protected IdentityResource()
        {

        }

        public IdentityResource(Guid id)
        {
            Id = id;
        }
    }
}
