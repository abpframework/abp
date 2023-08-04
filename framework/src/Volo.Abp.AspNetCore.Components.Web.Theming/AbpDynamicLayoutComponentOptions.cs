using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Components.Web.Theming;

public class AbpDynamicLayoutComponentOptions
{
    /// <summary>
    /// Used to define components that renders in the layout
    /// </summary>
    [NotNull]
    public Dictionary<Type, IDictionary<string,object>?> Components { get; set; }

    public AbpDynamicLayoutComponentOptions()
    {
        Components = new Dictionary<Type, IDictionary<string, object>?>();
    }
}