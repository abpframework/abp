using System.Threading.Tasks;

namespace Volo.Abp.EventBus
{
    public interface IEventErrorHandler
    {
        Task HandleAsync(EventExecutionErrorContext context);
    }
}
