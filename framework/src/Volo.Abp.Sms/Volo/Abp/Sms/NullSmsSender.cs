using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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

        public Task SendAsync(string text)
        {
            Logger.LogWarning($"SMS Sending was not implemented! Using {nameof(NullSmsSender)}:");
            Logger.LogWarning("SMS Text: " + text);
            return Task.CompletedTask;
        }
    }
}