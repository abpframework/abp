using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Sms
{
    public class SmsMessage
    {
        public string PhoneNumber { get; }

        public string Text { get; }

        public IDictionary<string, object> Properties { get; }

        public SmsMessage([NotNull] string phoneNumber, [NotNull] string text)
            : this(phoneNumber, text, null)
        {
        }

        public SmsMessage([NotNull] string phoneNumber, [NotNull] string text, IDictionary<string, object> properties)
        {
            PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Text = Check.NotNullOrWhiteSpace(text, nameof(text));

            Properties = properties ?? new Dictionary<string, object>();
        }
    }
}
