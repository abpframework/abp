using System;
using RabbitMQ.Client;

namespace Volo.Abp.RabbitMQ
{
    public interface IChannelAccessor : IDisposable
    {
        IModel Channel { get; }
    }
}