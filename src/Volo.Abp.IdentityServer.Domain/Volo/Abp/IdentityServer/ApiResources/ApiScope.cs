using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiScope : Entity<Guid>
    {
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Emphasize { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; } = true;

        public virtual List<ApiScopeClaim> UserClaims { get; set; }

        public virtual Guid ApiResourceId { get; set; }

        protected ApiScope()
        {
            
        }

        public ApiScope(Guid id)
        {
            Id = id;
        }
    }
}