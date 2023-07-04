using System;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.Modularity;

/// <summary>
/// Used to define additional assemblies for a module.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AdditionalAssemblyAttribute : Attribute, IAdditionalModuleAssemblyProvider
{
    public Type[] TypesInAssemblies { get; }

    public AdditionalAssemblyAttribute(params Type[]? typesInAssemblies)
    {
        TypesInAssemblies = typesInAssemblies ?? Type.EmptyTypes;
    }

    public virtual Assembly[] GetAssemblies()
    {
        return TypesInAssemblies.Select(t => t.Assembly).Distinct().ToArray();
    }
}