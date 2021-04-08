using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer
{
    public class AbpStrictRedirectUriValidator_Tests : AbpIdentityServerTestBase
    {
        private readonly IRedirectUriValidator _abpStrictRedirectUriValidator;

        private readonly Client _testClient = new Client
        {
            RedirectUris = new List<string>
            {
                "https://{0}.api.abp.io:8080/signin-oidc",
                "http://{0}.ng.abp.io/index.html"
            },
            PostLogoutRedirectUris = new List<string>
            {
                "https://{0}.api.abp.io:8080/signin-oidc",
                "http://{0}.ng.abp.io/index.html"
            }
        };

        public AbpStrictRedirectUriValidator_Tests()
        {
            _abpStrictRedirectUriValidator = GetRequiredService<IRedirectUriValidator>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.AddAbpStrictRedirectUriValidator();
        }

        [Fact]
        public void Should_Register_AbpStrictRedirectUriValidator()
        {
            _abpStrictRedirectUriValidator.GetType().ShouldBe(typeof(AbpStrictRedirectUriValidator));
        }

        [Fact]
        public async Task IsRedirectUriValidAsync()
        {
            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("https://t1.api.abp.io:8080/signin-oidc", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("https://api.abp.io:8080/signin-oidc", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("http://t2.ng.abp.io/index.html", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("http://ng.abp.io/index.html", _testClient)).ShouldBeTrue();

            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("https://api.abp:8080/", _testClient)).ShouldBeFalse();
            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("http://ng.abp.io", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("https://api.t1.abp:8080/", _testClient)).ShouldBeFalse();
            (await _abpStrictRedirectUriValidator.IsRedirectUriValidAsync("http://ng.t1.abp.io", _testClient)).ShouldBeFalse();
        }

        [Fact]
        public async Task IsPostLogoutRedirectUriValidAsync()
        {
            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("https://t1.api.abp.io:8080/signin-oidc", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("https://api.abp.io:8080/signin-oidc", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("http://t2.ng.abp.io/index.html", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("http://ng.abp.io/index.html", _testClient)).ShouldBeTrue();

            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("https://api.abp:8080/", _testClient)).ShouldBeFalse();
            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("http://ng.abp.io", _testClient)).ShouldBeTrue();
            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("https://api.t1.abp:8080/", _testClient)).ShouldBeFalse();
            (await _abpStrictRedirectUriValidator.IsPostLogoutRedirectUriValidAsync("http://ng.t1.abp.io", _testClient)).ShouldBeFalse();
        }
    }
}
