using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpPageSearchBox
{
    public class AbpPageSearchBoxViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke(string placeHolder)
        {
            return View("~/Pages/Shared/Components/AbpPageSearchBox/Default.cshtml");
        }
    }
}