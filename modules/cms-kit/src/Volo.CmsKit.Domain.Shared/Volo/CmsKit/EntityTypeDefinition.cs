using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Volo.CmsKit;

public abstract class EntityTypeDefinition : IEquatable<EntityTypeDefinition>
{
    public EntityTypeDefinition([NotNull] string entityType)
    {
        EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
    }

    [NotNull]
    public string EntityType { get; protected set; }

    public bool Equals(EntityTypeDefinition other)
    {
        return EntityType == other?.EntityType;
    }
}
