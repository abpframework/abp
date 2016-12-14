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

        public ModuleManager(AbpApplication application)
        {
            _modules = application.Modules.ToList();
        }
        
    }
}