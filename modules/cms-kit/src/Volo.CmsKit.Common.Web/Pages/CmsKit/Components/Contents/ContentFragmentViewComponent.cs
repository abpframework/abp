using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Web.Pages.CmsKit.Components.Contents;

[ViewComponent(Name = "ContentFragment")]
[Widget(
    AutoInitialize = true
)]
public class ContentFragmentViewComponent : AbpViewComponent
{
    public IContent ContentDto { get; set; }

    public virtual IViewComponentResult Invoke(IContent contentDto)
    {
        return View("~/Pages/CmsKit/Components/Contents/ContentFragment.cshtml", new ContentFragmentViewComponent() { ContentDto = contentDto });
    }
}