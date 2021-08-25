using System.Threading.Tasks;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// Used to trigger entity change events.
    /// </summary>
    public interface IEntityChangeEventHelper
    {
        Task TriggerEventsAsync(EntityChangeReport changeReport);

        Task TriggerEntityCreatingEventAsync(object entity);
        Task TriggerEntityCreatedEventAsync(object entity);

        Task TriggerEntityUpdatingEventAsync(object entity);
        Task TriggerEntityUpdatedEventAsync(object entity);

        Task TriggerEntityDeletingEventAsync(object entity);
        Task TriggerEntityDeletedEventAsync(object entity);
    }
}