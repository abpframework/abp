using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResource : AggregateRoot
    {
        public virtual bool Enabled { get; set; } = true;

        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual List<ApiSecret> Secrets { get; set; }

        public virtual List<ApiScope> Scopes { get; set; }

        public virtual List<ApiResourceClaim> UserClaims { get; set; }

        protected ApiResource()
        {
            
        }

        public ApiResource(Guid id)
        {
            Id = id;
        }
    }
}
