using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp
{
    public class AbpApplicationOptions
    {
        public PlugInSourceList PlugInSources { get; set; }

        public AbpApplicationOptions()
        {
            PlugInSources = new PlugInSourceList();
        }
    }
}