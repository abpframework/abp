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
        public void Should_Resolve_Services()
        {
            ServiceProvider.GetRequiredService<IdentityUserManager>();

            //TODO: Move this service to Volo.Abp.EntityFrameworkCore.Tests since it's actually testing the EF Core repository registration!

            (ServiceProvider.GetRequiredService<IIdentityUserRepository>() is EfCoreIdentityUserRepository).ShouldBeTrue();

            (ServiceProvider.GetRequiredService<IRepository<IdentityUser, string>>() is EfCoreIdentityUserRepository).ShouldBeTrue();
            (ServiceProvider.GetRequiredService<IRepository<IdentityUser>>() is EfCoreIdentityUserRepository).ShouldBeTrue();

            (ServiceProvider.GetRequiredService<IQueryableRepository<IdentityUser, string>>() is EfCoreIdentityUserRepository).ShouldBeTrue();
            (ServiceProvider.GetRequiredService<IQueryableRepository<IdentityUser>>() is EfCoreIdentityUserRepository).ShouldBeTrue();
        }
    }
}
