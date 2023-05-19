namespace Volo.Abp.BackgroundWorkers.Quartz;

public interface IQuartzBackgroundWorkerAdapter : IQuartzBackgroundWorker
{
    void BuildWorker(IBackgroundWorker worker);
}
