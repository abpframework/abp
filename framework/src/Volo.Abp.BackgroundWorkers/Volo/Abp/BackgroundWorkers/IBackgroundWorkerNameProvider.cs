namespace Volo.Abp.BackgroundWorkers;

public interface IBackgroundWorkerNameProvider
{
    string Name { get; }
}