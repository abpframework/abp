using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs;

public class DynamicJobExecutionTracker
{
    public List<string> ExecutedJsonData { get; } = new();
}
