using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Menus
{
    public class MenuItem : AuditedEntity<Guid>
    {
        /// <summary>
        /// Parent <see cref="Menu"/> Id.
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// Presents another <see cref="MenuItem"/> Id.
        /// If it's <see langword="null"/>, then it's a root menu item.
        /// </summary>
        public Guid? ParentId { get; set; }

        [NotNull]
        public string DisplayName { get; protected set; }

        public bool IsActive { get; set; }
        
        [NotNull]
        public string Url { get; protected set; }

        public string Icon { get; set; }

        public int Order { get; set; }

        public string Target { get; set; }

        public string ElementId { get; set; }

        public string CssClass { get; set; }

        public Guid? PageId { get; protected set; }


        public MenuItem(Guid id,
                        Guid menuId,
                        [NotNull] string displayName,
                        [NotNull] string url,
                        bool isActive = true,
                        [CanBeNull] Guid? parentId = null,
                        [CanBeNull] string icon = null,
                        int order = 0,
                        [CanBeNull] string target = null,
                        [CanBeNull] string elementId = null,
                        [CanBeNull] string cssClass = null) 
            :base(id)
        {
            MenuId = menuId;
            SetDisplayName(displayName);
            IsActive = isActive;
            ParentId = parentId;
            SetUrl(url);
            Icon = icon;
            Order = order;
            Target = target;
            ElementId = elementId;
            CssClass = cssClass;
        }

        public void SetDisplayName([NotNull] string displayName)
        {
            DisplayName = Check.NotNullOrEmpty(displayName, nameof(displayName), MenuItemConsts.MaxDisplayNameLength);
        }

        public void SetUrl([NotNull]string url)
        {
            Url = Check.NotNullOrEmpty(url, nameof(url), MenuItemConsts.MaxUrlLength);
        }

        internal void SetPageId(Guid pageId)
        {
            PageId = pageId;
        }
    }
}
