using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class WidgetResourceItem
    {
        [CanBeNull]
        public string Src { get; }

        [CanBeNull]
        public Type Type { get; }

        public WidgetResourceItem([NotNull] string src)
        {
            Src = Check.NotNullOrWhiteSpace(src, nameof(src));
        }

        public WidgetResourceItem([NotNull] Type type)
        {
            Type = Check.NotNull(type, nameof(type));
        }
    }
}