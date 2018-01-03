using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientClaim : Entity
    {
        public virtual string Type { get; set; }

        public virtual string Value { get; set; }

        public virtual Guid ClientId { get; set; }

        protected ClientClaim()
        {
            
        }

        public ClientClaim(Guid id)
        {
            Id = id;
        }
    }
}