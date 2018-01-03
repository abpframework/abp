using System;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientSecret : Secret
    {
        public virtual Guid ClientId { get; set; }

        protected ClientSecret()
        {

        }

        public ClientSecret(Guid id)
            : base(id)
        {
            Id = id;
        }
    }
}