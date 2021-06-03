using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Menus
{
    public class MenuItemDto : AuditedEntityDto<Guid>
    {
        public Guid MenuId { get; set; }
        public Guid? ParentMenuId { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public string Target { get; set; }
        public string ElementId { get; set; }
        public string CssClass { get; set; }
        public string RequiredPermissionName { get; set; }

        public List<MenuItemDto> SubMenuItems { get; set; }
    }
}
