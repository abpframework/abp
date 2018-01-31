using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientClaim : Entity<Guid>
    {
        public virtual Guid ClientId { get; set; }

        public virtual string Type { get; set; }

        public virtual string Value { get; set; }

        protected ClientClaim()
        {
            
        }

        protected internal ClientClaim(Guid id, Guid clientId, [NotNull] string type, string value)
        {
            Check.NotNull(type, nameof(type));

            Id = id;
            ClientId = clientId;
            Type = type;
            Value = value;
        }
    }
}