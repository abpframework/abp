using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Pages.Shared.Components.Head
{
    public class HeadViewComponent : AbpViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View("/Pages/Shared/Components/Head/Default.cshtml");
        }
    }
}
