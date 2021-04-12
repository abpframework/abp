using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer
{
    public class AbpWildcardSubdomainCorsPolicyService_Tests : AbpIdentityServerTestBase
    {
        private readonly ICorsPolicyService _corsPolicyService;

        public AbpWildcardSubdomainCorsPolicyService_Tests()
        {
            _corsPolicyService = GetRequiredService<ICorsPolicyService>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.AddAbpWildcardSubdomainCorsPolicyService();
        }

        [Fact]
        public void Should_Register_AbpWildcardSubdomainCorsPolicyService()
        {
            _corsPolicyService.GetType().ShouldBe(typeof(AbpWildcardSubdomainCorsPolicyService));
        }

        [Fact]
        public async Task IsOriginAllowedAsync()
        {
            (await _corsPolicyService.IsOriginAllowedAsync("https://client1-origin.com")).ShouldBeTrue();
            (await _corsPolicyService.IsOriginAllowedAsync("https://client2-origin.com")).ShouldBeFalse();

            (await _corsPolicyService.IsOriginAllowedAsync("https://abp.io")).ShouldBeTrue();
            (await _corsPolicyService.IsOriginAllowedAsync("https://t1.abp.io")).ShouldBeTrue();
            (await _corsPolicyService.IsOriginAllowedAsync("https://t1.ng.abp.io")).ShouldBeTrue();
        }
    }
}
