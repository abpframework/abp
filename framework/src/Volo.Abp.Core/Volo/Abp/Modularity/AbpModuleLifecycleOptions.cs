using Volo.Abp.Collections;

namespace Volo.Abp.Modularity
{
    public class AbpModuleLifecycleOptions
    {
        public ITypeList<IModuleLifecycleContributor> Contributors { get; }

        public AbpModuleLifecycleOptions()
        {
            Contributors = new TypeList<IModuleLifecycleContributor>();
        }
    }
}
