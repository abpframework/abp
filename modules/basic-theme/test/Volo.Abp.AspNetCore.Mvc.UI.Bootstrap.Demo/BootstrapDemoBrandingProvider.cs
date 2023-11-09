using Volo.Abp.Ui.Branding;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo;

public class BootstrapDemoBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Bootstrap Tag Helpers";

    public override string LogoUrl => "/imgs/demo/abp-io-light.png";
}