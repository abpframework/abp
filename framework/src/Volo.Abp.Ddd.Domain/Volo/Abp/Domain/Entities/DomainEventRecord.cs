namespace Volo.Abp.Domain.Entities
{
    public class DomainEventRecord
    {
        public object EventData { get; }
        
        public long EventOrder { get; }
        
        public DomainEventRecord(object eventData, long eventOrder)
        {
            EventData = eventData;
            EventOrder = eventOrder;
        }
    }
}