using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenant_Tests : MultiTenancyTestBase
    {
        private readonly ICurrentTenant _currentTenant;

        private readonly string _tenantA = "A";
        private readonly string _tenantB = "B";
        private string _tenantToBeResolved;

        public CurrentTenant_Tests()
        {
            _currentTenant = ServiceProvider.GetRequiredService<ICurrentTenant>();
        }

        [Fact]
        public void CurrentTenant_Should_Be_Null_As_Default()
        {
            //Assert

            _currentTenant.Id.ShouldBeNull();
        }

        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.Configure<ConfigurationTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantInfo(Guid.NewGuid(), _tenantA),
                    new TenantInfo(Guid.NewGuid(), _tenantB)
                };
            });
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<TenantResolveOptions>(options =>
            {
                options.TenantResolvers.Add(new ActionTenantResolveContributer(context =>
                {
                    if (_tenantToBeResolved == _tenantA)
                    {
                        context.TenantIdOrName = _tenantA;
                    }
                }));

                options.TenantResolvers.Add(new ActionTenantResolveContributer(context =>
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

            Assert.NotNull(_currentTenant.Id);
            _currentTenant.Name.ShouldBe(_tenantA);
        }

        [Fact]
        public void Should_Get_Current_Tenant_From_Multiple_Resolvers()
        {
            //Arrange

            _tenantToBeResolved = _tenantB;

            //Assert

            Assert.NotNull(_currentTenant.Id);
            _currentTenant.Name.ShouldBe(_tenantB);
        }

        [Fact]
        public void Should_Get_Changed_Tenant_If_Wanted()
        {
            _currentTenant.Id.ShouldBe(null);

            _tenantToBeResolved = _tenantB;

            Assert.NotNull(_currentTenant.Id);
            _currentTenant.Name.ShouldBe(_tenantB);

            using (_currentTenant.Change(_tenantA))
            {
                Assert.NotNull(_currentTenant.Id);
                _currentTenant.Name.ShouldBe(_tenantA);

                using (_currentTenant.Change(_tenantB))
                {
                    Assert.NotNull(_currentTenant.Id);
                    _currentTenant.Name.ShouldBe(_tenantB);
                }

                Assert.NotNull(_currentTenant.Id);
                _currentTenant.Name.ShouldBe(_tenantA);
            }

            Assert.NotNull(_currentTenant.Id);
            _currentTenant.Name.ShouldBe(_tenantB);
        }
    }
}
