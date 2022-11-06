using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.ContentTitle;

public class ContentTitleViewComponent : AbpViewComponent
{
    protected IPageLayout PageLayout { get; }

    public ContentTitleViewComponent(IPageLayout pageLayout)
    {
        PageLayout = pageLayout;
    }

    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/Basic/Components/ContentTitle/Default.cshtml", PageLayout.Content);
    }
}
