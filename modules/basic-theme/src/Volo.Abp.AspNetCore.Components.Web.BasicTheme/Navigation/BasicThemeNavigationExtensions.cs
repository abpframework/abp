using System;
using JetBrains.Annotations;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme.Navigation;

public static class BasicThemeNavigationExtensions
{
    public const string CustomDataComponentKey = "BasicTheme.CustomComponent";

    public static ApplicationMenuItem UseComponent(this ApplicationMenuItem applicationMenuItem, Type componentType)
    {
        return applicationMenuItem.WithCustomData(CustomDataComponentKey, componentType);
    }

    [CanBeNull]
    public static Type GetComponentTypeOrDefault(this ApplicationMenuItem applicationMenuItem)
    {
        if (applicationMenuItem.CustomData.TryGetValue(CustomDataComponentKey, out object componentType))
        {
            return componentType as Type;
        }

        return default;
    }
}