using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed
{
    public interface IEventOutbox
    {
        Task EnqueueAsync(string eventName, byte[] eventData);
    }
}