using System;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

[Serializable]
public class EntityEto : EtoBase
{
    public string EntityType { get; set; } = default!;

    public string KeysAsString { get; set; } = default!;

    public EntityEto()
    {

    }

    public EntityEto(string entityType, string keysAsString)
    {
        EntityType = entityType;
        KeysAsString = keysAsString;
    }
}

public abstract class EntityEto<TKey> : IEntityEto<TKey>
{
    public TKey Id { get; set; } = default!;
}