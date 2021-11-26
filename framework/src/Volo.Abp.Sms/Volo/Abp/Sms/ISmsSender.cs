using System.Threading.Tasks;

namespace Volo.Abp.Sms;

public interface ISmsSender
{
    Task SendAsync(SmsMessage smsMessage);
}
