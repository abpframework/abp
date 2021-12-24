using JetBrains.Annotations;
using System;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

public class PageToolbarContributionContext
{
    [NotNull]
    public IServiceProvider ServiceProvider { get; }

    [NotNull]
    public PageToolbarItemList Items { get; }

    public PageToolbarContributionContext(
        [NotNull] IServiceProvider serviceProvider)
    {
        ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
        Items = new PageToolbarItemList();
    }
}
