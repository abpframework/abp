namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public interface IHangfireBackgroundWorkerAdapter : IHangfireBackgroundWorker
    {
        void BuildWorker(IBackgroundWorker worker);
    }
}