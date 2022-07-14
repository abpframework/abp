using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

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
        return View("~/ViewComponents/DecisionCommentDate.cshtml");
    }
}
