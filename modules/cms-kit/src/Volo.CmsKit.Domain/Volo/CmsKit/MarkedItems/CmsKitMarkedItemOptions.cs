using JetBrains.Annotations;
using System.Collections.Generic;

namespace Volo.CmsKit.MarkedItems;

public class CmsKitMarkedItemOptions
{
    [NotNull]
    public List<MarkedItemEntityTypeDefinition> EntityTypes { get; } = new();
}