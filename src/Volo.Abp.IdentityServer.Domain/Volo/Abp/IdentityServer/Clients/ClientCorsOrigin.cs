using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientCorsOrigin : Entity
    {
        public virtual string Origin { get; set; }

        public virtual Guid ClientId { get; set; }

        protected ClientCorsOrigin()
        {
            
        }

        public ClientCorsOrigin(Guid id)
        {
            Id = id;
        }
    }
}