using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpAuthenticationSessionState;

public class AbpAuthenticationSessionStateViewComponent : AbpViewComponent
{
public virtual IViewComponentResult Invoke()
{
    return View("~/Pages/Shared/Components/AbpAuthenticationSessionState/Default.cshtml");
}
}
