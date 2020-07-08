using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SecurityLog
{
    public class DefaultSecurityLogManager : ISecurityLogManager, ITransientDependency
    {
        protected AbpSecurityLogOptions SecurityLogOptions { get; }

        protected ISecurityLogStore SecurityLogStore { get; }

        public DefaultSecurityLogManager(
            IOptions<AbpSecurityLogOptions> securityLogOptions,
            ISecurityLogStore securityLogStore)
        {
            SecurityLogStore = securityLogStore;
            SecurityLogOptions = securityLogOptions.Value;
        }

        public virtual Task<SecurityLogInfo> CreateAsync()
        {
            return Task.FromResult(new SecurityLogInfo
            {
                ApplicationName = SecurityLogOptions.ApplicationName
            });
        }

        public async Task SaveAsync(SecurityLogInfo securityLogInfo)
        {
            await SecurityLogStore.SaveAsync(securityLogInfo);
        }
    }
}
