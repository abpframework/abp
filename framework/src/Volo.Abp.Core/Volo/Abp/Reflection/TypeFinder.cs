using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Reflection;

public class TypeFinder : ITypeFinder
{
    private readonly ILogger<TypeFinder> _logger;

    private readonly IAssemblyFinder _assemblyFinder;

    private readonly Lazy<IReadOnlyList<Type>> _types;

    public TypeFinder(ILogger<TypeFinder> logger, IAssemblyFinder assemblyFinder)
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
            catch (ReflectionTypeLoadException e)
            {
                allTypes = e.Types.Select(x => x!).ToList();
                _logger.LogException(e);
            }
            catch (Exception e)
            {
                //TODO: Trigger a global event?
                _logger.LogException(e);
            }
        }

        return allTypes;
    }
}
