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

            var fakeTenant = new TenantInfo(Guid.NewGuid().ToString(), "acme");

            var manager = new MultiTenancyManager(
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
        public void Should_Get_Current_Tenant_From_Two_Resolvers()
        {
            //Arrange

            var fakeTenant = new TenantInfo(Guid.NewGuid().ToString(), "acme");

            var manager = new MultiTenancyManager(
                Substitute.For<ITenantScopeProvider>(),
                new OptionsWrapper<MultiTenancyOptions>(new MultiTenancyOptions
                    {
                        TenantResolvers =
                        {
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = new TenantInfo(Guid.NewGuid().ToString(), "skipped-tenant-1");
                            }),
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = fakeTenant;
                                context.Handled = true;
                            }),
                            new SimpleTenantResolver(context =>
                            {
                                context.Tenant = new TenantInfo(Guid.NewGuid().ToString(), "skipped-tenant-2");
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
        public void Should_Get_Ambient_Tenant_If_Changed()
        {
            //Arrange

            var oldTenant = new TenantInfo(Guid.NewGuid().ToString(), "old-tenant");

            var manager = new MultiTenancyManager(
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

            var overridedTenant = new TenantInfo(Guid.NewGuid().ToString(), "overrided-tenant");
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
