using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp.Reflection
{
    public class AssemblyFinder : IAssemblyFinder, ITransientDependency
    {
        private readonly IModuleContainer _moduleContainer;

        private readonly Lazy<IReadOnlyList<Assembly>> _assemblies;

        public AssemblyFinder(IModuleContainer moduleContainer)
        {
            _moduleContainer = moduleContainer;

            _assemblies = new Lazy<IReadOnlyList<Assembly>>(FindAll, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IReadOnlyList<Assembly> Assemblies => _assemblies.Value;

        public IReadOnlyList<Assembly> FindAll()
        {
            var assemblies = new List<Assembly>();

            foreach (var module in _moduleContainer.Modules)
            {
                assemblies.Add(module.Type.GetAssembly());
            }

            return assemblies.Distinct().ToImmutableList();
        }
    }
}