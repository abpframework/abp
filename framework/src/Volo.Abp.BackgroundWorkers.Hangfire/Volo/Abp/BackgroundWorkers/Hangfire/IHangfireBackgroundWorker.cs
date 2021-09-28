using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public interface IHangfireBackgroundWorker : IBackgroundWorker
    {
        string Name { get; set; }

        string Schedule { get; set; }

        Task RunAsync();

        Task DoWorkAsync(HangfireBackgroundWorkerContext workerContext);
    }
}