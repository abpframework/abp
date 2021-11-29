using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.IdentityServer
{
    public abstract class IdentityResourceRepository_Tests<TStartupModule> : AbpIdentityServerTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IIdentityResourceRepository identityResourceRepository;

        public IdentityResourceRepository_Tests()
        {
            identityResourceRepository = ServiceProvider.GetRequiredService<IIdentityResourceRepository>();
        }

        [Fact]
        public async Task GetListByScopesAsync()
        {
            (await identityResourceRepository.GetListByScopeNameAsync(new[] { "", "NewIdentityResource2" })).Count.ShouldBe(1);
        }
    }
}
