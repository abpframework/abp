using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.Cli.Bundling
{
    public class BundleConfig
    {
        public BundlingMode Mode { get; set; } = BundlingMode.BundleAndMinify;
        public string Name { get; set; } = "global";
        public BundleParameterDictionary Parameters { get; set; } = new();
    }
}