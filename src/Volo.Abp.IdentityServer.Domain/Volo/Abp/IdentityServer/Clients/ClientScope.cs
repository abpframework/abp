using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientScope : Entity
    {
        public virtual Guid ClientId { get; protected set; }

        public virtual string Scope { get; protected set; }

        protected ClientScope()
        {

        }

        protected internal ClientScope(Guid clientId, string scope)
        {
            ClientId = clientId;
            Scope = scope;
        }
    }
}