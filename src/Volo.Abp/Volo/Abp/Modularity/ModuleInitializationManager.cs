using System.Collections.Generic;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class ModuleInitializationManager : IModuleInitializationManager, ISingletonDependency
    {
        private readonly IModuleLoader _moduleLoader;
        private readonly IEnumerable<IModuleInitializer> _initializers;

        public ModuleInitializationManager(IModuleLoader moduleLoader, IEnumerable<IModuleInitializer> initializers)
        {
            _moduleLoader = moduleLoader;
            _initializers = initializers;
        }

        public void InitializeModules()
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