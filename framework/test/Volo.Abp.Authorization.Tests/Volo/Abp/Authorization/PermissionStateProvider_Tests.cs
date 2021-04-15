using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Authorization
{
    public abstract class PermissionStateProvider_Tests : AuthorizationTestBase
    {
        protected IPermissionStateManager PermissionStateManager { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }

        public PermissionStateProvider_Tests()
        {
            PermissionStateManager = GetRequiredService<IPermissionStateManager>();
            PermissionDefinitionManager = GetRequiredService<IPermissionDefinitionManager>();
            CurrentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }
    }

    public class SpecifyPermissionStateProvider : PermissionStateProvider_Tests
    {
        [Fact]
        public async Task PermissionState_Test()
        {
            var myPermission1 = PermissionDefinitionManager.Get("MyPermission1");
            myPermission1.StateProviders.ShouldContain(x => x.GetType() == typeof(TestRequireEditionPermissionStateProvider));

            (await PermissionStateManager.IsEnabledAsync(myPermission1)).ShouldBeFalse();

            using (CurrentPrincipalAccessor.Change(new Claim(AbpClaimTypes.EditionId, Guid.NewGuid().ToString())))
            {
                (await PermissionStateManager.IsEnabledAsync(myPermission1)).ShouldBeTrue();
            }
        }
    }

    public class GlobalPermissionStateProvider : PermissionStateProvider_Tests
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<AbpPermissionOptions>(options => options.GlobalStateProviders.Add<TestGlobalRequireRolePermissionStateProvider>());
        }

        [Fact]
        public async Task Global_PermissionState_Test()
        {
            var myPermission2 = PermissionDefinitionManager.Get("MyPermission2");

            (await PermissionStateManager.IsEnabledAsync(myPermission2)).ShouldBeFalse();

            using (CurrentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "admin")))
            {
                (await PermissionStateManager.IsEnabledAsync(myPermission2)).ShouldBeTrue();
            }
        }
    }
}
