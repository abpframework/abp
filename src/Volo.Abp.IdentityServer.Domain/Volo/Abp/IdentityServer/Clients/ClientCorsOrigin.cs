using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientCorsOrigin : Entity
    {
        public virtual Guid ClientId { get; protected set; }

        public virtual string Origin { get; protected set; }

        protected ClientCorsOrigin()
        {
            
        }

        protected internal ClientCorsOrigin(Guid clientId, [NotNull] string origin)
        {
            Check.NotNull(origin, nameof(origin));

            ClientId = clientId;
            Origin = origin;
        }
    }
}