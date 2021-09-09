using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed
{
    public interface IRawEventPublisher //TODO: Rename: ISupportsEventBoxes
    {
        Task PublishRawAsync(
            Guid eventId,
            string eventName,
            byte[] eventData);

        Task ProcessRawAsync(
            string eventName,
            byte[] eventDataBytes
        );
    }
}