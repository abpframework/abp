using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SecurityLog
{
    public class SimpleSecurityLogStore : ISecurityLogStore, ITransientDependency
    {
        public ILogger<SimpleSecurityLogStore> Logger { get; set; }

        public SimpleSecurityLogStore(ILogger<SimpleSecurityLogStore> logger)
        {
            Logger = logger;
        }

        public Task SaveAsync(SecurityLogInfo securityLogInfo)
        {
            Logger.LogInformation(securityLogInfo.ToString());
            return Task.FromResult(0);
        }
    }
}
