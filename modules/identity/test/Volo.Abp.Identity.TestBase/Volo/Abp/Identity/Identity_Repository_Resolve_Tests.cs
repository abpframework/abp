using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity;

public abstract class Identity_Repository_Resolve_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    [Fact] //Move this test to Volo.Abp.EntityFrameworkCore.Tests since it's actually testing the EF Core repository registration!
    public void Should_Resolve_Repositories()
    {
        ServiceProvider.GetService<IRepository<IdentityUser>>().ShouldNotBeNull();
        ServiceProvider.GetService<IRepository<IdentityUser, Guid>>().ShouldNotBeNull();
        ServiceProvider.GetService<IIdentityUserRepository>().ShouldNotBeNull();

        ServiceProvider.GetService<IRepository<IdentityRole>>().ShouldNotBeNull();
        ServiceProvider.GetService<IRepository<IdentityRole, Guid>>().ShouldNotBeNull();
        ServiceProvider.GetService<IIdentityRoleRepository>().ShouldNotBeNull();

        ServiceProvider.GetService<IOrganizationUnitRepository>().ShouldNotBeNull();
    }
}
