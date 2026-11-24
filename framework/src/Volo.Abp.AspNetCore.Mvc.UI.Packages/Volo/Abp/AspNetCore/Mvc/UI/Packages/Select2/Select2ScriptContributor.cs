using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Core;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Select2;

[DependsOn(typeof(CoreScriptContributor))]
public class Select2ScriptContributor : BundleContributor
{
    public const string PackageName = "select2";
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        //TODO: Add select2.full.min.js
        context.Files.AddIfNotContains("/libs/select2/js/select2.min.js");
    }
    public override void ConfigureDynamicResources(BundleConfigurationContext context)
    {
        var fileName = context.LazyServiceProvider.LazyGetRequiredService<IOptions<AbpLocalizationOptions>>().Value.GetCurrentUICultureLanguageFilesMap(PackageName);
        var filePath = $"/libs/select2/js/i18n/{fileName}.js";
        if (context.FileProvider.GetFileInfo(filePath).Exists)
        {
            context.Files.AddIfNotContains(filePath);
        }
    }
}
