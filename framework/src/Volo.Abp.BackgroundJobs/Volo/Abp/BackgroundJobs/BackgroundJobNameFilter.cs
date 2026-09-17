using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Filters the waiting jobs of a background job worker by job name.
/// A worker is exactly one of: no filter (<see cref="None"/>), include-only (a dedicated worker) or
/// exclude-only (the default worker in a multi-worker setup) — the two can never be combined.
/// </summary>
public class BackgroundJobNameFilter
{
    /// <summary>
    /// A filter that matches every job name.
    /// </summary>
    public static BackgroundJobNameFilter None { get; } = new(BackgroundJobNameFilterMode.None);

    public BackgroundJobNameFilterMode Mode { get; }

    public IReadOnlyList<string> JobNames { get; }

    public BackgroundJobNameFilter(BackgroundJobNameFilterMode mode, IReadOnlyList<string>? jobNames = null)
    {
        if (!Enum.IsDefined(typeof(BackgroundJobNameFilterMode), mode))
        {
            throw new ArgumentException($"Invalid background job name filter mode: {mode}", nameof(mode));
        }

        var names = jobNames?.Where(x => !x.IsNullOrWhiteSpace()).Distinct(StringComparer.Ordinal).ToList() ?? new List<string>();

        if (mode == BackgroundJobNameFilterMode.None && names.Count > 0)
        {
            throw new ArgumentException("Job names must be empty when the filter mode is None.", nameof(jobNames));
        }

        if (mode != BackgroundJobNameFilterMode.None && names.Count == 0)
        {
            throw new ArgumentException("Job names cannot be empty when the filter mode is Include or Exclude.", nameof(jobNames));
        }

        Mode = mode;
        JobNames = names.AsReadOnly();
    }

    public static BackgroundJobNameFilter Include(IReadOnlyList<string> jobNames)
    {
        return new BackgroundJobNameFilter(BackgroundJobNameFilterMode.Include, jobNames);
    }

    public static BackgroundJobNameFilter Exclude(IReadOnlyList<string> jobNames)
    {
        return new BackgroundJobNameFilter(BackgroundJobNameFilterMode.Exclude, jobNames);
    }

    /// <summary>
    /// Whether the given job name passes this filter, using an ordinal (case-sensitive) comparison for the
    /// in-memory eligibility re-check. The persistent stores translate <see cref="Mode"/> and
    /// <see cref="JobNames"/> into a database query instead, so their filtering follows the database collation.
    /// Job names are expected to be unique beyond case (they are derived from the type name by default).
    /// </summary>
    public virtual bool IsMatch(string jobName)
    {
        return Mode switch
        {
            BackgroundJobNameFilterMode.Include => JobNames.Contains(jobName, StringComparer.Ordinal),
            BackgroundJobNameFilterMode.Exclude => !JobNames.Contains(jobName, StringComparer.Ordinal),
            _ => true
        };
    }
}
