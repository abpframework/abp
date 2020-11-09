using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Sms
{
    public static class SmsSenderExtensions
    {
        public static Task SendAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text)
        {
            return smsSender.SendAsync(phoneNumber, text, null);
        }
        public static Task SendAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text, IDictionary<string, object> properties)
        {
            Check.NotNull(smsSender, nameof(smsSender));
            return smsSender.SendAsync(new SmsMessage(phoneNumber, text, properties));
        }
        public static Task QueueAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text)
        {
            return smsSender.QueueAsync(phoneNumber, text, null);
        }
        public static Task QueueAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text, IDictionary<string, object> properties)
        {
            Check.NotNull(smsSender, nameof(smsSender));
            return smsSender.QueueAsync(new SmsMessage(phoneNumber, text, properties));
        }
    }
}
