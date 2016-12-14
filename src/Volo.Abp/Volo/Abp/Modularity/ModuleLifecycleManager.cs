using System.Collections.Generic;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class ModuleLifecycleManager : IModuleLifecycleManager, ISingletonDependency
    {
        private readonly IModuleManager _moduleManager;
        private readonly IEnumerable<IModuleLifecycleContributer> _contributers;

        public ModuleLifecycleManager(IModuleManager moduleManager, IEnumerable<IModuleLifecycleContributer> contributers)
        {
            _moduleManager = moduleManager;
            _contributers = contributers;
        }

        public void InitializeModules()
        {
            foreach (var contributer in _contributers)
            {
                foreach (var module in _moduleManager.Modules)
                {
                    contributer.Initialize(module.Instance);
                }
            }
        }

        public void ShutdownModules()
        {
            foreach (var contributer in _contributers)
            {
                foreach (var module in _moduleManager.Modules)
                {
                    contributer.Shutdown(module.Instance);
                }
            }
        }
    }
}