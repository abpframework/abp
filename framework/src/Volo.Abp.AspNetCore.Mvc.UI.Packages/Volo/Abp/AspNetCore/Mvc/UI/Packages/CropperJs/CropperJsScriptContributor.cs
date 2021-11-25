using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.CropperJs;

public class CropperJsScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/cropperjs/js/cropper.min.js");
    }
}
