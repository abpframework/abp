using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientProperty : Entity<Guid>
    {
        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        public virtual Guid ClientId { get; set; }

        protected ClientProperty()
        {

        }

        public ClientProperty(Guid id)
        {
            Id = id;
        }
    }
}