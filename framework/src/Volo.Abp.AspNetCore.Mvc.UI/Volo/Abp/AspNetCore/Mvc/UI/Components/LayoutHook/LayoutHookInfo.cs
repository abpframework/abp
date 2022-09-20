using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;

public class LayoutHookInfo
{
    /// <summary>
    /// ViewComponent type.
    /// </summary>
    public Type ComponentType { get; }

    /// <summary>
    /// Specifies the layout name to apply this hook.
    /// null indicates that this hook will be applied to all layouts.
    /// </summary>
    public string Layout { get; }

    public LayoutHookInfo(Type componentType, string layout = null)
    {
        ComponentType = componentType;
        Layout = layout;
    }
}
