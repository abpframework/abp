using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Volo.Abp.Sms
{
    public static class SmsSenderExtensions
    {
        public static Task SendAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text)
        {
            Check.NotNull(smsSender, nameof(smsSender));
            return smsSender.SendAsync(new SmsMessage(phoneNumber, text));
        }
    }
}
