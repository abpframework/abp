using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.ClientSimulation.Demo.Controllers
{
    public class HomeController : AbpController
    {
        public RedirectToPageResult Index()
        {
            return RedirectToPage("/ClientSimulation/Index");
        }
    }
}
