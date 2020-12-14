using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.Cli.Bundling
{
    public class BundleConfig
    {
        public BundlingMode Mode { get; set; }
        public string Name { get; set; }
        public BundleParameterDictionary Parameters { get; set; }
    }
}