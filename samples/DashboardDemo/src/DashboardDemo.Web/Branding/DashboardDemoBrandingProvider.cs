using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace DashboardDemo.Branding
{
    [Dependency(ReplaceServices = true)]
    public class DashboardDemoBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "DashboardDemo";
    }
}
