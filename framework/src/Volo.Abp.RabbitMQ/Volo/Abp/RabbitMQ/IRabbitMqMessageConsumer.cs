using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Volo.Abp.RabbitMQ
{
    public interface IRabbitMqMessageConsumer
    {
        void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback);
    }
}