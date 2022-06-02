using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.EventBus.Azure
{
    public interface IAzureEventETO
    {
        public string MessageId { get; set; }
    }
}
