using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients;

public class ClientPostLogoutRedirectUri : Entity
{
    public virtual Guid ClientId { get; protected set; }

    public virtual string PostLogoutRedirectUri { get; protected set; }

    protected ClientPostLogoutRedirectUri()
    {

    }

    public virtual bool Equals(Guid clientId, [NotNull] string uri)
    {
        return ClientId == clientId && PostLogoutRedirectUri == uri;
    }

    protected internal ClientPostLogoutRedirectUri(Guid clientId, [NotNull] string postLogoutRedirectUri)
    {
        Check.NotNull(postLogoutRedirectUri, nameof(postLogoutRedirectUri));

        ClientId = clientId;
        PostLogoutRedirectUri = postLogoutRedirectUri;
    }

    public override object[] GetKeys()
    {
        return new object[] { ClientId, PostLogoutRedirectUri };
    }
}
