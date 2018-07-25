namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public interface IJobListener
    {
        void Start();
        void Stop();
    }
}