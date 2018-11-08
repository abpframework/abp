using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Sms
{
    public class NullSmsSender : ISmsSender, ISingletonDependency
    {
        public ILogger<NullSmsSender> Logger { get; set; }

        public NullSmsSender()
        {
            Logger = NullLogger<NullSmsSender>.Instance;
        }

        public Task SendAsync(string phoneNumber, string text, IDictionary<string, object> extraParameter)
        {
            Logger.LogWarning($"SMS Sending was not implemented! Using {nameof(NullSmsSender)}:");
            Logger.LogWarning("Phone Number : " + phoneNumber);
            Logger.LogWarning("SMS Text     : " + text);

            return Task.CompletedTask;
        }
    }
}