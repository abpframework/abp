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

        public void TriggerEntityCreatingEvent(object entity)
        {
            
        }

        public void TriggerEntityCreatedEventOnUowCompleted(object entity)
        {
            
        }

        public void TriggerEntityUpdatingEvent(object entity)
        {
            
        }

        public void TriggerEntityUpdatedEventOnUowCompleted(object entity)
        {
            
        }

        public void TriggerEntityDeletingEvent(object entity)
        {
            
        }

        public void TriggerEntityDeletedEventOnUowCompleted(object entity)
        {
            
        }

        public void TriggerEvents(EntityChangeReport changeReport)
        {
            
        }

        public Task TriggerEventsAsync(EntityChangeReport changeReport)
        {
            return Task.CompletedTask;
        }
    }
}