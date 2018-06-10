namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Toastr
{
    public class ToastrStyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/toastr/toastr.min.css");
        }
    }
}