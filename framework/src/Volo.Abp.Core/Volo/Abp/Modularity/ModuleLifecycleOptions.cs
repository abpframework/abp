using Volo.Abp.Collections;

namespace Volo.Abp.Modularity
{
    public class ModuleLifecycleOptions
    {
        public ITypeList<IModuleLifecycleContributor> Contributors { get; }

        public ModuleLifecycleOptions()
        {
            Contributors = new TypeList<IModuleLifecycleContributor>();
        }
    }
}
