using DashboardDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace DashboardDemo.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class DashboardDemoController : AbpController
    {
        protected DashboardDemoController()
        {
            LocalizationResource = typeof(DashboardDemoResource);
        }
    }
}