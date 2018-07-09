using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Auditing
{
    public class ClientInfoAuditLogContributor : AuditLogContributor
    {
        public override Task ContributeAsync(AuditLogContributionContext context)
        {
            var clientInfoProvider = context.ServiceProvider.GetRequiredService<IClientInfoProvider>();

            if (context.AuditInfo.ClientIpAddress.IsNullOrEmpty())
            {
                context.AuditInfo.ClientIpAddress = clientInfoProvider.ClientIpAddress;
            }

            if (context.AuditInfo.BrowserInfo.IsNullOrEmpty())
            {
                context.AuditInfo.BrowserInfo = clientInfoProvider.BrowserInfo;
            }

            if (context.AuditInfo.ClientName.IsNullOrEmpty())
            {
                context.AuditInfo.ClientName = clientInfoProvider.ComputerName;
            }

            return Task.CompletedTask;
        }
    }
}
