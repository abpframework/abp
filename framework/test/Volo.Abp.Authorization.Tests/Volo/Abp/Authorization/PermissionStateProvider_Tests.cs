using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Volo.Abp.SimpleStateChecking;
using Xunit;

namespace Volo.Abp.Authorization
{
    public abstract class PermissionStateProvider_Tests : AuthorizationTestBase
    {
        protected ISimpleStateCheckerManager<PermissionDefinition> PermissionSimpleStateCheckerManager { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }

        public PermissionStateProvider_Tests()
        {
            PermissionSimpleStateCheckerManager = GetRequiredService<ISimpleStateCheckerManager<PermissionDefinition>>();
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
            myPermission1.SimpleStateCheckers.ShouldContain(x => x.GetType() == typeof(TestRequireEditionPermissionSimpleStateChecker));

            (await PermissionSimpleStateCheckerManager.IsEnabledAsync(myPermission1)).ShouldBeFalse();

            using (CurrentPrincipalAccessor.Change(new Claim(AbpClaimTypes.EditionId, Guid.NewGuid().ToString())))
            {
                (await PermissionSimpleStateCheckerManager.IsEnabledAsync(myPermission1)).ShouldBeTrue();
            }
        }
    }

    public class GlobalPermissionStateProvider : PermissionStateProvider_Tests
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<AbpSimpleStateCheckerOptions<PermissionDefinition>>(options =>
            {
                options.GlobalSimpleStateCheckers.Add<TestGlobalRequireRolePermissionSimpleStateChecker>();
            });
        }

        [Fact]
        public async Task Global_PermissionState_Test()
        {
            var myPermission2 = PermissionDefinitionManager.Get("MyPermission2");

            (await PermissionSimpleStateCheckerManager.IsEnabledAsync(myPermission2)).ShouldBeFalse();

            using (CurrentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "admin")))
            {
                (await PermissionSimpleStateCheckerManager.IsEnabledAsync(myPermission2)).ShouldBeTrue();
            }
        }
    }
}
