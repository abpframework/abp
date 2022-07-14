using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;

namespace Volo.CmsKit.ViewComponents;

[Widget(
    AutoInitialize = true
)]

[ViewComponent(Name = "DecisionCommentDate")]
public class DecisionCommentDateViewComponent : AbpViewComponent
{
    public DecisionCommentDateViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/ViewComponents/DecisionCommentDate.cshtml", new DecisionCommentDateViewModel());
    }
}

public class DecisionCommentDateViewModel
{
    [DisplayName("Show date in the component")]
    public bool IsShow { get; set; } = true;
}
