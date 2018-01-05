using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenantManager_Tests : MultiTenancyTestBase
    {
        private readonly IMultiTenancyManager _multiTenancyManager;

        private readonly string _tenantA = "A";
        private readonly string _tenantB = "B";
        private string _tenantToBeResolved;

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

        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.Configure<ConfigurationTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantInformation(Guid.NewGuid(), _tenantA),
                    new TenantInformation(Guid.NewGuid(), _tenantB)
                };
            });
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<MultiTenancyOptions>(options =>
            {
                options.TenantResolvers.Add(new ActionTenantResolver(context =>
                {
                    if (_tenantToBeResolved == _tenantA)
                    {
                        context.TenantIdOrName = _tenantA;
                    }
                }));

                options.TenantResolvers.Add(new ActionTenantResolver(context =>
                {
                    if (_tenantToBeResolved == _tenantB)
                    {
                        context.TenantIdOrName = _tenantB;
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

            Assert.NotNull(_multiTenancyManager.CurrentTenant);
            _multiTenancyManager.CurrentTenant.Name.ShouldBe(_tenantA);
        }

        [Fact]
        public void Should_Get_Current_Tenant_From_Multiple_Resolvers()
        {
            //Arrange

            _tenantToBeResolved = _tenantB;

            //Assert

            Assert.NotNull(_multiTenancyManager.CurrentTenant);
            _multiTenancyManager.CurrentTenant.Name.ShouldBe(_tenantB);
        }

        [Fact]
        public void Should_Get_Changed_Tenant_If_Wanted()
        {
            _multiTenancyManager.CurrentTenant.ShouldBe(null);

            _tenantToBeResolved = _tenantB;

            Assert.NotNull(_multiTenancyManager.CurrentTenant);
            _multiTenancyManager.CurrentTenant.Name.ShouldBe(_tenantB);

            using (_multiTenancyManager.ChangeTenant(_tenantA))
            {
                Assert.NotNull(_multiTenancyManager.CurrentTenant);
                _multiTenancyManager.CurrentTenant.Name.ShouldBe(_tenantA);

                using (_multiTenancyManager.ChangeTenant(_tenantB))
                {
                    Assert.NotNull(_multiTenancyManager.CurrentTenant);
                    _multiTenancyManager.CurrentTenant.Name.ShouldBe(_tenantB);
                }

                Assert.NotNull(_multiTenancyManager.CurrentTenant);
                _multiTenancyManager.CurrentTenant.Name.ShouldBe(_tenantA);
            }

            Assert.NotNull(_multiTenancyManager.CurrentTenant);
            _multiTenancyManager.CurrentTenant.Name.ShouldBe(_tenantB);
        }
    }
}
