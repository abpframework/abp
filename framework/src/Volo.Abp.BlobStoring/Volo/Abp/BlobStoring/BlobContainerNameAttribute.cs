using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

public class BlobContainerNameAttribute : Attribute
{
    [NotNull]
    public string Name { get; }

    public BlobContainerNameAttribute([NotNull] string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Name = name;
    }

    public virtual string GetName(Type type)
    {
        return Name;
    }

    public static string GetContainerName<T>()
    {
        return GetContainerName(typeof(T));
    }

    public static string GetContainerName(Type type)
    {
        var nameAttribute = type.GetCustomAttribute<BlobContainerNameAttribute>();

        if (nameAttribute == null)
        {
            return type.FullName;
        }

        return nameAttribute.GetName(type);
    }
}
