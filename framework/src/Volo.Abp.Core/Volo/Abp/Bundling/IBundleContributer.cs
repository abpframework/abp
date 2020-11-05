using System.Collections.Generic;

namespace Volo.Abp.Bundling
{
    public interface IBundleContributer
    {
        void AddScripts(List<BundleDefinition> scriptDefinitions);
        void AddStyles(List<BundleDefinition> styleDefinitions);
    }
}
