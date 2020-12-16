using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Volo.ClientSimulation.Demo
{
    [Dependency(ReplaceServices = true)]
    public class BrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Client Simulation";
    }
}
