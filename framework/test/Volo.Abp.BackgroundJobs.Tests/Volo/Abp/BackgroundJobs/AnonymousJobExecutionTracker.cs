using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs;

public class AnonymousJobExecutionTracker
{
    public List<string> ExecutedJsonData { get; } = new();
}
