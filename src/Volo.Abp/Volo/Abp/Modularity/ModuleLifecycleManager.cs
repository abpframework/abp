using System.Collections.Generic;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class ModuleLifecycleManager : IModuleLifecycleManager, ISingletonDependency
    {
        private readonly IModuleLoader _moduleLoader;
        private readonly IEnumerable<IModuleLifecycleContributer> _contributers;

        public ModuleLifecycleManager(IModuleLoader moduleLoader, IEnumerable<IModuleLifecycleContributer> contributers)
        {
            _moduleLoader = moduleLoader;
            _contributers = contributers;
        }

        public void InitializeModules()
        {
            foreach (var contributer in _contributers)
            {
                foreach (var module in _moduleLoader.Modules)
                {
                    contributer.Initialize(module.Instance);
                }
            }
        }

        public void ShutdownModules()
        {
            foreach (var contributer in _contributers)
            {
                foreach (var module in _moduleLoader.Modules)
                {
                    contributer.Shutdown(module.Instance);
                }
            }
        }
    }
}