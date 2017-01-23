using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Identity
{
    public class Initialize_Tests : AbpIntegratedTest<AbpIdentityTestModule>
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
            (ServiceProvider.GetRequiredService<IIdentityUserRepository>() is EfCoreIdentityUserRepository).ShouldBeTrue();

            (ServiceProvider.GetRequiredService<IRepository<IdentityUser, Guid>>() is EfCoreIdentityUserRepository).ShouldBeTrue();
            (ServiceProvider.GetRequiredService<IRepository<IdentityUser>>() is EfCoreIdentityUserRepository).ShouldBeTrue();

            (ServiceProvider.GetRequiredService<IQueryableRepository<IdentityUser, Guid>>() is EfCoreIdentityUserRepository).ShouldBeTrue();
            (ServiceProvider.GetRequiredService<IQueryableRepository<IdentityUser>>() is EfCoreIdentityUserRepository).ShouldBeTrue();
        }
    }
}
