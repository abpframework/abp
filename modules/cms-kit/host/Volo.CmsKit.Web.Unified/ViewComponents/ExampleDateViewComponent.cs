using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.ViewComponents;

[Widget(
    AutoInitialize = true
)]

[ViewComponent(Name = "ExampleDate")]
public class ExampleDateViewComponent : AbpViewComponent
{
    public ExampleDateViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string isShow)
    {
        return View("~/ViewComponents/ExampleDate.cshtml", new ExampleDateViewModel() { IsShow = bool.Parse(isShow) });
    }
}

public class ExampleDateViewModel
{
    public bool IsShow { get; set; }
}