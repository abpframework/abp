using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Sms
{
    [Dependency(TryRegister = true)]
    public class NullSmsSender : SmsSenderBase, ISingletonDependency
    {
        public ILogger<NullSmsSender> Logger { get; set; }

        public NullSmsSender(IBackgroundJobManager backgroundJobManager): base(backgroundJobManager)
        {
            Logger = NullLogger<NullSmsSender>.Instance;
        }

        protected override Task SendSmsAsync(SmsMessage smsMessage)
        {
            Logger.LogWarning($"SMS Sending was not implemented! Using {nameof(NullSmsSender)}:");

            Logger.LogWarning("Phone Number  : " + smsMessage.PhoneNumber);
            Logger.LogWarning("SMS Text      : " + smsMessage.Text);
            if (smsMessage.Properties != null)
            {
                Logger.LogWarning("SMS Properties: " + JsonConvert.SerializeObject(smsMessage.Properties));
            }
            
            return Task.CompletedTask;
        }
    }
}