using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WidgetAttribute : Attribute
    {
        [CanBeNull]
        public string[] StyleFiles { get; set; }

        [CanBeNull]
        public Type[] StyleTypes { get; set; }

        [CanBeNull]
        public string[] ScriptFiles { get; set; }

        [CanBeNull]
        public Type[] ScriptTypes { get; set; }

        [CanBeNull]
        public string DisplayName { get; set; }

        [CanBeNull]
        public Type DisplayNameResource { get; set; }

        [CanBeNull]
        public string[] RequiredPolicies { get; set; }

        public bool RequiresAuthentication { get; set; }

        public string RefreshUrl { get; set; }

        public static bool IsWidget(Type type)
        {
            return type.IsSubclassOf(typeof(ViewComponent)) &&
                   type.IsDefined(typeof(WidgetAttribute), true);
        }

        public static WidgetAttribute Get(Type viewComponentType)
        {
            return viewComponentType.GetCustomAttribute<WidgetAttribute>(true)
                   ?? throw new AbpException($"Given type '{viewComponentType.AssemblyQualifiedName}' does not declare a {typeof(WidgetAttribute).AssemblyQualifiedName}");
        }
    }
}
