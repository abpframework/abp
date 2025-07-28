using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout;

public class PageLayout : IPageLayout, IScopedDependency
{
    public ContentLayout Content { get; } = new();

    public virtual bool RenderLayoutElements { get; set; } = true;
}
