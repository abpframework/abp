using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs;

public class DynamicJobExecutionTracker
{
    public List<Dictionary<string, object>> ExecutedArgs { get; } = new();
}
