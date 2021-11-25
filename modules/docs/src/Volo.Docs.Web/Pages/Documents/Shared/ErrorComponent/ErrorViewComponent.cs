using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Docs.Pages.Documents.Shared.ErrorComponent
{
    public class ErrorViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke(ErrorPageModel model)
        {
            return View("~/Pages/Documents/Shared/ErrorComponent/Default.cshtml", model);
        }
    }
}
