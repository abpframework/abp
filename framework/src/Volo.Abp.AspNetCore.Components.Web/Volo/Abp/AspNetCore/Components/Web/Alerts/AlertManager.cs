using Volo.Abp.AspNetCore.Components.Alerts;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Alerts
{
    public class AlertManager : IAlertManager, IScopedDependency
    {
        public AlertList Alerts { get; }

        public AlertManager()
        {
            Alerts = new AlertList();
        }
    }
}