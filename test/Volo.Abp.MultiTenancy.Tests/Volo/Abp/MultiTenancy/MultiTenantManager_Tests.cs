using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenantManager_Tests : MultiTenancyTestBase
    {
        private readonly IMultiTenancyManager _multiTenancyManager;

        private readonly TenantInfo _tenantA = new TenantInfo("A");
        private readonly TenantInfo _tenantB = new TenantInfo("B");
        private TenantInfo _tenantToBeResolved;

        public MultiTenantManager_Tests()
        {
            _multiTenancyManager = ServiceProvider.GetRequiredService<IMultiTenancyManager>();
        }

        [Fact]
        public void CurrentTenant_Should_Be_Null_As_Default()
        {
            //Assert

            _multiTenancyManager.CurrentTenant.ShouldBeNull();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<MultiTenancyOptions>(options =>
            {
                options.TenantResolvers.Add(new SimpleTenantResolver(context =>
                {
                    if (_tenantToBeResolved == _tenantA)
                    {
                        context.Tenant = _tenantA;
                    }
                }));

                options.TenantResolvers.Add(new SimpleTenantResolver(context =>
                {
                    if (_tenantToBeResolved == _tenantB)
                    {
                        context.Tenant = _tenantB;
                    }
                }));
            });
        }

        [Fact]
        public void Should_Get_Current_Tenant_From_Single_Resolver()
        {
            //Arrange

            _tenantToBeResolved = _tenantA;

            //Assert

            _multiTenancyManager.CurrentTenant.ShouldBe(_tenantA);
        }


        [Fact]
        public void Should_Get_Current_Tenant_From_Multiple_Resolvers()
        {
            //Arrange

            _tenantToBeResolved = _tenantB;

            //Assert

            _multiTenancyManager.CurrentTenant.ShouldBe(_tenantB);
        }

        [Fact]
        public void Should_Get_Changed_Tenant_If_Wanted()
        {
            _multiTenancyManager.CurrentTenant.ShouldBe(null);

            _tenantToBeResolved = _tenantB;

            _multiTenancyManager.CurrentTenant.ShouldBe(_tenantB);

            using (_multiTenancyManager.ChangeTenant(_tenantA))
            {
                _multiTenancyManager.CurrentTenant.ShouldBe(_tenantA);

                using (_multiTenancyManager.ChangeTenant(_tenantB))
                {
                    _multiTenancyManager.CurrentTenant.ShouldBe(_tenantB);
                }

                _multiTenancyManager.CurrentTenant.ShouldBe(_tenantA);
            }

            _multiTenancyManager.CurrentTenant.ShouldBe(_tenantB);
        }
    }
}
