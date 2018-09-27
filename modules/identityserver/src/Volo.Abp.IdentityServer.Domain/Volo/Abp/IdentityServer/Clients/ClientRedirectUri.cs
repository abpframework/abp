using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientRedirectUri : Entity
    {
        public virtual Guid ClientId { get; protected set; }

        public virtual string RedirectUri { get; protected set; }

        protected ClientRedirectUri()
        {

        }

        protected internal ClientRedirectUri(Guid clientId, [NotNull] string redirectUri)
        {
            Check.NotNull(redirectUri, nameof(redirectUri));

            ClientId = clientId;
            RedirectUri = redirectUri;
        }

        public override object[] GetKeys()
        {
            return new object[] { ClientId, RedirectUri };
        }
    }
}