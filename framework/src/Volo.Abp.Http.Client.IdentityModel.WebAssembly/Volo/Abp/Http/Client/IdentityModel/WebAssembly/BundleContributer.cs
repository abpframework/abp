using Volo.Abp.Bundling;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly
{
    public class BundleContributer : BaseBundleContributer
    {
        public override string[] GetScripts()
        {
            return new string[]
            {
                "_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js",
            };
        }
    }
}
