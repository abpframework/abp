using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// A validated, ready-to-start dedicated worker: the distributed lock name it runs under and the
/// resolved job names it is responsible for. Built by <see cref="BackgroundJobWorkerManager"/> from a
/// <see cref="BackgroundJobWorkerConfiguration"/> after all configurations have been validated.
/// </summary>
public class DedicatedWorkerDefinition
{
    /// <summary>
    /// The distributed lock name this worker runs under.
    /// </summary>
    public string LockName { get; }

    /// <summary>
    /// The resolved job names this worker is responsible for.
    /// </summary>
    public IReadOnlyList<string> JobNames { get; }

    public DedicatedWorkerDefinition(string lockName, IReadOnlyList<string> jobNames)
    {
        LockName = lockName;
        JobNames = jobNames;
    }
}
