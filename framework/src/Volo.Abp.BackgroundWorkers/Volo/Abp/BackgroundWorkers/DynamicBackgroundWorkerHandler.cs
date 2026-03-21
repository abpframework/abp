using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers;

public delegate Task DynamicBackgroundWorkerHandler(DynamicBackgroundWorkerExecutionContext context, CancellationToken cancellationToken);