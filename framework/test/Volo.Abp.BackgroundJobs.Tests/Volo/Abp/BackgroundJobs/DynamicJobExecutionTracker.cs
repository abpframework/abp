using System.Collections.Concurrent;

namespace Volo.Abp.BackgroundJobs;

public class DynamicJobExecutionTracker
{
    public ConcurrentBag<string> ExecutedJsonData { get; } = new();
}
