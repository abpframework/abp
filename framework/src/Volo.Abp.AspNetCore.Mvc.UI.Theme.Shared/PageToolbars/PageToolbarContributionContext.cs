using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars;

public class PageToolbarContributionContext
{
    [NotNull]
    public string PageName { get; }

    [NotNull]
    public IServiceProvider ServiceProvider { get; }

    [NotNull]
    public PageToolbarItemList Items { get; }

    public PageToolbarContributionContext(
        [NotNull] string pageName,
        [NotNull] IServiceProvider serviceProvider)
    {
        PageName = Check.NotNull(pageName, nameof(pageName));
        ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));

        Items = new PageToolbarItemList();
    }
}
