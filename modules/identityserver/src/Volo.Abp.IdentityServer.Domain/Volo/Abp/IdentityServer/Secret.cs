using System;
using IdentityServer4;
using IdentityServer4.Models;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer
{
    public abstract class Secret : Entity
    {
        public virtual string Type { get; protected set; }

        public virtual string Value { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime? Expiration { get; set; }

        protected Secret()
        {

        }

        protected Secret(
            [NotNull] string value, 
            DateTime? expiration = null, 
            string type = IdentityServerConstants.SecretTypes.SharedSecret, 
            string description = null)
        {
            Check.NotNull(value, nameof(value));

            Value = value;
            Expiration = expiration;
            Type = type;
            Description = description;
        }
    }
}