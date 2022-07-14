using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.ViewComponents;

[Widget(
    AutoInitialize = true
)]

[ViewComponent(Name = "ExampleModalDate")]
public class ExampleModalDateViewComponent : AbpViewComponent
{
    public ExampleModalDateViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/ViewComponents/ExampleModalDate.cshtml", new ExampleModalDateViewModel());
    }
}

public class ExampleModalDateViewModel
{
    [DisplayName("Show date in the component")]
    public bool IsShow { get; set; } = true;
}
