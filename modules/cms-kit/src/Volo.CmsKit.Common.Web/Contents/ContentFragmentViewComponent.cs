using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Web.Contents;

[Widget(
    AutoInitialize = true
)]
[ViewComponent(Name = "ContentFragment")]
public class ContentFragmentViewComponent : ViewComponent
{
    public PageDto PageDto { get; set; }

    public ContentFragmentViewComponent()
    {
        PageDto = new();
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(PageDto pageDto)
    {
        return View("~/Contents/ContentFragment.cshtml", new ContentFragmentViewComponent() { PageDto = pageDto });
    }
}

