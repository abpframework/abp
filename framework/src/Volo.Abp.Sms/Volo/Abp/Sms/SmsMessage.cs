using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Sms
{
    public class SmsMessage
    {
        public string PhoneNumber { get; set; }

        public string Text { get; set; }

        public IDictionary<string, object> Properties { get; set; }
    }
}
