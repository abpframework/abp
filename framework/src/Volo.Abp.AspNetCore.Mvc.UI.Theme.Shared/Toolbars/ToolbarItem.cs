using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarItem
    {
        public Type ComponentType
        {
            get => _componentType;
            set => _componentType = Check.NotNull(value, nameof(value));
        }
        private Type _componentType;

        public int Order { get; set; }

        [CanBeNull]
        public string RequiredPermissionName { get; set; }

        public ToolbarItem([NotNull] Type componentType, int order = 0, string requiredPermissionName = null)
        {
            Order = order;
            ComponentType = Check.NotNull(componentType, nameof(componentType));
            RequiredPermissionName = requiredPermissionName;
        }
    }
}
