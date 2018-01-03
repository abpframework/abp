using System;
using IdentityServer4;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer
{
    //TODO: Eleminate Secret class for simplicity.

    public abstract class Secret : Entity
    {
        public virtual string Description { get; set; }

        public virtual string Value { get; set; }

        public virtual DateTime? Expiration { get; set; }

        public virtual string Type { get; set; } = IdentityServerConstants.SecretTypes.SharedSecret;

        protected Secret()
        {

        }

        protected Secret(Guid id)
        {
            Id = id;
        }
    }
}