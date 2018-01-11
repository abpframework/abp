using System;
using Volo.Abp.Domain.Values;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientGrantType : ValueObject<ClientGrantType>
    {
        public virtual string GrantType { get; protected set; }

        public virtual Guid ClientId { get; protected set; }

        protected ClientGrantType()
        {

        }

        public ClientGrantType(Guid clientId, string grantType)
        {
            ClientId = clientId;
            GrantType = grantType;
        }
    }
}