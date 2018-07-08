using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    //TODO: Implement as multiple providers can contribute!

    /// <summary>
    /// Default implementation of <see cref="IAuditInfoProvider" />.
    /// </summary>
    public class DefaultAuditInfoProvider : IAuditInfoProvider, ITransientDependency
    {
        protected IClientInfoProvider ClientInfoProvider { get; }

        public DefaultAuditInfoProvider(IClientInfoProvider clientInfoProvider)
        {
            ClientInfoProvider = clientInfoProvider;
        }

        public virtual void Fill(AuditInfo auditInfo)
        {
            if (auditInfo.ClientIpAddress.IsNullOrEmpty())
            {
                auditInfo.ClientIpAddress = ClientInfoProvider.ClientIpAddress;
            }

            if (auditInfo.BrowserInfo.IsNullOrEmpty())
            {
                auditInfo.BrowserInfo = ClientInfoProvider.BrowserInfo;
            }

            if (auditInfo.ClientName.IsNullOrEmpty())
            {
                auditInfo.ClientName = ClientInfoProvider.ComputerName;
            }
        }
    }
}