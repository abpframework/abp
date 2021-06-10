using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Menus
{
    [Serializable]
    public class MenuItemUpdateInput
    {
        [Required]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public string Target { get; set; }

        public string ElementId { get; set; }

        public string CssClass { get; set; }

        public string RequiredPermissionName { get; set; }

        public Guid? PageId { get; set; }
    }
}