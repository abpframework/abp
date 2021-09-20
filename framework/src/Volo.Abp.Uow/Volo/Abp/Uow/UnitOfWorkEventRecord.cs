using System;
using System.Collections.Generic;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkEventRecord
    {
        public object EventData { get; }
        
        public Type EventType { get; }
        
        public long EventOrder { get; }
        
        /// <summary>
        /// Extra properties can be used if needed.
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public UnitOfWorkEventRecord(
            Type eventType,
            object eventData,
            long eventOrder)
        {
            EventType = eventType;
            EventData = eventData;
            EventOrder = eventOrder;
        }
    }
}