using System;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Admin.Menus;

[Serializable]
public class MenuItemWithDetailsDto : MenuItemDto
{
    public string? PageTitle { get; set; }
}
