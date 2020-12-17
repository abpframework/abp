using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore
{
    public class AbpRepositoryConventionalRegistrar_Tests : EntityFrameworkCoreTestBase
    {
        [Fact]
        public void DoNotRegisterRepositoryClassesToDIByDefault()
        {
            GetService<ICityRepository>().ShouldNotBeNull();
            GetService<CityRepository>().ShouldBeNull();
        }
    }

    public class AbpRepositoryConventionalRegistrar_ExposeRepositoryClasses_Tests : EntityFrameworkCoreTestBase
    {
        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.PreConfigure<AbpRepositoryConventionalRegistrarOptions>(options =>
            {
                options.ExposeRepositoryClasses = true;
            });
        }

        [Fact]
        public void RegisterRepositoryClassesToDI()
        {
            GetService<ICityRepository>().ShouldNotBeNull();
            GetService<CityRepository>().ShouldNotBeNull();
        }
    }
}
