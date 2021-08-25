using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkEventPublisher
    {
        Task PublishLocalEventsAsync(IEnumerable<object> localEvents);
        Task PublishDistributedEventsAsync(IEnumerable<object> distributedEvents);
    }
}