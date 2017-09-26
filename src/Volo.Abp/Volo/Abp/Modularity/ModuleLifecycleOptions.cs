using Volo.Abp.Collections;

namespace Volo.Abp.Modularity
{
    public class ModuleLifecycleOptions
    {
        public ITypeList<IModuleLifecycleContributer> Contributers { get; }

        public ModuleLifecycleOptions()
        {
            Contributers = new TypeList<IModuleLifecycleContributer>();
        }
    }
}
