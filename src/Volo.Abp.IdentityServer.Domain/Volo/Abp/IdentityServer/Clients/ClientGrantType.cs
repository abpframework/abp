using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientGrantType : Entity
    {
        public virtual string GrantType { get; protected set; }

        public virtual Guid ClientId { get; protected set; }

        protected ClientGrantType()
        {

        }

        public ClientGrantType(Guid id, Guid clientId, string grantType)
        {
            Id = id;
            ClientId = clientId;
            GrantType = grantType;
        }
    }
}