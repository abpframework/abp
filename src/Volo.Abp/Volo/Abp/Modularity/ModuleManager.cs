using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class ModuleManager : IModuleManager, ISingletonDependency
    {
        public IReadOnlyList<AbpModuleDescriptor> Modules => _modules.ToImmutableList();

        private readonly List<AbpModuleDescriptor> _modules;
        private readonly IEnumerable<IModuleLifecycleContributer> _lifecycleContributers;

        public ModuleManager(
            AbpApplication application,
            IEnumerable<IModuleLifecycleContributer> lifecycleContributers)
        {
            _lifecycleContributers = lifecycleContributers;
            _modules = application.Modules.ToList();
        }

        public void InitializeModules(ApplicationInitializationContext context)
        {
            foreach (var contributer in _lifecycleContributers)
            {
                foreach (var module in Modules)
                {
                    contributer.Initialize(context, module.Instance);
                }
            }
        }

        public void ShutdownModules(ApplicationShutdownContext context)
        {
            foreach (var contributer in _lifecycleContributers)
            {
                foreach (var module in Modules)
                {
                    contributer.Shutdown(context, module.Instance);
                }
            }
        }
    }
}