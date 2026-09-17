using System;
using System.Linq;
using System.Collections.Concurrent;

namespace Volo.Abp.AI;

[AttributeUsage(AttributeTargets.Class)]
public class WorkspaceNameAttribute : Attribute
{
    public string Name { get; }

    public WorkspaceNameAttribute(string name)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
    }

    private static readonly ConcurrentDictionary<Type, string> _nameCache = new();

    public static string GetWorkspaceName<TWorkspace>()
    {
        return GetWorkspaceName(typeof(TWorkspace));
    }
    
    public static string GetWorkspaceName(Type workspaceType)
    {
        return _nameCache.GetOrAdd(workspaceType, type =>
        {
            var workspaceNameAttribute = type
                .GetCustomAttributes(true)
                .OfType<WorkspaceNameAttribute>()
                .FirstOrDefault();

            return workspaceNameAttribute?.Name ?? type.FullName!;
        });
    }
}