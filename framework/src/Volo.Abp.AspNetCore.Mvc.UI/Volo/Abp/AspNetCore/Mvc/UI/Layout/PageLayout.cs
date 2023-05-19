using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout;

public class PageLayout : IPageLayout, IScopedDependency
{
    public ContentLayout Content { get; }

    public PageLayout()
    {
        Content = new ContentLayout();
    }
}
