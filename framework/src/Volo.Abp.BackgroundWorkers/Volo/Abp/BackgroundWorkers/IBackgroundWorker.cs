using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Interface for a worker (thread) that runs on background to perform some tasks.
/// </summary>
public interface IBackgroundWorker : IRunnable, ISingletonDependency
{

}
