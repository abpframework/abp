using System.Threading.Tasks;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// Null-object implementation of <see cref="IEntityChangeEventHelper"/>.
    /// </summary>
    public class NullEntityChangeEventHelper : IEntityChangeEventHelper
    {
        /// <summary>
        /// Gets single instance of <see cref="NullEntityChangeEventHelper"/> class.
        /// </summary>
        public static NullEntityChangeEventHelper Instance { get; } = new NullEntityChangeEventHelper();

        private NullEntityChangeEventHelper()
        {

        }

        public Task TriggerEntityCreatingEventAsync(object entity)
        {
            return Task.CompletedTask;
        }

        public Task TriggerEntityCreatedEventAsync(object entity)
        {
            return Task.CompletedTask;
        }

        public Task TriggerEntityUpdatingEventAsync(object entity)
        {
            return Task.CompletedTask;
        }

        public Task TriggerEntityUpdatedEventAsync(object entity)
        {
            return Task.CompletedTask;
        }

        public Task TriggerEntityDeletingEventAsync(object entity)
        {
            return Task.CompletedTask;
        }

        public Task TriggerEntityDeletedEventAsync(object entity)
        {
            return Task.CompletedTask;
        }

        public Task TriggerEventsAsync(EntityChangeReport changeReport)
        {
            return Task.CompletedTask;
        }
    }
}