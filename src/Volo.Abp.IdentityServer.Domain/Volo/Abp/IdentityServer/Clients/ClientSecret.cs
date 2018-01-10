using System;
using IdentityServer4;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientSecret : Secret
    {
        public virtual Guid ClientId { get; protected set; }

        protected ClientSecret()
        {

        }

        public ClientSecret(Guid id, Guid clientId, string value, DateTime? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string description = null)
            : base(id, value, expiration, type, description)
        {
            ClientId = clientId;
        }
    }
}