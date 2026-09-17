using System;
using System.Threading.Tasks;

namespace Volo.Abp.RabbitMQ;

public interface IChannelPool : IAsyncDisposable
{
    Task<IChannelAccessor> AcquireAsync(string? channelName = null, string? connectionName = null);
}
