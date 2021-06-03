using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Admin.Menus
{
    public class MenuItemCreateInput
    {
        public Guid MenuId { get; set; }

        public Guid? ParentMenuId { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Url { get; set; }

        public string Icon { get; set; }

        public int Order { get; set; }

        public string Target { get; set; }

        public string ElementId { get; set; }

        public string CssClass { get; set; }

        public string RequiredPermissionName { get; set; }

        public Guid? PageId { get; set; }

        public List<MenuItemCreateInput> MenuItems { get; set; }
    }
}
