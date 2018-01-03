using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientGrantType : Entity
    {
        public virtual string GrantType { get; set; }

        public virtual Guid ClientId { get; set; }

        protected ClientGrantType()
        {

        }

        public ClientGrantType(Guid id)
        {
            Id = id;
        }
    }
}