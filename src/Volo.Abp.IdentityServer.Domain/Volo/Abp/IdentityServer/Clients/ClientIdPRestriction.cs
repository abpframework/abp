using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientIdPRestriction : Entity
    {
        public virtual string Provider { get; set; }

        public virtual Guid ClientId { get; set; }

        protected ClientIdPRestriction()
        {

        }

        public ClientIdPRestriction(Guid id)
        {
            Id = id;
        }
    }
}