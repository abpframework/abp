using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.SignalR.Auditing
{
    public class AspNetCoreSignalRAuditLogContributor : AuditLogContributor, ITransientDependency
    {
        public ILogger<AspNetCoreSignalRAuditLogContributor> Logger { get; set; }

        public AspNetCoreSignalRAuditLogContributor()
        {
            Logger = NullLogger<AspNetCoreSignalRAuditLogContributor>.Instance;
        }

        public override void PreContribute(AuditLogContributionContext context)
        {
            var hubContext = context.ServiceProvider.GetRequiredService<IAbpHubContextAccessor>().Context;
            if (hubContext == null)
            {
                return;
            }

            var clientInfoProvider = context.ServiceProvider.GetRequiredService<IWebClientInfoProvider>();
            if (context.AuditInfo.ClientIpAddress == null)
            {
                context.AuditInfo.ClientIpAddress = clientInfoProvider.ClientIpAddress;
            }

            if (context.AuditInfo.BrowserInfo == null)
            {
                context.AuditInfo.BrowserInfo = clientInfoProvider.BrowserInfo;
            }

            //TODO: context.AuditInfo.ClientName
        }

        public override void PostContribute(AuditLogContributionContext context)
        {
            var hubContext = context.ServiceProvider.GetRequiredService<IAbpHubContextAccessor>().Context;
            if (hubContext == null)
            {
                return;
            }

            var firstAction = context.AuditInfo.Actions.FirstOrDefault();
            context.AuditInfo.Url = firstAction?.ServiceName + "." + firstAction?.MethodName;
            context.AuditInfo.HttpStatusCode = null;
        }
    }
}
