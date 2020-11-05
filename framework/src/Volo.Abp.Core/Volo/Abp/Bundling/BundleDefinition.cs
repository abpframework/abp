using System.Collections.Generic;

namespace Volo.Abp.Bundling
{
    public class BundleDefinition
    {
        public string Source { get; set; }
        public Dictionary<string, string> AdditionalProperties { get; set; }
    }
}
