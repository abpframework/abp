using System;
using IdentityServer4;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer
{
    //TODO: Eleminate Secret class for simplicity.

    public abstract class Secret : Entity
    {
        public virtual string Description { get; protected set; }

        public virtual string Value { get; protected set; }

        public virtual DateTime? Expiration { get; protected set; }

        public virtual string Type { get; protected set; }

        protected Secret()
        {

        }

        protected Secret(Guid id, string value, DateTime? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string description = null)
        {
            Id = id;
            Value = value;
            Expiration = expiration;
            Type = type;
            Description = description;
        }
    }
}