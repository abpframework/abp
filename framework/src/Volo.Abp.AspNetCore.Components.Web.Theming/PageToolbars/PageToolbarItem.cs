using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars
{
    public class PageToolbarItem
    {
        [NotNull]
        public Type ComponentType { get; }

        [CanBeNull]
        public Dictionary<string, object> Arguments { get; set; }

        public int Order { get; set; }

        public PageToolbarItem(
            [NotNull] Type componentType,
            [CanBeNull] Dictionary<string, object> arguments = null,
            int order = 0)
        {
            ComponentType = Check.NotNull(componentType, nameof(componentType));
            Arguments = arguments;
            Order = order;
        }
    }
}
