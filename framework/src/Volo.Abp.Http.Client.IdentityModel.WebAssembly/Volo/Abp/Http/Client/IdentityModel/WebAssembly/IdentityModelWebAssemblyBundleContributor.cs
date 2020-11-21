using Volo.Abp.Bundling;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly
{
    public class IdentityModelWebAssemblyBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {
            context.Add("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
        }

        public void AddStyles(BundleContext context)
        {

        }
    }
}
