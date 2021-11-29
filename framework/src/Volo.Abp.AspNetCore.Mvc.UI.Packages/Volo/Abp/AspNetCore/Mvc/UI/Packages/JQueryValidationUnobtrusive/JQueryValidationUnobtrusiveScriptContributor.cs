using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryValidation;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryValidationUnobtrusive
{
    [DependsOn(typeof(JQueryValidationScriptContributor))]
    public class JQueryValidationUnobtrusiveScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js");
        }
    }
}
