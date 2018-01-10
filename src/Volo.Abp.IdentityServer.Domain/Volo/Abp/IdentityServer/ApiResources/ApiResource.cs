using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResource : AggregateRoot
    {
        public virtual bool Enabled { get; set; }

        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

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
        }
    }
}
