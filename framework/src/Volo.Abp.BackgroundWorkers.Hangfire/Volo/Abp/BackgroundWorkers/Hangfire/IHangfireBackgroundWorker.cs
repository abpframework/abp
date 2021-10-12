using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public interface IHangfireBackgroundWorker :　IBackgroundWorker
    {
        string RecurringJobId { get; set; }

        string CronExpression { get; set; }

        Task ExecuteAsync();
    }
}


