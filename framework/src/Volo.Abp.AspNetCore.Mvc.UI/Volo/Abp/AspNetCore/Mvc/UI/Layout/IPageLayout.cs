namespace Volo.Abp.AspNetCore.Mvc.UI.Layout;

public interface IPageLayout
{
    ContentLayout Content { get; }

    /// <summary>
    /// Whether the application layout (navigation menu, toolbar, etc.) should be rendered around the page content.
    /// </summary>
    bool RenderLayoutElements { get; set; }
}
