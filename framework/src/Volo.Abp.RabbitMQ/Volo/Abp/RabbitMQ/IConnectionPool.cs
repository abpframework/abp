using RabbitMQ.Client;

namespace Volo.Abp.RabbitMQ
{
    public interface IConnectionPool
    {
        IConnection Get(string connectionName = null);
    }
}