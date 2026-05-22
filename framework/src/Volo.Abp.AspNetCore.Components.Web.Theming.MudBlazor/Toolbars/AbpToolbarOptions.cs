using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Toolbars;

public class AbpToolbarOptions
{
    [NotNull]
    public List<IToolbarContributor> Contributors { get; }

    public AbpToolbarOptions()
    {
        Contributors = new List<IToolbarContributor>();
    }
}
