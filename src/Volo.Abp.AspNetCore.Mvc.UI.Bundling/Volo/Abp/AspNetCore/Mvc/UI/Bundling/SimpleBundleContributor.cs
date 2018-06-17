using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class SimpleBundleContributor : BundleContributor
    {
        public string[] Files { get; }

        public SimpleBundleContributor(params string[] files)
        {
            Files = files ?? Array.Empty<string>();
        }

        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            foreach (var file in Files)
            {
                context.Files.AddIfNotContains(file);
            }
        }
    }
}