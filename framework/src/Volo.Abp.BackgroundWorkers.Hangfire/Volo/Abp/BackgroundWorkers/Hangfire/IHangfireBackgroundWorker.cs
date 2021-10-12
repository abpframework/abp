using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public interface IHangfireBackgroundWorker :　IBackgroundWorker
    {
        string CronExpression { get; set; }

        Task ExecuteAsync();
    }
}


