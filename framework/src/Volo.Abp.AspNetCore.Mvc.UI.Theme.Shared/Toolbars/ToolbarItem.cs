using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

public class ToolbarItem : IHasSimpleStateCheckers<ToolbarItem>
{
    public Type ComponentType
    {
        get => _componentType;
        set => _componentType = Check.NotNull(value, nameof(value));
    }
    private Type _componentType;

    public int Order { get; set; }

    [CanBeNull]
    [Obsolete("Use RequirePermissions extension method.")]
    public string RequiredPermissionName { get; set; }

    public List<ISimpleStateChecker<ToolbarItem>> StateCheckers { get; }

    public ToolbarItem([NotNull] Type componentType, int order = 0, string requiredPermissionName = null)
    {
        Order = order;
        ComponentType = Check.NotNull(componentType, nameof(componentType));
        RequiredPermissionName = requiredPermissionName;
        StateCheckers = new List<ISimpleStateChecker<ToolbarItem>>();
    }
}
