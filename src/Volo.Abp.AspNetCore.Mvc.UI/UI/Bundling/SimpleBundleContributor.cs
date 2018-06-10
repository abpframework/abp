using System;

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
            context.Files.AddRange(Files);
        }
    }
}