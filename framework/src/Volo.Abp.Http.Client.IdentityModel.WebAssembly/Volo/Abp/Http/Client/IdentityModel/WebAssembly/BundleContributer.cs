using System;
using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<string> scripts)
        {
            scripts.AddIfNotContains("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
        }

        public void AddStyles(List<string> styles)
        {
        }
    }
}
