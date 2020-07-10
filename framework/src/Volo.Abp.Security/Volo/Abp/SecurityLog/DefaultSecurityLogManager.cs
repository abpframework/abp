using System;
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

        public async Task SaveAsync(Action<SecurityLogInfo> saveAction = null)
        {
            if (!SecurityLogOptions.IsEnabled)
            {
                return;
            }

            var securityLogInfo = await CreateAsync();
            saveAction?.Invoke(securityLogInfo);
            await SecurityLogStore.SaveAsync(securityLogInfo);
        }

        protected virtual Task<SecurityLogInfo> CreateAsync()
        {
            return Task.FromResult(new SecurityLogInfo
            {
                ApplicationName = SecurityLogOptions.ApplicationName
            });
        }
    }
}
