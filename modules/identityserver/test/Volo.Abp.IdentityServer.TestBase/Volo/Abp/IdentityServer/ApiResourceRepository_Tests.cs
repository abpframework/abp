﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.IdentityServer
{
    public abstract class ApiResourceRepository_Tests<TStartupModule> : AbpIdentityServerTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IApiResourceRepository apiResourceRepository { get; }

        public ApiResourceRepository_Tests()
        {
            apiResourceRepository = ServiceProvider.GetRequiredService<IApiResourceRepository>();
        }

        [Fact]
        public async Task FindByNormalizedNameAsync()
        {
            (await apiResourceRepository.FindByNameAsync("NewApiResource2").ConfigureAwait(false)).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetListByScopesAsync()
        {
            (await apiResourceRepository.GetListByScopesAsync(new[] { "NewApiResource2", "NewApiResource3" }).ConfigureAwait(false)).Count.ShouldBe(2);
        }
    }
}
