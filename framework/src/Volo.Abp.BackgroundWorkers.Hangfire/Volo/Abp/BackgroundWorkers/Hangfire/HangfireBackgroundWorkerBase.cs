using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public abstract class HangfireBackgroundWorkerBase : BackgroundWorkerBase, IHangfireBackgroundWorker
    {
        public string CronExpression { get; set; }

        public abstract Task ExecuteAsync();
    }
}
