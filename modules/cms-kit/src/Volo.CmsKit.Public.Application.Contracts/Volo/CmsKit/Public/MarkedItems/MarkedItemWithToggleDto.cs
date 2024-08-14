using System;

namespace Volo.CmsKit.Public.MarkedItems;

[Serializable]
public class MarkedItemWithToggleDto
{
    public MarkedItemDto MarkedItem { get; set; }

    public bool IsMarkedByCurrentUser { get; set; }
}