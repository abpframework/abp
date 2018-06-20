using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityOptions_Tests : AbpIdentityDomainTestBase
    {
        private ISettingManager _settingManager;

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _settingManager = Substitute.For<ISettingManager>();
            _settingManager.GetOrNullAsync(Arg.Any<string>()).Returns((string) null);
            services.Replace(ServiceDescriptor.Singleton(_settingManager));
        }

        [Fact]
        public void Should_Resolve_AbpIdentityOptionsFactory()
        {
            GetRequiredService<IOptionsFactory<IdentityOptions>>().ShouldBeOfType(typeof(AbpIdentityOptionsFactory));
        }

        [Fact]
        public void Should_Get_Options_From_Custom_Settings_If_Available()
        {
            using (var scope1 = ServiceProvider.CreateScope())
            {
                var options = scope1.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;
                options.Password.RequiredLength.ShouldBe(6); //Default value
                options.Password.RequiredUniqueChars.ShouldBe(1); //Default value
            }

            _settingManager.GetOrNullAsync(IdentitySettingNames.Password.RequiredLength).Returns(Task.FromResult("42"));

            using (var scope2 = ServiceProvider.CreateScope())
            {
                var options = scope2.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;
                options.Password.RequiredLength.ShouldBe(42); //Setting value
                options.Password.RequiredUniqueChars.ShouldBe(1); //Default value
            }
        }
    }
}
