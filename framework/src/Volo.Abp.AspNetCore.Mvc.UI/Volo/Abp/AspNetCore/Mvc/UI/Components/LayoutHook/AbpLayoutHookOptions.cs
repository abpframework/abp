using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;

public class AbpLayoutHookOptions
{
    public IDictionary<string, List<LayoutHookInfo>> Hooks { get; }

    public AbpLayoutHookOptions()
    {
        Hooks = new Dictionary<string, List<LayoutHookInfo>>();
    }

    public AbpLayoutHookOptions Add(string name, Type componentType, string layout = null)
    {
        Hooks
            .GetOrAdd(name, () => new List<LayoutHookInfo>())
            .Add(new LayoutHookInfo(componentType, layout));

        return this;
    }
}
