namespace Volo.Abp.BackgroundJobs
{
    public interface IBackgroundJobNameProvider
    {
        string Name { get; }
    }
}