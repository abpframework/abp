using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Contributors
{
    public class SimpleBundleContributor : IBundleContributor
    {
        public string[] Files { get; }

        public SimpleBundleContributor(params string[] files)
        {
            Files = files ?? Array.Empty<string>();
        }

        public void Contribute(List<string> files)
        {
            files.AddRange(Files);
        }
    }
}