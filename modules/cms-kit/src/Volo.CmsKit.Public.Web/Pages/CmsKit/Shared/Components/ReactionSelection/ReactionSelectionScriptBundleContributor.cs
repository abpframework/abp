﻿using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.ReactionSelection
{
    public class ReactionSelectionScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/CmsKit/Shared/Components/ReactionSelection/default.js");
        }
    }
}
