using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.PageAlerts
{
    public class PageAlertsViewComponent : AbpViewComponent
    {
        protected IAlertManager AlertManager { get; }

        public PageAlertsViewComponent(IAlertManager alertManager)
        {
            AlertManager = alertManager;
        }

        public IViewComponentResult Invoke(string name)
        {
            return View("~/Themes/Basic/Components/PageAlerts/Default.cshtml", AlertManager.Alerts);
        }
    }
}
