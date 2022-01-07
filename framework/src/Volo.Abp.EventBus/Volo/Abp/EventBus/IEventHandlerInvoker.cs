using System.Threading.Tasks;

namespace Volo.Abp.EventBus;

public interface IEventHandlerInvoker
{
    Task InvokeAsync(IEventHandler eventHandler, object eventData);
}
