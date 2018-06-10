namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.SweetAlert
{
    public class SweetalertScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/sweetalert/sweetalert.min.js");
        }
    }
}
