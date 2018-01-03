using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientScope : Entity
    {
        public virtual string Scope { get; set; }

        public virtual Guid ClientId { get; set; }

        protected ClientScope()
        {

        }

        public ClientScope(Guid id)
        {
            Id = id;
        }
    }
}