using JetBrains.Annotations;
using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp
{
    public class AbpApplicationCreationOptions
    {
        [NotNull]
        public PlugInSourceList PlugInSources { get; private set; }

        public AbpApplicationCreationOptions()
        {
            PlugInSources = new PlugInSourceList();
        }
    }
}