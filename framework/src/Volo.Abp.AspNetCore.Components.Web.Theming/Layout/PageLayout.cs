using System.Collections.Generic;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Layout;

public class PageLayout : IScopedDependency
{
    // TODO: Consider using this property for setting Page Title too.
    public virtual string Title { get; set; }

    public virtual List<BreadcrumbItem> BreadcrumbItems { get; set; } = new();

    public virtual List<PageToolbarItem> ToolbarItems { get; set; } = new();
}