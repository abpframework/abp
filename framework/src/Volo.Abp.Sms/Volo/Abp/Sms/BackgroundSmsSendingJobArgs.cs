using System;
using System.Collections.Generic;

namespace Volo.Abp.Sms
{
    [Serializable]
    public class BackgroundSmsSendingJobArgs
    {
        public string PhoneNumber { get; set; }

        public string Text { get; set; }

        public IDictionary<string, object> Properties { get; set; }
    }
}
