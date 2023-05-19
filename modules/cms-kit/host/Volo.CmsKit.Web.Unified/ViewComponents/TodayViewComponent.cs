using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.ViewComponents;

[Widget(
    AutoInitialize = true
)]

[ViewComponent(Name = "CmsToday")]
public class TodayViewComponent : AbpViewComponent
{
    public string Format { get; set; }

    public TodayViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string format)
    {
        return View("~/ViewComponents/Today.cshtml", new TodayViewComponent() { Format = format });
    }
}
