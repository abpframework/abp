using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientPostLogoutRedirectUri : Entity
    {
        public virtual string PostLogoutRedirectUri { get; set; }

        public virtual Guid ClientId { get; set; }
        
        protected ClientPostLogoutRedirectUri()
        {

        }

        public ClientPostLogoutRedirectUri(Guid id)
        {
            Id = id;
        }
    }
}