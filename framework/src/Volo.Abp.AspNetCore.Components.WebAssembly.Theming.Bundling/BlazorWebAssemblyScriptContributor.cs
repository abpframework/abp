using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling;

public class BlazorWebAssemblyScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/abp.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/lang-utils.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/lang-utils.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/authentication-state-listener.js");
    }
}
