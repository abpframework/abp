using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.Clients;

public class ClientProperty : Entity
{
    public virtual Guid ClientId { get; set; }

    public virtual string Key { get; set; }

    public virtual string Value { get; set; }

    protected ClientProperty()
    {

    }

    public virtual bool Equals(Guid clientId, [NotNull] string key, [NotNull] string value)
    {
        return ClientId == clientId && Key == key && Value == value;
    }

    protected internal ClientProperty(Guid clientId, [NotNull] string key, [NotNull] string value)
    {
        Check.NotNull(key, nameof(key));

        ClientId = clientId;
        Key = key;
        Value = value;
    }

    public override object[] GetKeys()
    {
        return new object[] { ClientId, Key };
    }
}
