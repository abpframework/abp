using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        var hooks = Options.Hooks.GetOrDefault(name)?.ToArray() ?? Array.Empty<LayoutHookInfo>();

        return View(
            "~/Volo/Abp/AspNetCore/Mvc/UI/Components/LayoutHook/Default.cshtml",
            new LayoutHookViewModel(hooks, layout)
        );
    }
}
