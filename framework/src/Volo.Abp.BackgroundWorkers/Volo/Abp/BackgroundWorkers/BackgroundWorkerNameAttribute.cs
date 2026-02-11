using System;
using System.Linq;

namespace Volo.Abp.BackgroundWorkers;

public class BackgroundWorkerNameAttribute : Attribute, IBackgroundWorkerNameProvider
{
    public string Name { get; }

    public BackgroundWorkerNameAttribute(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }

    public static string GetName<TWorkerType>()
    {
        return GetName(typeof(TWorkerType));
    }
    
    public static string GetName(Type workerType)
    {
        Check.NotNull(workerType, nameof(workerType));

        return GetNameOrNull(workerType) ?? workerType.FullName!;
    }
    
    public static string? GetNameOrNull<TWorkerType>()
    {
        return GetNameOrNull(typeof(TWorkerType));
    }
    
    public static string? GetNameOrNull(Type workerType)
    {
        Check.NotNull(workerType, nameof(workerType));

        return workerType
            .GetCustomAttributes(true)
            .OfType<IBackgroundWorkerNameProvider>()
            .FirstOrDefault()
            ?.Name;
    }
}
