using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientIdPRestriction : Entity
    {
        public virtual Guid ClientId { get; set; }

        public virtual string Provider { get; set; }

        protected ClientIdPRestriction()
        {

        }

        protected internal ClientIdPRestriction(Guid clientId, [NotNull] string provider)
        {
            Check.NotNull(provider, nameof(provider));

            ClientId = clientId;
            Provider = provider;
        }
    }
}