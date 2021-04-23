using System.Threading.Tasks;

namespace Volo.Abp.EventBus
{
    public interface IEventErrorHandler
    {
        Task Handle(EventExecutionErrorContext context);
    }
}
