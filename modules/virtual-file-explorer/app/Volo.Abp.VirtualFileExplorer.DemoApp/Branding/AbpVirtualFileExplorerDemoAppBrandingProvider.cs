using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.VirtualFileExplorer.DemoApp.Branding
{
    [Dependency(ReplaceServices = true)]
    public class AbpVirtualFileExplorerDemoAppBrandingProvider : DefaultBrandingProvider
    {
        public AbpVirtualFileExplorerDemoAppBrandingProvider()
        {
            AppName = "Virtual file explorer demo app";

            
        }

        public override string AppName { get; }

        public override string LogoUrl { get; }
    }
}
