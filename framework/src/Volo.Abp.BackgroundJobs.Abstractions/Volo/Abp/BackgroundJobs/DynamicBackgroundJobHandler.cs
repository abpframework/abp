using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Represents a handler delegate for dynamic background jobs.
/// </summary>
public delegate Task DynamicBackgroundJobHandler(DynamicBackgroundJobExecutionContext context, CancellationToken cancellationToken);
