using System;
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

            var manager = new MultiTenancyManager(new ITenantResolver[0]);

            //Act

            manager.CurrentTenant.ShouldBeNull();
        }

        [Fact]
        public void Should_Get_Current_Tenant_From_Single_Resolver()
        {
            //Arrange

            var fakeTenant = new TenantInfo(Guid.NewGuid().ToString(), "acme");

            //Act

            var manager = new MultiTenancyManager(new[]
            {
                new TenantResolverAction(context =>
                {
                    context.Tenant = fakeTenant;
                    context.Handled = true;
                })
            });

            //Assert

            manager.CurrentTenant.ShouldBe(fakeTenant);
        }


        [Fact]
        public void Should_Get_Current_Tenant_From_Two_Resolvers()
        {
            //Arrange

            var fakeTenant = new TenantInfo(Guid.NewGuid().ToString(), "acme");

            //Act

            var manager = new MultiTenancyManager(new[]
            {
                new TenantResolverAction(context =>
                {
                    context.Tenant = new TenantInfo(Guid.NewGuid().ToString(), "skipped-tenant");
                }),
                new TenantResolverAction(context =>
                {
                    context.Tenant = fakeTenant;
                    context.Handled = true;
                }),
                new TenantResolverAction(context =>
                {
                    context.Tenant = new TenantInfo(Guid.NewGuid().ToString(), "skipped-tenant");
                    context.Handled = true;
                })
            });

            //Assert

            manager.CurrentTenant.ShouldBe(fakeTenant);
        }
    }
}
