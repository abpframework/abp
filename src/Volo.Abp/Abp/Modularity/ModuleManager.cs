using System.Collections.Generic;

namespace Volo.Abp.Modularity
{
    public class ModuleManager : IModuleManager
    {
        private readonly IModuleLoader _moduleLoader;
        private readonly IEnumerable<IModuleInitializer> _initializers;

        public ModuleManager(IModuleLoader moduleLoader, IEnumerable<IModuleInitializer> initializers)
        {
            _moduleLoader = moduleLoader;
            _initializers = initializers;
        }

        public void Initialize()
        {
            foreach (var initializer in _initializers)
            {
                foreach (var module in _moduleLoader.Modules)
                {
                    initializer.Initialize(module.Instance);
                }
            }
        }
    }
}