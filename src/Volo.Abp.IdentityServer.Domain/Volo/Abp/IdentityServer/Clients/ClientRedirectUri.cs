using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientRedirectUri : Entity<Guid>
    {
        public virtual string RedirectUri { get; set; }

        public virtual Guid ClientId { get; set; }

        protected ClientRedirectUri()
        {

        }

        public ClientRedirectUri(Guid id)
        {
            Id = id;
        }
    }
}