using System.Collections.Generic;

namespace Volo.Abp.Bundling
{
    public class BundleDefinition
    {
        public string Source { get; set; }
        public Dictionary<string, string> AdditionalProperties { get; set; }

        public BundleDefinition(string source)
        {
            Source = source;
            AdditionalProperties = new Dictionary<string, string>();
        }
    }
}
