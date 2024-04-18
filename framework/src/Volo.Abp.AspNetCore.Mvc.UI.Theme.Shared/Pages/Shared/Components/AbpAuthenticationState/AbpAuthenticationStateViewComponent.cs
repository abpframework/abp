using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpAuthenticationState;

public class AbpAuthenticationStateViewComponent : AbpViewComponent
{
public virtual IViewComponentResult Invoke()
{
    return View("~/Pages/Shared/Components/AbpAuthenticationState/Default.cshtml");
}
}
