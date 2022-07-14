using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.ViewComponents;

[Widget(
    AutoInitialize = true
)]

[ViewComponent(Name = "CommentDate")]
public class CommentDateViewComponent : AbpViewComponent
{
    public bool IsShow { get; set; }

    public CommentDateViewComponent()
    {
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string isShow)
    {
        return View("~/ViewComponents/CommentDate.cshtml", new CommentDateViewComponent() { IsShow = bool.Parse(isShow) });
    }
}
