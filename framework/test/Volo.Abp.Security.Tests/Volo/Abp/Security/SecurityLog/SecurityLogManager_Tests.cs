using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.SecurityLog;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.SecurityLog
{

    public class SecurityLogManager_Tests : AbpIntegratedTest<AbpSecurityTestModule>
    {
        private readonly ISecurityLogManager _securityLogManager;

        private ISecurityLogStore _auditingStore;

        public SecurityLogManager_Tests()
        {
            _securityLogManager = GetRequiredService<ISecurityLogManager>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _auditingStore = Substitute.For<ISecurityLogStore>();
            services.AddSingleton(_auditingStore);
        }

        [Fact]
        public async Task SaveAsync()
        {
            await _securityLogManager.SaveAsync(securityLog =>
            {
                securityLog.Identity = "Test";
                securityLog.Action = "Test-Action";
                securityLog.UserName = "Test-User";
            });

            await _auditingStore.Received().SaveAsync(Arg.Is<SecurityLogInfo>(log =>
                log.ApplicationName == "AbpSecurityTest" &&
                log.Identity == "Test" &&
                log.Action == "Test-Action" &&
                log.UserName == "Test-User"));
        }
    }
}
