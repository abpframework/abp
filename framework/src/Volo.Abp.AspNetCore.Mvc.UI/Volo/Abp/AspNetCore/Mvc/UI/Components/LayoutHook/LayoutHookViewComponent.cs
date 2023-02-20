using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Ui.LayoutHooks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;

public class LayoutHookViewComponent : AbpViewComponent
{
    protected AbpLayoutHookOptions Options { get; }

    public LayoutHookViewComponent(IOptions<AbpLayoutHookOptions> options)
    {
        Options = options.Value;
    }

    public virtual IViewComponentResult Invoke(string name, string layout)
    {
        var hooks = Options.Hooks.GetOrDefault(name)?.Where(IsViewComponent).ToArray()
                          ?? Array.Empty<LayoutHookInfo>();
        
        return View(
            "~/Volo/Abp/AspNetCore/Mvc/UI/Components/LayoutHook/Default.cshtml",
            new LayoutHookViewModel(hooks, layout)
        );
    }

    protected virtual bool IsViewComponent(LayoutHookInfo layoutHook)
    {
        return typeof(ViewComponent).IsAssignableFrom(layoutHook.ComponentType);
    }
}
