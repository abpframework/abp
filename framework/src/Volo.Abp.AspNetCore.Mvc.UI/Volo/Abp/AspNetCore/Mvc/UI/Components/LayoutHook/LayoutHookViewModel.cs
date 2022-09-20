namespace Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;

public class LayoutHookViewModel
{
    public LayoutHookInfo[] Hooks { get; }

    public string Layout { get; }

    public LayoutHookViewModel(LayoutHookInfo[] hooks, string layout)
    {
        Hooks = hooks;
        Layout = layout;
    }
}
