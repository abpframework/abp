using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed
{
    public interface IRawEventPublisher
    {
        Task PublishRawAsync(
            Guid eventId,
            string eventName,
            byte[] eventData);
    }
}