namespace Volo.Abp.RabbitMQ
{
    public interface IChannelPool
    {
        IChannelAccessor Acquire(string channelName = null);
    }
}