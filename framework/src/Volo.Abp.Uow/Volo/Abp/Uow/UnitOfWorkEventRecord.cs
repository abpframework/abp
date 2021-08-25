using System;
using System.Collections.Generic;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkEventRecord
    {
        public object EventData { get; }
        
        public Type EventType { get; }
        
        /// <summary>
        /// Extra properties can be used if needed.
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public UnitOfWorkEventRecord(
            Type eventType,
            object eventData)
        {
            EventType = eventType;
            EventData = eventData;
        }
    }
}