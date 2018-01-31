using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientGrantType : Entity
    {
        public virtual Guid ClientId { get; protected set; }

        public virtual string GrantType { get; protected set; }

        protected ClientGrantType()
        {

        }

        protected internal ClientGrantType(Guid clientId, string grantType)
        {
            ClientId = clientId;
            GrantType = grantType;
        }
    }
}