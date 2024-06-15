using System;
using JetBrains.Annotations;

namespace Volo.CmsKit.Public.MarkedItems;

[Serializable]
public class MarkedItemDto
{
    [NotNull]
    public string IconName { get; set; }
}