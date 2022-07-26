using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Web.Contents;

[ViewComponent(Name = "ContentFragment")]
[Widget(
    AutoInitialize = true
)]
public class ContentFragmentViewComponent : AbpViewComponent
{
    public IContent ContentDto { get; set; }

    public virtual async Task<IViewComponentResult> InvokeAsync(IContent contentDto)
    {
        return View("~/Contents/ContentFragment.cshtml", new ContentFragmentViewComponent() { ContentDto = contentDto });
    }
}

