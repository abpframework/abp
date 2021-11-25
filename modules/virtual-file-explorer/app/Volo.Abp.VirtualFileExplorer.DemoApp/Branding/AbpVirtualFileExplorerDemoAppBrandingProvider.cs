using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.VirtualFileExplorer.DemoApp.Branding;

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
