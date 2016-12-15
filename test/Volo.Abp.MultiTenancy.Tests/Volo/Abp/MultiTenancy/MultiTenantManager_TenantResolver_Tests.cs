using System;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenantManager_TenantResolver_Tests
    {
        [Fact]
        public void Should_Get_Current_Tenant_As_Null_If_No_Resolver()
        {
            //Arrange

            var manager = new MultiTenancyManager(
                Substitute.For<IServiceProvider>(),
                Substitute.For<ITenantScopeProvider>(),
                new OptionsWrapper<MultiTenancyOptions>(new MultiTenancyOptions())
            );

            //Assert

            manager.CurrentTenant.ShouldBeNull();
        }

        [Fact]
        public void Should_Get_Current_Tenant_From_Single_Resolver()
        {
            //Arrange

            var fakeTenant = new TenantInfo("A");

            var manager = new MultiTenancyManager(
                Substitute.For<IServiceProvider>(),
                Substitute.For<ITenantScopeProvider>(),
                new OptionsWrapper<MultiTenancyOptions>(new MultiTenancyOptions
                    {
                        TenantResolvers =
                        {
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = fakeTenant;
                                context.Handled = true;
                            })
                        }
                    }
                )
            );

            //Assert

            manager.CurrentTenant.ShouldBe(fakeTenant);
        }


        [Fact]
        public void Should_Get_Current_Tenant_From_Multiple_Resolvers()
        {
            //Arrange

            var selectedTenant = new TenantInfo("A");

            var manager = new MultiTenancyManager(
                Substitute.For<IServiceProvider>(),
                Substitute.For<ITenantScopeProvider>(),
                new OptionsWrapper<MultiTenancyOptions>(new MultiTenancyOptions
                    {
                        TenantResolvers =
                        {
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = new TenantInfo("B");
                                context.Handled = false; //Causes go to the next resolver
                            }),
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = selectedTenant;
                            }),
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = new TenantInfo("C");
                            })
                        }
                    }
                )
            );

            //Assert

            manager.CurrentTenant.ShouldBe(selectedTenant);
        }

        [Fact]
        public void Should_Get_Ambient_Tenant_If_Changed()
        {
            //Arrange

            var oldTenant = new TenantInfo("A");

            var manager = new MultiTenancyManager(
                Substitute.For<IServiceProvider>(),
                new AsyncLocalTenantScopeProvider(),
                new OptionsWrapper<MultiTenancyOptions>(
                    new MultiTenancyOptions
                    {
                        TenantResolvers =
                        {
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = oldTenant;
                            })
                        }
                    }
                )
            );

            manager.CurrentTenant.ShouldBe(oldTenant);

            //Act

            var overridedTenant = new TenantInfo("B");
            using (manager.ChangeTenant(overridedTenant))
            {
                //Assert
                manager.CurrentTenant.ShouldBe(overridedTenant);
            }

            //Assert
            manager.CurrentTenant.ShouldBe(oldTenant);
        }
    }
}
