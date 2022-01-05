using System;
using Volo.Abp.Reflection;

namespace Volo.Abp.Studio.Analyzing.Models;

public class PackageContentItemNameAttribute : Attribute
{
    public string Name { get; }

    public PackageContentItemNameAttribute(string name)
    {
        Name = name;
    }

    public static string GetName(Type type)
    {
        var attribute = ReflectionHelper.GetSingleAttributeOrDefault<PackageContentItemNameAttribute>(type);
        if (attribute == null)
        {
            throw new ApplicationException($"Given type {type.FullName} must have an {nameof(PackageContentItemNameAttribute)}, but not defined!");
        }

        return attribute.Name;
    }
}
