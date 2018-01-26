using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientScope : Entity<Guid>
    {
        public virtual string Scope { get; protected set; }

        public virtual Guid ClientId { get; protected set; }

        protected ClientScope()
        {

        }

        public ClientScope(Guid id, Guid clientId, string scope)
        {
            Id = id;
            ClientId = clientId;
            Scope = scope;
        }
    }
}