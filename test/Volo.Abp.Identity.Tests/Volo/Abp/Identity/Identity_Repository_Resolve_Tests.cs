using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DynamicProxy;
using Xunit;

namespace Volo.Abp.Identity
{
    public class Identity_Repository_Resolve_Tests : AbpIdentityDomainTestBase
    {
        [Fact]
        public void Should_Resolve_UserManager()
        {
            ServiceProvider.GetRequiredService<IdentityUserManager>();
        }

        [Fact]
        public void Should_Resolve_RoleManager()
        {
            ServiceProvider.GetRequiredService<IdentityRoleManager>();
        }
        
        [Fact] //Move this test to Volo.Abp.EntityFrameworkCore.Tests since it's actually testing the EF Core repository registration!
        public void Should_Resolve_Repositories()
        {
            var x = ServiceProvider.GetRequiredService<IIdentityUserRepository>();
            (ProxyHelper.UnProxy(ServiceProvider.GetRequiredService<IIdentityUserRepository>()) is EfCoreIdentityUserRepository).ShouldBeTrue();

            (ProxyHelper.UnProxy(ServiceProvider.GetRequiredService<IBasicRepository<IdentityUser, Guid>>()) is EfCoreIdentityUserRepository).ShouldBeTrue();
            //(ServiceProvider.GetRequiredService<IRepository<IdentityUser>>() is EfCoreIdentityUserRepository).ShouldBeTrue();

            (ProxyHelper.UnProxy(ServiceProvider.GetRequiredService<IRepository<IdentityUser, Guid>>()) is EfCoreIdentityUserRepository).ShouldBeTrue();
            //(ServiceProvider.GetRequiredService<IQueryableRepository<IdentityUser>>() is EfCoreIdentityUserRepository).ShouldBeTrue();
        }
    }
}
