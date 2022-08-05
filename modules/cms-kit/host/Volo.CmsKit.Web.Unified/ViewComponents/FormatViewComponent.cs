using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.ViewComponents;

[Widget(
    AutoInitialize = true
)]

[ViewComponent(Name = "Format")]
public class FormatViewComponent : AbpViewComponent
{
    public FormatViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/ViewComponents/Format.cshtml", new FormatViewModel());
    }
}

public class FormatViewModel
{
    [DisplayName("Format your date in the component")]
    public string Format { get; set; }
}
