using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Reflection
{
    //TODO: What if we need to this type finder while registering dependencies?
    public class TypeFinder : ITypeFinder, ITransientDependency
    {
        private readonly IAssemblyFinder _assemblyFinder;
        private readonly ILogger<TypeFinder> _logger;

        private readonly Lazy<IReadOnlyList<Type>> _types;

        public TypeFinder(IAssemblyFinder assemblyFinder, ILogger<TypeFinder> logger)
        {
            _assemblyFinder = assemblyFinder;
            _logger = logger;

            _types = new Lazy<IReadOnlyList<Type>>(FindAll, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IReadOnlyList<Type> Types => _types.Value;

        private IReadOnlyList<Type> FindAll()
        {
            var allTypes = new List<Type>();

            foreach (var assembly in _assemblyFinder.Assemblies)
            {
                try
                {
                    var typesInThisAssembly = AssemblyHelper.GetAllTypes(assembly);

                    if (!typesInThisAssembly.Any())
                    {
                        continue;
                    }

                    allTypes.AddRange(typesInThisAssembly.Where(type => type != null));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.ToString());
                }
            }

            return allTypes;
        }
    }
}